using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	public partial class OutputPlugInUIBase : Form
	{
		public XmlNode DataNode = null;
		public IVixenMDI ExecutionParent = null;

		public OutputPlugInUIBase()
		{
			InitializeComponent();
		}
	}
}