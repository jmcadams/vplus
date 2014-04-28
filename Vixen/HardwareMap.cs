namespace VixenPlus {
    /// <summary>
    /// Holds the information about the hardware configuration to display on the listviewoutput on the plugin selection screen.
    /// </summary>
    public class HardwareMap
    {
        public readonly int PortTypeIndex;
        public readonly string PortTypeName;

        public HardwareMap(string portTypeName, int portTypeIndex)
        {
            PortTypeName = portTypeName;
            PortTypeIndex = portTypeIndex;
        }
    }
}

//todo this can be refactored out when all of the plugins are converted.