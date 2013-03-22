namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class CurveLibraryRecord
    {
        public int Color;
        public string Controller;
        private const char CURVE_DATA_DELIM = '|';
        public byte[] CurveData;
        private const char FIELD_DELIM = ',';
        public string LightCount;
        public string Manufacturer;

        public CurveLibraryRecord(string text)
        {
            string[] strArray = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length == 3)
            {
                this.Manufacturer = strArray[0];
                this.LightCount = strArray[1];
                this.Color = System.Drawing.Color.White.ToArgb();
                this.Controller = strArray[3];
                this.CurveData = null;
            }
            else if (strArray.Length == 4)
            {
                this.Manufacturer = strArray[0];
                this.LightCount = strArray[1];
                if (!int.TryParse(strArray[2], out this.Color))
                {
                    this.Color = System.Drawing.Color.White.ToArgb();
                    this.Controller = strArray[2];
                    this.CurveData = this.BreakCurveDataString(strArray[3]);
                }
                else if ((this.CurveData = this.BreakCurveDataString(strArray[3])).Length == 0x100)
                {
                    this.Controller = "";
                }
                else
                {
                    this.Controller = strArray[3];
                }
            }
            else
            {
                if (strArray.Length != 5)
                {
                    throw new Exception("Curve record is incorrectly formatted.");
                }
                this.Manufacturer = strArray[0];
                this.LightCount = strArray[1];
                int.TryParse(strArray[2], out this.Color);
                this.Controller = strArray[3];
                this.CurveData = this.BreakCurveDataString(strArray[4]);
            }
        }

        public CurveLibraryRecord(string manufacturer, string lightCount, string controller) : this(manufacturer, lightCount, System.Drawing.Color.White.ToArgb(), controller)
        {
        }

        public CurveLibraryRecord(string manufacturer, string lightCount, int color, string controller)
        {
            this.Manufacturer = manufacturer;
            this.LightCount = lightCount;
            this.Color = color;
            this.Controller = controller;
            this.CurveData = null;
        }

        public CurveLibraryRecord(string manufacturer, string lightCount, int color, string controller, string curveDataString) : this(manufacturer, lightCount, color, controller)
        {
            this.CurveData = this.BreakCurveDataString(curveDataString);
        }

        private byte[] BreakCurveDataString(string text)
        {
            List<byte> dataBytes = new List<byte>();
            Array.ForEach<string>(text.Split(new char[] { '|' }), delegate (string s) {
                dataBytes.Add(byte.Parse(s));
            });
            return dataBytes.ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", new object[] { this.Manufacturer, this.LightCount, this.Color, this.Controller });
        }
    }
}

