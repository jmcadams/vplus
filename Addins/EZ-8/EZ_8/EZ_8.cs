namespace EZ_8
{
    using System;
    using System.Globalization;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class EZ_8
    {
        public const int BAUD_RATE = 0x9600;
        private const int DATA_BLOCK_LENGTH = 0x10;
        private bool m_busy = false;
        private EZ8Configuration m_config = new EZ8Configuration();
        private int m_portRefCount = 0;
        private SerialPort m_serialPort = null;
        private bool m_setCodeProtect = false;
        private const int MAX_CHANNEL_COUNT = 8;

        public event StringEventHandler DeviceError;

        public event VoidEventHandler EraseEnd;

        public event VoidEventHandler EraseStart;

        public event VoidEventHandler Timeout;

        public event DataEventHandler TransferEnd;

        public event IntEventHandler TransferProgress;

        public event VoidEventHandler TransferStart;

        private bool CanGetExclusive()
        {
            return !this.m_busy;
        }

        public void Close()
        {
            this.GetExclusive(true);
            try
            {
                if ((this.m_serialPort != null) && this.m_serialPort.IsOpen)
                {
                    if (this.m_portRefCount > 0)
                    {
                        this.m_portRefCount--;
                    }
                    else
                    {
                        this.m_portRefCount = 0;
                    }
                    if (this.m_portRefCount == 0)
                    {
                        this.m_serialPort.Close();
                        while (this.m_serialPort.IsOpen)
                        {
                        }
                    }
                }
            }
            finally
            {
                this.ReleaseExclusive();
            }
        }

        public void Download(byte[] values)
        {
            this.DownloadThread(values);
        }

        private void DownloadThread(byte[] values)
        {
            Exception exception;
            try
            {
                byte[] array = new byte[0x10];
                this.OnTransferStart();
                try
                {
                    if (this.GetExclusive())
                    {
                        this.Open();
                        try
                        {
                            byte[] buffer2 = new byte[0x40 + values.Length];
                            this.m_config.CodeProtect = this.m_setCodeProtect;
                            this.m_config.GetConfigurationBytes().CopyTo(buffer2, 0);
                            values.CopyTo(buffer2, 0x40);
                            for (ushort i = 0; i < buffer2.Length; i = (ushort) (i + 0x10))
                            {
                                this.m_serialPort.Write("!D");
                                this.m_serialPort.Write(BitConverter.GetBytes(i), 0, 2);
                                Array.Clear(array, 0, array.Length);
                                Array.Copy(buffer2, i, array, 0, Math.Min(array.Length, buffer2.Length - i));
                                this.m_serialPort.Write(array, 0, array.Length);
                                this.OnTransferProgress((i * 100) / buffer2.Length);
                                while (this.m_serialPort.BytesToRead < 1)
                                {
                                    Thread.Sleep(1);
                                }
                                if (this.m_serialPort.ReadByte() != 0x3e)
                                {
                                    throw new Exception(string.Format("Acknowledgment not received during download.\nAddress = {0:X4}.", i));
                                }
                            }
                        }
                        catch (TimeoutException)
                        {
                            this.OnTimeout();
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            this.OnError(exception.Message);
                        }
                        finally
                        {
                            this.Close();
                        }
                    }
                }
                finally
                {
                    this.OnTransferEnd(null);
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                this.OnError("Download:\n\n" + exception.Message + "\n\n" + exception.StackTrace);
            }
        }

        public void EraseAll()
        {
            this.OnEraseStart();
            new Thread(new ThreadStart(this.EraseAllThread)).Start();
        }

        private void EraseAllThread()
        {
            try
            {
                if (this.GetExclusive())
                {
                    this.Open();
                    try
                    {
                        this.m_serialPort.Write("!XeZ8");
                        while (this.m_serialPort.BytesToRead < 1)
                        {
                            Thread.Sleep(1);
                        }
                        byte num = (byte) this.m_serialPort.ReadByte();
                        if (num != 0x3e)
                        {
                            throw new Exception(string.Format("Unexpected response.\nValue = {0:X2}.", num));
                        }
                    }
                    catch (TimeoutException)
                    {
                        this.OnTimeout();
                    }
                    catch (Exception exception)
                    {
                        this.OnError(exception.Message);
                    }
                    finally
                    {
                        this.Close();
                    }
                }
            }
            finally
            {
                this.OnEraseEnd();
            }
        }

        public int GetEndOfShowAddress()
        {
            byte[] buffer = new byte[4];
            if (!this.GetExclusive())
            {
                return 0;
            }
            this.Open();
            try
            {
                this.m_serialPort.Write("!E");
                while (this.m_serialPort.BytesToRead < buffer.Length)
                {
                    Thread.Sleep(1);
                }
                this.m_serialPort.Read(buffer, 0, buffer.Length);
            }
            catch (TimeoutException)
            {
                this.OnTimeout();
            }
            catch (Exception exception)
            {
                this.OnError(exception.Message);
            }
            finally
            {
                this.Close();
            }
            return int.Parse(Encoding.UTF8.GetString(buffer), NumberStyles.AllowHexSpecifier);
        }

        private bool GetExclusive()
        {
            return this.GetExclusive(false);
        }

        private bool GetExclusive(bool wait)
        {
            if (!this.CanGetExclusive())
            {
                return false;
            }
            while (wait && this.m_busy)
            {
                Thread.Sleep(1);
            }
            this.m_busy = true;
            return true;
        }

        public InputState GetInputs()
        {
            InputState state = null;
            if (!this.GetExclusive())
            {
                return null;
            }
            this.Open();
            try
            {
                this.m_serialPort.Write("!G");
                while (this.m_serialPort.BytesToRead < 1)
                {
                    Thread.Sleep(1);
                }
                state = new InputState((byte) this.m_serialPort.ReadByte());
            }
            catch (TimeoutException)
            {
                this.OnTimeout();
            }
            catch (Exception exception)
            {
                this.OnError(exception.Message);
            }
            finally
            {
                this.Close();
            }
            return state;
        }

        public string GetRevision()
        {
            string str = null;
            if (!this.GetExclusive())
            {
                return null;
            }
            this.Open();
            try
            {
                this.m_serialPort.Write("!R");
                Thread.Sleep(1);
                str = this.m_serialPort.ReadLine();
                this.m_config.CodeProtect = str.EndsWith("-CP");
                if (this.m_config.CodeProtect)
                {
                    str = str.Substring(0, str.Length - 3);
                }
            }
            catch (TimeoutException)
            {
                this.OnTimeout();
            }
            catch (Exception exception)
            {
                this.OnError(exception.Message);
            }
            finally
            {
                this.Close();
            }
            return str;
        }

        protected void OnEraseEnd()
        {
            if (this.EraseEnd != null)
            {
                this.EraseEnd();
            }
        }

        protected void OnEraseStart()
        {
            if (this.EraseStart != null)
            {
                this.EraseStart();
            }
        }

        protected void OnError(string msg)
        {
            if (this.DeviceError != null)
            {
                this.DeviceError(msg);
            }
        }

        protected void OnTimeout()
        {
            if (this.Timeout != null)
            {
                this.Timeout();
            }
        }

        protected void OnTransferEnd(byte[] data)
        {
            if (this.TransferEnd != null)
            {
                this.TransferEnd(data);
            }
        }

        protected void OnTransferProgress(int percent)
        {
            if (this.TransferProgress != null)
            {
                this.TransferProgress(percent);
            }
        }

        protected void OnTransferStart()
        {
            if (this.TransferStart != null)
            {
                this.TransferStart();
            }
        }

        public void Open()
        {
            if ((this.m_serialPort != null) && !((this.m_portRefCount++ != 0) || this.m_serialPort.IsOpen))
            {
                this.m_serialPort.Open();
            }
        }

        public void Play()
        {
            if (this.GetExclusive())
            {
                this.Open();
                try
                {
                    this.m_serialPort.Write("!P");
                }
                finally
                {
                    this.Close();
                }
            }
        }

        private void ReleaseExclusive()
        {
            this.m_busy = false;
        }

        public void SetCodeProtect(bool state)
        {
            this.m_setCodeProtect = state;
        }

        public void SetOutputs(byte outputState)
        {
            if (this.GetExclusive())
            {
                this.Open();
                try
                {
                    this.m_serialPort.Write("!S");
                    this.m_serialPort.Write(new byte[] { outputState }, 0, 1);
                }
                catch (TimeoutException)
                {
                    this.OnTimeout();
                }
                catch (Exception exception)
                {
                    this.OnError(exception.Message);
                }
                finally
                {
                    this.Close();
                }
            }
        }

        public void Upload()
        {
            this.UploadThread();
        }

        private void UploadThread()
        {
            Exception exception;
            try
            {
                byte[] buffer = new byte[2];
                byte[] buffer2 = null;
                byte[] destinationArray = null;
                this.OnTransferStart();
                try
                {
                    if (this.GetExclusive())
                    {
                        this.Open();
                        try
                        {
                            this.m_serialPort.Write("!U");
                            while (this.m_serialPort.BytesToRead < buffer.Length)
                            {
                                Thread.Sleep(1);
                            }
                            this.m_serialPort.Read(buffer, 0, buffer.Length);
                            ushort num = BitConverter.ToUInt16(buffer, 0);
                            buffer2 = new byte[num + 1];
                            for (int i = 0; i <= num; i += 0x10)
                            {
                                int count = Math.Min(0x10, (num - i) + 1);
                                while (this.m_serialPort.BytesToRead < count)
                                {
                                    Thread.Sleep(1);
                                }
                                this.m_serialPort.Read(buffer2, i, count);
                                this.OnTransferProgress((i * 100) / num);
                            }
                        }
                        catch (TimeoutException)
                        {
                            this.OnTimeout();
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            this.OnError(exception.Message);
                        }
                        finally
                        {
                            this.Close();
                        }
                        this.Configuration.ReadConfigurationBytes(buffer2);
                        destinationArray = new byte[buffer2.Length - 0x40];
                        Array.Copy(buffer2, 0x40, destinationArray, 0, destinationArray.Length);
                    }
                }
                finally
                {
                    this.OnTransferEnd(destinationArray);
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                this.OnError("Upload:\n\n" + exception.Message + "\n\n" + exception.StackTrace);
            }
        }

        public EZ8Configuration Configuration
        {
            get
            {
                return this.m_config;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return ((!this.m_busy && (this.m_serialPort != null)) && this.m_serialPort.IsOpen);
            }
        }

        public bool IsOpen
        {
            get
            {
                return ((this.m_serialPort != null) && this.m_serialPort.IsOpen);
            }
        }

        public string PortName
        {
            get
            {
                if (this.m_serialPort != null)
                {
                    return this.m_serialPort.PortName;
                }
                return null;
            }
            set
            {
                if (((this.m_serialPort == null) && (value != null)) || ((this.m_serialPort != null) && (value != this.m_serialPort.PortName)))
                {
                    this.Close();
                    if (value == null)
                    {
                        this.m_serialPort = null;
                    }
                    else
                    {
                        this.m_serialPort = new SerialPort(value, 0x9600);
                        this.m_serialPort.ReadTimeout = 0x3e8;
                        this.m_serialPort.NewLine = new string(new char[] { '\r' });
                    }
                }
            }
        }

        public delegate void DataEventHandler(byte[] data);

        public delegate void IntEventHandler(int value);

        public delegate void StringEventHandler(string value);

        public delegate void VoidEventHandler();
    }
}

