namespace Preview
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Preview : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private List<Channel> m_channels = null;
        private List<Form> m_dialogList;
        private PreviewDialog m_previewDialog = null;
        private SetupData m_setupData;
        private SetupDialog m_setupDialog = null;
        private XmlNode m_setupNode;
        private int m_startChannel;

        public Preview()
        {
            this.m_channels = new List<Channel>();
            this.m_dialogList = new List<Form>();
        }

        public void Event(byte[] channelValues)
        {
            if (((this.m_previewDialog != null) && !this.m_previewDialog.Disposing) && !this.m_previewDialog.IsDisposed)
            {
                this.m_previewDialog.UpdateWith(channelValues);
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_channels.Clear();
            this.m_channels.AddRange(executableObject.Channels);
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_startChannel = Convert.ToInt32(this.m_setupNode.Attributes["from"].Value) - 1;
            this.m_setupData.GetBytes(this.m_setupNode, "BackgroundImage", new byte[0]);
        }

        public void Setup()
        {
            if (this.m_channels.Count == 0)
            {
                MessageBox.Show("The item you are trying to create a preview for has no channels.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.m_setupDialog = new SetupDialog(this.m_setupData, this.m_setupNode, this.m_channels, this.m_startChannel);
                this.m_setupDialog.ShowDialog();
                this.m_setupDialog.Dispose();
            }
        }

        public void Shutdown()
        {
            if (this.m_previewDialog != null)
            {
                if (this.m_previewDialog.InvokeRequired)
                {
                    this.m_previewDialog.BeginInvoke(new MethodInvoker(this.m_previewDialog.Dispose));
                }
                else
                {
                    this.m_previewDialog.Dispose();
                }
                this.m_previewDialog = null;
            }
            this.m_channels.Clear();
            this.m_setupData = null;
            this.m_setupNode = null;
        }

        public void Startup()
        {
            if (this.m_channels.Count != 0)
            {
                ISystem system = (ISystem) Interfaces.Available["ISystem"];
                ConstructorInfo constructor = typeof(PreviewDialog).GetConstructor(new System.Type[] { typeof(XmlNode), typeof(List<Channel>), typeof(int) });
                this.m_previewDialog = (PreviewDialog) system.InstantiateForm(constructor, new object[] { this.m_setupNode, this.m_channels, this.m_startChannel });
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
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Adjustable sequence preview plugin for Vixen";
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
                return "Adjustable preview";
            }
        }
    }
}

