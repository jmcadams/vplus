namespace FC4_RC4
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class FC4_RC4 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private List<IModule> m_modules = new List<IModule>();
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            Thread.BeginCriticalRegion();
            foreach (IModule module in this.m_modules)
            {
                module.OutputEvent(this.m_serialPort, channelValues);
            }
            Thread.EndCriticalRegion();
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
        }

        public void Setup()
        {
            frmSetupDialog dialog = new frmSetupDialog(this.m_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetPort();
            }
        }

        public void Shutdown()
        {
            if (this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
        }

        public void Startup()
        {
            if (this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
            byte threshold = 50;
            string innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
            if (innerText.Length > 0)
            {
                threshold = (byte) Math.Round((double) (((float) (int.Parse(innerText) * 0xff)) / 100f), MidpointRounding.AwayFromZero);
            }
            this.m_modules.Clear();
            for (int i = 0; i < 8; i++)
            {
                string str2 = Xml.GetNodeAlways(this.m_setupNode, "Addr" + i.ToString()).InnerText;
                byte address = (str2.Length > 0) ? byte.Parse(str2.Substring(str2.Length - 2, 1)) : ((byte) 0);
                string str3 = Xml.GetNodeAlways(this.m_setupNode, "Type" + i.ToString()).InnerText.Substring(0, 2);
                if (str3 != null)
                {
                    if (!(str3 == "FC"))
                    {
                        if (str3 == "RC")
                        {
                            goto Label_0128;
                        }
                    }
                    else
                    {
                        this.m_modules.Add(new FC4(address, i * 4));
                    }
                }
                continue;
            Label_0128:
                this.m_modules.Add(new RC4(address, i * 4, threshold));
            }
            this.m_serialPort.Open();
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
                return "Unified plugin for the EFX-TEK RC-4/FC-4 modules";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Serial", int.Parse(this.m_serialPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "EFX-TEK RC-4/FC-4";
            }
        }
    }
}

