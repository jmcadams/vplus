using System.Windows.Forms;
using System.Xml;

namespace VixenPlus {
    public partial class OutputPlugInUIBase : Form
    {
        public XmlNode DataNode;
        public IVixenMDI ExecutionParent;


        protected OutputPlugInUIBase()
        {
            InitializeComponent();
        }
    }
}