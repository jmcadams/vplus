namespace CurrentExecution
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class CurrentExecution : IEventlessOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private int m_boardLayoutHeight = 1;
        private int m_boardLayoutWidth = 1;
        private int m_dotPitch;
        private IExecution m_executionInterface;
        private int m_frameIndex = 0;
        private List<Frame> m_frames = new List<Frame>();
        private Generator m_generator = null;
        private IFrameOutput m_hardware;
        private Color m_ledColor;
        private int m_ledSize;
        private string m_messageFormat;
        private ushort m_portAddress = 0;
        private bool m_running = false;
        private string m_sequenceName = string.Empty;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private ISystem m_systemInterface;
        private ITickSource m_tick;
        private System.Timers.Timer m_timer = new System.Timers.Timer(1000.0);
        private Thread m_updateThread = null;
        private bool m_virtual = false;
        internal const string NAME_PARAM = "[NAME]";

        public CurrentExecution()
        {
            this.m_timer.Elapsed += new ElapsedEventHandler(this.m_timer_Elapsed);
            this.m_systemInterface = (ISystem) Interfaces.Available["ISystem"];
            this.m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
        }

        private void FrameOutputThread()
        {
            Frame frame = null;
            int num = 0;
            this.m_running = true;
            while (this.m_running)
            {
                if (this.m_frames.Count > 0)
                {
                    frame = this.m_frames[this.m_frameIndex];
                    this.m_frameIndex++;
                    if (this.m_frameIndex >= this.m_frames.Count)
                    {
                        this.m_frameIndex = 0;
                    }
                    num = this.m_tick.Milliseconds + frame.Length;
                }
                else if (frame != null)
                {
                    num += 10;
                }
                if (frame != null)
                {
                    this.m_hardware.UpdateFrame(frame);
                    while (this.m_tick.Milliseconds < num)
                    {
                        if (!this.m_running)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private string GetCurrentSequenceName()
        {
            return (this.m_sequenceName = this.m_executionInterface.LoadedSequence(this.m_executionInterface.FindExecutionContextHandle(this)));
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_tick = timer;
            this.UpdateFromSetup();
        }

        private void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string sequenceName = this.m_sequenceName;
            if (this.GetCurrentSequenceName() != sequenceName)
            {
                this.m_frames = this.m_generator.GenerateTextFrames(this.m_messageFormat.Replace("[NAME]", this.m_sequenceName));
                this.m_frameIndex = 0;
            }
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.UpdateFromSetup();
            }
            dialog.Dispose();
        }

        public void Shutdown()
        {
            this.m_timer.Stop();
            this.StopThread();
            this.m_hardware.Stop();
            this.m_hardware = null;
            if (this.m_generator != null)
            {
                this.m_generator.Dispose();
            }
        }

        public void Startup()
        {
            if (this.m_hardware != null)
            {
                this.m_hardware.Stop();
                this.m_hardware = null;
            }
            if (this.m_virtual)
            {
                this.m_hardware = new VirtualHardware(this.m_boardLayoutWidth, this.m_boardLayoutHeight, this.m_ledSize, this.m_ledColor, this.m_dotPitch);
            }
            else
            {
                this.m_hardware = new Hardware(this.m_portAddress, this.m_boardLayoutWidth, this.m_boardLayoutHeight);
            }
            this.m_generator = new Generator();
            this.m_generator.TextScrollExtent = ScrollExtent.OnAndOff;
            this.m_generator.LoadFromXml(this.m_setupNode["TextOptions"]);
            this.m_generator.BoardPixelWidth = 0x30 * this.m_boardLayoutWidth;
            this.m_generator.BoardPixelHeight = 0x10 * this.m_boardLayoutHeight;
            this.m_hardware.Start();
            this.StopThread();
            this.m_updateThread = new Thread(new ThreadStart(this.FrameOutputThread));
            this.m_updateThread.Start();
            this.m_timer.Start();
        }

        private void StopThread()
        {
            if ((this.m_updateThread != null) && this.m_updateThread.IsAlive)
            {
                this.m_running = false;
                Application.DoEvents();
                this.m_updateThread.Join();
                this.m_updateThread = null;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private void UpdateFromSetup()
        {
            this.m_portAddress = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "Address", 0x378);
            XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Boards");
            if (nodeAlways.Attributes["width"] != null)
            {
                this.m_boardLayoutWidth = int.Parse(nodeAlways.Attributes["width"].Value);
            }
            if (nodeAlways.Attributes["height"] != null)
            {
                this.m_boardLayoutHeight = int.Parse(nodeAlways.Attributes["height"].Value);
            }
            this.m_messageFormat = Xml.GetNodeAlways(this.m_setupNode, "Message", "[NAME]").InnerText;
            XmlNode contextNode = Xml.GetNodeAlways(this.m_setupNode, "Virtual");
            if (contextNode.Attributes["enabled"] != null)
            {
                this.m_virtual = bool.Parse(contextNode.Attributes["enabled"].Value);
            }
            else
            {
                this.m_virtual = false;
            }
            if (this.m_virtual)
            {
                this.m_ledSize = int.Parse(Xml.GetNodeAlways(contextNode, "LEDSize", "3").InnerText);
                this.m_ledColor = Color.FromArgb(int.Parse(Xml.GetNodeAlways(contextNode, "LEDColor", "-65536").InnerText));
                this.m_dotPitch = int.Parse(Xml.GetNodeAlways(contextNode, "DotPitch", "9").InnerText);
            }
            else
            {
                this.m_ledSize = 3;
                this.m_ledColor = Color.Red;
                this.m_dotPitch = 9;
            }
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
                return "Shows currently-executing sequence information on a LedTriks display.";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                if (!this.m_virtual)
                {
                    return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Parallel", this.m_portAddress, "X") };
                }
                return new Vixen.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Current execution";
            }
        }
    }
}

