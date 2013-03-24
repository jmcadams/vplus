using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Vixen
{
	public class CurveLibrary : IDisposable
	{
		public const string COL_COLOR = "Color";
		public const string COL_CONTROLLER = "Controller";
		public const string COL_CURVE_DATA = "CurveData";
		public const string COL_LIGHT_COUNT = "LightCount";
		public const string COL_MANUFACTURER = "Manufacturer";
		public const string CURVE_LIBRARY_FILE = "library.xml";
		private readonly string m_localFilePath;
		private string StringDelimiter;
		private Filter[] m_colorFilters;
		private Filter[] m_controllerFilters;
		private DataTable m_dataTable;
		private Filter[] m_lightCountFilters;
		private Filter[] m_manufacturerFilters;
		private bool m_modified;
		private Sort m_sort;

		public CurveLibrary() : this(Path.Combine(Paths.CurveLibraryPath, "library.xml"))
		{
		}

		public CurveLibrary(string filePath)
		{
			m_dataTable = null;
			m_manufacturerFilters = null;
			m_lightCountFilters = null;
			m_colorFilters = null;
			m_controllerFilters = null;
			m_sort = null;
			m_modified = false;
			StringDelimiter = "\"";
			m_localFilePath = filePath;
		}

		public Filter[] ColorFilter
		{
			get { return m_colorFilters; }
			set
			{
				m_colorFilters = value;
				if (value != null)
				{
					foreach (Filter filter in value)
					{
						filter.ColumnName = "Color";
						filter.ColumnType = typeof (string);
					}
				}
			}
		}

		public Filter[] ControllerFilter
		{
			get { return m_controllerFilters; }
			set
			{
				m_controllerFilters = value;
				if (value != null)
				{
					foreach (Filter filter in value)
					{
						filter.ColumnName = "Controller";
						filter.ColumnType = typeof (string);
					}
				}
			}
		}

		public string FilePath
		{
			get { return m_localFilePath; }
		}

		public bool IsFiltered
		{
			get
			{
				return (((((m_manufacturerFilters != null) && (m_manufacturerFilters.Length > 0)) ||
				          ((m_lightCountFilters != null) && (m_lightCountFilters.Length > 0))) ||
				         ((m_colorFilters != null) && (m_colorFilters.Length > 0))) ||
				        ((m_controllerFilters != null) && (m_controllerFilters.Length > 0)));
			}
		}

		public Filter[] LightCountFilter
		{
			get { return m_lightCountFilters; }
			set
			{
				m_lightCountFilters = value;
				if (value != null)
				{
					foreach (Filter filter in value)
					{
						filter.ColumnName = "LightCount";
						filter.ColumnType = typeof (string);
					}
				}
			}
		}

		public Filter[] ManufacturerFilter
		{
			get { return m_manufacturerFilters; }
			set
			{
				m_manufacturerFilters = value;
				if (value != null)
				{
					foreach (Filter filter in value)
					{
						filter.ColumnName = "Manufacturer";
						filter.ColumnType = typeof (string);
					}
				}
			}
		}

		public Sort SortOrder
		{
			get { return m_sort; }
			set { m_sort = value; }
		}

		public void Dispose()
		{
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
			if (m_sort != null)
			{
				return string.Format("{0} {1}", m_sort.ColumnName, m_sort.SortDirection);
			}
			return "";
		}

		private string BuildWhereClause()
		{
			var builder = new StringBuilder();
			builder.Append(BuildWhereFragment(m_manufacturerFilters));
			builder.Append(BuildWhereFragment(m_lightCountFilters));
			builder.Append(BuildWhereFragment(m_colorFilters));
			builder.Append(BuildWhereFragment(m_controllerFilters));
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
			var sb = new StringBuilder();
			if ((filters != null) && (filters.Length > 0))
			{
				StringDelimiter = "'";
				if (action == null)
				{
					action =
						delegate(Filter f)
							{
								sb.AppendFormat(" {0} {1} {2} {3}",
								                new object[]
									                {
										                f.JoinOperator, f.ColumnName, OperatorString(f.ComparisonOperator),
										                FormatValue(f.Value, f.ColumnType)
									                });
							};
				}
				Array.ForEach(filters, action);
			}
			return sb.ToString();
		}

		public CurveLibraryRecord Find(string manufacturer, string lightCount, int color, string controller)
		{
			Load(false);
			if (m_dataTable != null)
			{
				DataRow[] rowArray = m_dataTable.Select(GetSelectString(manufacturer, lightCount, color, controller));
				if (rowArray.Length == 0)
				{
					return null;
				}
				DataRow row = rowArray[0];
				return new CurveLibraryRecord(row["Manufacturer"].ToString(), row["LightCount"].ToString(), (int) row["Color"],
				                              row["Controller"].ToString(), row["CurveData"].ToString());
			}
			return null;
		}

		private string FormatValue(string value, Type valueType)
		{
			string name = valueType.Name;
			if (((name == null) || (name == "String")) || (name != "Int32"))
			{
				return string.Format("{0}{1}{2}", StringDelimiter, value, StringDelimiter);
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
			if (m_dataTable != null)
			{
				DataRow[] rowArray = m_dataTable.Select(BuildWhereClause(), BuildSortClause());
				foreach (DataRow row in rowArray)
				{
					list.Add(row[columnName].ToString());
				}
			}
			return list.ToArray();
		}

		public int GetRecordCount()
		{
			Load(false);
			if (m_dataTable != null)
			{
				return m_dataTable.Rows.Count;
			}
			return -1;
		}

		private string GetSelectString(string manufacturer, string lightCount, int color, string controller)
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
			if (m_dataTable != null)
			{
				DataRow[] rowArray = m_dataTable.Select(GetSelectString(clr.Manufacturer, clr.LightCount, clr.Color, clr.Controller));
				if ((rowArray.Length == 0) || updateIfPresent)
				{
					var curveDataStrings = new List<string>();
					Array.ForEach(clr.CurveData, delegate(byte b) { curveDataStrings.Add(b.ToString()); });
					if (rowArray.Length == 0)
					{
						DataRow row = m_dataTable.NewRow();
						row["Manufacturer"] = clr.Manufacturer;
						row["LightCount"] = clr.LightCount;
						row["Color"] = clr.Color;
						row["Controller"] = clr.Controller;
						row["CurveData"] = string.Join("|", curveDataStrings.ToArray());
						m_dataTable.Rows.Add(row);
					}
					else
					{
						rowArray[0]["CurveData"] = string.Join("|", curveDataStrings.ToArray());
					}
					m_modified = true;
				}
			}
		}

		public void Load()
		{
			Load(false);
		}

		public void Load(bool forcedLoad)
		{
			if (forcedLoad || (m_dataTable == null))
			{
				if (!File.Exists(m_localFilePath))
				{
					Save(true);
				}
				try
				{
					if (m_dataTable != null)
					{
						m_dataTable.Dispose();
						m_dataTable = null;
					}
					m_dataTable = new DataTable();
					m_dataTable.ReadXml(m_localFilePath);
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
			if (force || m_modified)
			{
				if (m_dataTable != null)
				{
					m_dataTable.WriteXml(m_localFilePath, XmlWriteMode.WriteSchema);
				}
				else
				{
					m_dataTable = new DataTable("CurveLibrary");
					m_dataTable.Columns.Add("Manufacturer").AllowDBNull = false;
					m_dataTable.Columns.Add("LightCount").AllowDBNull = false;
					m_dataTable.Columns.Add("Color").AllowDBNull = false;
					m_dataTable.Columns.Add("Controller").AllowDBNull = false;
					m_dataTable.Columns.Add("CurveData").AllowDBNull = false;
					m_dataTable.WriteXml(m_localFilePath, XmlWriteMode.WriteSchema);
				}
				m_modified = false;
			}
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
		//                    if (this.<>4__this.m_dataTable != null)
		//                    {
		//                        this.<rows>5__6 = this.<>4__this.m_dataTable.Select(this.<>4__this.BuildWhereClause(), this.<>4__this.BuildSortClause());
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