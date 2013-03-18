namespace IDMX
{
    using System;
    using System.Threading;

    public class IDMX
    {
        private static byte[] m_data = null;
        private static IntPtr m_FTD2XXHandle = IntPtr.Zero;
        private static object m_lockObject = new object();
        private static int m_refCount = 0;
        private static ThreadState m_running = ThreadState.Stopped;
        private static Thread m_thread;

        public void Close()
        {
            lock (m_lockObject)
            {
                if ((m_refCount > 0) && (--m_refCount == 0))
                {
                    if (this.Running == ThreadState.Running)
                    {
                        this.Running = ThreadState.StopRequested;
                        while (this.Running != ThreadState.Stopped)
                        {
                            Thread.Sleep(1);
                        }
                    }
                    m_thread = null;
                    if (m_FTD2XXHandle != IntPtr.Zero)
                    {
                        FTD2XX.FT_Close(m_FTD2XXHandle);
                        m_FTD2XXHandle = IntPtr.Zero;
                    }
                    m_data = null;
                }
            }
        }

        public void Init()
        {
            lock (m_lockObject)
            {
                if (((++m_refCount == 1) && (this.Running != ThreadState.Running)) && (m_thread == null))
                {
                    try
                    {
                        if (m_FTD2XXHandle == IntPtr.Zero)
                        {
                            m_data = new byte[0x201];
                            if (FTD2XX.FT_Open(0, ref m_FTD2XXHandle) != FTD2XX.FT_STATUS.FT_OK)
                            {
                                throw new Exception("Error opening FTD2XX device 0");
                            }
                            FTD2XX.FT_ResetDevice(m_FTD2XXHandle);
                            FTD2XX.FT_SetTimeouts(m_FTD2XXHandle, 0x10, 50);
                            FTD2XX.FT_SetBaudRate(m_FTD2XXHandle, 0x3d090);
                            FTD2XX.FT_SetDataCharacteristics(m_FTD2XXHandle, 8, 2, 0);
                            FTD2XX.FT_SetFlowControl(m_FTD2XXHandle, 0, 0, 0);
                        }
                        m_thread = new Thread(new ThreadStart(this.SendDataThread));
                        m_thread.Start();
                    }
                    catch
                    {
                        this.Close();
                        throw;
                    }
                }
            }
        }

        public void SendData(byte[] values)
        {
            if (m_data != null)
            {
                if (values.Length > 0x200)
                {
                    Array.Resize<byte>(ref values, 0x200);
                }
                lock (m_data)
                {
                    values.CopyTo(m_data, 1);
                }
            }
        }

        private void SendDataThread()
        {
            uint bytesWritten = 0;
            this.Running = ThreadState.Running;
            while (this.Running == ThreadState.Running)
            {
                FTD2XX.FT_SetBreakOn(m_FTD2XXHandle);
                Thread.Sleep(1);
                FTD2XX.FT_SetBreakOff(m_FTD2XXHandle);
                if ((m_data != null) && (this.Running == ThreadState.Running))
                {
                    lock (m_data)
                    {
                        FTD2XX.FT_Write(m_FTD2XXHandle, m_data, (uint) m_data.Length, ref bytesWritten);
                    }
                }
                Thread.Sleep(2);
            }
            this.Running = ThreadState.Stopped;
        }

        public int References
        {
            get
            {
                return m_refCount;
            }
        }

        private ThreadState Running
        {
            get
            {
                return m_running;
            }
            set
            {
                m_running = value;
            }
        }

        private enum ThreadState
        {
            Running,
            Stopped,
            StopRequested
        }
    }
}

