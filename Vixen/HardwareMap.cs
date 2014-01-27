/// <summary>
/// Holds the information about the hardware configuration to display on the listviewoutput on the plugin selection screen.
/// </summary>
public class HardwareMap
{
    public readonly int PortTypeIndex;
    public readonly string PortTypeName;
    public readonly string StringFormat;

    public HardwareMap(string portTypeName, int portTypeIndex)
    {
        PortTypeName = portTypeName;
        PortTypeIndex = portTypeIndex;
        StringFormat = "d";
    }
}