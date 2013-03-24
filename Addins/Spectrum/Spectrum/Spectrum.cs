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
            bool flag2;
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
            bool flag5 = true;
            bool.TryParse(Xml.GetNodeAlways(this.m_dataNode, "LockSliders", flag5.ToString()).InnerText, out result);
            int num2 = int.Parse(Xml.GetNodeAlways(this.m_dataNode, "LastStartChannel", "0").InnerText);
            int num3 = int.Parse(Xml.GetNodeAlways(this.m_dataNode, "LastStartBand", "0").InnerText);
            List<FrequencyBandMapping> list = new List<FrequencyBandMapping>();
            foreach (XmlNode node in this.m_dataNode.SelectNodes("Bands/*"))
            {
                list.Add(new FrequencyBandMapping(node));
            }
            SpectrumDialog dialog = new SpectrumDialog(sequence);
            dialog.ScaleFactor = num;
            dialog.LockSliders = result;
            dialog.LastStartChannel = num2;
            dialog.LastStartBand = num3;
            dialog.Mappings = list.ToArray();
	        flag2 = dialog.ShowDialog() == DialogResult.OK;
            if (flag2)
            {
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "Bands");
                foreach (FrequencyBandMapping mapping in dialog.Mappings)
                {
                    mapping.SaveToXml(emptyNodeAlways);
                }
            }
            Xml.SetValue(this.m_dataNode, "Scale", dialog.ScaleFactor.ToString());
            Xml.SetValue(this.m_dataNode, "LockSliders", dialog.LockSliders.ToString());
            Xml.SetValue(this.m_dataNode, "LastStartChannel", dialog.LastStartChannel.ToString());
            Xml.SetValue(this.m_dataNode, "LastStartBand", dialog.LastStartBand.ToString());
            dialog.Dispose();
            return flag2;
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
                return "Frequency spectrum";
            }
        }
    }
}

