using System.Globalization;

namespace Spectrum
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class Spectrum : IAddIn
    {
        private XmlNode _mDataNode;

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
            string optionalNodeValue = Xml.GetOptionalNodeValue(_mDataNode, "Scale");
            if (optionalNodeValue.Length > 0)
            {
                num = float.Parse(optionalNodeValue);
            }
            bool result;
            bool.TryParse(Xml.GetNodeAlways(_mDataNode, "LockSliders", true.ToString()).InnerText, out result);
            var list = new List<FrequencyBandMapping>();
	        var xmlNodeList = _mDataNode.SelectNodes("Bands/*");
	        if (xmlNodeList != null)
		        foreach (XmlNode node in xmlNodeList)
		        {
			        list.Add(new FrequencyBandMapping(node));
		        }
	        var dialog = new SpectrumDialog(sequence) {LockSliders = result, Mappings = list.ToArray(), ScaleFactor = num};
	        if (dialog.ShowDialog() == DialogResult.OK)
            {
                Xml.SetValue(_mDataNode, "Scale", dialog.ScaleFactor.ToString(CultureInfo.InvariantCulture));
                Xml.SetValue(_mDataNode, "LockSliders", dialog.LockSliders.ToString());
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(_mDataNode, "Bands");
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
            _mDataNode = dataNode;
        }

        public override string ToString()
        {
            return Name;
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

