namespace TriggerResponse
{
    using System;
    using System.IO;
    using System.Xml;
    using VixenPlus;

    internal class MappedTriggerResponse
    {
        private string m_description;
        private int m_ecHandle;
        private string m_key;
        private string m_sequenceFile;
        private WeakReference m_sequenceReference;
        private int m_triggerIndex;
        private string m_triggerType;

        public MappedTriggerResponse()
        {
            this.m_sequenceReference = null;
            this.m_triggerIndex = 0;
            this.m_sequenceFile = string.Empty;
        }

        public MappedTriggerResponse(XmlNode contextNode)
        {
            this.m_sequenceReference = null;
            this.m_triggerType = contextNode.Attributes["type"].Value;
            this.m_triggerIndex = Convert.ToInt32(contextNode.Attributes["index"].Value);
            this.m_description = contextNode.Attributes["description"].Value;
            this.m_sequenceFile = contextNode.InnerText;
            this.CreateKey();
        }

        private void CreateKey()
        {
            this.m_key = string.Format("{0}{1}", this.m_triggerType, this.m_triggerIndex);
        }

        public XmlNode SaveToXml(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("TriggerResponse");
            Xml.SetAttribute(node, "type", this.m_triggerType);
            Xml.SetAttribute(node, "index", this.m_triggerIndex.ToString());
            Xml.SetAttribute(node, "description", this.m_description);
            node.InnerText = this.m_sequenceFile;
            return node;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.m_triggerType, this.m_triggerIndex);
        }

        public string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }

        public int EcHandle
        {
            get
            {
                return this.m_ecHandle;
            }
            set
            {
                this.m_ecHandle = value;
            }
        }

        public string Key
        {
            get
            {
                return this.m_key;
            }
        }

        public EventSequence Sequence
        {
            get
            {
                if (this.m_sequenceFile == string.Empty)
                {
                    return null;
                }
                if (this.m_sequenceReference == null)
                {
                    this.m_sequenceReference = new WeakReference(new EventSequence(Path.Combine(Paths.SequencePath, this.m_sequenceFile)));
                }
                else if (!this.m_sequenceReference.IsAlive)
                {
                    this.m_sequenceReference.Target = new EventSequence(Path.Combine(Paths.SequencePath, this.m_sequenceFile));
                }
                return (EventSequence) this.m_sequenceReference.Target;
            }
        }

        public string SequenceFile
        {
            get
            {
                return this.m_sequenceFile;
            }
            set
            {
                this.m_sequenceFile = value;
            }
        }

        public int TriggerIndex
        {
            get
            {
                return this.m_triggerIndex;
            }
            set
            {
                this.m_triggerIndex = value;
                this.CreateKey();
            }
        }

        public string TriggerType
        {
            get
            {
                return this.m_triggerType;
            }
            set
            {
                this.m_triggerType = value;
                this.CreateKey();
            }
        }
    }
}

