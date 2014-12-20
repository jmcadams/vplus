using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using VixenPlusCommon;

namespace VixenPlus {
    public class CurveLibrary : IDisposable
    {
        private Filter[] _colorFilters;
        private Filter[] _controllerFilters;
        private DataTable _dataTable;
        private string _delimiter;
        private Filter[] _lightCountFilters;
        private Filter[] _manufacturerFilters;
        private bool _modified;


        public CurveLibrary() : this(Path.Combine(Paths.CurveLibraryPath, "library.xml"))
        {
        }

        public CurveLibrary(string filePath)
        {
            _dataTable = null;
            _manufacturerFilters = null;
            _lightCountFilters = null;
            _colorFilters = null;
            _controllerFilters = null;
            SortOrder = null;
            _modified = false;
            _delimiter = "\"";
            FilePath = filePath;
        }

        public Filter[] ColorFilter
        {
            set
            {
                _colorFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value)
                {
                    filter.ColumnName = "Color";
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Filter[] ControllerFilter
        {
            set
            {
                _controllerFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value)
                {
                    filter.ColumnName = "Controller";
                    filter.ColumnType = typeof (string);
                }
            }
        }

        private string FilePath { get; set; }

        public bool IsFiltered
        {
            get
            {
                return (((((_manufacturerFilters != null) && (_manufacturerFilters.Length > 0)) ||
                          ((_lightCountFilters != null) && (_lightCountFilters.Length > 0))) ||
                         ((_colorFilters != null) && (_colorFilters.Length > 0))) ||
                        ((_controllerFilters != null) && (_controllerFilters.Length > 0)));
            }
        }

        public Filter[] LightCountFilter
        {
            set
            {
                _lightCountFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value)
                {
                    filter.ColumnName = "LightCount";
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Filter[] ManufacturerFilter
        {
            set
            {
                _manufacturerFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value)
                {
                    filter.ColumnName = "Manufacturer";
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Sort SortOrder { private get; set; }


        public void Dispose()
        {
        }

        private string BuildSortClause() {
            return SortOrder == null ? "" : string.Format("{0} {1}", SortOrder.ColumnName, SortOrder.SortDirection);
        }


        private string BuildWhereClause()
        {
            var builder = new StringBuilder();
            builder.Append(BuildWhereFragment(_manufacturerFilters));
            builder.Append(BuildWhereFragment(_lightCountFilters));
            builder.Append(BuildWhereFragment(_colorFilters));
            builder.Append(BuildWhereFragment(_controllerFilters));
            if (builder.Length <= 0) {
                return builder.ToString();
            }
            var str = builder.ToString();
            if (str.StartsWith(" " + Filter.Join.And))
            {
                builder.Remove(0, 4);
            }
            else if (str.StartsWith(" " + Filter.Join.Or))
            {
                builder.Remove(0, 3);
            }
            return builder.ToString();
        }

        private string BuildWhereFragment(Filter[] filters)
        {
            var sb = new StringBuilder();
            if ((filters == null) || (filters.Length <= 0)) {
                return sb.ToString();
            }
            _delimiter = "'";
            Action<Filter> action = f => sb.AppendFormat(" {0} {1} {2} {3}", Filter.JoinOperator, f.ColumnName, OperatorString(f.ComparisonOperator), FormatValue(f.Value, f.ColumnType));
            Array.ForEach(filters, action);
            return sb.ToString();
        }

        private string FormatValue(string value, Type valueType)
        {
            var name = valueType.Name;
            if (name == "String" || name != "Int32")
            {
                return string.Format("{0}{1}{2}", _delimiter, value, _delimiter);
            }
            return value;
        }

        public string[] GetAllControllers()
        {
            return GetColumnData("Controller");
        }

        public IEnumerable<string> GetAllLightColors()
        {
            return GetColumnData("Color");
        }

        public IEnumerable<string> GetAllLightCounts()
        {
            return GetColumnData("LightCount");
        }

        public string[] GetAllManufacturers()
        {
            return GetColumnData("Manufacturer");
        }

        private string[] GetColumnData(string columnName)
        {
            var list = new List<string>();
            Load(false);
            if (_dataTable == null) {
                return list.ToArray();
            }
            var rowArray = _dataTable.Select(BuildWhereClause(), BuildSortClause());
            list.AddRange(rowArray.Select(row => row[columnName].ToString()));
            return list.ToArray();
        }

        private static string GetSelectString(string manufacturer, string lightCount, int color, string controller)
        {
            return string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and {6} = '{7}'", "Manufacturer", manufacturer, "LightCount", lightCount, "Color", color, "Controller", controller);
        }


        public void Import(CurveLibraryRecord clr, bool updateIfPresent = false)
        {
            Load(false);
            if (_dataTable == null) {
                return;
            }
            var rowArray = _dataTable.Select(GetSelectString(clr.Manufacturer, clr.LightCount, clr.Color, clr.Controller));
            if ((rowArray.Length != 0) && !updateIfPresent) {
                return;
            }
            var curveDataStrings = new List<string>();
            Array.ForEach(clr.CurveData, b => curveDataStrings.Add(b.ToString(CultureInfo.InvariantCulture)));
            if (rowArray.Length == 0)
            {
                var row = _dataTable.NewRow();
                row["Manufacturer"] = clr.Manufacturer;
                row["LightCount"] = clr.LightCount;
                row["Color"] = clr.Color;
                row["Controller"] = clr.Controller;
                row["CurveData"] = string.Join("|", curveDataStrings.ToArray());
                _dataTable.Rows.Add(row);
            }
            else
            {
                rowArray[0]["CurveData"] = string.Join("|", curveDataStrings.ToArray());
            }
            _modified = true;
        }

        public void Load(bool forcedLoad)
        {
            if (!forcedLoad && (_dataTable != null)) {
                return;
            }
            if (!File.Exists(FilePath))
            {
                Save(true);
            }
            if (_dataTable != null)
            {
                _dataTable.Dispose();
                _dataTable = null;
            }
            _dataTable = new DataTable();
            _dataTable.ReadXml(FilePath);
        }

        private string OperatorString(Filter.Operator op)
        {
            switch (op)
            {
                case Filter.Operator.Equals:
                    return "=";

                case Filter.Operator.LessThan:
                    return "<";

                case Filter.Operator.GreaterThan:
                    return ">";

                case Filter.Operator.LessThanEqualTo:
                    return "<=";

                case Filter.Operator.GreaterThanEqualTo:
                    return ">=";
            }
            return "=";
        }

        //TODO: Implement
        public IEnumerable<CurveLibraryRecord> Read()
        {
            return null;
        }

        public void Save(bool force = false)
        {
            if (!force && !_modified) {
                return;
            }
            if (_dataTable != null)
            {
                _dataTable.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
            else
            {
                _dataTable = new DataTable("CurveLibrary");
                _dataTable.Columns.Add("Manufacturer").AllowDBNull = false;
                _dataTable.Columns.Add("LightCount").AllowDBNull = false;
                _dataTable.Columns.Add("Color").AllowDBNull = false;
                _dataTable.Columns.Add("Controller").AllowDBNull = false;
                _dataTable.Columns.Add("CurveData").AllowDBNull = false;
                _dataTable.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
            _modified = false;
        }

        public class Filter
        {
            public enum Join
            {
                And,
                Or
            }

            public enum Operator
            {
                Equals,
                LessThan,
                GreaterThan,
                LessThanEqualTo,
                GreaterThanEqualTo
            }

            public string ColumnName;
            public Type ColumnType;
            public readonly Operator ComparisonOperator;
            public const Join JoinOperator = Join.And;
            public readonly string Value;

            public Filter(Operator op, string value)
            {
                ComparisonOperator = op;
                Value = value;
            }
        }

        public class Sort
        {
            public enum Direction
            {
                Asc,
                Desc
            }

            public readonly string ColumnName;
            public readonly Direction SortDirection;

            public Sort(string columnName, Direction direction)
            {
                ColumnName = columnName;
                SortDirection = direction;
            }
        }
    }
}