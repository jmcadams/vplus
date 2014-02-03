 //TODO: What is this used for?

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace VixenPlus {
    internal class MappingSets : ICloneable, IEnumerable<MappingSet>
    {
        private const string DefaultSetName = "Mapping set 1";
        private readonly List<MappingSet> _mappingSets = new List<MappingSet>();
        private int _currentMappingSetIndex;

        public MappingSets()
        {
            _mappingSets.Add(new MappingSet(DefaultSetName));
            _currentMappingSetIndex = 0;
        }

        internal MappingSet[] AllSets
        {
            get { return _mappingSets.ToArray(); }
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


        private int FindMappingSetIndex(MappingSet mappingSet)
        {
            if (mappingSet == null)
            {
                return -1;
            }
            return _mappingSets.FindIndex(m => m == mappingSet);
        }

        public void GetMappingSet(string mappingSetName, Input input) {
            foreach (var set in _mappingSets.Where(set => string.Equals(set.Name, mappingSetName, StringComparison.OrdinalIgnoreCase))) {
                set.GetOutputChannelIdList(input);
                return;
            }
        }

        internal List<string> GetOutputChannelIdList(Input input)
        {
            return _mappingSets[_currentMappingSetIndex].GetOutputChannelIdList(input);
        }

        public void MoveMappingTo(int oldIndex, int newIndex)
        {
            var item = _mappingSets[oldIndex];
            _mappingSets.RemoveAt(oldIndex);
            _mappingSets.Insert(newIndex, item);
        }

        public void ReadData(XmlNode node)
        {
            _mappingSets.Clear();
            var mappingSetNode = node.SelectNodes("MappingSet");
            if (mappingSetNode == null) {
                return;
            }
            foreach (XmlNode node2 in mappingSetNode)
            {
                _mappingSets.Add(new MappingSet(node2));
            }
        }

        public void RemoveMappingAt(int index)
        {
            _mappingSets.RemoveAt(index);
            CheckIndex();
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
            foreach (var set in _mappingSets)
            {
                var dataNode = Xml.SetNewValue(node, "MappingSet", "");
                set.WriteData(dataNode);
            }
        }
    }
}