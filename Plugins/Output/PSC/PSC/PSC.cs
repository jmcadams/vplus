namespace PSC
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class PSC : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_channelValues;
        private List<Form> m_dialogList = new List<Form>();
        private int m_eventPeriod;
        private Thread m_eventThread;
        private AutoResetEvent m_eventTrigger;
        private const float m_fastestRampTime = 7.5f;
        private const int m_maxRampValue = 0x3f;
        private const double m_millisecondsPerPercent = 9.2578125;
        private byte[] m_packet = new byte[] { 0x21, 0x53, 0x43, 0, 0, 0, 0, 13 };
        private const int m_readTimeout = 500;
        private byte[] m_recvPacket;
        private SerialPort m_serialPort = null;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private byte[] m_shadow = null;
        private const float m_slowestRampTime = 600f;
        private bool m_useRamps = false;
        private RunState State = RunState.Stopped;

        public PSC()
        {
            this.m_recvPacket = new byte[this.m_packet.Length];
            this.m_eventThread = new Thread(new ThreadStart(this.EventThread));
            this.m_eventTrigger = new AutoResetEvent(false);
        }

        private bool DeterminePSCBaudRate()
        {
            if (this.m_serialPort.BaudRate != 0x960)
            {
                this.SetBaudRate(0x960);
            }
            if (this.Ping())
            {
                return true;
            }
            this.SetBaudRate(0x9600);
            return this.Ping();
        }

        public void Event(byte[] channelValues)
        {
            this.m_channelValues = channelValues;
            this.m_eventTrigger.Set();
        }

        private void EventThread()
        {
            this.State = RunState.Running;
            while (this.State == RunState.Running)
            {
                this.m_eventTrigger.WaitOne();
                if (this.State == RunState.Running)
                {
                    this.FireEvent();
                }
            }
            this.State = RunState.Stopped;
        }

        private void FireEvent()
        {
            if (this.m_useRamps && (this.m_shadow == null))
            {
                this.m_shadow = new byte[this.m_channelValues.Length];
            }
            int num = Math.Min(0x20, this.m_channelValues.Length);
            for (byte i = 0; i < num; i = (byte) (i + 1))
            {
                int num3 = (int) Math.Round((double) ((this.m_channelValues[i] * 100f) / 255f), MidpointRounding.AwayFromZero);
                ushort num2 = (ushort) ((num3 * 10) + 250);
                this.m_packet[3] = i;
                this.m_packet[4] = 0;
                if (this.m_useRamps)
                {
                    int num5 = (this.m_shadow[i] * 100) / 0xff;
                    int num6 = (this.m_channelValues[i] * 100) / 0xff;
                    int num7 = Math.Abs((int) (num5 - num6));
                    int num8 = (int) Math.Round((double) (((double) (this.m_eventPeriod - (7.5f * num7))) / 9.2578125), MidpointRounding.AwayFromZero);
                    if ((num8 >= 1) && (num8 <= 0x3f))
                    {
                        this.m_packet[4] = (byte) num8;
                    }
                }
                this.m_packet[5] = (byte) num2;
                this.m_packet[6] = (byte) (num2 >> 8);
                this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
                this.m_serialPort.DiscardInBuffer();
                if (this.m_useRamps)
                {
                    this.m_channelValues.CopyTo(this.m_shadow, 0);
                }
                Thread.Sleep(10);
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            if (this.m_serialPort == null)
            {
                this.SetPort();
            }
            this.m_useRamps = false;
            if (executableObject is EventSequence)
            {
                this.m_eventPeriod = ((EventSequence) executableObject).EventPeriod;
                this.m_useRamps = this.m_setupData.GetBoolean(this.m_setupNode, "Ramps", false);
            }
        }

        private void Pause(double Second)
        {
            long num2;
            long num3 = (long) (Second * 10000000.0);
            long ticks = DateTime.Now.Ticks;
            do
            {
                num2 = DateTime.Now.Ticks;
            }
            while ((num2 - ticks) < num3);
        }

        private bool Ping()
        {
            byte[] commandPacket = new byte[] { 0x21, 0x53, 0x43, 0x56, 0x45, 0x52, 0x3f, 13 };
            byte[] buffer2 = this.PSCCommandAndResponse(commandPacket);
            return ((char.IsDigit((char) buffer2[commandPacket.Length]) && (buffer2[commandPacket.Length + 1] == 0x2e)) && char.IsDigit((char) buffer2[commandPacket.Length + 2]));
        }

        private void PSCCommand(byte[] commandPacket)
        {
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            this.m_serialPort.Write(commandPacket, 0, commandPacket.Length);
        }

        private byte[] PSCCommandAndResponse(byte[] commandPacket)
        {
            bool isOpen = this.m_serialPort.IsOpen;
            byte[] buffer = new byte[commandPacket.Length + 3];
            this.m_serialPort.ReadTimeout = 500;
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            this.m_serialPort.Write(commandPacket, 0, commandPacket.Length);
            this.Pause(0.4);
            try
            {
                int num = 0;
                while (num < buffer.Length)
                {
                    buffer[num++] = (byte) this.m_serialPort.ReadByte();
                }
            }
            catch (TimeoutException)
            {
            }
            catch (Exception exception)
            {
                this.m_serialPort.Close();
                throw new Exception("PSCCommandAndResponse:\n" + exception.Message);
            }
            if (this.m_serialPort.IsOpen != isOpen)
            {
                if (isOpen)
                {
                    this.m_serialPort.Open();
                    return buffer;
                }
                this.m_serialPort.Close();
            }
            return buffer;
        }

        private void SetBaudRate(int baudRate)
        {
            bool isOpen = this.m_serialPort.IsOpen;
            if (isOpen)
            {
                this.m_serialPort.Close();
            }
            this.m_serialPort.BaudRate = baudRate;
            if (isOpen)
            {
                this.Pause(0.1);
                this.m_serialPort.Open();
            }
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "Name", "COM1"), 0x960, Parity.None, 8, StopBits.Two);
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
        }

        private bool SetPSCBaudRate(int targetRate)
        {
            bool flag;
            int baudRate = this.m_serialPort.BaudRate;
            byte num2 = (targetRate == 0x960) ? ((byte) 0) : ((byte) 1);
            byte[] buffer3 = new byte[] { 0x21, 0x53, 0x43, 0x53, 0x42, 0x52, 0, 13 };
            buffer3[6] = num2;
            byte[] commandPacket = buffer3;
            try
            {
                if (!this.m_serialPort.IsOpen)
                {
                    this.m_serialPort.Open();
                }
                this.m_serialPort.DiscardInBuffer();
                this.PSCCommand(commandPacket);
                byte[] buffer = new byte[8];
                this.m_serialPort.Read(buffer, 0, 8);
                this.SetBaudRate(targetRate);
                this.m_serialPort.DiscardInBuffer();
                flag = true;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("SetPSCBaudRate:\n{0}", exception.Message));
            }
            return flag;
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_setupNode, this.m_serialPort);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_serialPort = dialog.SelectedPort;
                this.m_useRamps = dialog.Ramps;
            }
        }

        public void Shutdown()
        {
            if (this.State == RunState.Running)
            {
                this.State = RunState.Stopping;
                this.m_eventTrigger.Set();
                while (this.State != RunState.Stopped)
                {
                    Thread.Sleep(1);
                }
            }
            this.m_serialPort.Close();
            Thread.Sleep(10);
            this.m_shadow = null;
        }

        public void Startup()
        {
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            if (!this.DeterminePSCBaudRate())
            {
                throw new Exception("Could not determine the baud rate for the PSC unit.");
            }
            if (this.m_serialPort.BaudRate != 0x9600)
            {
                this.SetPSCBaudRate(0x9600);
            }
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            this.m_eventThread.Start();
        }

        public override string ToString()
        {
            return this.Name;
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
                return "Plugin for the Parallax Servo Controller.";
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
                return "Parallax PSC";
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

