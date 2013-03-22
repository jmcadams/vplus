namespace Vixen
{
    using System;
    using System.Xml;

    public abstract class Input : ICloneable
    {
        private MappingSet m_assignedMappingSet;
        private bool m_enabled;
        private ulong m_id;
        private bool m_isMappingIterator;
        private string m_name;
        private InputPlugin m_owner;
        private bool m_wasChanged = false;

        public Input(InputPlugin owner, string name, bool isIterator)
        {
            this.m_owner = owner;
            this.m_name = name;
            this.m_enabled = true;
            this.m_isMappingIterator = isIterator;
            this.m_owner.MappingSets.GetMappingSet("Mapping set 1", this);
            this.m_id = Host.GetUniqueKey();
        }

        public object Clone()
        {
            Input input = (Input) base.MemberwiseClone();
            input.m_id = this.m_id;
            return input;
        }

        internal bool GetChangedInternal()
        {
            if (this.m_isMappingIterator)
            {
                bool changed = this.Changed;
                if (!(this.m_wasChanged || !changed))
                {
                    this.m_owner.IteratorTriggered(this);
                }
                this.m_wasChanged = changed;
                return false;
            }
            return this.Changed;
        }

        public abstract byte GetValue();
        internal byte GetValueInternal()
        {
            if (this.m_isMappingIterator)
            {
                return 0;
            }
            return this.GetValue();
        }

        public void ReadData(XmlNode node)
        {
            this.m_name = node.Attributes["name"].Value;
            this.m_enabled = bool.Parse(node.Attributes["enabled"].Value);
            this.m_id = ulong.Parse(node.Attributes["id"].Value);
            this.m_isMappingIterator = bool.Parse(node.Attributes["isIterator"].Value);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public XmlNode WriteData(XmlNode parentNode)
        {
            XmlNode node = Xml.SetNewValue(parentNode, "Input", "");
            Xml.SetAttribute(node, "name", this.Name);
            Xml.SetAttribute(node, "enabled", this.Enabled.ToString());
            Xml.SetAttribute(node, "id", this.Id.ToString());
            Xml.SetAttribute(node, "isIterator", this.m_isMappingIterator.ToString());
            return node;
        }

        public MappingSet AssignedMappingSet
        {
            get
            {
                return this.m_assignedMappingSet;
            }
            set
            {
                this.m_assignedMappingSet = value;
            }
        }

        public abstract bool Changed { get; }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
            }
        }

        public ulong Id
        {
            get
            {
                return this.m_id;
            }
        }

        public bool IsMappingIterator
        {
            get
            {
                return this.m_isMappingIterator;
            }
            set
            {
                this.m_isMappingIterator = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        internal InputPlugin Owner
        {
            get
            {
                return this.m_owner;
            }
        }
    }
}

