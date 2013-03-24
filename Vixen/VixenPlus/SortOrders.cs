using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace VixenPlus
{
	public class SortOrders : IEnumerable<SortOrder>, IEnumerable
	{
		private readonly List<SortOrder> m_sortOrders = new List<SortOrder>();
		private int m_lastSort = -1;

		public SortOrder CurrentOrder
		{
			get
			{
				if (LastSort == -1)
				{
					return null;
				}
				return m_sortOrders[LastSort];
			}
			set
			{
				for (int i = 0; i < m_sortOrders.Count; i++)
				{
					if (m_sortOrders[i].Name == value.Name)
					{
						LastSort = i;
					}
				}
			}
		}

		public int LastSort
		{
			get { return m_lastSort; }
			set { m_lastSort = value; }
		}

		public IEnumerator<SortOrder> GetEnumerator()
		{
			return m_sortOrders.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_sortOrders.GetEnumerator();
		}

		public void Add(SortOrder sortOrder)
		{
			m_sortOrders.Add(sortOrder);
		}

		internal SortOrders Clone()
		{
			var target = new SortOrders();
			CloneTo(target);
			return target;
		}

		private void CloneTo(SortOrders target)
		{
			target.m_lastSort = m_lastSort;
			target.m_sortOrders.Clear();
			target.m_sortOrders.AddRange(m_sortOrders);
		}

		public void DeleteChannel(int naturalIndex)
		{
			foreach (SortOrder order in m_sortOrders)
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
			for (int i = 0; i < m_sortOrders.Count; i++)
			{
				SortOrder order = m_sortOrders[i];
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
			m_sortOrders.Clear();
			XmlNode node = contextNode.SelectSingleNode("SortOrders");
			if (node != null)
			{
				foreach (XmlNode node2 in node.SelectNodes("SortOrder"))
				{
					m_sortOrders.Add(new SortOrder(node2));
				}
				if (node != null)
				{
					XmlAttribute attribute = node.Attributes["lastSort"];
					if (attribute != null)
					{
						m_lastSort = Convert.ToInt32(attribute.Value);
					}
				}
			}
		}

		public void Remove(SortOrder sortOrder)
		{
			m_sortOrders.Remove(sortOrder);
		}

		public void SaveToXml(XmlNode contextNode)
		{
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "SortOrders");
			Xml.SetAttribute(emptyNodeAlways, "lastSort", m_lastSort.ToString());
			XmlDocument ownerDocument = contextNode.OwnerDocument;
			foreach (SortOrder order in m_sortOrders)
			{
				emptyNodeAlways.AppendChild(order.SaveToXml(ownerDocument));
			}
		}

		public void UpdateChannelCounts(int count)
		{
			foreach (SortOrder order in m_sortOrders)
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