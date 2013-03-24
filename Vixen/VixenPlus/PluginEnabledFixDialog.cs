namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

	[Obsolete]
    internal partial class PluginEnabledFixDialog : Form
    {
        public PluginEnabledFixDialog(XmlDocument doc)
        {
            this.InitializeComponent();
            foreach (XmlNode node in doc.SelectNodes("//Program/PlugInData/PlugIn"))
            {
                this.checkedListBoxPlugIns.Items.Add(node.Attributes["name"].Value);
            }
        }
        public bool PlugInEnabled(int index)
        {
            return this.checkedListBoxPlugIns.GetItemChecked(index);
        }
    }
}
