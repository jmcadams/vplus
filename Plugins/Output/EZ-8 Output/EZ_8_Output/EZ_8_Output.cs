namespace EZ_8_Output
{
    using EZ_8;
    using System;
    using System.IO.Ports;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class EZ_8_Output : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private XmlNode m_dataNode;
        private EZ_8 m_hardware = null;

        public void Event(byte[] channelValues)
        {
            byte outputState = 0;
            for (int i = Math.Min(8, channelValues.Length) - 1; i >= 0; i--)
            {
                outputState = (byte) (outputState << 1);
                outputState = (byte) (outputState | ((channelValues[i] > 0x80) ? 1 : 0));
            }
            this.m_hardware.SetOutputs(outputState);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_dataNode = setupNode;
            if (this.m_hardware == null)
            {
                this.m_hardware = new EZ_8();
            }
            string innerText = Xml.GetNodeAlways(this.m_dataNode, "Port").InnerText;
            this.m_hardware.PortName = (innerText.Length != 0) ? innerText : null;
        }

        public void Setup()
        {
            SerialSetupDialog dialog = new SerialSetupDialog(new SerialPort((this.m_hardware.PortName == null) ? SerialPort.GetPortNames()[0] : this.m_hardware.PortName, 0x9600), true, false, false, false, false);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Xml.SetValue(this.m_dataNode, "Port", dialog.SelectedPort.PortName);
            }
        }

        public void Shutdown()
        {
            this.m_hardware.Close();
        }

        public void Startup()
        {
            this.m_hardware.Open();
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public string Description
        {
            get
            {
                return "Output plugin for the EFX-TEK EZ-8 bit banger.";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                if (this.m_hardware.PortName != null)
                {
                    return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Serial", int.Parse(this.m_hardware.PortName.Substring(3))) };
                }
                return new Vixen.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "EFX-TEK EZ-8";
            }
        }
    }
}

