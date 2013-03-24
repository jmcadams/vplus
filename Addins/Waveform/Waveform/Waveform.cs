namespace Waveform
{
    using System;
    using System.Xml;
    using Vixen;

    public class Waveform : IAddIn, ILoadable, IPlugIn
    {
        private XmlNode m_dataNode;
        private EventSequence m_sequence = null;

        public bool Execute(EventSequence sequence)
        {
            this.m_sequence = sequence;
            if (this.m_sequence == null)
            {
                throw new Exception("Waveform add-in requires a sequence.");
            }
            if (this.m_sequence.Audio == null)
            {
                throw new Exception("Waveform add-in requires the sequence to have audio assigned.");
            }
            ParamsDialog dialog = new ParamsDialog(this.m_sequence);
            dialog.ShowDialog();
            bool completed = dialog.Completed;
            dialog.Dispose();
            return completed;
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
                return "Creates sequence event data based on an audio waveform";
            }
        }

        public string Name
        {
            get
            {
                return "Waveform";
            }
        }
    }
}

