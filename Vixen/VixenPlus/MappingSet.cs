﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public class MappingSet : ICloneable
    {
        private readonly Dictionary<ulong, List<string>> _inputMappings;
        private readonly Dictionary<ulong, List<int>> _inputOutputMappings;


        public MappingSet(string name)
        {
            Name = name;
            _inputMappings = new Dictionary<ulong, List<string>>();
            _inputOutputMappings = new Dictionary<ulong, List<int>>();
            Id = Host.GetUniqueKey();
        }

        public MappingSet(XmlNode dataNode)
        {
            _inputMappings = new Dictionary<ulong, List<string>>();
            _inputOutputMappings = new Dictionary<ulong, List<int>>();
            ReadData(dataNode);
        }


        public ulong Id { get; private set; }

        public string Name { get; set; }

        public object Clone()
        {
            var set = new MappingSet(Name) {Id = Id};
            foreach (ulong num in _inputMappings.Keys)
            {
                List<string> list;
                set._inputMappings[num] = list = new List<string>();
                list.AddRange(_inputMappings[num]);
            }
            return set;
        }

        public List<string> GetOutputChannelIdList(ulong inputId)
        {
            List<string> list;
            if (!_inputMappings.TryGetValue(inputId, out list))
            {
                _inputMappings[inputId] = list = new List<string>();
            }
            return list;
        }

        public List<string> GetOutputChannelIdList(Input input)
        {
            return GetOutputChannelIdList(input.Id);
        }

        internal List<int> GetOutputChannelIndexList(Input input)
        {
            return _inputOutputMappings[input.Id];
        }

        public bool HasMappingFor(Input input)
        {
            return _inputMappings.ContainsKey(input.Id);
        }

        public void ReadData(XmlNode dataNode)
        {
            if (dataNode.Attributes != null)
            {
                Name = dataNode.Attributes["name"].Value;
                Id = ulong.Parse(dataNode.Attributes["id"].Value);
            }
            XmlNode node = dataNode["Inputs"];
            if (node != null)
            {
                XmlNodeList inputNodes = node.SelectNodes("Input");
                if (inputNodes != null)
                {
                    foreach (XmlNode node2 in inputNodes)
                    {
                        if (node2.InnerText.Trim().Length > 0)
                        {
                            if (node2.Attributes != null)
                            {
                                _inputMappings[ulong.Parse(node2.Attributes["id"].Value)] = new List<string>(node2.InnerText.Split(new[] {','}));
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void WriteData(XmlNode dataNode)
        {
            Xml.SetAttribute(dataNode, "name", Name);
            Xml.SetAttribute(dataNode, "id", Id.ToString(CultureInfo.InvariantCulture));
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(dataNode, "Inputs");
            foreach (ulong num in _inputMappings.Keys)
            {
                if (_inputMappings[num].Count > 0)
                {
                    Xml.SetAttribute(Xml.SetNewValue(emptyNodeAlways, "Input", string.Join(",", _inputMappings[num].ToArray())), "id",
                                     num.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}