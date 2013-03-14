namespace Vixen
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class CurveLibrary : IDisposable
    {
        public const string COL_COLOR = "Color";
        public const string COL_CONTROLLER = "Controller";
        public const string COL_CURVE_DATA = "CurveData";
        public const string COL_LIGHT_COUNT = "LightCount";
        public const string COL_MANUFACTURER = "Manufacturer";
        public const string CURVE_LIBRARY_FILE = "library.xml";
        private Filter[] m_colorFilters;
        private Filter[] m_controllerFilters;
        private DataTable m_dataTable;
        private Filter[] m_lightCountFilters;
        private string m_localFilePath;
        private Filter[] m_manufacturerFilters;
        private bool m_modified;
        private Sort m_sort;
        private string StringDelimiter;

        public CurveLibrary() : this(Path.Combine(Paths.CurveLibraryPath, "library.xml"))
        {
        }

        public CurveLibrary(string filePath)
        {
            this.m_dataTable = null;
            this.m_manufacturerFilters = null;
            this.m_lightCountFilters = null;
            this.m_colorFilters = null;
            this.m_controllerFilters = null;
            this.m_sort = null;
            this.m_modified = false;
            this.StringDelimiter = "\"";
            this.m_localFilePath = filePath;
        }

        private bool ArrayMatch(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string BuildSortClause()
        {
            if (this.m_sort != null)
            {
                return string.Format("{0} {1}", this.m_sort.ColumnName, this.m_sort.SortDirection);
            }
            return "";
        }

        private string BuildWhereClause()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.BuildWhereFragment(this.m_manufacturerFilters));
            builder.Append(this.BuildWhereFragment(this.m_lightCountFilters));
            builder.Append(this.BuildWhereFragment(this.m_colorFilters));
            builder.Append(this.BuildWhereFragment(this.m_controllerFilters));
            if (builder.Length > 0)
            {
                string str = builder.ToString();
                if (str.StartsWith(" " + Filter.Join.And.ToString()))
                {
                    builder.Remove(0, 4);
                }
                else if (str.StartsWith(" " + Filter.Join.Or.ToString()))
                {
                    builder.Remove(0, 3);
                }
            }
            return builder.ToString();
        }

        private string BuildWhereFragment(Filter[] filters)
        {
            Action<Filter> action = null;
            StringBuilder sb = new StringBuilder();
            if ((filters != null) && (filters.Length > 0))
            {
                this.StringDelimiter = "'";
                if (action == null)
                {
                    action = delegate (Filter f) {
                        sb.AppendFormat(" {0} {1} {2} {3}", new object[] { f.JoinOperator, f.ColumnName, this.OperatorString(f.ComparisonOperator), this.FormatValue(f.Value, f.ColumnType) });
                    };
                }
                Array.ForEach<Filter>(filters, action);
            }
            return sb.ToString();
        }

        public void Dispose()
        {
        }

        public CurveLibraryRecord Find(string manufacturer, string lightCount, int color, string controller)
        {
            this.Load(false);
            if (this.m_dataTable != null)
            {
                DataRow[] rowArray = this.m_dataTable.Select(this.GetSelectString(manufacturer, lightCount, color, controller));
                if (rowArray.Length == 0)
                {
                    return null;
                }
                DataRow row = rowArray[0];
                return new CurveLibraryRecord(row["Manufacturer"].ToString(), row["LightCount"].ToString(), (int) row["Color"], row["Controller"].ToString(), row["CurveData"].ToString());
            }
            return null;
        }

        private string FormatValue(string value, System.Type valueType)
        {
            string name = valueType.Name;
            if (((name == null) || (name == "String")) || (name != "Int32"))
            {
                return string.Format("{0}{1}{2}", this.StringDelimiter, value, this.StringDelimiter);
            }
            return value;
        }

        public string[] GetAllControllers()
        {
            return this.GetColumnData("Controller");
        }

        public string[] GetAllLightColors()
        {
            return this.GetColumnData("Color");
        }

        public string[] GetAllLightCounts()
        {
            return this.GetColumnData("LightCount");
        }

        public string[] GetAllManufacturers()
        {
            return this.GetColumnData("Manufacturer");
        }

        private string[] GetColumnData(string columnName)
        {
            List<string> list = new List<string>();
            this.Load(false);
            if (this.m_dataTable != null)
            {
                DataRow[] rowArray = this.m_dataTable.Select(this.BuildWhereClause(), this.BuildSortClause());
                foreach (DataRow row in rowArray)
                {
                    list.Add(row[columnName].ToString());
                }
            }
            return list.ToArray();
        }

        public int GetRecordCount()
        {
            this.Load(false);
            if (this.m_dataTable != null)
            {
                return this.m_dataTable.Rows.Count;
            }
            return -1;
        }

        private string GetSelectString(string manufacturer, string lightCount, int color, string controller)
        {
            return string.Format("{0} = '{1}' and {2} = {3} and {4} = {5} and {6} = '{7}'", new object[] { "Manufacturer", manufacturer, "LightCount", lightCount, "Color", color, "Controller", controller });
        }

        public void Import(CurveLibraryRecord clr)
        {
            this.Import(clr, false);
        }

        public void Import(CurveLibraryRecord clr, bool updateIfPresent)
        {
            this.Load(false);
            if (this.m_dataTable != null)
            {
                DataRow[] rowArray = this.m_dataTable.Select(this.GetSelectString(clr.Manufacturer, clr.LightCount, clr.Color, clr.Controller));
                if ((rowArray.Length == 0) || updateIfPresent)
                {
                    List<string> curveDataStrings = new List<string>();
                    Array.ForEach<byte>(clr.CurveData, delegate (byte b) {
                        curveDataStrings.Add(b.ToString());
                    });
                    if (rowArray.Length == 0)
                    {
                        DataRow row = this.m_dataTable.NewRow();
                        row["Manufacturer"] = clr.Manufacturer;
                        row["LightCount"] = clr.LightCount;
                        row["Color"] = clr.Color;
                        row["Controller"] = clr.Controller;
                        row["CurveData"] = string.Join("|", curveDataStrings.ToArray());
                        this.m_dataTable.Rows.Add(row);
                    }
                    else
                    {
                        rowArray[0]["CurveData"] = string.Join("|", curveDataStrings.ToArray());
                    }
                    this.m_modified = true;
                }
            }
        }

        public void Load()
        {
            this.Load(false);
        }

        public void Load(bool forcedLoad)
        {
            if (forcedLoad || (this.m_dataTable == null))
            {
                if (!File.Exists(this.m_localFilePath))
                {
                    this.Save(true);
                }
                try
                {
                    if (this.m_dataTable != null)
                    {
                        this.m_dataTable.Dispose();
                        this.m_dataTable = null;
                    }
                    this.m_dataTable = new DataTable();
                    this.m_dataTable.ReadXml(this.m_localFilePath);
                }
                finally
                {
                }
            }
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

        public IEnumerable<CurveLibraryRecord> Read()
        {
            <Read>d__0 d__ = new <Read>d__0(-2);
            d__.<>4__this = this;
            return d__;
        }

        private IEnumerable<CurveLibraryRecord> ReadRecords()
        {
            <ReadRecords>d__5 d__ = new <ReadRecords>d__5(-2);
            d__.<>4__this = this;
            return d__;
        }

        public void Save()
        {
            this.Save(false);
        }

        public void Save(bool force)
        {
            if (force || this.m_modified)
            {
                if (this.m_dataTable != null)
                {
                    this.m_dataTable.WriteXml(this.m_localFilePath, XmlWriteMode.WriteSchema);
                }
                else
                {
                    this.m_dataTable = new DataTable("CurveLibrary");
                    this.m_dataTable.Columns.Add("Manufacturer").AllowDBNull = false;
                    this.m_dataTable.Columns.Add("LightCount").AllowDBNull = false;
                    this.m_dataTable.Columns.Add("Color").AllowDBNull = false;
                    this.m_dataTable.Columns.Add("Controller").AllowDBNull = false;
                    this.m_dataTable.Columns.Add("CurveData").AllowDBNull = false;
                    this.m_dataTable.WriteXml(this.m_localFilePath, XmlWriteMode.WriteSchema);
                }
                this.m_modified = false;
            }
        }

        public void SynchronizeWith(CurveLibrary library)
        {
            int recordCount = library.GetRecordCount();
            List<CurveLibraryRecord> list = new List<CurveLibraryRecord>();
            foreach (CurveLibraryRecord record2 in library.ReadRecords())
            {
                CurveLibraryRecord record = this.Find(record2.Manufacturer, record2.LightCount, record2.Color, record2.Controller);
                if (record != null)
                {
                    if (!this.ArrayMatch(record.CurveData, record2.CurveData))
                    {
                        list.Add(record2);
                    }
                }
                else
                {
                    this.Import(record2);
                }
            }
            if (list.Count > 0)
            {
                CurveConflictResolutionDialog dialog = new CurveConflictResolutionDialog(list.ToArray());
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (CurveLibraryRecord record3 in dialog.SelectedRecords)
                    {
                        this.Import(record3, true);
                    }
                }
                dialog.Dispose();
            }
        }

        public Filter[] ColorFilter
        {
            get
            {
                return this.m_colorFilters;
            }
            set
            {
                this.m_colorFilters = value;
                if (value != null)
                {
                    foreach (Filter filter in value)
                    {
                        filter.ColumnName = "Color";
                        filter.ColumnType = typeof(string);
                    }
                }
            }
        }

        public Filter[] ControllerFilter
        {
            get
            {
                return this.m_controllerFilters;
            }
            set
            {
                this.m_controllerFilters = value;
                if (value != null)
                {
                    foreach (Filter filter in value)
                    {
                        filter.ColumnName = "Controller";
                        filter.ColumnType = typeof(string);
                    }
                }
            }
        }

        public string FilePath
        {
            get
            {
                return this.m_localFilePath;
            }
        }

        public bool IsFiltered
        {
            get
            {
                return (((((this.m_manufacturerFilters != null) && (this.m_manufacturerFilters.Length > 0)) || ((this.m_lightCountFilters != null) && (this.m_lightCountFilters.Length > 0))) || ((this.m_colorFilters != null) && (this.m_colorFilters.Length > 0))) || ((this.m_controllerFilters != null) && (this.m_controllerFilters.Length > 0)));
            }
        }

        public Filter[] LightCountFilter
        {
            get
            {
                return this.m_lightCountFilters;
            }
            set
            {
                this.m_lightCountFilters = value;
                if (value != null)
                {
                    foreach (Filter filter in value)
                    {
                        filter.ColumnName = "LightCount";
                        filter.ColumnType = typeof(string);
                    }
                }
            }
        }

        public Filter[] ManufacturerFilter
        {
            get
            {
                return this.m_manufacturerFilters;
            }
            set
            {
                this.m_manufacturerFilters = value;
                if (value != null)
                {
                    foreach (Filter filter in value)
                    {
                        filter.ColumnName = "Manufacturer";
                        filter.ColumnType = typeof(string);
                    }
                }
            }
        }

        public Sort SortOrder
        {
            get
            {
                return this.m_sort;
            }
            set
            {
                this.m_sort = value;
            }
        }

        [CompilerGenerated]
        private sealed class <Read>d__0 : IEnumerable<CurveLibraryRecord>, IEnumerable, IEnumerator<CurveLibraryRecord>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private CurveLibraryRecord <>2__current;
            public CurveLibrary <>4__this;
            public IEnumerator<CurveLibraryRecord> <>7__wrap2;
            public CurveLibraryRecord <clr>5__1;

            [DebuggerHidden]
            public <Read>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap2 = this.<>4__this.ReadRecords().GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap2.MoveNext())
                            {
                                this.<clr>5__1 = this.<>7__wrap2.Current;
                                this.<>2__current = this.<clr>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_0075:
                                this.<>1__state = 1;
                            }
                            this.<>1__state = -1;
                            if (this.<>7__wrap2 != null)
                            {
                                this.<>7__wrap2.Dispose();
                            }
                            break;

                        case 2:
                            goto Label_0075;
                    }
                    return false;
                }
                fault
                {
                    ((IDisposable) this).Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<CurveLibraryRecord> IEnumerable<CurveLibraryRecord>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
                {
                    return this;
                }
                CurveLibrary.<Read>d__0 d__ = new CurveLibrary.<Read>d__0(0);
                d__.<>4__this = this.<>4__this;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Vixen.CurveLibraryRecord>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>1__state = -1;
                            if (this.<>7__wrap2 != null)
                            {
                                this.<>7__wrap2.Dispose();
                            }
                        }
                        break;
                }
            }

            CurveLibraryRecord IEnumerator<CurveLibraryRecord>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReadRecords>d__5 : IEnumerable<CurveLibraryRecord>, IEnumerable, IEnumerator<CurveLibraryRecord>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private CurveLibraryRecord <>2__current;
            public CurveLibrary <>4__this;
            public DataRow[] <>7__wrap8;
            public int <>7__wrap9;
            public DataRow <row>5__7;
            public DataRow[] <rows>5__6;

            [DebuggerHidden]
            public <ReadRecords>d__5(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>4__this.Load(false);
                            if (this.<>4__this.m_dataTable != null)
                            {
                                this.<rows>5__6 = this.<>4__this.m_dataTable.Select(this.<>4__this.BuildWhereClause(), this.<>4__this.BuildSortClause());
                                this.<>1__state = 1;
                                this.<>7__wrap8 = this.<rows>5__6;
                                this.<>7__wrap9 = 0;
                                while (this.<>7__wrap9 < this.<>7__wrap8.Length)
                                {
                                    this.<row>5__7 = this.<>7__wrap8[this.<>7__wrap9];
                                    this.<>2__current = new CurveLibraryRecord(this.<row>5__7["Manufacturer"].ToString(), this.<row>5__7["LightCount"].ToString(), int.Parse(this.<row>5__7["Color"].ToString()), this.<row>5__7["Controller"].ToString(), this.<row>5__7["CurveData"].ToString());
                                    this.<>1__state = 2;
                                    return true;
                                Label_0136:
                                    this.<>1__state = 1;
                                    this.<>7__wrap9++;
                                }
                                this.<>1__state = -1;
                            }
                            break;

                        case 2:
                            goto Label_0136;
                    }
                    return false;
                }
                fault
                {
                    ((IDisposable) this).Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<CurveLibraryRecord> IEnumerable<CurveLibraryRecord>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
                {
                    return this;
                }
                CurveLibrary.<ReadRecords>d__5 d__ = new CurveLibrary.<ReadRecords>d__5(0);
                d__.<>4__this = this.<>4__this;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Vixen.CurveLibraryRecord>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        this.<>1__state = -1;
                        break;
                }
            }

            CurveLibraryRecord IEnumerator<CurveLibraryRecord>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        public class Filter
        {
            public string ColumnName;
            public System.Type ColumnType;
            public Operator ComparisonOperator;
            public Join JoinOperator = Join.And;
            public string Value;

            public Filter(Operator op, string value)
            {
                this.ComparisonOperator = op;
                this.Value = value;
            }

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
        }

        public class Sort
        {
            public string ColumnName;
            public Direction SortDirection;

            public Sort(string columnName, Direction direction)
            {
                this.ColumnName = columnName;
                this.SortDirection = direction;
            }

            public enum Direction
            {
                Asc,
                Desc
            }
        }
    }
}

