using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace VixenPlus
{
    internal class MappingSets : ICloneable, IEnumerable<MappingSet>
    {
        public const string DefaultSetName = "Mapping set 1";
        private readonly List<MappingSet> _mappingSets = new List<MappingSet>();
        private int _currentMappingSetIndex;

        public MappingSets()
        {
            _mappingSets.Add(new MappingSet("Mapping set 1"));
            _currentMappingSetIndex = 0;
        }

        internal MappingSet[] AllSets
        {
            get { return _mappingSets.ToArray(); }
        }

        public int Count
        {
            get { return _mappingSets.Count; }
        }

        public MappingSet CurrentMappingSet
        {
            get { return _mappingSets[_currentMappingSetIndex]; }
            set
            {
                if (value != null)
                {
                    _currentMappingSetIndex = FindMappingSetIndex(value);
                }
            }
        }

        public MappingSet this[int index]
        {
            get { return _mappingSets[index]; }
        }


        public object Clone() {
            var sets = new MappingSets {_currentMappingSetIndex = _currentMappingSetIndex};
            sets._mappingSets.Clear();
            sets._mappingSets.AddRange(_mappingSets);
            //TODO Is this working as expected?
            // ReSharper disable RedundantAssignment
            sets._mappingSets.ForEach(delegate(MappingSet m) { m = (MappingSet) m.Clone(); });
            // ReSharper restore RedundantAssignment
            return sets;
        }


        public IEnumerator<MappingSet> GetEnumerator()
        {
            return _mappingSets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mappingSets.GetEnumerator();
        }

        public MappingSet AddMapping()
        {
            var item = new MappingSet("Mapping set " + (_mappingSets.Count + 1));
            _mappingSets.Add(item);
            return item;
        }

        private void CheckIndex()
        {
            if (_currentMappingSetIndex >= _mappingSets.Count)
            {
                _currentMappingSetIndex = _mappingSets.Count - 1;
            }
        }

        public MappingSet FindMappingSet(ulong id) {
            return id == 0L ? null : _mappingSets.Find(m => m.Id == id);
        }


        public int FindMappingSetIndex(MappingSet mappingSet)
        {
            if (mappingSet == null)
            {
                return -1;
            }
            return _mappingSets.FindIndex(m => m == mappingSet);
        }

        public MappingSet GetMappingSet(string mappingSetName, Input input)
        {
            foreach (MappingSet set in _mappingSets)
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
            foreach (MappingSet set in _mappingSets)
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
            return _mappingSets[_currentMappingSetIndex].GetOutputChannelIdList(input);
        }

        public void InsertMappingAt(int index)
        {
            _mappingSets.Insert(index, new MappingSet("Mapping set " + (index + 1)));
        }

        public void MoveMappingTo(int oldIndex, int newIndex)
        {
            MappingSet item = _mappingSets[oldIndex];
            _mappingSets.RemoveAt(oldIndex);
            _mappingSets.Insert(newIndex, item);
        }

        public void ReadData(XmlNode node)
        {
            _mappingSets.Clear();
            XmlNodeList mappingSetNode = node.SelectNodes("MappingSet");
            if (mappingSetNode != null)
            {
                foreach (XmlNode node2 in mappingSetNode)
                {
                    _mappingSets.Add(new MappingSet(node2));
                }
            }
        }

        public void RemoveMappingAt(int index)
        {
            _mappingSets.RemoveAt(index);
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
            if (++_currentMappingSetIndex == _mappingSets.Count)
            {
                _currentMappingSetIndex = 0;
            }
        }

        public void WriteData(XmlNode node)
        {
            foreach (MappingSet set in _mappingSets)
            {
                XmlNode dataNode = Xml.SetNewValue(node, "MappingSet", "");
                set.WriteData(dataNode);
            }
        }
    }
}