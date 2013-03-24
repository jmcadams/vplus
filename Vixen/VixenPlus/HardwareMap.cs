namespace VixenPlus
{
	public class HardwareMap
	{
		public int PortTypeIndex;
		public string PortTypeName;
		public bool Shared;
		public string StringFormat;

		public HardwareMap(string portTypeName, int portTypeIndex)
		{
			PortTypeName = portTypeName;
			PortTypeIndex = portTypeIndex;
			Shared = false;
			StringFormat = "d";
		}

		public HardwareMap(string portTypeName, int portTypeIndex, bool shared)
		{
			PortTypeName = portTypeName;
			PortTypeIndex = portTypeIndex;
			Shared = shared;
			StringFormat = "d";
		}

		public HardwareMap(string portTypeName, int portTypeIndex, string stringFormat)
		{
			PortTypeName = portTypeName;
			PortTypeIndex = portTypeIndex;
			Shared = false;
			StringFormat = stringFormat;
		}

		public HardwareMap(string portTypeName, int portTypeIndex, bool shared, string stringFormat)
		{
			PortTypeName = portTypeName;
			PortTypeIndex = portTypeIndex;
			Shared = shared;
			StringFormat = stringFormat;
		}
	}
}