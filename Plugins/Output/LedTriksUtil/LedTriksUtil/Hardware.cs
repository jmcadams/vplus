namespace LedTriksUtil
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Hardware : IFrameOutput
    {
        private const short CLOCK = 1;
        private const short COL_STROBE = 2;
        private ParsedFrame m_blankFrame = null;
        private int m_boardLayoutHeight;
        private int m_boardLayoutWidth;
        private ushort m_controlPort;
        private ushort m_dataPort;
        private ParsedFrame m_frameBuffer = null;
        private Thread m_maintenanceThread = null;
        private ushort m_portAddress = 0;
        private bool m_running = false;
        private const short PD = 4;
        private const short ROW_STROBE = 8;

        public Hardware(ushort portBaseAddress, int boardLayoutWidth, int boardLayoutHeight)
        {
            this.PortBaseAddress = portBaseAddress;
            this.m_boardLayoutWidth = boardLayoutWidth;
            this.m_boardLayoutHeight = boardLayoutHeight;
            this.m_blankFrame = new ParsedFrame(1, this.m_boardLayoutWidth, this.m_boardLayoutHeight);
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        private void MaintenanceThread()
        {
            ParsedFrame frameBuffer = null;
            this.m_running = true;
            while (this.m_running)
            {
                if (this.m_frameBuffer != null)
                {
                    frameBuffer = this.m_frameBuffer;
                    this.m_frameBuffer = null;
                }
                if (frameBuffer != null)
                {
                    this.OutputFrame(frameBuffer);
                }
            }
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, short data);
        private void OutputFrame(ParsedFrame frame)
        {
            for (short i = 0; i < 0x10; i = (short) (i + 1))
            {
                for (short j = 0; j < 0x30; j = (short) (j + 1))
                {
                    Out(this.m_dataPort, frame.CellData(i, j));
                    Out(this.m_controlPort, 10);
                    Out(this.m_controlPort, 11);
                }
                Out(this.m_controlPort, 12);
                Out(this.m_dataPort, i);
                Out(this.m_controlPort, 6);
                Out(this.m_controlPort, 11);
            }
        }

        public void Start()
        {
            if (this.m_portAddress == 0)
            {
                throw new Exception("No port has been specified for LedTriks hardware.");
            }
            this.StopThread();
            this.m_maintenanceThread = new Thread(new ThreadStart(this.MaintenanceThread));
            this.m_maintenanceThread.Start();
        }

        public void Stop()
        {
            this.StopThread();
            if (this.m_blankFrame != null)
            {
                this.OutputFrame(this.m_blankFrame);
            }
            Out(this.m_controlPort, 14);
        }

        private void StopThread()
        {
            this.m_running = false;
            if ((this.m_maintenanceThread != null) && this.m_maintenanceThread.IsAlive)
            {
                this.m_maintenanceThread.Join();
                this.m_maintenanceThread = null;
            }
        }

        public void UpdateFrame(Frame frame)
        {
            this.m_frameBuffer = ParsedFrame.ParseFrame(frame, this.m_boardLayoutWidth, this.m_boardLayoutHeight);
        }

        public ushort PortBaseAddress
        {
            set
            {
                this.m_portAddress = value;
                this.m_dataPort = this.m_portAddress;
                this.m_controlPort = (ushort) (this.m_portAddress + 2);
            }
        }
    }
}

