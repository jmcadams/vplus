namespace VirtualLedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class VirtualLedTriks : IEventlessOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private Size m_boardLayout;
        private int m_dotPitch;
        private IExecutable m_executable;
        private int m_frameEnd;
        private int m_frameIndex;
        private List<Frame> m_frames = new List<Frame>();
        private int m_frameStart;
        private Color m_ledColor;
        private int m_ledSize;
        private XmlNode m_ledUINode;
        private bool m_running = false;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private Thread m_thread;
        private ITickSource m_timer;
        private bool m_useWithScript = false;
        private VirtualHardware m_virtualHardware;

        private void DynamicExecutionThread()
        {
            Frame frame = null;
            int length = 0;
            int num2 = 0;
            int num3 = 0;
            while (this.m_running)
            {
                if (this.m_frames.Count > 0)
                {
                    frame = this.m_frames[0];
                    this.m_frames.RemoveAt(0);
                    if (this.m_running)
                    {
                        this.m_virtualHardware.UpdateFrame(frame);
                        length = (num2 = this.m_timer.Milliseconds) + frame.Length;
                        while (this.m_running && ((num3 = this.m_timer.Milliseconds) < length))
                        {
                            if (num2 > num3)
                            {
                                length = frame.Length;
                            }
                            Thread.Sleep(5);
                        }
                    }
                }
                else if (frame != null)
                {
                    Thread.Sleep(10);
                }
            }
        }

        private void FrameTimesFromFrameIndex(int frameIndex)
        {
            this.m_frameStart = 0;
            int num = 0;
            while (num < frameIndex)
            {
                this.m_frameStart += this.m_frames[num].Length;
                num++;
            }
            this.m_frameEnd = this.m_frameStart + this.m_frames[num].Length;
        }

        private int GetFrameAt(int milliseconds)
        {
            int num = 0;
            int num2 = 0;
            while (num2 < milliseconds)
            {
                num2 += this.m_frames[num].Length;
                num++;
            }
            return num;
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer)
        {
            this.m_setupNode = setupNode;
            this.m_setupData = setupData;
            this.m_timer = timer;
            XmlNode node = ((EventSequence) executableObject).Extensions[".led"]["Boards"];
            this.m_useWithScript = node == null;
            if (node != null)
            {
                this.m_boardLayout = new Size(int.Parse(node.Attributes["width"].Value), int.Parse(node.Attributes["height"].Value));
            }
            else
            {
                string[] strArray = this.m_setupData.GetString(this.m_setupNode, "BoardLayout", "1,1").Split(new char[] { ',' });
                this.m_boardLayout = new Size(int.Parse(strArray[0].Trim()), int.Parse(strArray[1].Trim()));
            }
            this.m_ledSize = this.m_setupData.GetInteger(this.m_setupNode, "LEDSize", 3);
            this.m_ledColor = Color.FromArgb(this.m_setupData.GetInteger(this.m_setupNode, "LEDColor", -65536));
            this.m_dotPitch = this.m_setupData.GetInteger(this.m_setupNode, "DotPitch", 9);
            executableObject.UserData = this.m_frames;
            this.m_executable = executableObject;
        }

        private void LoadFrames()
        {
            try
            {
                this.m_frames.Clear();
                XmlNode node = this.m_ledUINode.SelectSingleNode("Boards");
                if (node == null)
                {
                    throw new ArgumentException("Cannot use the LedTriks output plugin with a non-LedTriks sequence.");
                }
                this.m_boardLayout.Width = int.Parse(node.Attributes["width"].Value);
                this.m_boardLayout.Height = int.Parse(node.Attributes["height"].Value);
                foreach (XmlNode node2 in this.m_ledUINode.SelectNodes("Frames/Frame"))
                {
                    this.m_frames.Add(new Frame(node2));
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch
            {
                throw new Exception("Error loading frames.\n\nIf you are executing a scripted sequence, make sure the LedTriks plugin is setup for that.");
            }
        }

        public void Setup()
        {
            VirtualHardwareSetupDialog dialog = new VirtualHardwareSetupDialog(this.m_useWithScript);
            dialog.BoardLayout = this.m_boardLayout;
            dialog.LEDColor = this.m_ledColor;
            dialog.LEDSize = this.m_ledSize;
            dialog.DotPitch = this.m_dotPitch;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (this.m_useWithScript)
                {
                    this.m_boardLayout = dialog.BoardLayout;
                    this.m_setupData.SetString(this.m_setupNode, "BoardLayout", string.Format("{0},{1}", this.m_boardLayout.Width, this.m_boardLayout.Height));
                }
                this.m_ledSize = dialog.LEDSize;
                this.m_ledColor = dialog.LEDColor;
                this.m_dotPitch = dialog.DotPitch;
                this.m_setupData.SetInteger(this.m_setupNode, "LEDSize", this.m_ledSize);
                this.m_setupData.SetInteger(this.m_setupNode, "LEDColor", this.m_ledColor.ToArgb());
                this.m_setupData.SetInteger(this.m_setupNode, "DotPitch", this.m_dotPitch);
            }
        }

        public void Shutdown()
        {
            this.m_running = false;
            this.m_virtualHardware.Stop();
        }

        public void Startup()
        {
            if (((this.m_thread != null) && this.m_running) && this.m_thread.IsAlive)
            {
                this.Shutdown();
            }
            this.m_virtualHardware = new VirtualHardware(this.m_boardLayout.Width, this.m_boardLayout.Height, this.m_ledSize, this.m_ledColor, this.m_dotPitch);
            EventSequence executable = (EventSequence) this.m_executable;
            this.m_ledUINode = executable.Extensions[".led"];
            if (!this.m_useWithScript)
            {
                this.LoadFrames();
                this.m_frameIndex = this.GetFrameAt(this.m_timer.Milliseconds);
                this.FrameTimesFromFrameIndex(this.m_frameIndex);
                this.m_thread = new Thread(new ThreadStart(this.StaticExecutionThread));
            }
            else
            {
                this.m_frames.Clear();
                this.m_thread = new Thread(new ThreadStart(this.DynamicExecutionThread));
            }
            this.m_virtualHardware.Start();
            this.m_running = true;
            this.m_thread.Start();
        }

        private void StaticExecutionThread()
        {
            if (this.m_frames.Count > 0)
            {
                int count = this.m_frames.Count;
                while (this.m_running)
                {
                    int milliseconds = this.m_timer.Milliseconds;
                    if ((milliseconds < this.m_frameStart) || (milliseconds >= this.m_frameEnd))
                    {
                        if (milliseconds >= this.m_frameEnd)
                        {
                            if ((this.m_frameIndex + 1) < count)
                            {
                                this.m_frameStart = this.m_frameEnd;
                                this.m_frameIndex++;
                                this.m_frameEnd = this.m_frameStart + this.m_frames[this.m_frameIndex].Length;
                                if (this.m_running)
                                {
                                    this.m_virtualHardware.UpdateFrame(this.m_frames[this.m_frameIndex]);
                                }
                            }
                        }
                        else
                        {
                            int frameAt = this.GetFrameAt(this.m_timer.Milliseconds);
                            if (frameAt < count)
                            {
                                this.m_frameIndex = frameAt;
                                this.FrameTimesFromFrameIndex(this.m_frameIndex);
                                if (this.m_running)
                                {
                                    this.m_virtualHardware.UpdateFrame(this.m_frames[this.m_frameIndex]);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int TimeFromFrameCount(int frameCount)
        {
            int num = 0;
            int num2 = 0;
            while (num < frameCount)
            {
                num2 += this.m_frames[num].Length;
                num++;
            }
            return num2;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Virtual LedTriks output display";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Virtual LedTriks";
            }
        }
    }
}

