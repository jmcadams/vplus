using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VixenPlus
{
	public class Profile : IExecutable
	{
		private readonly List<int> _channelOutputs;
		private readonly ulong _key;
		private readonly SetupData _plugInData;
		private List<Channel> _channelObjects;
		private string _fileName;
		private List<Channel> _frozenChannelList;
		private byte[][] _frozenMask;
		private List<Channel> _frozenOutputChannelList;
		private bool _isFrozen;
		private SortOrders _sortOrders;

		public Profile()
		{
			_fileName = string.Empty;
			_isFrozen = false;
			TreatAsLocal = false;
			UserData = null;
			_key = Host.GetUniqueKey();
			_channelObjects = new List<Channel>();
			_channelOutputs = new List<int>();
			_plugInData = new SetupData();
			_sortOrders = new SortOrders();
		}

		public Profile(string fileName)
		{
			_fileName = string.Empty;
			_isFrozen = false;
			TreatAsLocal = false;
			UserData = null;
			_key = Host.GetUniqueKey();
			_channelObjects = new List<Channel>();
			_channelOutputs = new List<int>();
			_plugInData = new SetupData();
			_sortOrders = new SortOrders();
			ReloadFrom(fileName);
		}

		public int LastSort
		{
			get { return _sortOrders.LastSort; }
			set { _sortOrders.LastSort = value; }
		}

		public SortOrders Sorts
		{
			get { return _sortOrders; }
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
				if (_isFrozen)
				{
					return _frozenChannelList;
				}
				var list = new List<Channel>(_channelObjects);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].OutputChannel = _channelOutputs[i];
				}
				return list;
			}
		}

		public string FileName
		{
			get { return _fileName; }
		}

		public ulong Key
		{
			get { return _key; }
		}

		public byte[][] Mask
		{
			get
			{
				if (_isFrozen)
				{
					return _frozenMask;
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
			get { return Path.GetFileNameWithoutExtension(_fileName); }
			set
			{
				_fileName = Path.Combine(string.IsNullOrEmpty(_fileName) ? Paths.ProfilePath : Path.GetDirectoryName(_fileName),
				                         value + ".pro");
			}
		}

		public List<Channel> OutputChannels
		{
			get
			{
				if (_isFrozen)
				{
					return _frozenOutputChannelList;
				}
				List<Channel> channels = Channels;
				var list2 = new List<Channel>(channels);
				for (int i = 0; i < list2.Count; i++)
				{
					list2[_channelOutputs[i]] = channels[i];
				}
				return list2;
			}
			set
			{
				List<Channel> channels = Channels;
				for (int i = 0; i < channels.Count; i++)
				{
					_channelOutputs[i] = value.IndexOf(channels[i]);
				}
			}
		}

		public SetupData PlugInData
		{
			get { return _plugInData; }
		}

		public bool TreatAsLocal { get; set; }

		public object UserData { get; set; }

		public void AddChannelObject(Channel channelObject)
		{
			_channelObjects.Add(channelObject);
			_channelOutputs.Add(_channelOutputs.Count);
			_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void Dispose(bool disposing)
		{
			foreach (Channel channel in _channelObjects)
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
			if (!_isFrozen)
			{
				_frozenChannelList = Channels;
				_frozenOutputChannelList = OutputChannels;
				_frozenMask = Mask;
				_isFrozen = true;
			}
		}

		public void InheritChannelsFrom(EventSequence sequence)
		{
			_channelObjects = sequence.Channels;
			_channelOutputs.Clear();
			foreach (Channel channel in sequence.Channels)
			{
				_channelOutputs.Add(channel.OutputChannel);
			}
			_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void InheritPlugInDataFrom(EventSequence sequence)
		{
			_plugInData.LoadFromXml(sequence.PlugInData.RootNode.ParentNode);
		}

		public void InheritSortsFrom(EventSequence sequence)
		{
			_sortOrders = (sequence.Sorts == null) ? null : sequence.Sorts.Clone();
		}

		public void MoveChannelObject(int oldIndex, int newIndex)
		{
			Channel item = _channelObjects[oldIndex];
			_channelObjects.RemoveAt(oldIndex);
			_channelObjects.Insert(newIndex, item);
		}

		private void RedirectAndRemoveOutput(int channelObjectIndex, int channelObjectOutputIndex)
		{
			int num = _channelOutputs[channelObjectIndex];
			int index = _channelOutputs.IndexOf(channelObjectOutputIndex);
			_channelOutputs[index] = num;
			_channelOutputs.RemoveAt(channelObjectIndex);
		}

		public void Reload()
		{
			var document = new XmlDocument();
			document.Load(_fileName);
			XmlNode documentElement = document.DocumentElement;
			_channelObjects.Clear();
			_channelOutputs.Clear();
			if (documentElement != null)
			{
				XmlNodeList channelObjectsNode = documentElement.SelectNodes("ChannelObjects/*");
				if (channelObjectsNode != null)
				{
					foreach (XmlNode node2 in channelObjectsNode)
					{
						_channelObjects.Add(new Channel(node2));
					}
				}

				XmlNode outputNodes = documentElement.SelectSingleNode("Outputs");
				if (outputNodes != null)
				{
					foreach (string str in outputNodes.InnerText.Split(new[] {','}))
					{
						if (str.Length > 0)
						{
							_channelOutputs.Add(Convert.ToInt32(str));
						}
					}
				}
			}
			_plugInData.LoadFromXml(documentElement);
			_sortOrders.LoadFromXml(documentElement);
			List<Channel> channels = Channels;
			if (documentElement != null)
			{
				XmlNode disabledChannelsNode = documentElement.SelectSingleNode("DisabledChannels");
				if (disabledChannelsNode != null)
				{
					foreach (string str2 in disabledChannelsNode.InnerText.Split(new[] {','}))
					{
						if (str2 != string.Empty)
						{
							channels[Convert.ToInt32(str2)].Enabled = false;
						}
					}
				}
			}
			if (_isFrozen)
			{
				_isFrozen = false;
				Freeze();
			}
		}

		public void ReloadFrom(string fileName)
		{
			_fileName = fileName;
			Reload();
		}

		public void RemoveChannelObject(Channel channelObject)
		{
			int index = _channelObjects.IndexOf(channelObject);
			RedirectAndRemoveOutput(index, index);
			_channelObjects.Remove(channelObject);
			_sortOrders.UpdateChannelCounts(Channels.Count);
		}

		public void SaveToFile()
		{
			XmlDocument ownerDocument = SaveToXml(null).OwnerDocument;
			if (ownerDocument != null)
			{
				ownerDocument.Save(_fileName);
			}
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
			foreach (Channel channel in _channelObjects)
			{
				emptyNodeAlways.AppendChild(channel.SaveToXml(doc));
			}
			var builder = new StringBuilder();
			foreach (int num in _channelOutputs)
			{
				builder.AppendFormat("{0},", num);
			}
			Xml.GetEmptyNodeAlways(documentElement, "Outputs").InnerText = builder.ToString().TrimEnd(new[] {','});
			if (documentElement != null)
			{
				documentElement.AppendChild(doc.ImportNode(_plugInData.RootNode, true));
				_sortOrders.SaveToXml(documentElement);
			}
			var list = new List<string>();
			List<Channel> channels = Channels;
			for (int i = 0; i < channels.Count; i++)
			{
				if (!channels[i].Enabled)
				{
					list.Add(i.ToString(CultureInfo.InvariantCulture));
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