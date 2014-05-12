using System;
using System.Threading;

namespace Controllers.IDMX
{
    public class Idmx
    {
        private static byte[] _data;
        private static IntPtr _ftd2XxHandle = IntPtr.Zero;
        private static readonly object LockObject = new object();
        private static int _refCount;
        private static Thread _thread;


        static Idmx() {
            Running = ThreadState.Stopped;
        }


        public void Close()
        {
            lock (LockObject)
            {
                if ((_refCount <= 0) || (--_refCount != 0)) {
                    return;
                }
                if (Running == ThreadState.Running)
                {
                    Running = ThreadState.StopRequested;
                    while (Running != ThreadState.Stopped)
                    {
                        Thread.Sleep(1); //todo replace with Task.Delay() when using 4.5
                    }
                }
                _thread = null;
                if (_ftd2XxHandle != IntPtr.Zero)
                {
                    FTD2XX.FT_Close(_ftd2XxHandle);
                    _ftd2XxHandle = IntPtr.Zero;
                }
                _data = null;
            }
        }

        public void Init()
        {
            lock (LockObject) {
                if (((++_refCount != 1) || (Running == ThreadState.Running)) || (_thread != null)) {
                    return;
                }
                try
                {
                    if (_ftd2XxHandle == IntPtr.Zero)
                    {
                        _data = new byte[0x201];
                        if (FTD2XX.FT_Open(0, ref _ftd2XxHandle) != FTD2XX.FT_STATUS.FT_OK)
                        {
                            throw new Exception("Error opening FTD2XX device 0");
                        }
                        FTD2XX.FT_ResetDevice(_ftd2XxHandle);
                        FTD2XX.FT_SetTimeouts(_ftd2XxHandle, 16, 50);
                        FTD2XX.FT_SetBaudRate(_ftd2XxHandle, 250000);
                        FTD2XX.FT_SetDataCharacteristics(_ftd2XxHandle, 8, 2, 0);
                        FTD2XX.FT_SetFlowControl(_ftd2XxHandle, 0, 0, 0);
                    }
                    _thread = new Thread(SendDataThread);
                    _thread.Start();
                }
                catch
                {
                    Close();
                    throw;
                }
            }
        }

        public void SendData(byte[] values)
        {
            if (_data == null) {
                return;
            }
            if (values.Length > 512)
            {
                Array.Resize(ref values, 512);
            }
            lock (_data)
            {
                values.CopyTo(_data, 1);
            }
        }

        private void SendDataThread()
        {
            uint bytesWritten = 0;
            Running = ThreadState.Running;
            while (Running == ThreadState.Running)
            {
                FTD2XX.FT_SetBreakOn(_ftd2XxHandle);
                Thread.Sleep(1); //todo replace with Task.Delay() when using 4.5
                FTD2XX.FT_SetBreakOff(_ftd2XxHandle);
                if ((_data != null) && (Running == ThreadState.Running))
                {
                    lock (_data)
                    {
                        FTD2XX.FT_Write(_ftd2XxHandle, _data, (uint) _data.Length, ref bytesWritten);
                    }
                }
                Thread.Sleep(2); //todo replace with Task.Delay() when using 4.5
            }
            Running = ThreadState.Stopped;
        }

        public int References
        {
            get
            {
                return _refCount;
            }
        }

        private static ThreadState Running { get; set; }

        private enum ThreadState
        {
            Running,
            Stopped,
            StopRequested
        }
    }
}

