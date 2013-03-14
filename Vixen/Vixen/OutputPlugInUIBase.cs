namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml;

    public class OutputPlugInUIBase : Form
    {
        private IContainer components = null;
        public XmlNode DataNode = null;
        public VixenMDI ExecutionParent = null;

        public OutputPlugInUIBase()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "OutputPlugInUIBase";
        }
    }
}

