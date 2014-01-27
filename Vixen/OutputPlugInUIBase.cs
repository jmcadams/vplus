using System.Windows.Forms;
using System.Xml;

public partial class OutputPlugInUIBase : Form
{
    public XmlNode DataNode;
    public IVixenMDI ExecutionParent;


    protected OutputPlugInUIBase()
    {
        InitializeComponent();
    }
}