namespace EZ_8_Transfer
{
    using EZ_8;
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class EZ8 : IAddIn, ILoadable, IPlugIn
    {
        private XmlNode m_dataNode;
        private EZ_8 m_hardware;
        private string m_portName = null;
        private EventSequence m_sequence;
        private EZ8_UI m_ui = null;

        public bool Execute(EventSequence sequence)
        {
            bool flag = false;
            this.m_sequence = sequence;
            if (this.m_sequence == null)
            {
                throw new Exception("A sequence is required.");
            }
            if (this.m_sequence.ChannelCount < 8)
            {
                throw new Exception("The EZ-8 requires 8 channels of data in the sequence.");
            }
            if (this.m_sequence.ChannelCount > 9)
            {
                MessageBox.Show("The EZ-8 only has 8 channels of data.  Any channels beyond that will not be affected.", "EZ-8", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (this.m_sequence.EventPeriod != 0x21)
            {
                this.m_sequence.EventPeriod = 0x21;
                flag = true;
                MessageBox.Show("The event period of the sequence has been reset to 33 ms.", "EZ-8", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            this.m_ui.Sequence = this.m_sequence;
            this.m_ui.ShowDialog();
            this.m_portName = this.m_hardware.PortName;
            return (flag || this.m_ui.SequenceModified);
        }

        public void Loading(XmlNode dataNode)
        {
            this.m_dataNode = dataNode;
            XmlNode node = this.m_dataNode["Port"];
            if (node != null)
            {
                this.m_portName = node.InnerText;
            }
            List<string> list = new List<string>(SerialPort.GetPortNames());
            if (!list.Contains(this.m_portName))
            {
                this.m_portName = null;
            }
            this.m_hardware = new EZ_8();
            this.m_hardware.PortName = this.m_portName;
            this.m_ui = new EZ8_UI(this.m_hardware);
        }

        public void Unloading()
        {
            if (this.m_portName != null)
            {
                Xml.SetValue(this.m_dataNode, "Port", this.m_portName);
            }
            this.m_ui.Dispose();
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Application;
            }
        }

        public string Description
        {
            get
            {
                return "Upload/download utility for the EFX-TEK EZ-8";
            }
        }

        public string Name
        {
            get
            {
                return "EFX-TEK EZ-8 Upload/Download";
            }
        }
    }
}

