namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public abstract class InputPlugin : IInputPlugin, IHardwarePlugin, IPlugIn, ISetup
    {
        private const string ATTRIBUTE_ID = "id";
        private const string ATTRIBUTE_LIVE_UPDATE = "liveUpdate";
        private const string ATTRIBUTE_MAPPING_ID = "mappingId";
        private const string ATTRIBUTE_NAME = "name";
        private const string ATTRIBUTE_RECORD = "record";
        private const string ATTRIBUTE_TYPE = "type";
        private const string ELEMENT_INPUT = "Input";
        private const string ELEMENT_INPUTS = "Inputs";
        private const string ELEMENT_ITERATOR = "Iterator";
        private const string ELEMENT_MAPPING_SETS = "MappingSets";
        private const int INVALID_ID = 0;
        private bool m_liveUpdate = false;
        private MappingIterator m_mappingIterator = MappingIterator.None;
        private Vixen.MappingSets m_mappingSets = new Vixen.MappingSets();
        private bool m_record = false;
        private XmlNode m_setupNode;
        private Input m_singleIterator = null;

        protected InputPlugin()
        {
        }

        private Input FindInput(ulong id)
        {
            if (id == 0L)
            {
                return null;
            }
            return Array.Find<Input>(this.Inputs, delegate (Input i) {
                return i.Id == id;
            });
        }

        public Input[] GetIterators()
        {
            List<Input> list = new List<Input>();
            foreach (Input input in this.Inputs)
            {
                if (input.IsMappingIterator)
                {
                    list.Add(input);
                }
            }
            return list.ToArray();
        }

        public abstract void Initialize(SetupData setupData, XmlNode setupNode);
        internal void InitializeInternal(SetupData setupData, XmlNode setupNode)
        {
            this.m_setupNode = setupNode;
            this.Initialize(setupData, setupNode);
        }

        internal void IteratorTriggered(Input input)
        {
            if (this.m_mappingIterator == MappingIterator.SingleInput)
            {
                if (input == this.m_singleIterator)
                {
                    this.m_mappingSets.StepMapping();
                }
            }
            else
            {
                this.m_mappingSets.CurrentMappingSet = input.AssignedMappingSet;
            }
        }

        internal void PluginToSetupData()
        {
            XmlNode node2;
            this.m_mappingSets.WriteData(Xml.GetEmptyNodeAlways(this.m_setupNode, "MappingSets"));
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_setupNode, "Inputs");
            foreach (Input input in this.Inputs)
            {
                node2 = input.WriteData(emptyNodeAlways);
            }
            XmlNode node = Xml.GetEmptyNodeAlways(this.m_setupNode, "Iterator");
            Xml.SetAttribute(node, "type", this.m_mappingIterator.ToString());
            Input[] iterators = this.GetIterators();
            if (iterators.Length > 0)
            {
                if (this.MappingIteratorType == MappingIterator.SingleInput)
                {
                    Xml.SetAttribute(node, "Input", "id", this.m_singleIterator.Id.ToString());
                }
                else
                {
                    foreach (Input input in iterators)
                    {
                        node2 = Xml.SetNewValue(node, "Input", "");
                        Xml.SetAttribute(node2, "id", input.Id.ToString());
                        Xml.SetAttribute(node2, "mappingId", (input.AssignedMappingSet != null) ? input.AssignedMappingSet.Id.ToString() : 0.ToString());
                    }
                }
            }
            Xml.SetAttribute(this.m_setupNode, "liveUpdate", this.m_liveUpdate.ToString());
            Xml.SetAttribute(this.m_setupNode, "record", this.m_record.ToString());
        }

        public virtual void Setup()
        {
        }

        internal void SetupDataToPlugin()
        {
            XmlNode node = this.m_setupNode["MappingSets"];
            if (node != null)
            {
                this.m_mappingSets.ReadData(node);
            }
            XmlNode node2 = this.m_setupNode["Inputs"];
            if (node2 != null)
            {
                foreach (Input input in this.Inputs)
                {
                    XmlNode node3 = node2.SelectSingleNode(string.Format("{0}[@{1}=\"{2}\"]", "Input", "name", input.Name));
                    if (node3 != null)
                    {
                        input.ReadData(node3);
                    }
                }
            }
            XmlNode node4 = this.m_setupNode["Iterator"];
            if (node4 != null)
            {
                this.MappingIteratorType = (MappingIterator) Enum.Parse(typeof(MappingIterator), node4.Attributes["type"].Value);
            }
            else
            {
                this.MappingIteratorType = MappingIterator.None;
            }
            this.m_singleIterator = null;
            if (node4 != null)
            {
                if (this.MappingIteratorType == MappingIterator.SingleInput)
                {
                    XmlNode node5 = node4["Input"];
                    if (node5 != null)
                    {
                        this.m_singleIterator = this.FindInput(ulong.Parse(node5.Attributes["id"].Value));
                    }
                }
                else if (this.MappingIteratorType == MappingIterator.MultiInput)
                {
                    foreach (XmlNode node5 in node4.SelectNodes("Input"))
                    {
                        Input input2 = this.FindInput(ulong.Parse(node5.Attributes["id"].Value));
                        MappingSet set = this.m_mappingSets.FindMappingSet(ulong.Parse(node5.Attributes["mappingId"].Value));
                        if (input2 != null)
                        {
                            input2.AssignedMappingSet = set;
                        }
                    }
                }
            }
            if (this.m_setupNode.Attributes["liveUpdate"] != null)
            {
                this.m_liveUpdate = bool.Parse(this.m_setupNode.Attributes["liveUpdate"].Value);
            }
            if (this.m_setupNode.Attributes["record"] != null)
            {
                this.m_record = bool.Parse(this.m_setupNode.Attributes["record"].Value);
            }
        }

        public virtual void Shutdown()
        {
        }

        internal void ShutdownInternal()
        {
            this.Shutdown();
        }

        public virtual void Startup()
        {
        }

        internal void StartupInternal()
        {
            this.Startup();
        }

        public abstract string Author { get; }

        public abstract string Description { get; }

        public abstract Vixen.HardwareMap[] HardwareMap { get; }

        public abstract Input[] Inputs { get; }

        public bool LiveUpdate
        {
            get
            {
                return this.m_liveUpdate;
            }
            set
            {
                this.m_liveUpdate = value;
            }
        }

        public MappingIterator MappingIteratorType
        {
            get
            {
                return this.m_mappingIterator;
            }
            set
            {
                this.m_mappingIterator = value;
            }
        }

        internal Vixen.MappingSets MappingSets
        {
            get
            {
                return this.m_mappingSets;
            }
            set
            {
                this.m_mappingSets = value;
            }
        }

        public abstract string Name { get; }

        public bool Record
        {
            get
            {
                return this.m_record;
            }
            set
            {
                this.m_record = value;
            }
        }

        public Input SingleIterator
        {
            get
            {
                return this.m_singleIterator;
            }
            set
            {
                this.m_singleIterator = value;
            }
        }

        public enum MappingIterator
        {
            None,
            SingleInput,
            MultiInput
        }
    }
}

