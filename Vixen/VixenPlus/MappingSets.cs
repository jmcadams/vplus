namespace Vixen
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;

    internal class MappingSets : ICloneable, IEnumerable<MappingSet>, IEnumerable
    {
        public const string DEFAULT_SET_NAME = "Mapping set 1";
        private const string ELEMENT_MAPPING_SET = "MappingSet";
        private const int INVALID_ID = 0;
        private int m_currentMappingSetIndex;
        private List<MappingSet> m_mappingSets = new List<MappingSet>();

        public MappingSets()
        {
            this.m_mappingSets.Add(new MappingSet("Mapping set 1"));
            this.m_currentMappingSetIndex = 0;
        }

        public MappingSet AddMapping()
        {
            MappingSet item = new MappingSet("Mapping set " + (this.m_mappingSets.Count + 1));
            this.m_mappingSets.Add(item);
            return item;
        }

        private void CheckIndex()
        {
            if (this.m_currentMappingSetIndex >= this.m_mappingSets.Count)
            {
                this.m_currentMappingSetIndex = this.m_mappingSets.Count - 1;
            }
        }

        public object Clone()
        {
            MappingSets sets = new MappingSets();
            sets.m_currentMappingSetIndex = this.m_currentMappingSetIndex;
            sets.m_mappingSets.Clear();
            sets.m_mappingSets.AddRange(this.m_mappingSets);
            sets.m_mappingSets.ForEach(delegate (MappingSet m) {
                m = (MappingSet) m.Clone();
            });
            return sets;
        }

        public MappingSet FindMappingSet(ulong id)
        {
            if (id == 0L)
            {
                return null;
            }
            return this.m_mappingSets.Find(delegate (MappingSet m) {
                return m.Id == id;
            });
        }

        public int FindMappingSetIndex(MappingSet mappingSet)
        {
            if (mappingSet == null)
            {
                return -1;
            }
            return this.m_mappingSets.FindIndex(delegate (MappingSet m) {
                return m == mappingSet;
            });
        }

        public IEnumerator<MappingSet> GetEnumerator()
        {
            return this.m_mappingSets.GetEnumerator();
        }

        public MappingSet GetMappingSet(string mappingSetName, Input input)
        {
            foreach (MappingSet set in this.m_mappingSets)
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
            foreach (MappingSet set in this.m_mappingSets)
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
            return this.m_mappingSets[this.m_currentMappingSetIndex].GetOutputChannelIdList(input);
        }

        public void InsertMappingAt(int index)
        {
            this.m_mappingSets.Insert(index, new MappingSet("Mapping set " + (index + 1)));
        }

        public void MoveMappingTo(int oldIndex, int newIndex)
        {
            MappingSet item = this.m_mappingSets[oldIndex];
            this.m_mappingSets.RemoveAt(oldIndex);
            this.m_mappingSets.Insert(newIndex, item);
        }

        public void ReadData(XmlNode node)
        {
            this.m_mappingSets.Clear();
            foreach (XmlNode node2 in node.SelectNodes("MappingSet"))
            {
                this.m_mappingSets.Add(new MappingSet(node2));
            }
        }

        public void RemoveMappingAt(int index)
        {
            this.m_mappingSets.RemoveAt(index);
            this.CheckIndex();
        }

        public void RenameMapping(ulong id, string name)
        {
            MappingSet set = this.FindMappingSet(id);
            if (set != null)
            {
                set.Name = name;
            }
        }

        public void StepMapping()
        {
            if (++this.m_currentMappingSetIndex == this.m_mappingSets.Count)
            {
                this.m_currentMappingSetIndex = 0;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_mappingSets.GetEnumerator();
        }

        public void WriteData(XmlNode node)
        {
            foreach (MappingSet set in this.m_mappingSets)
            {
                XmlNode dataNode = Xml.SetNewValue(node, "MappingSet", "");
                set.WriteData(dataNode);
            }
        }

        internal MappingSet[] AllSets
        {
            get
            {
                return this.m_mappingSets.ToArray();
            }
        }

        public int Count
        {
            get
            {
                return this.m_mappingSets.Count;
            }
        }

        public MappingSet CurrentMappingSet
        {
            get
            {
                return this.m_mappingSets[this.m_currentMappingSetIndex];
            }
            set
            {
                if (value != null)
                {
                    this.m_currentMappingSetIndex = this.FindMappingSetIndex(value);
                }
            }
        }

        public MappingSet this[int index]
        {
            get
            {
                return this.m_mappingSets[index];
            }
        }
    }
}

