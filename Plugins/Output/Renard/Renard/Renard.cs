namespace Renard
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Renard : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_channelValues;
        private AutoResetEvent m_eventTrigger;
        private bool m_holdPort;
        private byte[] m_p1Packet = new byte[0x12];
        private byte[] m_p1Zeroes;
        private byte[] m_p2Packet;
        private byte[] m_p2Zeroes;
        private int m_protocolVersion = 1;
        private SerialPort m_selectedPort = null;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private RunState m_state = RunState.Stopped;
        private const int PAD_DISTANCE = 100;

        public Renard()
        {
            this.m_p1Packet[0] = 0x7e;
            this.m_p2Packet = new byte[11];
            this.m_p2Packet[0] = 0;
            this.m_p1Zeroes = new byte[0x10];
            this.m_p2Zeroes = new byte[8];
        }

        public void Event(byte[] channelValues)
        {
            this.m_channelValues = channelValues;
            if (this.m_holdPort)
            {
                this.m_eventTrigger.Set();
            }
            else
            {
                if (!this.m_selectedPort.IsOpen)
                {
                    this.m_selectedPort.Open();
                }
                this.FireEvent();
                this.m_selectedPort.Close();
            }
        }

        private void EventThread()
        {
            this.State = RunState.Running;
            this.m_eventTrigger = new AutoResetEvent(false);
            try
            {
                while (this.State == RunState.Running)
                {
                    this.m_eventTrigger.WaitOne();
                    try
                    {
                        this.FireEvent();
                    }
                    catch (TimeoutException)
                    {
                    }
                }
            }
            catch
            {
                if (this.State == RunState.Running)
                {
                    this.State = RunState.Stopping;
                }
            }
            finally
            {
                this.State = RunState.Stopped;
            }
        }

        private void FireEvent()
        {
            if (this.State == RunState.Running)
            {
                if (this.m_protocolVersion == 1)
                {
                    this.Protocol1Event(this.m_channelValues);
                }
                else if (this.m_protocolVersion == 2)
                {
                    this.Protocol2Event(this.m_channelValues);
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_selectedPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x4b00), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_protocolVersion = this.m_setupData.GetInteger(this.m_setupNode, "ProtocolVersion", 1);
            this.m_holdPort = this.m_setupData.GetBoolean(this.m_setupNode, "HoldPort", true);
            this.m_selectedPort.WriteTimeout = 500;
        }

        private void Protocol1Event(byte[] channelValues)
        {
            int length = channelValues.Length;
            int count = 2;
            int num3 = (2 + (2 * length)) + ((2 + (2 * length)) / 100);
            if (this.m_p1Packet.Length < num3)
            {
                this.m_p1Packet = new byte[num3];
            }
            this.m_p1Packet[0] = 0x7e;
            this.m_p1Packet[1] = 0x80;
            for (int i = 0; i < length; i++)
            {
                if (channelValues[i] == 0x7d)
                {
                    this.m_p1Packet[count++] = 0x7c;
                }
                else if (channelValues[i] == 0x7e)
                {
                    this.m_p1Packet[count++] = 0x7c;
                }
                else if (channelValues[i] == 0x7f)
                {
                    this.m_p1Packet[count++] = 0x80;
                }
                else
                {
                    this.m_p1Packet[count++] = channelValues[i];
                }
                if ((count % 100) == 0)
                {
                    this.m_p1Packet[count++] = 0x7d;
                }
            }
            while ((this.m_selectedPort.WriteBufferSize - this.m_selectedPort.BytesToWrite) <= count)
            {
                Thread.Sleep(5);
            }
            this.m_selectedPort.Write(this.m_p1Packet, 0, count);
        }

        private void Protocol2Event(byte[] channelValues)
        {
            byte num3 = 0x80;
            int length = channelValues.Length;
            byte[] array = new byte[8];
            for (int i = 0; i < length; i += 8)
            {
                int num2 = Math.Min((int) (i + 7), (int) (length - 1));
                num3 = (byte) (num3 + 1);
                this.m_p2Packet[1] = num3;
                if (num2 >= (length - 1))
                {
                    this.m_p2Zeroes.CopyTo(this.m_p2Packet, 3);
                }
                Array.Clear(array, 0, 8);
                int index = i;
                while (index <= num2)
                {
                    byte num8 = channelValues[index];
                    byte num9 = (byte)-num8;
                    if ((num8 >= 1) && (num8 <= 8))
                    {
                        array[num8 - 1] = 1;
                    }
                    else if ((num9 >= 1) && (num9 <= 8))
                    {
                        array[num9 - 1] = 1;
                    }
                    index++;
                }
                byte num7 = (byte) (1 + Array.IndexOf<byte>(array, 0));
                this.m_p2Packet[2] = num7;
                index = i;
                int num5 = 3;
                while (index <= num2)
                {
                    this.m_p2Packet[num5] = (byte) (channelValues[index] - num7);
                    index++;
                    num5++;
                }
                this.m_selectedPort.Write(this.m_p2Packet, 0, num5);
            }
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_selectedPort, this.m_protocolVersion, this.m_holdPort);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_selectedPort = dialog.SelectedPort;
                this.m_setupData.SetString(this.m_setupNode, "name", this.m_selectedPort.PortName);
                this.m_setupData.SetInteger(this.m_setupNode, "baud", this.m_selectedPort.BaudRate);
                this.m_setupData.SetString(this.m_setupNode, "parity", this.m_selectedPort.Parity.ToString());
                this.m_setupData.SetInteger(this.m_setupNode, "data", this.m_selectedPort.DataBits);
                this.m_setupData.SetString(this.m_setupNode, "stop", this.m_selectedPort.StopBits.ToString());
                this.m_protocolVersion = dialog.ProtocolVersion;
                this.m_setupData.SetInteger(this.m_setupNode, "ProtocolVersion", this.m_protocolVersion);
                this.m_holdPort = dialog.HoldPort;
                this.m_setupData.SetBoolean(this.m_setupNode, "HoldPort", this.m_holdPort);
            }
            dialog.Dispose();
        }

        public void Shutdown()
        {
            if (this.State == RunState.Running)
            {
                this.State = RunState.Stopping;
                while (this.State != RunState.Stopped)
                {
                    Thread.Sleep(5);
                }
                if (this.m_selectedPort.IsOpen)
                {
                    this.m_selectedPort.Close();
                }
            }
        }

        public void Startup()
        {
            if (!(!this.m_holdPort || this.m_selectedPort.IsOpen))
            {
                this.m_selectedPort.Open();
            }
            this.m_selectedPort.Handshake = Handshake.None;
            this.m_selectedPort.Encoding = Encoding.UTF8;
            this.m_selectedPort.RtsEnable = true;
            this.m_selectedPort.DtrEnable = true;
            if (this.m_holdPort)
            {
                new Thread(new ThreadStart(this.EventThread)).Start();
                while (this.State != RunState.Running)
                {
                    Thread.Sleep(1);
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
                return "Phil Short";
            }
        }

        public string Description
        {
            get
            {
                return "Phil Short's 'Renard Dimmer' output plugin";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Serial", int.Parse(this.m_selectedPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "Renard Dimmer (modified)";
            }
        }

        private RunState State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                this.m_state = value;
                if (value == RunState.Stopping)
                {
                    this.m_eventTrigger.Set();
                    this.m_eventTrigger.Close();
                    this.m_eventTrigger = null;
                }
            }
        }

        private enum RunState
        {
            Running,
            Stopping,
            Stopped
        }
    }
}

