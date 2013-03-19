namespace LedTriksUtil
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using Vixen;

    public class VirtualHardware : IFrameOutput
    {
        private Size m_boardLayout;
        private VirtualHardwareDisplayDialog m_display;
        private int m_dotPitch;
        private Color m_ledColor;
        private int m_ledSize;
        private RunState m_state;

        public VirtualHardware(int boardLayoutWidth, int boardLayoutHeight)
        {
            this.m_display = null;
            this.m_state = RunState.Stopped;
            this.m_boardLayout = new Size(boardLayoutWidth, boardLayoutHeight);
            this.m_ledSize = 3;
            this.m_ledColor = Color.FromArgb(-65536);
            this.m_dotPitch = 9;
        }

        public VirtualHardware(int boardLayoutWidth, int boardLayoutHeight, int ledSize, Color ledColor, int dotPitch)
        {
            this.m_display = null;
            this.m_state = RunState.Stopped;
            this.m_boardLayout = new Size(boardLayoutWidth, boardLayoutHeight);
            this.m_ledSize = ledSize;
            this.m_ledColor = ledColor;
            this.m_dotPitch = dotPitch;
        }

        private void DestroyDisplay()
        {
            if (this.m_display != null)
            {
                if (this.m_display.InvokeRequired)
                {
                    this.m_display.Invoke(new System.Windows.Forms.MethodInvoker(this.DestroyDisplay));
                }
                else
                {
                    lock (this.m_display)
                    {
                        this.m_display.Close();
                        this.m_display = null;
                    }
                }
            }
        }

        public void RebuildDisplay()
        {
            this.RebuildDisplay(this.m_ledSize, this.m_ledColor, this.m_dotPitch);
        }

        public void RebuildDisplay(int ledSize, Color ledColor, int dotPitch)
        {
            this.m_ledSize = ledSize;
            this.m_ledColor = ledColor;
            this.m_dotPitch = dotPitch;
            if ((this.m_display != null) && this.m_display.InvokeRequired)
            {
                lock (this.m_display)
                {
                    this.m_display.BeginInvoke(new System.Windows.Forms.MethodInvoker(this.RebuildDisplay));
                }
            }
            else
            {
                this.DestroyDisplay();
                ISystem system = (ISystem) Interfaces.Available["ISystem"];
                ConstructorInfo constructor = typeof(VirtualHardwareDisplayDialog).GetConstructor(new Type[] { typeof(Size), typeof(int), typeof(Color), typeof(int) });
                this.m_display = (VirtualHardwareDisplayDialog) system.InstantiateForm(constructor, new object[] { this.m_boardLayout, this.m_ledSize, this.m_ledColor, this.m_dotPitch });
            }
        }

        public void Start()
        {
            if (this.m_state != RunState.Running)
            {
                this.RebuildDisplay();
                this.m_state = RunState.Running;
            }
        }

        public void Stop()
        {
            new Thread(new ThreadStart(this.StopThread)).Start();
        }

        private void StopThread()
        {
            if (this.m_state == RunState.Running)
            {
                this.m_state = RunState.Stopping;
                this.DestroyDisplay();
                this.m_state = RunState.Stopped;
            }
        }

        public void UpdateFrame(Frame frame)
        {
            if (((this.m_state == RunState.Running) && (this.m_display != null)) && !this.m_display.IsDisposed)
            {
                lock (this.m_display)
                {
                    this.m_display.OutputFrame(ParsedFrame.ParseFrame(frame, this.m_boardLayout.Width, this.m_boardLayout.Height));
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

