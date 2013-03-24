using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace VixenPlus
{
	public class Profile : IExecutable, IMaskable, IDisposable
	{
		private readonly List<int> m_channelOutputs;
		private readonly ulong m_key;
		private readonly SetupData m_plugInData;
		private List<Channel> m_channelObjects;
		private string m_fileName;
		private List<Channel> m_frozenChannelList;
		private byte[][] m_frozenMask;
		private List<Channel> m_frozenOutputChannelList;
		private bool m_isFrozen;
		private SortOrders m_sortOrders;

		public Profile()
		{
			m_fileName = string.Empty;
			m_isFrozen = false;
			TreatAsLocal = false;
			UserData = null;
			m_key = Host.GetUniqueKey();
			m_channelObjects = new List<Channel>();
			m_channelOutputs = new List<int>();
			m_plugInData = new SetupData();
			m_sortOrders = new SortOrders();
		}

		public Profile(string fileName)
		{
			m_fileName = string.Empty;
			m_isFrozen = false;
			TreatAsLocal = false;
			UserData = null;
			m_key = Host.GetUniqueKey();
			m_channelObjects = new List<Channel>();
			m_channelOutputs = new List<int>();
			m_plugInData = new SetupData();
			m_sortOrders = new SortOrders();
			ReloadFrom(fileName);
		}

		public int LastSort
		{
			get { return m_sortOrders.LastSort; }
			set { m_sortOrders.LastSort = value; }
		}

		public SortOrders Sorts
		{
			get { return m_sortOrders; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public int AudioDeviceIndex
		{
			get { return -1; }
		}

		public int AudioDeviceVolume
		{
			get { return 100; }
		}

		public bool CanBePlayed
		{
			get { return false; }
		}

		public List<Channel> Channels
		{
			get
			{
				if (m_isFrozen)
				{
					return m_frozenChannelList;
				}
				var list = new List<Channel>(m_channelObjects);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].OutputChannel = m_channelOutputs[i];
				}
				return list;
			}
		}

		public string FileName
		{
			get { return m_fileName; }
		}

		public ulong Key
		{
			get { return m_key; }
		}

		public byte[][] Mask
		{
			get
			{
				if (m_isFrozen)
				{
					return m_frozenMask;
				}
				List<Channel> channels = Channels;
				var buffer = new byte[channels.Count];
				for (int i = 0; i < channels.Count; i++)
				{
					buffer[i] = channels[i].Enabled ? ((byte) 0xff) : ((byte) 0);
				}
				return new[] {buffer};
			}
			set { }
		}

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(m_fileName); }
			set
			{
				m_fileName = Path.Combine(m_fileName.Length == 0 ? Paths.ProfilePath : Path.GetDirectoryName(m_fileName), value + ".pro");
			}
		}

		public List<Channel> OutputChannels
		{
			get
			{
				if (m_isFrozen)
				{
					return m_frozenOutputChannelList;
				}
				List<Channel> channels = Channels;
				var list2 = new List<Channel>(channels);
				for (int i = 0; i < list2.Count; i++)
				{
					list2[m_channelOutputs[i]] = channels[i];
				}
				return list2;
			}
			set
			{
				List<Channel> channels = Channels;
				for (int i = 0; i < channels.Count; i++)
				{
					m_channelOutputs[i] = value.IndexOf(channels[i]);
				}
			}
		}

		public SetupData PlugInData
		{
			get { return m_plugInData; }
		}

		public bool TreatAsLocal { get; set; }

		public object UserData { get; set; }

		public void AddChannelObject(Channel channelObject)
		{
			m_channelObjects.Add(channelObject);
			m_channelOutputs.Add(m_channelOutputs.Count);
			m_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void Dispose(bool disposing)
		{
			foreach (Channel channel in m_channelObjects)
			{
				channel.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		~Profile()
		{
			Dispose(false);
		}

		public void Freeze()
		{
			if (!m_isFrozen)
			{
				m_frozenChannelList = Channels;
				m_frozenOutputChannelList = OutputChannels;
				m_frozenMask = Mask;
				m_isFrozen = true;
			}
		}

		public void InheritChannelsFrom(EventSequence sequence)
		{
			m_channelObjects = sequence.Channels;
			m_channelOutputs.Clear();
			foreach (Channel channel in sequence.Channels)
			{
				m_channelOutputs.Add(channel.OutputChannel);
			}
			m_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void InheritPlugInDataFrom(EventSequence sequence)
		{
			m_plugInData.LoadFromXml(sequence.PlugInData.RootNode.ParentNode);
		}

		public void InheritSortsFrom(EventSequence sequence)
		{
			m_sortOrders = (sequence.Sorts == null) ? null : sequence.Sorts.Clone();
		}

		public void MoveChannelObject(int oldIndex, int newIndex)
		{
			Channel item = m_channelObjects[oldIndex];
			m_channelObjects.RemoveAt(oldIndex);
			m_channelObjects.Insert(newIndex, item);
		}

		private void RedirectAndRemoveOutput(int channelObjectIndex, int channelObjectOutputIndex)
		{
			int num = m_channelOutputs[channelObjectIndex];
			int index = m_channelOutputs.IndexOf(channelObjectOutputIndex);
			m_channelOutputs[index] = num;
			m_channelOutputs.RemoveAt(channelObjectIndex);
		}

		public void Reload()
		{
			var document = new XmlDocument();
			document.Load(m_fileName);
			XmlNode documentElement = document.DocumentElement;
			m_channelObjects.Clear();
			foreach (XmlNode node2 in documentElement.SelectNodes("ChannelObjects/*"))
			{
				m_channelObjects.Add(new Channel(node2));
			}
			m_channelOutputs.Clear();
			foreach (string str in documentElement.SelectSingleNode("Outputs").InnerText.Split(new[] {','}))
			{
				if (str.Length > 0)
				{
					m_channelOutputs.Add(Convert.ToInt32(str));
				}
			}
			m_plugInData.LoadFromXml(documentElement);
			m_sortOrders.LoadFromXml(documentElement);
			List<Channel> channels = Channels;
			foreach (string str2 in documentElement.SelectSingleNode("DisabledChannels").InnerText.Split(new[] {','}))
			{
				if (!(str2 == string.Empty))
				{
					channels[Convert.ToInt32(str2)].Enabled = false;
				}
			}
			if (m_isFrozen)
			{
				m_isFrozen = false;
				Freeze();
			}
		}

		public void ReloadFrom(string fileName)
		{
			m_fileName = fileName;
			Reload();
		}

		public void RemoveChannelObject(Channel channelObject)
		{
			int index = m_channelObjects.IndexOf(channelObject);
			RedirectAndRemoveOutput(index, index);
			m_channelObjects.Remove(channelObject);
			m_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void SaveToFile()
		{
			SaveToXml(null).OwnerDocument.Save(m_fileName);
		}

		public XmlNode SaveToXml(XmlDocument doc)
		{
			XmlNode documentElement;
			if (doc == null)
			{
				doc = Xml.CreateXmlDocument("Profile");
				documentElement = doc.DocumentElement;
			}
			else
			{
				documentElement = doc.CreateElement("Profile");
			}
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(documentElement, "ChannelObjects");
			foreach (Channel channel in m_channelObjects)
			{
				emptyNodeAlways.AppendChild(channel.SaveToXml(doc));
			}
			var builder = new StringBuilder();
			foreach (int num in m_channelOutputs)
			{
				builder.AppendFormat("{0},", num);
			}
			Xml.GetEmptyNodeAlways(documentElement, "Outputs").InnerText = builder.ToString().TrimEnd(new[] {','});
			documentElement.AppendChild(doc.ImportNode(m_plugInData.RootNode, true));
			m_sortOrders.SaveToXml(documentElement);
			var list = new List<string>();
			List<Channel> channels = Channels;
			for (int i = 0; i < channels.Count; i++)
			{
				if (!channels[i].Enabled)
				{
					list.Add(i.ToString());
				}
			}
			Xml.SetValue(documentElement, "DisabledChannels", string.Join(",", list.ToArray()));
			return documentElement;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}