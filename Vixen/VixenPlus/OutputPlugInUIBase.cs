namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml;

    public partial class OutputPlugInUIBase : Form
    {
        public XmlNode DataNode = null;
        public VixenMDI ExecutionParent = null;

        public OutputPlugInUIBase()
        {
            this.InitializeComponent();
        }
    }
}