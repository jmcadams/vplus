namespace Spectrum
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Spectrum : IAddIn, ILoadable, IPlugIn
    {
        private XmlNode m_dataNode;

        public bool Execute(EventSequence sequence)
        {
            if (sequence == null)
            {
                throw new Exception("Frequency spectrum add-in requires a sequence.");
            }
            if (sequence.Audio == null)
            {
                throw new Exception("Frequency spectrum add-in requires the sequence to have audio assigned.");
            }
            float num = 1f;
            string optionalNodeValue = Xml.GetOptionalNodeValue(this.m_dataNode, "Scale");
            if (optionalNodeValue.Length > 0)
            {
                num = float.Parse(optionalNodeValue);
            }
            bool result = true;
            bool flag4 = true;
            bool.TryParse(Xml.GetNodeAlways(this.m_dataNode, "LockSliders", flag4.ToString()).InnerText, out result);
            List<FrequencyBandMapping> list = new List<FrequencyBandMapping>();
            foreach (XmlNode node in this.m_dataNode.SelectNodes("Bands/*"))
            {
                list.Add(new FrequencyBandMapping(node));
            }
            SpectrumDialog dialog = new SpectrumDialog(sequence);
            dialog.LockSliders = result;
            dialog.Mappings = list.ToArray();
            dialog.ScaleFactor = num;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Xml.SetValue(this.m_dataNode, "Scale", dialog.ScaleFactor.ToString());
                Xml.SetValue(this.m_dataNode, "LockSliders", dialog.LockSliders.ToString());
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "Bands");
                foreach (FrequencyBandMapping mapping in dialog.Mappings)
                {
                    mapping.SaveToXml(emptyNodeAlways);
                }
                return true;
            }
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
            this.m_dataNode = dataNode;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Unloading()
        {
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
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
                return "Audio waveform frequency spectrum analysis";
            }
        }

        public string Name
        {
            get
            {
                return "Analog frequency spectrum";
            }
        }
    }
}

