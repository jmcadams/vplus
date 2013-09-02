using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VixenPlus {
    public class SortOrders : IEnumerable<SortOrder> {
        private readonly List<SortOrder> _sortOrders = new List<SortOrder>();


        public SortOrders() {
            LastSort = -1;
        }

        public SortOrder CurrentOrder {
            get { return LastSort == -1 ? null : _sortOrders[LastSort-1]; }
            set {
                for (var i = 0; i < _sortOrders.Count; i++) {
                    if (_sortOrders[i].Name == value.Name) {
                        LastSort = i + 1;
                    }
                }
            }
        }

        public int LastSort { get; set; }


        public IEnumerator<SortOrder> GetEnumerator() {
            return _sortOrders.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return _sortOrders.GetEnumerator();
        }


        public void Add(SortOrder sortOrder) {
            _sortOrders.Add(sortOrder);
        }


        internal SortOrders Clone() {
            var target = new SortOrders();
            CloneTo(target);
            return target;
        }


        private void CloneTo(SortOrders target) {
            target.LastSort = LastSort;
            target._sortOrders.Clear();
            target._sortOrders.AddRange(_sortOrders);
        }


        public void DeleteChannel(int naturalIndex) {
            foreach (var order in _sortOrders) {
                order.ChannelIndexes.Remove(naturalIndex);
                for (var i = 0; i < order.ChannelIndexes.Count; i++) {
                    if (order.ChannelIndexes[i] <= naturalIndex) {
                        continue;
                    }

                    List<int> list;
                    int num2;
                    (list = order.ChannelIndexes)[num2 = i] = list[num2] - 1;
                }
            }
        }


        public void InsertChannel(int naturalIndex, int currentSortIndex) {
            for (var i = 0; i < _sortOrders.Count; i++) {
                var order = _sortOrders[i];
                var index = (i == LastSort) ? currentSortIndex : naturalIndex;
                for (var j = 0; j < order.ChannelIndexes.Count; j++) {
                    if (order.ChannelIndexes[j] < naturalIndex) {
                        continue;
                    }

                    List<int> list;
                    int num4;
                    (list = order.ChannelIndexes)[num4 = j] = list[num4] + 1;
                }
                order.ChannelIndexes.Insert(index, naturalIndex);
            }
        }


        public void LoadFrom(SortOrders sortOrders) {
            sortOrders.CloneTo(this);
        }


        public void LoadFromXml(XmlNode contextNode) {
            _sortOrders.Clear();
            var node = contextNode.SelectSingleNode("SortOrders");
            if (node == null) {
                return;
            }

            var sortOrder = node.SelectNodes("SortOrder");
            if (sortOrder != null) {
                foreach (XmlNode node2 in sortOrder) {
                    _sortOrders.Add(new SortOrder(node2));
                }
            }

            if (node.Attributes == null) {
                return;
            }

            var attribute = node.Attributes["lastSort"];
            if (attribute != null) {
                LastSort = Convert.ToInt32(attribute.Value);
            }
        }


        public void Remove(SortOrder sortOrder) {
            _sortOrders.Remove(sortOrder);
        }


        public void SaveToXml(XmlNode contextNode) {
            var emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "SortOrders");
            Xml.SetAttribute(emptyNodeAlways, "lastSort", LastSort.ToString(CultureInfo.InvariantCulture));
            var ownerDocument = contextNode.OwnerDocument;
            foreach (var order in _sortOrders) {
                emptyNodeAlways.AppendChild(order.SaveToXml(ownerDocument));
            }
        }


        public void UpdateChannelCounts(int count) {
            foreach (var order in _sortOrders) {
                int current;
                if (order.ChannelIndexes.Count > count) {
                    var list = new List<int>();
                    list.AddRange(order.ChannelIndexes);
                    using (var enumerator2 = list.GetEnumerator()) {
                        while (enumerator2.MoveNext()) {
                            current = enumerator2.Current;
                            if (current >= count) {
                                order.ChannelIndexes.Remove(current);
                            }
                        }
                    }
                }
                else {
                    for (current = order.ChannelIndexes.Count; current < count; current++) {
                        order.ChannelIndexes.Add(current);
                    }
                }
            }
        }
    }
}
