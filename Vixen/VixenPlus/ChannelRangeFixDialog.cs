namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

	[Obsolete]
    internal partial class ChannelRangeFixDialog : Form
    {
        private XmlDocument m_doc;
        private int m_lastIndex = -1;

        public ChannelRangeFixDialog(XmlDocument doc)
        {
            this.InitializeComponent();
            this.m_doc = doc;
            this.labelChannelCount.Text = this.m_doc.SelectNodes("//Program/Channels/Channel").Count.ToString();
            foreach (XmlNode node in this.m_doc.SelectNodes("//Program/PlugInData/PlugIn"))
            {
                this.listBoxPlugIns.Items.Add(new PlugInMapping(node));
            }
        }

        public int From(int plugInIndex)
        {
            if (plugInIndex < this.listBoxPlugIns.Items.Count)
            {
                return ((PlugInMapping) this.listBoxPlugIns.Items[plugInIndex]).From;
            }
            return 0;
        }

        private void listBoxPlugIns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.m_lastIndex != -1)
            {
                PlugInMapping mapping = (PlugInMapping) this.listBoxPlugIns.Items[this.m_lastIndex];
                try
                {
                    mapping.From = Convert.ToInt32(this.textBoxFrom.Text);
                }
                catch
                {
                    mapping.From = 0;
                }
                try
                {
                    mapping.To = Convert.ToInt32(this.textBoxTo.Text);
                }
                catch
                {
                    mapping.To = 0;
                }
            }
            if (this.listBoxPlugIns.SelectedItem != null)
            {
                PlugInMapping selectedItem = (PlugInMapping) this.listBoxPlugIns.SelectedItem;
                this.textBoxFrom.Text = selectedItem.From.ToString();
                this.textBoxTo.Text = selectedItem.To.ToString();
            }
        }

        public int To(int plugInIndex)
        {
            if (plugInIndex < this.listBoxPlugIns.Items.Count)
            {
                return ((PlugInMapping) this.listBoxPlugIns.Items[plugInIndex]).To;
            }
            return 0;
        }
    }
}
