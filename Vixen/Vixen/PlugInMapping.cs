namespace Vixen
{
    using System;
    using System.Xml;

    internal class PlugInMapping
    {
        public int From;
        public XmlNode Node;
        public int To;

        public PlugInMapping(XmlNode node)
        {
            try
            {
                this.From = Convert.ToInt32(node.Attributes["from"].Value);
            }
            catch
            {
                this.From = 0;
            }
            try
            {
                this.To = Convert.ToInt32(node.Attributes["to"].Value);
            }
            catch
            {
                this.To = 0;
            }
            this.Node = node;
        }

        public override string ToString()
        {
            return this.Node.Attributes["name"].Value;
        }
    }
}

