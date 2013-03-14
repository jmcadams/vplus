namespace Vixen
{
    using System;

    public class HardwareMap
    {
        public int PortTypeIndex;
        public string PortTypeName;
        public bool Shared;
        public string StringFormat;

        public HardwareMap(string portTypeName, int portTypeIndex)
        {
            this.PortTypeName = portTypeName;
            this.PortTypeIndex = portTypeIndex;
            this.Shared = false;
            this.StringFormat = "d";
        }

        public HardwareMap(string portTypeName, int portTypeIndex, bool shared)
        {
            this.PortTypeName = portTypeName;
            this.PortTypeIndex = portTypeIndex;
            this.Shared = shared;
            this.StringFormat = "d";
        }

        public HardwareMap(string portTypeName, int portTypeIndex, string stringFormat)
        {
            this.PortTypeName = portTypeName;
            this.PortTypeIndex = portTypeIndex;
            this.Shared = false;
            this.StringFormat = stringFormat;
        }

        public HardwareMap(string portTypeName, int portTypeIndex, bool shared, string stringFormat)
        {
            this.PortTypeName = portTypeName;
            this.PortTypeIndex = portTypeIndex;
            this.Shared = shared;
            this.StringFormat = stringFormat;
        }
    }
}

