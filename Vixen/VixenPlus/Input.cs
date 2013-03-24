using System;
using System.Xml;

namespace Vixen
{
	public abstract class Input : ICloneable
	{
		private readonly InputPlugin m_owner;
		private bool m_enabled;
		private ulong m_id;
		private bool m_isMappingIterator;
		private string m_name;
		private bool m_wasChanged;

		public Input(InputPlugin owner, string name, bool isIterator)
		{
			m_owner = owner;
			m_name = name;
			m_enabled = true;
			m_isMappingIterator = isIterator;
			m_owner.MappingSets.GetMappingSet("Mapping set 1", this);
			m_id = Host.GetUniqueKey();
		}

		public MappingSet AssignedMappingSet { get; set; }

		public abstract bool Changed { get; }

		public bool Enabled
		{
			get { return m_enabled; }
			set { m_enabled = value; }
		}

		public ulong Id
		{
			get { return m_id; }
		}

		public bool IsMappingIterator
		{
			get { return m_isMappingIterator; }
			set { m_isMappingIterator = value; }
		}

		public string Name
		{
			get { return m_name; }
		}

		internal InputPlugin Owner
		{
			get { return m_owner; }
		}

		public object Clone()
		{
			var input = (Input) base.MemberwiseClone();
			input.m_id = m_id;
			return input;
		}

		internal bool GetChangedInternal()
		{
			if (m_isMappingIterator)
			{
				bool changed = Changed;
				if (!(m_wasChanged || !changed))
				{
					m_owner.IteratorTriggered(this);
				}
				m_wasChanged = changed;
				return false;
			}
			return Changed;
		}

		public abstract byte GetValue();

		internal byte GetValueInternal()
		{
			if (m_isMappingIterator)
			{
				return 0;
			}
			return GetValue();
		}

		public void ReadData(XmlNode node)
		{
			m_name = node.Attributes["name"].Value;
			m_enabled = bool.Parse(node.Attributes["enabled"].Value);
			m_id = ulong.Parse(node.Attributes["id"].Value);
			m_isMappingIterator = bool.Parse(node.Attributes["isIterator"].Value);
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
			Xml.SetAttribute(node, "id", Id.ToString());
			Xml.SetAttribute(node, "isIterator", m_isMappingIterator.ToString());
			return node;
		}
	}
}