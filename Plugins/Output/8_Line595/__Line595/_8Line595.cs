namespace __Line595
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class _8Line595 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private int m_channelCount;
        private Array[] m_dataLineValueArrays = new Array[8];
        private byte m_linesUsed = 0;
        private ushort m_portBase = 0;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            ushort port = (ushort) (this.m_portBase + 2);
            int sourceIndex = 0;
            int index = 0;
            while (index < 8)
            {
                if (this.m_dataLineValueArrays[index] != null)
                {
                    Array.Copy(channelValues, sourceIndex, this.m_dataLineValueArrays[index], 0, this.m_channelCount);
                    Array.Reverse(this.m_dataLineValueArrays[index]);
                    sourceIndex += this.m_channelCount;
                }
                index++;
            }
            for (int i = 0; i < this.m_channelCount; i++)
            {
                ushort data = 0;
                for (index = 0; index < 8; index++)
                {
                    if (this.m_dataLineValueArrays[index] != null)
                    {
                        data = (ushort) (data | ((ushort) (((((int[]) this.m_dataLineValueArrays[index])[i] > 0) ? 1 : 0) << index)));
                    }
                }
                Out(this.m_portBase, data);
                Out(port, 2);
                Out(port, 3);
            }
            Out(port, 1);
            Out(port, 3);
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portBase = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "Port", 0x378);
            this.m_channelCount = this.m_setupData.GetInteger(this.m_setupNode, "ChannelsPerController", 0);
            this.m_linesUsed = (byte) this.m_setupData.GetInteger(this.m_setupNode, "LinesUsed", 0);
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, ushort data);
        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_channelCount, this.m_portBase, this.m_linesUsed);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_portBase = (ushort) dialog.PortAddress;
                this.m_channelCount = dialog.ChannelCount;
                this.m_linesUsed = dialog.LinesUsed;
                this.m_setupData.SetInteger(this.m_setupNode, "Port", this.m_portBase);
                this.m_setupData.SetInteger(this.m_setupNode, "ChannelsPerController", this.m_channelCount);
                this.m_setupData.SetInteger(this.m_setupNode, "LinesUsed", this.m_linesUsed);
            }
        }

        public void Shutdown()
        {
        }

        public void Startup()
        {
            for (int i = 0; i < 8; i++)
            {
                if ((this.m_linesUsed & (((int) 1) << i)) != 0)
                {
                    this.m_dataLineValueArrays[i] = new int[this.m_channelCount];
                }
                else
                {
                    this.m_dataLineValueArrays[i] = null;
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "8 data line, single parallel port plugin for Vixen";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Parallel", this.m_portBase, "X") };
            }
        }

        public string Name
        {
            get
            {
                return "8-line 595";
            }
        }
    }
}

