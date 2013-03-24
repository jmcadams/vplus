namespace K8055
{
    using System;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class K8055Output : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private int m_channelCount = 0;
        private int[] m_deviceStarts = new int[4];
        private int m_offset;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private bool[] m_validDevices = new bool[4];

        public void Event(byte[] channelValues)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.m_validDevices[i])
                {
                    int num = this.m_deviceStarts[i] - this.m_offset;
                    int num2 = Math.Min(num + 8, channelValues.Length);
                    byte num3 = 0;
                    while (num < num2)
                    {
                        num3 = (byte) (num3 >> 1);
                        num3 = (byte) (num3 | ((channelValues[num++] > 0) ? 0x80 : 0));
                    }
                    K8055.Write(i, (long) num3);
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_channelCount = executableObject.Channels.Count;
            this.m_deviceStarts[0] = setupData.GetInteger(setupNode, "Device0", 0);
            this.m_deviceStarts[1] = setupData.GetInteger(setupNode, "Device1", 8);
            this.m_deviceStarts[2] = setupData.GetInteger(setupNode, "Device2", 0x10);
            this.m_deviceStarts[3] = setupData.GetInteger(setupNode, "Device3", 0x18);
            this.m_offset = int.Parse(this.m_setupNode.Attributes["from"].Value) - 1;
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_channelCount, this.m_deviceStarts);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_deviceStarts = dialog.DeviceStartChannels;
                this.m_setupData.SetInteger(this.m_setupNode, "Device0", this.m_deviceStarts[0]);
                this.m_setupData.SetInteger(this.m_setupNode, "Device1", this.m_deviceStarts[1]);
                this.m_setupData.SetInteger(this.m_setupNode, "Device2", this.m_deviceStarts[2]);
                this.m_setupData.SetInteger(this.m_setupNode, "Device3", this.m_deviceStarts[3]);
            }
        }

        public void Shutdown()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.m_validDevices[i])
                {
                    K8055.Close(i);
                }
            }
        }

        public void Startup()
        {
            long num = K8055.SearchDevices();
            for (int i = 0; i < 4; i++)
            {
                this.m_validDevices[i] = (num & (((int) 1) << i)) != 0L;
                if (this.m_validDevices[i])
                {
                    K8055.Open(i);
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Velleman K8055 USB interface board";
            }
        }

        public VixenPlus.HardwareMap[] HardwareMap
        {
            get
            {
                return new VixenPlus.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Velleman K8055";
            }
        }
    }
}

