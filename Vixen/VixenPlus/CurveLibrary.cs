using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace VixenPlus
{
    public class CurveLibrary : IDisposable
    {
        public const string Color = "Color";
        public const string Controller = "Controller";
        public const string CurveData = "CurveData";
        public const string LightCount = "LightCount";
        public const string Manufacturer = "Manufacturer";
        public const string LibraryFile = "library.xml";
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
            get { return _colorFilters; }
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
            get { return _controllerFilters; }
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

        public string FilePath { get; private set; }

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
            get { return _lightCountFilters; }
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
            get { return _manufacturerFilters; }
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

        public Sort SortOrder { get; set; }


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
            if (builder.Length > 0)
            {
                var str = builder.ToString();
                if (str.StartsWith(" " + Filter.Join.And))
                {
                    builder.Remove(0, 4);
                }
                else if (str.StartsWith(" " + Filter.Join.Or))
                {
                    builder.Remove(0, 3);
                }
            }
            return builder.ToString();
        }

        private string BuildWhereFragment(Filter[] filters)
        {
            var sb = new StringBuilder();
            if ((filters != null) && (filters.Length > 0))
            {
                _delimiter = "'";
                Action<Filter> action = f => sb.AppendFormat(" {0} {1} {2} {3}",
                                                             new object[]
                                                                 {
                                                                     f.JoinOperator, f.ColumnName, OperatorString(f.ComparisonOperator),
                                                                     FormatValue(f.Value, f.ColumnType)
                                                                 });
                Array.ForEach(filters, action);
            }
            return sb.ToString();
        }

        public CurveLibraryRecord Find(string manufacturer, string lightCount, int color, string controller)
        {
            Load(false);
            if (_dataTable != null)
            {
                var rowArray = _dataTable.Select(GetSelectString(manufacturer, lightCount, color, controller));
                if (rowArray.Length == 0)
                {
                    return null;
                }
                var row = rowArray[0];
                return new CurveLibraryRecord(row["Manufacturer"].ToString(), row["LightCount"].ToString(), (int) row["Color"],
                                              row["Controller"].ToString(), row["CurveData"].ToString());
            }
            return null;
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

        public string[] GetAllLightColors()
        {
            return GetColumnData("Color");
        }

        public string[] GetAllLightCounts()
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
            if (_dataTable != null)
            {
                var rowArray = _dataTable.Select(BuildWhereClause(), BuildSortClause());
                foreach (var row in rowArray)
                {
                    list.Add(row[columnName].ToString());
                }
            }
            return list.ToArray();
        }

        public int GetRecordCount()
        {
            Load(false);
            if (_dataTable != null)
            {
                return _dataTable.Rows.Count;
            }
            return -1;
        }

        private static string GetSelectString(string manufacturer, string lightCount, int color, string controller)
        {
            return string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and {6} = '{7}'",
                                 new object[]
                                     {
                                         "Manufacturer", manufacturer, "LightCount", lightCount, "Color", color, "Controller",
                                         controller
                                     });
        }

        public void Import(CurveLibraryRecord clr)
        {
            Import(clr, false);
        }

        public void Import(CurveLibraryRecord clr, bool updateIfPresent)
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

        public void Load()
        {
            Load(false);
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

        //public IEnumerable<CurveLibraryRecord> Read()
        //{
        //    <Read>d__0 d__ = new <Read>d__0(-2);
        //    d__.<>4__this = this;
        //    return d__;
        //}

        //private IEnumerable<CurveLibraryRecord> ReadRecords()
        //{
        //    <ReadRecords>d__5 d__ = new <ReadRecords>d__5(-2);
        //    d__.<>4__this = this;
        //    return d__;
        //}

        public void Save()
        {
            Save(false);
        }

        public void Save(bool force)
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

        //public void SynchronizeWith(CurveLibrary library)
        //{
        //    int recordCount = library.GetRecordCount();
        //    List<CurveLibraryRecord> list = new List<CurveLibraryRecord>();
        //    foreach (CurveLibraryRecord record2 in library.ReadRecords())
        //    {
        //        CurveLibraryRecord record = this.Find(record2.Manufacturer, record2.LightCount, record2.Color, record2.Controller);
        //        if (record != null)
        //        {
        //            if (!this.ArrayMatch(record.CurveData, record2.CurveData))
        //            {
        //                list.Add(record2);
        //            }
        //        }
        //        else
        //        {
        //            this.Import(record2);
        //        }
        //    }
        //    if (list.Count > 0)
        //    {
        //        CurveConflictResolutionDialog dialog = new CurveConflictResolutionDialog(list.ToArray());
        //        if (dialog.ShowDialog() == DialogResult.OK)
        //        {
        //            foreach (CurveLibraryRecord record3 in dialog.SelectedRecords)
        //            {
        //                this.Import(record3, true);
        //            }
        //        }
        //        dialog.Dispose();
        //    }
        //}

        //[CompilerGenerated]
        //private sealed class <Read>d__0 : IEnumerable<CurveLibraryRecord>, IEnumerable, IEnumerator<CurveLibraryRecord>, IEnumerator, IDisposable
        //{
        //    private int <>1__state;
        //    private CurveLibraryRecord <>2__current;
        //    public CurveLibrary <>4__this;
        //    public IEnumerator<CurveLibraryRecord> <>7__wrap2;
        //    public CurveLibraryRecord <clr>5__1;

        //    [DebuggerHidden]
        //    public <Read>d__0(int <>1__state)
        //    {
        //        this.<>1__state = <>1__state;
        //    }

        //    private bool MoveNext()
        //    {
        //        try
        //        {
        //            switch (this.<>1__state)
        //            {
        //                case 0:
        //                    this.<>1__state = -1;
        //                    this.<>7__wrap2 = this.<>4__this.ReadRecords().GetEnumerator();
        //                    this.<>1__state = 1;
        //                    while (this.<>7__wrap2.MoveNext())
        //                    {
        //                        this.<clr>5__1 = this.<>7__wrap2.Current;
        //                        this.<>2__current = this.<clr>5__1;
        //                        this.<>1__state = 2;
        //                        return true;
        //                    Label_0075:
        //                        this.<>1__state = 1;
        //                    }
        //                    this.<>1__state = -1;
        //                    if (this.<>7__wrap2 != null)
        //                    {
        //                        this.<>7__wrap2.Dispose();
        //                    }
        //                    break;

        //                case 2:
        //                    goto Label_0075;
        //            }
        //            return false;
        //        }
        //        fault
        //        {
        //            ((IDisposable) this).Dispose();
        //        }
        //    }

        //    [DebuggerHidden]
        //    IEnumerator<CurveLibraryRecord> IEnumerable<CurveLibraryRecord>.GetEnumerator()
        //    {
        //        if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
        //        {
        //            return this;
        //        }
        //        CurveLibrary.<Read>d__0 d__ = new CurveLibrary.<Read>d__0(0);
        //        d__.<>4__this = this.<>4__this;
        //        return d__;
        //    }

        //    [DebuggerHidden]
        //    IEnumerator IEnumerable.GetEnumerator()
        //    {
        //        return this.System.Collections.Generic.IEnumerable<Vixen.CurveLibraryRecord>.GetEnumerator();
        //    }

        //    [DebuggerHidden]
        //    void IEnumerator.Reset()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void IDisposable.Dispose()
        //    {
        //        switch (this.<>1__state)
        //        {
        //            case 1:
        //            case 2:
        //                try
        //                {
        //                }
        //                finally
        //                {
        //                    this.<>1__state = -1;
        //                    if (this.<>7__wrap2 != null)
        //                    {
        //                        this.<>7__wrap2.Dispose();
        //                    }
        //                }
        //                break;
        //        }
        //    }

        //    CurveLibraryRecord IEnumerator<CurveLibraryRecord>.Current
        //    {
        //        [DebuggerHidden]
        //        get
        //        {
        //            return this.<>2__current;
        //        }
        //    }

        //    object IEnumerator.Current
        //    {
        //        [DebuggerHidden]
        //        get
        //        {
        //            return this.<>2__current;
        //        }
        //    }
        //}

        //[CompilerGenerated]
        //private sealed class <ReadRecords>d__5 : IEnumerable<CurveLibraryRecord>, IEnumerable, IEnumerator<CurveLibraryRecord>, IEnumerator, IDisposable
        //{
        //    private int <>1__state;
        //    private CurveLibraryRecord <>2__current;
        //    public CurveLibrary <>4__this;
        //    public DataRow[] <>7__wrap8;
        //    public int <>7__wrap9;
        //    public DataRow <row>5__7;
        //    public DataRow[] <rows>5__6;

        //    [DebuggerHidden]
        //    public <ReadRecords>d__5(int <>1__state)
        //    {
        //        this.<>1__state = <>1__state;
        //    }

        //    private bool MoveNext()
        //    {
        //        try
        //        {
        //            switch (this.<>1__state)
        //            {
        //                case 0:
        //                    this.<>1__state = -1;
        //                    this.<>4__this.Load(false);
        //                    if (this.<>4__this._dataTable != null)
        //                    {
        //                        this.<rows>5__6 = this.<>4__this._dataTable.Select(this.<>4__this.BuildWhereClause(), this.<>4__this.BuildSortClause());
        //                        this.<>1__state = 1;
        //                        this.<>7__wrap8 = this.<rows>5__6;
        //                        this.<>7__wrap9 = 0;
        //                        while (this.<>7__wrap9 < this.<>7__wrap8.Length)
        //                        {
        //                            this.<row>5__7 = this.<>7__wrap8[this.<>7__wrap9];
        //                            this.<>2__current = new CurveLibraryRecord(this.<row>5__7["Manufacturer"].ToString(), this.<row>5__7["LightCount"].ToString(), int.Parse(this.<row>5__7["Color"].ToString()), this.<row>5__7["Controller"].ToString(), this.<row>5__7["CurveData"].ToString());
        //                            this.<>1__state = 2;
        //                            return true;
        //                        Label_0136:
        //                            this.<>1__state = 1;
        //                            this.<>7__wrap9++;
        //                        }
        //                        this.<>1__state = -1;
        //                    }
        //                    break;

        //                case 2:
        //                    goto Label_0136;
        //            }
        //            return false;
        //        }
        //        fault
        //        {
        //            ((IDisposable) this).Dispose();
        //        }
        //    }

        //    [DebuggerHidden]
        //    IEnumerator<CurveLibraryRecord> IEnumerable<CurveLibraryRecord>.GetEnumerator()
        //    {
        //        if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
        //        {
        //            return this;
        //        }
        //        CurveLibrary.<ReadRecords>d__5 d__ = new CurveLibrary.<ReadRecords>d__5(0);
        //        d__.<>4__this = this.<>4__this;
        //        return d__;
        //    }

        //    [DebuggerHidden]
        //    IEnumerator IEnumerable.GetEnumerator()
        //    {
        //        return this.System.Collections.Generic.IEnumerable<Vixen.CurveLibraryRecord>.GetEnumerator();
        //    }

        //    [DebuggerHidden]
        //    void IEnumerator.Reset()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void IDisposable.Dispose()
        //    {
        //        switch (this.<>1__state)
        //        {
        //            case 1:
        //            case 2:
        //                this.<>1__state = -1;
        //                break;
        //        }
        //    }

        //    CurveLibraryRecord IEnumerator<CurveLibraryRecord>.Current
        //    {
        //        [DebuggerHidden]
        //        get
        //        {
        //            return this.<>2__current;
        //        }
        //    }

        //    object IEnumerator.Current
        //    {
        //        [DebuggerHidden]
        //        get
        //        {
        //            return this.<>2__current;
        //        }
        //    }
        //}

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
            public Operator ComparisonOperator;
            public Join JoinOperator = Join.And;
            public string Value;

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

            public string ColumnName;
            public Direction SortDirection;

            public Sort(string columnName, Direction direction)
            {
                ColumnName = columnName;
                SortDirection = direction;
            }
        }
    }
}