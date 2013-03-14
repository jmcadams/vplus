namespace WattMeter
{
    using System;
    using System.Xml;
    using Vixen;

    public class WattMeter : IAddIn, ILoadable, IPlugIn
    {
        private XmlNode m_dataNode;
        private EventSequence m_sequence = null;

        public bool Execute(EventSequence sequence)
        {
            this.m_sequence = sequence;
            WattMeterDialog dialog = new WattMeterDialog(this.m_sequence, this.m_dataNode);
            dialog.ShowDialog();
            dialog.Dispose();
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
            this.m_dataNode = dataNode;
        }

        public void Unloading()
        {
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Sequence;
            }
        }

        public string Description
        {
            get
            {
                return "Calculates loads based on sequence data";
            }
        }

        public string Name
        {
            get
            {
                return "Watt meter";
            }
        }
    }
}

