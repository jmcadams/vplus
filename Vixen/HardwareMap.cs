//todo is this necessary
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
}