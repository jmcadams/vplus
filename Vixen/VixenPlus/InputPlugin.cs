using System;
using System.Collections.Generic;
using System.Xml;

namespace Vixen
{
	public abstract class InputPlugin : IInputPlugin, IHardwarePlugin, IPlugIn, ISetup
	{
		public enum MappingIterator
		{
			None,
			SingleInput,
			MultiInput
		}

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
		private bool m_liveUpdate;
		private MappingIterator m_mappingIterator = MappingIterator.None;
		private MappingSets m_mappingSets = new MappingSets();
		private bool m_record;
		private XmlNode m_setupNode;
		private Input m_singleIterator;

		public MappingIterator MappingIteratorType
		{
			get { return m_mappingIterator; }
			set { m_mappingIterator = value; }
		}

		internal MappingSets MappingSets
		{
			get { return m_mappingSets; }
			set { m_mappingSets = value; }
		}

		public Input SingleIterator
		{
			get { return m_singleIterator; }
			set { m_singleIterator = value; }
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
			get { return m_liveUpdate; }
			set { m_liveUpdate = value; }
		}

		public abstract string Name { get; }

		public bool Record
		{
			get { return m_record; }
			set { m_record = value; }
		}

		private Input FindInput(ulong id)
		{
			if (id == 0L)
			{
				return null;
			}
			return Array.Find(Inputs, delegate(Input i) { return i.Id == id; });
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
			m_setupNode = setupNode;
			Initialize(setupData, setupNode);
		}

		internal void IteratorTriggered(Input input)
		{
			if (m_mappingIterator == MappingIterator.SingleInput)
			{
				if (input == m_singleIterator)
				{
					m_mappingSets.StepMapping();
				}
			}
			else
			{
				m_mappingSets.CurrentMappingSet = input.AssignedMappingSet;
			}
		}

		internal void PluginToSetupData()
		{
			XmlNode node2;
			m_mappingSets.WriteData(Xml.GetEmptyNodeAlways(m_setupNode, "MappingSets"));
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(m_setupNode, "Inputs");
			foreach (Input input in Inputs)
			{
				node2 = input.WriteData(emptyNodeAlways);
			}
			XmlNode node = Xml.GetEmptyNodeAlways(m_setupNode, "Iterator");
			Xml.SetAttribute(node, "type", m_mappingIterator.ToString());
			Input[] iterators = GetIterators();
			if (iterators.Length > 0)
			{
				if (MappingIteratorType == MappingIterator.SingleInput)
				{
					Xml.SetAttribute(node, "Input", "id", m_singleIterator.Id.ToString());
				}
				else
				{
					foreach (Input input in iterators)
					{
						node2 = Xml.SetNewValue(node, "Input", "");
						Xml.SetAttribute(node2, "id", input.Id.ToString());
						Xml.SetAttribute(node2, "mappingId",
						                 (input.AssignedMappingSet != null) ? input.AssignedMappingSet.Id.ToString() : 0.ToString());
					}
				}
			}
			Xml.SetAttribute(m_setupNode, "liveUpdate", m_liveUpdate.ToString());
			Xml.SetAttribute(m_setupNode, "record", m_record.ToString());
		}

		internal void SetupDataToPlugin()
		{
			XmlNode node = m_setupNode["MappingSets"];
			if (node != null)
			{
				m_mappingSets.ReadData(node);
			}
			XmlNode node2 = m_setupNode["Inputs"];
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
			XmlNode node4 = m_setupNode["Iterator"];
			if (node4 != null)
			{
				MappingIteratorType = (MappingIterator) Enum.Parse(typeof (MappingIterator), node4.Attributes["type"].Value);
			}
			else
			{
				MappingIteratorType = MappingIterator.None;
			}
			m_singleIterator = null;
			if (node4 != null)
			{
				if (MappingIteratorType == MappingIterator.SingleInput)
				{
					XmlNode node5 = node4["Input"];
					if (node5 != null)
					{
						m_singleIterator = FindInput(ulong.Parse(node5.Attributes["id"].Value));
					}
				}
				else if (MappingIteratorType == MappingIterator.MultiInput)
				{
					foreach (XmlNode node5 in node4.SelectNodes("Input"))
					{
						Input input2 = FindInput(ulong.Parse(node5.Attributes["id"].Value));
						MappingSet set = m_mappingSets.FindMappingSet(ulong.Parse(node5.Attributes["mappingId"].Value));
						if (input2 != null)
						{
							input2.AssignedMappingSet = set;
						}
					}
				}
			}
			if (m_setupNode.Attributes["liveUpdate"] != null)
			{
				m_liveUpdate = bool.Parse(m_setupNode.Attributes["liveUpdate"].Value);
			}
			if (m_setupNode.Attributes["record"] != null)
			{
				m_record = bool.Parse(m_setupNode.Attributes["record"].Value);
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