using System;
using System.Collections.Generic;

namespace Vixen
{
	public class CurveLibraryRecord
	{
		private const char CURVE_DATA_DELIM = '|';
		private const char FIELD_DELIM = ',';
		public int Color;
		public string Controller;
		public byte[] CurveData;
		public string LightCount;
		public string Manufacturer;

		public CurveLibraryRecord(string text)
		{
			string[] strArray = text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			if (strArray.Length == 3)
			{
				Manufacturer = strArray[0];
				LightCount = strArray[1];
				Color = System.Drawing.Color.White.ToArgb();
				Controller = strArray[3];
				CurveData = null;
			}
			else if (strArray.Length == 4)
			{
				Manufacturer = strArray[0];
				LightCount = strArray[1];
				if (!int.TryParse(strArray[2], out Color))
				{
					Color = System.Drawing.Color.White.ToArgb();
					Controller = strArray[2];
					CurveData = BreakCurveDataString(strArray[3]);
				}
				else if ((CurveData = BreakCurveDataString(strArray[3])).Length == 0x100)
				{
					Controller = "";
				}
				else
				{
					Controller = strArray[3];
				}
			}
			else
			{
				if (strArray.Length != 5)
				{
					throw new Exception("Curve record is incorrectly formatted.");
				}
				Manufacturer = strArray[0];
				LightCount = strArray[1];
				int.TryParse(strArray[2], out Color);
				Controller = strArray[3];
				CurveData = BreakCurveDataString(strArray[4]);
			}
		}

		public CurveLibraryRecord(string manufacturer, string lightCount, string controller)
			: this(manufacturer, lightCount, System.Drawing.Color.White.ToArgb(), controller)
		{
		}

		public CurveLibraryRecord(string manufacturer, string lightCount, int color, string controller)
		{
			Manufacturer = manufacturer;
			LightCount = lightCount;
			Color = color;
			Controller = controller;
			CurveData = null;
		}

		public CurveLibraryRecord(string manufacturer, string lightCount, int color, string controller, string curveDataString)
			: this(manufacturer, lightCount, color, controller)
		{
			CurveData = BreakCurveDataString(curveDataString);
		}

		private byte[] BreakCurveDataString(string text)
		{
			var dataBytes = new List<byte>();
			Array.ForEach(text.Split(new[] {'|'}), delegate(string s) { dataBytes.Add(byte.Parse(s)); });
			return dataBytes.ToArray();
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[] {Manufacturer, LightCount, Color, Controller});
		}
	}
}