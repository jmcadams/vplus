using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public class SortOrders : IEnumerable<SortOrder>
    {
        private readonly List<SortOrder> _sortOrders = new List<SortOrder>();
        private int _lastSort = -1;

        public SortOrder CurrentOrder
        {
            get
            {
                if (LastSort == -1)
                {
                    return null;
                }
                return _sortOrders[LastSort];
            }
            set
            {
                for (int i = 0; i < _sortOrders.Count; i++)
                {
                    if (_sortOrders[i].Name == value.Name)
                    {
                        LastSort = i;
                    }
                }
            }
        }

        public int LastSort
        {
            get { return _lastSort; }
            set { _lastSort = value; }
        }

        public IEnumerator<SortOrder> GetEnumerator()
        {
            return _sortOrders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sortOrders.GetEnumerator();
        }

        public void Add(SortOrder sortOrder)
        {
            _sortOrders.Add(sortOrder);
        }

        internal SortOrders Clone()
        {
            var target = new SortOrders();
            CloneTo(target);
            return target;
        }

        private void CloneTo(SortOrders target)
        {
            target._lastSort = _lastSort;
            target._sortOrders.Clear();
            target._sortOrders.AddRange(_sortOrders);
        }

        public void DeleteChannel(int naturalIndex)
        {
            foreach (SortOrder order in _sortOrders)
            {
                order.ChannelIndexes.Remove(naturalIndex);
                for (int i = 0; i < order.ChannelIndexes.Count; i++)
                {
                    if (order.ChannelIndexes[i] > naturalIndex)
                    {
                        List<int> list;
                        int num2;
                        (list = order.ChannelIndexes)[num2 = i] = list[num2] - 1;
                    }
                }
            }
        }

        public void InsertChannel(int naturalIndex, int currentSortIndex)
        {
            for (int i = 0; i < _sortOrders.Count; i++)
            {
                SortOrder order = _sortOrders[i];
                int index = (i == LastSort) ? currentSortIndex : naturalIndex;
                for (int j = 0; j < order.ChannelIndexes.Count; j++)
                {
                    if (order.ChannelIndexes[j] >= naturalIndex)
                    {
                        List<int> list;
                        int num4;
                        (list = order.ChannelIndexes)[num4 = j] = list[num4] + 1;
                    }
                }
                order.ChannelIndexes.Insert(index, naturalIndex);
            }
        }

        public void LoadFrom(SortOrders sortOrders)
        {
            sortOrders.CloneTo(this);
        }

        public void LoadFromXml(XmlNode contextNode)
        {
            _sortOrders.Clear();
            XmlNode node = contextNode.SelectSingleNode("SortOrders");
            if (node != null)
            {
                foreach (XmlNode node2 in node.SelectNodes("SortOrder"))
                {
                    _sortOrders.Add(new SortOrder(node2));
                }
                XmlAttribute attribute = node.Attributes["lastSort"];
                if (attribute != null)
                {
                    _lastSort = Convert.ToInt32(attribute.Value);
                }
            }
        }

        public void Remove(SortOrder sortOrder)
        {
            _sortOrders.Remove(sortOrder);
        }

        public void SaveToXml(XmlNode contextNode)
        {
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "SortOrders");
            Xml.SetAttribute(emptyNodeAlways, "lastSort", _lastSort.ToString(CultureInfo.InvariantCulture));
            XmlDocument ownerDocument = contextNode.OwnerDocument;
            foreach (SortOrder order in _sortOrders)
            {
                emptyNodeAlways.AppendChild(order.SaveToXml(ownerDocument));
            }
        }

        public void UpdateChannelCounts(int count)
        {
            foreach (SortOrder order in _sortOrders)
            {
                int current;
                if (order.ChannelIndexes.Count > count)
                {
                    var list = new List<int>();
                    list.AddRange(order.ChannelIndexes);
                    using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            current = enumerator2.Current;
                            if (current >= count)
                            {
                                order.ChannelIndexes.Remove(current);
                            }
                        }
                    }
                }
                else
                {
                    for (current = order.ChannelIndexes.Count; current < count; current++)
                    {
                        order.ChannelIndexes.Add(current);
                    }
                }
            }
        }
    }
}