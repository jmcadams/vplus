namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class LedTriks : IEventlessOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private Frame m_blankFrame = null;
        private int m_boardLayoutHeight;
        private int m_boardLayoutWidth;
        private IExecutable m_executable;
        private int m_frameEnd;
        private int m_frameIndex;
        private List<Frame> m_frames = new List<Frame>();
        private int m_frameStart;
        private Hardware m_hardware = null;
        private XmlNode m_ledUINode;
        private ushort m_portAddress = 0;
        private bool m_running = false;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private Thread m_thread;
        private ITickSource m_timer;
        private bool m_useWithScript;

        private void DynamicExecutionThread()
        {
            Frame frame = null;
            int num = 0;
            while (this.m_running)
            {
                if (this.m_frames.Count > 0)
                {
                    frame = this.m_frames[0];
                    this.m_frames.RemoveAt(0);
                    num = this.m_timer.Milliseconds + frame.Length;
                }
                else if (frame != null)
                {
                    num += 10;
                }
                if (frame != null)
                {
                    while (this.m_timer.Milliseconds < num)
                    {
                        this.m_hardware.UpdateFrame(frame);
                    }
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

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode, ITickSource timer)
        {
            this.m_executable = executableObject;
            this.m_timer = timer;
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portAddress = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "address", 0x378);
            this.m_useWithScript = this.m_setupData.GetBoolean(this.m_setupNode, "useWithScript", false);
            executableObject.UserData = this.m_frames;
        }

        private void LoadFrames()
        {
            try
            {
                this.m_frames.Clear();
                foreach (XmlNode node in this.m_ledUINode.SelectNodes("Frames/Frame"))
                {
                    Frame item = new Frame(node);
                    this.m_frames.Add(item);
                }
                this.m_blankFrame = new Frame(1);
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

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, short data);
        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_portAddress, this.m_useWithScript);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_portAddress = dialog.PortAddress;
                this.m_setupData.SetInteger(this.m_setupNode, "address", this.m_portAddress);
                this.m_useWithScript = dialog.UseWithScript;
                this.m_setupData.SetBoolean(this.m_setupNode, "useWithScript", this.m_useWithScript);
            }
        }

        public void Shutdown()
        {
            this.m_hardware.Stop();
            this.m_running = false;
            if ((this.m_thread != null) && this.m_thread.IsAlive)
            {
                this.m_thread.Join();
            }
            if (this.m_blankFrame != null)
            {
                this.m_hardware.UpdateFrame(this.m_blankFrame);
            }
        }

        public void Startup()
        {
            if (this.m_portAddress == 0)
            {
                throw new Exception("No port has been specified.");
            }
            if (((this.m_thread != null) && this.m_running) && this.m_thread.IsAlive)
            {
                this.Shutdown();
            }
            EventSequence executable = (EventSequence) this.m_executable;
            this.m_ledUINode = executable.Extensions[".led"];
            if (!this.m_useWithScript)
            {
                XmlNode node = this.m_ledUINode.SelectSingleNode("Boards");
                if (node == null)
                {
                    throw new ArgumentException("Cannot use the LedTriks output plugin with a non-LedTriks sequence.");
                }
                this.m_boardLayoutWidth = int.Parse(node.Attributes["width"].Value);
                this.m_boardLayoutHeight = int.Parse(node.Attributes["height"].Value);
                this.m_hardware = new Hardware(this.m_portAddress, this.m_boardLayoutWidth, this.m_boardLayoutHeight);
                this.LoadFrames();
                this.m_frameIndex = this.GetFrameAt(this.m_timer.Milliseconds);
                this.FrameTimesFromFrameIndex(this.m_frameIndex);
                this.m_thread = new Thread(new ThreadStart(this.StaticExecutionThread));
            }
            else
            {
                this.m_hardware = new Hardware(this.m_portAddress, 1, 1);
                this.m_frames.Clear();
                this.m_blankFrame = new Frame(1);
                this.m_thread = new Thread(new ThreadStart(this.DynamicExecutionThread));
            }
            this.m_hardware.Start();
            this.m_running = true;
            this.m_thread.Start();
        }

        private void StaticExecutionThread()
        {
            int frameIndex = -1;
            if (this.m_frames.Count > 0)
            {
                int count = this.m_frames.Count;
                while (this.m_running)
                {
                    if (this.m_frameIndex != frameIndex)
                    {
                        this.m_hardware.UpdateFrame(this.m_frames[this.m_frameIndex]);
                        frameIndex = this.m_frameIndex;
                    }
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
                            }
                        }
                        else
                        {
                            int frameAt = this.GetFrameAt(this.m_timer.Milliseconds);
                            if (frameAt < count)
                            {
                                this.m_frameIndex = frameAt;
                                this.FrameTimesFromFrameIndex(this.m_frameIndex);
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "LedTriks output plugin";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Parallel", this.m_portAddress, "X") };
            }
        }

        public string Name
        {
            get
            {
                return "LedTriks";
            }
        }
    }
}

