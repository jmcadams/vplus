using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public abstract class Input : ICloneable
    {
        private bool _isEnabled;
        private bool _isMappingIterator;
        private bool _wasChanged;

        protected Input(InputPlugin owner, string name, bool isIterator)
        {
            Owner = owner;
            Name = name;
            _isEnabled = true;
            _isMappingIterator = isIterator;
            Owner.MappingSets.GetMappingSet("Mapping set 1", this);
            Id = Host.GetUniqueKey();
        }

        public MappingSet AssignedMappingSet { get; set; }

        public abstract bool Changed { get; }

        public bool Enabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public ulong Id { get; private set; }

        public bool IsMappingIterator
        {
            get { return _isMappingIterator; }
            set { _isMappingIterator = value; }
        }

        public string Name { get; private set; }

        internal InputPlugin Owner { get; private set; }


        public object Clone()
        {
            var input = (Input) MemberwiseClone();
            input.Id = Id;
            return input;
        }

        internal bool GetChangedInternal()
        {
            if (!_isMappingIterator) {
                return Changed;
            }
            var changed = Changed;
            if (!(_wasChanged || !changed))
            {
                Owner.IteratorTriggered(this);
            }
            _wasChanged = changed;
            return false;
        }

        public abstract byte GetValue();

        internal byte GetValueInternal() {
            return _isMappingIterator ? (byte) 0 : GetValue();
        }


        public void ReadData(XmlNode node)
        {
            if (node.Attributes == null) {
                return;
            }
            Name = node.Attributes["name"].Value;
            _isEnabled = bool.Parse(node.Attributes["enabled"].Value);
            Id = ulong.Parse(node.Attributes["id"].Value);
            _isMappingIterator = bool.Parse(node.Attributes["isIterator"].Value);
        }

        public override string ToString()
        {
            return Name;
        }

        public XmlNode WriteData(XmlNode parentNode)
        {
            var node = Xml.SetNewValue(parentNode, "Input", "");
            Xml.SetAttribute(node, "name", Name);
            Xml.SetAttribute(node, "enabled", Enabled.ToString());
            Xml.SetAttribute(node, "id", Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "isIterator", _isMappingIterator.ToString());
            return node;
        }
    }
}