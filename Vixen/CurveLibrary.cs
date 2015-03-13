using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using VixenPlusCommon;

namespace VixenPlus {
    public class CurveLibrary : IDisposable {
        private const char Delimiter = '|';

        public const string ManufacturerCol = "Manufacturer";
        public const string LightCountCol = "LightCount";
        public const string ColorCol = "Color";
        public const string ControllerCol = "Controller";
        private const string CurveDataCol = "CurveData";

        private Filter[] _colorFilters;
        private Filter[] _controllerFilters;
        private DataTable _dataTable;
        private string _delimiter;
        private Filter[] _lightCountFilters;
        private Filter[] _manufacturerFilters;
        private bool _modified;


        public CurveLibrary() : this(Path.Combine(Paths.CurveLibraryPath, "library.xml")) {}


        public CurveLibrary(string filePath) {
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


        public Filter[] ColorFilter {
            set {
                _colorFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value) {
                    filter.ColumnName = ColorCol;
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Filter[] ControllerFilter {
            set {
                _controllerFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value) {
                    filter.ColumnName = ControllerCol;
                    filter.ColumnType = typeof (string);
                }
            }
        }

        private string FilePath { get; set; }

        public bool IsFiltered {
            get {
                return (((((_manufacturerFilters != null) && (_manufacturerFilters.Length > 0)) ||
                          ((_lightCountFilters != null) && (_lightCountFilters.Length > 0))) ||
                         ((_colorFilters != null) && (_colorFilters.Length > 0))) || ((_controllerFilters != null) && (_controllerFilters.Length > 0)));
            }
        }

        public Filter[] LightCountFilter {
            set {
                _lightCountFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value) {
                    filter.ColumnName = LightCountCol;
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Filter[] ManufacturerFilter {
            set {
                _manufacturerFilters = value;
                if (value == null) {
                    return;
                }
                foreach (var filter in value) {
                    filter.ColumnName = ManufacturerCol;
                    filter.ColumnType = typeof (string);
                }
            }
        }

        public Sort SortOrder { private get; set; }


        public void Dispose() {}


        private string BuildSortClause() {
            return SortOrder == null ? "" : string.Format("{0} {1}", SortOrder.ColumnName, SortOrder.SortDirection);
        }


        private string BuildWhereClause() {
            var builder = new StringBuilder();
            builder.Append(BuildWhereFragment(_manufacturerFilters));
            builder.Append(BuildWhereFragment(_lightCountFilters));
            builder.Append(BuildWhereFragment(_colorFilters));
            builder.Append(BuildWhereFragment(_controllerFilters));
            if (builder.Length <= 0) {
                return builder.ToString();
            }
            var str = builder.ToString();
            if (str.StartsWith(" " + Filter.Join.And)) {
                builder.Remove(0, 4);
            }
            else if (str.StartsWith(" " + Filter.Join.Or)) {
                builder.Remove(0, 3);
            }
            return builder.ToString();
        }


        private string BuildWhereFragment(Filter[] filters) {
            var sb = new StringBuilder();
            if ((filters == null) || (filters.Length <= 0)) {
                return sb.ToString();
            }
            _delimiter = "'";
            Action<Filter> action =
                f =>
                    sb.AppendFormat(" {0} {1} {2} {3}", Filter.JoinOperator, f.ColumnName, OperatorString(f.ComparisonOperator),
                        FormatValue(f.Value, f.ColumnType));
            Array.ForEach(filters, action);
            return sb.ToString();
        }


        private string FormatValue(string value, Type valueType) {
            var name = valueType.Name;
            if (name == "String" || name != "Int32") {
                return string.Format("{0}{1}{2}", _delimiter, value, _delimiter);
            }
            return value;
        }


        public string[] GetAllControllers() {
            return GetColumnData(ControllerCol);
        }


        public IEnumerable<string> GetAllLightColors() {
            return GetColumnData(ColorCol);
        }


        public IEnumerable<string> GetAllLightCounts() {
            return GetColumnData(LightCountCol);
        }


        public string[] GetAllManufacturers() {
            return GetColumnData(ManufacturerCol);
        }


        private string[] GetColumnData(string columnName) {
            var list = new List<string>();
            Load(false);
            if (_dataTable == null) {
                return list.ToArray();
            }
            var rowArray = _dataTable.Select(BuildWhereClause(), BuildSortClause());
            list.AddRange(rowArray.Select(row => row[columnName].ToString()));
            return list.ToArray();
        }


        private static string GetSelectString(string manufacturer, string lightCount, int color, string controller) {
            return string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and {6} = '{7}'", ManufacturerCol, manufacturer, LightCountCol, lightCount,
                ColorCol, color, ControllerCol, controller);
        }


        public void Import(CurveLibraryRecord clr, bool updateIfPresent = false) {
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
            if (rowArray.Length == 0) {
                var row = _dataTable.NewRow();
                row[ManufacturerCol] = clr.Manufacturer;
                row[LightCountCol] = clr.LightCount;
                row[ColorCol] = clr.Color;
                row[ControllerCol] = clr.Controller;
                row[CurveDataCol] = string.Join("|", curveDataStrings.ToArray());
                _dataTable.Rows.Add(row);
            }
            else {
                rowArray[0][CurveDataCol] = string.Join("|", curveDataStrings.ToArray());
            }
            _modified = true;
        }


        public void Load(bool forcedLoad) {
            if (!forcedLoad && (_dataTable != null)) {
                return;
            }
            if (!File.Exists(FilePath)) {
                Save(true);
            }
            if (_dataTable != null) {
                _dataTable.Dispose();
                _dataTable = null;
            }
            _dataTable = new DataTable();
            _dataTable.ReadXml(FilePath);
        }


        private string OperatorString(Filter.Operator op) {
            switch (op) {
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


        public IEnumerable<CurveLibraryRecord> Read() {
            var results = new List<CurveLibraryRecord>();

            if (_dataTable == null) {
                Load(false);
            }

            // ReSharper disable once PossibleNullReferenceException
            var filteredRecords = _dataTable.Select(BuildWhereClause(), BuildSortClause());

            foreach (var record in filteredRecords) {
                var clr = new CurveLibraryRecord(record[ManufacturerCol].ToString(), record[LightCountCol].ToString(),
                    int.Parse(record[ColorCol].ToString()), record[ControllerCol].ToString());

                var sp = record[CurveDataCol].ToString().Split(Delimiter);
                var bytes = new byte[sp.Count()];
                var count = 0;
                foreach (var val in sp) {
                    bytes[count++] = byte.Parse(val);
                }
                clr.CurveData = bytes;
                results.Add(clr);
            }

            return results;
        }


        public void Save(bool force = false) {
            if (!force && !_modified) {
                return;
            }
            if (_dataTable != null) {
                _dataTable.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
            else {
                _dataTable = new DataTable("CurveLibrary");
                _dataTable.Columns.Add(ManufacturerCol).AllowDBNull = false;
                _dataTable.Columns.Add(LightCountCol).AllowDBNull = false;
                _dataTable.Columns.Add(ColorCol).AllowDBNull = false;
                _dataTable.Columns.Add(ControllerCol).AllowDBNull = false;
                _dataTable.Columns.Add(CurveDataCol).AllowDBNull = false;
                _dataTable.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
            _modified = false;
        }


        public class Filter {
            public enum Join {
                And,
                Or
            }

            public enum Operator {
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


            public Filter(Operator op, string value) {
                ComparisonOperator = op;
                Value = value;
            }
        }

        public class Sort {
            public enum Direction {
                Asc,
                Desc
            }

            public readonly string ColumnName;
            public readonly Direction SortDirection;


            public Sort(string columnName, Direction direction) {
                ColumnName = columnName;
                SortDirection = direction;
            }
        }
    }
}