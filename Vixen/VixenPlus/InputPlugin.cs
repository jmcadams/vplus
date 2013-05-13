using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public abstract class InputPlugin : IInputPlugin
    {
        public enum MappingIterator
        {
            None,
            SingleInput,
            MultiInput
        }

        private bool _isRecord;

        //private const string AttributeId = "id";
        //private const string AttributeLiveUpdate = "liveUpdate";
        //private const string AttributeMappingId = "mappingId";
        //private const string AttributeName = "name";
        //private const string AttributeRecord = "record";
        //private const string AttributeType = "type";
        //private const string ElementInput = "Input";
        //private const string ElementInputs = "Inputs";
        //private const string ElementIterator = "Iterator";
        //private const string ElementMappingSets = "MappingSets";
        //private const int InvalidId = 0;
        private bool _liveUpdate;
        private MappingIterator _mappingIterator = MappingIterator.None;
        private MappingSets _mappingSets = new MappingSets();
        private XmlNode _setupNode;
        private Input _singleIterator;

        public MappingIterator MappingIteratorType
        {
            get { return _mappingIterator; }
            set { _mappingIterator = value; }
        }

        internal MappingSets MappingSets
        {
            get { return _mappingSets; }
            set { _mappingSets = value; }
        }

        public Input SingleIterator
        {
            get { return _singleIterator; }
            set { _singleIterator = value; }
        }

        public abstract void Initialize(SetupData setupData, XmlNode setupNode);

        public virtual void Setup()
        {
        }

        public virtual void Shutdown()
        {
        }

        public virtual void Startup()
        {
        }

        public abstract string Author { get; }

        public abstract string Description { get; }

        public abstract HardwareMap[] HardwareMap { get; }

        public abstract Input[] Inputs { get; }

        public bool LiveUpdate
        {
            get { return _liveUpdate; }
            set { _liveUpdate = value; }
        }

        public abstract string Name { get; }

        public bool Record
        {
            get { return _isRecord; }
            set { _isRecord = value; }
        }

        private Input FindInput(ulong id)
        {
            if (id == 0L)
            {
                return null;
            }
            return Array.Find(Inputs, i => i.Id == id);
        }

        public Input[] GetIterators()
        {
            var list = new List<Input>();
            foreach (Input input in Inputs)
            {
                if (input.IsMappingIterator)
                {
                    list.Add(input);
                }
            }
            return list.ToArray();
        }

        internal void InitializeInternal(SetupData setupData, XmlNode setupNode)
        {
            _setupNode = setupNode;
            Initialize(setupData, setupNode);
        }

        internal void IteratorTriggered(Input input)
        {
            if (_mappingIterator == MappingIterator.SingleInput)
            {
                if (input == _singleIterator)
                {
                    _mappingSets.StepMapping();
                }
            }
            else
            {
                _mappingSets.CurrentMappingSet = input.AssignedMappingSet;
            }
        }

        internal void PluginToSetupData()
        {
            _mappingSets.WriteData(Xml.GetEmptyNodeAlways(_setupNode, "MappingSets"));
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(_setupNode, "Inputs");
            foreach (Input input in Inputs)
            {
                input.WriteData(emptyNodeAlways);
            }
            XmlNode node = Xml.GetEmptyNodeAlways(_setupNode, "Iterator");
            Xml.SetAttribute(node, "type", _mappingIterator.ToString());
            Input[] iterators = GetIterators();
            if (iterators.Length > 0)
            {
                if (MappingIteratorType == MappingIterator.SingleInput)
                {
                    Xml.SetAttribute(node, "Input", "id", _singleIterator.Id.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    foreach (Input input in iterators)
                    {
                        XmlNode node2 = Xml.SetNewValue(node, "Input", "");
                        Xml.SetAttribute(node2, "id", input.Id.ToString(CultureInfo.InvariantCulture));
                        Xml.SetAttribute(node2, "mappingId",
                                         (input.AssignedMappingSet != null)
                                             ? input.AssignedMappingSet.Id.ToString(CultureInfo.InvariantCulture)
                                             : 0.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
            Xml.SetAttribute(_setupNode, "liveUpdate", _liveUpdate.ToString());
            Xml.SetAttribute(_setupNode, "record", _isRecord.ToString());
        }

        internal void SetupDataToPlugin()
        {
            XmlNode node = _setupNode["MappingSets"];
            if (node != null)
            {
                _mappingSets.ReadData(node);
            }
            XmlNode node2 = _setupNode["Inputs"];
            if (node2 != null)
            {
                foreach (Input input in Inputs)
                {
                    XmlNode node3 = node2.SelectSingleNode(string.Format("{0}[@{1}=\"{2}\"]", "Input", "name", input.Name));
                    if (node3 != null)
                    {
                        input.ReadData(node3);
                    }
                }
            }
            XmlNode node4 = _setupNode["Iterator"];
            if (node4 != null)
            {
                if (node4.Attributes != null)
                {
                    MappingIteratorType = (MappingIterator) Enum.Parse(typeof (MappingIterator), node4.Attributes["type"].Value);
                }
            }
            else
            {
                MappingIteratorType = MappingIterator.None;
            }
            _singleIterator = null;
            if (node4 != null)
            {
                if (MappingIteratorType == MappingIterator.SingleInput)
                {
                    XmlNode node5 = node4["Input"];
                    if (node5 != null)
                    {
                        if (node5.Attributes != null)
                        {
                            _singleIterator = FindInput(ulong.Parse(node5.Attributes["id"].Value));
                        }
                    }
                }
                else if (MappingIteratorType == MappingIterator.MultiInput)
                {
                    XmlNodeList inputNodes = node4.SelectNodes("Input");
                    if (inputNodes != null)
                    {
                        foreach (XmlNode node5 in inputNodes)
                        {
                            if (node5.Attributes != null)
                            {
                                Input input2 = FindInput(ulong.Parse(node5.Attributes["id"].Value));
                                MappingSet set = _mappingSets.FindMappingSet(ulong.Parse(node5.Attributes["mappingId"].Value));
                                if (input2 != null)
                                {
                                    input2.AssignedMappingSet = set;
                                }
                            }
                        }
                    }
                }
            }
            if (_setupNode.Attributes != null && _setupNode.Attributes["liveUpdate"] != null)
            {
                _liveUpdate = bool.Parse(_setupNode.Attributes["liveUpdate"].Value);
            }
            if (_setupNode.Attributes != null && _setupNode.Attributes["record"] != null)
            {
                _isRecord = bool.Parse(_setupNode.Attributes["record"].Value);
            }
        }

        internal void ShutdownInternal()
        {
            Shutdown();
        }

        internal void StartupInternal()
        {
            Startup();
        }
    }
}