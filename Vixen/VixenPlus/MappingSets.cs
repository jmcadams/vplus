using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Vixen
{
	internal class MappingSets : ICloneable, IEnumerable<MappingSet>, IEnumerable
	{
		public const string DEFAULT_SET_NAME = "Mapping set 1";
		private const string ELEMENT_MAPPING_SET = "MappingSet";
		private const int INVALID_ID = 0;
		private readonly List<MappingSet> m_mappingSets = new List<MappingSet>();
		private int m_currentMappingSetIndex;

		public MappingSets()
		{
			m_mappingSets.Add(new MappingSet("Mapping set 1"));
			m_currentMappingSetIndex = 0;
		}

		internal MappingSet[] AllSets
		{
			get { return m_mappingSets.ToArray(); }
		}

		public int Count
		{
			get { return m_mappingSets.Count; }
		}

		public MappingSet CurrentMappingSet
		{
			get { return m_mappingSets[m_currentMappingSetIndex]; }
			set
			{
				if (value != null)
				{
					m_currentMappingSetIndex = FindMappingSetIndex(value);
				}
			}
		}

		public MappingSet this[int index]
		{
			get { return m_mappingSets[index]; }
		}

		public object Clone()
		{
			var sets = new MappingSets();
			sets.m_currentMappingSetIndex = m_currentMappingSetIndex;
			sets.m_mappingSets.Clear();
			sets.m_mappingSets.AddRange(m_mappingSets);
			sets.m_mappingSets.ForEach(delegate(MappingSet m) { m = (MappingSet) m.Clone(); });
			return sets;
		}

		public IEnumerator<MappingSet> GetEnumerator()
		{
			return m_mappingSets.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_mappingSets.GetEnumerator();
		}

		public MappingSet AddMapping()
		{
			var item = new MappingSet("Mapping set " + (m_mappingSets.Count + 1));
			m_mappingSets.Add(item);
			return item;
		}

		private void CheckIndex()
		{
			if (m_currentMappingSetIndex >= m_mappingSets.Count)
			{
				m_currentMappingSetIndex = m_mappingSets.Count - 1;
			}
		}

		public MappingSet FindMappingSet(ulong id)
		{
			if (id == 0L)
			{
				return null;
			}
			return m_mappingSets.Find(delegate(MappingSet m) { return m.Id == id; });
		}

		public int FindMappingSetIndex(MappingSet mappingSet)
		{
			if (mappingSet == null)
			{
				return -1;
			}
			return m_mappingSets.FindIndex(delegate(MappingSet m) { return m == mappingSet; });
		}

		public MappingSet GetMappingSet(string mappingSetName, Input input)
		{
			foreach (MappingSet set in m_mappingSets)
			{
				if (string.Equals(set.Name, mappingSetName, StringComparison.OrdinalIgnoreCase))
				{
					set.GetOutputChannelIdList(input);
					return set;
				}
			}
			return null;
		}

		public int GetMappingSetCountFor(Input input)
		{
			int num = 0;
			foreach (MappingSet set in m_mappingSets)
			{
				if (set.HasMappingFor(input))
				{
					num++;
				}
			}
			return num;
		}

		internal List<string> GetOutputChannelIdList(Input input)
		{
			return m_mappingSets[m_currentMappingSetIndex].GetOutputChannelIdList(input);
		}

		public void InsertMappingAt(int index)
		{
			m_mappingSets.Insert(index, new MappingSet("Mapping set " + (index + 1)));
		}

		public void MoveMappingTo(int oldIndex, int newIndex)
		{
			MappingSet item = m_mappingSets[oldIndex];
			m_mappingSets.RemoveAt(oldIndex);
			m_mappingSets.Insert(newIndex, item);
		}

		public void ReadData(XmlNode node)
		{
			m_mappingSets.Clear();
			foreach (XmlNode node2 in node.SelectNodes("MappingSet"))
			{
				m_mappingSets.Add(new MappingSet(node2));
			}
		}

		public void RemoveMappingAt(int index)
		{
			m_mappingSets.RemoveAt(index);
			CheckIndex();
		}

		public void RenameMapping(ulong id, string name)
		{
			MappingSet set = FindMappingSet(id);
			if (set != null)
			{
				set.Name = name;
			}
		}

		public void StepMapping()
		{
			if (++m_currentMappingSetIndex == m_mappingSets.Count)
			{
				m_currentMappingSetIndex = 0;
			}
		}

		public void WriteData(XmlNode node)
		{
			foreach (MappingSet set in m_mappingSets)
			{
				XmlNode dataNode = Xml.SetNewValue(node, "MappingSet", "");
				set.WriteData(dataNode);
			}
		}
	}
}