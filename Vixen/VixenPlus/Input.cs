using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public abstract class Input : ICloneable
    {
        private readonly InputPlugin _owner;
        private ulong _id;
        private bool _isEnabled;
        private bool _isMappingIterator;
        private string _name;
        private bool _wasChanged;

        protected Input(InputPlugin owner, string name, bool isIterator)
        {
            _owner = owner;
            _name = name;
            _isEnabled = true;
            _isMappingIterator = isIterator;
            _owner.MappingSets.GetMappingSet("Mapping set 1", this);
            _id = Host.GetUniqueKey();
        }

        public MappingSet AssignedMappingSet { get; set; }

        public abstract bool Changed { get; }

        public bool Enabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public ulong Id
        {
            get { return _id; }
        }

        public bool IsMappingIterator
        {
            get { return _isMappingIterator; }
            set { _isMappingIterator = value; }
        }

        public string Name
        {
            get { return _name; }
        }

        internal InputPlugin Owner
        {
            get { return _owner; }
        }

        public object Clone()
        {
            var input = (Input) MemberwiseClone();
            input._id = _id;
            return input;
        }

        internal bool GetChangedInternal()
        {
            if (_isMappingIterator)
            {
                bool changed = Changed;
                if (!(_wasChanged || !changed))
                {
                    _owner.IteratorTriggered(this);
                }
                _wasChanged = changed;
                return false;
            }
            return Changed;
        }

        public abstract byte GetValue();

        internal byte GetValueInternal()
        {
            if (_isMappingIterator)
            {
                return 0;
            }
            return GetValue();
        }

        public void ReadData(XmlNode node)
        {
            if (node.Attributes != null)
            {
                _name = node.Attributes["name"].Value;
                _isEnabled = bool.Parse(node.Attributes["enabled"].Value);
                _id = ulong.Parse(node.Attributes["id"].Value);
                _isMappingIterator = bool.Parse(node.Attributes["isIterator"].Value);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public XmlNode WriteData(XmlNode parentNode)
        {
            XmlNode node = Xml.SetNewValue(parentNode, "Input", "");
            Xml.SetAttribute(node, "name", Name);
            Xml.SetAttribute(node, "enabled", Enabled.ToString());
            Xml.SetAttribute(node, "id", Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "isIterator", _isMappingIterator.ToString());
            return node;
        }
    }
}