namespace VixenPlus {
    public class CurveLibraryRecord
    {
        public int Color;
        public string Controller;
        public byte[] CurveData;
        public string LightCount;
        public string Manufacturer;

        public CurveLibraryRecord(string manufacturer, string lightCount, int color, string controller)
        {
            Manufacturer = manufacturer;
            LightCount = lightCount;
            Color = color;
            Controller = controller;
            CurveData = null;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", new object[] {Manufacturer, LightCount, Color, Controller});
        }
    }
}