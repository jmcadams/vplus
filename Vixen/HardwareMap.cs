public class HardwareMap
{
    public readonly int PortTypeIndex;
    public readonly string PortTypeName;
    public readonly bool Shared;
    public readonly string StringFormat;

    public HardwareMap(string portTypeName, int portTypeIndex)
    {
        PortTypeName = portTypeName;
        PortTypeIndex = portTypeIndex;
        Shared = false;
        StringFormat = "d";
    }

/*
    public HardwareMap(string portTypeName, int portTypeIndex, bool shared)
    {
        PortTypeName = portTypeName;
        PortTypeIndex = portTypeIndex;
        Shared = shared;
        StringFormat = "d";
    }
*/

/*
    public HardwareMap(string portTypeName, int portTypeIndex, string stringFormat)
    {
        PortTypeName = portTypeName;
        PortTypeIndex = portTypeIndex;
        Shared = false;
        StringFormat = stringFormat;
    }
*/

/*
    public HardwareMap(string portTypeName, int portTypeIndex, bool shared, string stringFormat)
    {
        PortTypeName = portTypeName;
        PortTypeIndex = portTypeIndex;
        Shared = shared;
        StringFormat = stringFormat;
    }
*/
}