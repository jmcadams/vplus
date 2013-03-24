using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Vixen
{
	public class EventSequence : IScheduledObject, IExecutable, IMaskable, IDisposable
	{
		private readonly ulong m_key;
		private Audio m_audio;
		private int m_audioDeviceIndex;
		private int m_audioDeviceVolume;
		private int m_channelWidth;
		private List<Channel> m_channels;
		private EngineType m_engineType;
		private int m_eventPeriod;
		private byte[,] m_eventValues;
		private SequenceExtensions m_extensions;
		private LoadableData m_loadableData;
		private byte m_maximumLevel;
		private byte m_minimumLevel;
		private SetupData m_plugInData;
		private Profile m_profile;
		private SortOrders m_sortOrders;
		private string m_sourceFileName;
		private int m_time;
		private int m_totalEventPeriods;
		private int m_windowHeight;
		private int m_windowWidth;

		public EventSequence(string fileName)
		{
			m_eventValues = null;
			m_eventPeriod = 100;
			m_minimumLevel = 0;
			m_maximumLevel = 0xff;
			m_audio = null;
			m_totalEventPeriods = 0;
			m_windowWidth = 0;
			m_windowHeight = 0;
			m_channelWidth = 0;
			m_engineType = EngineType.Standard;
			m_profile = null;
			TreatAsLocal = false;
			m_audioDeviceIndex = -1;
			m_audioDeviceVolume = 0;
			UserData = null;
			m_key = Host.GetUniqueKey();
			var contextNode = new XmlDocument();
			contextNode.Load(fileName);
			m_sourceFileName = fileName;
			LoadFromXml(contextNode);
		}

		public EventSequence(Preference2 preferences)
		{
			m_eventValues = null;
			m_eventPeriod = 100;
			m_minimumLevel = 0;
			m_maximumLevel = 0xff;
			m_audio = null;
			m_totalEventPeriods = 0;
			m_windowWidth = 0;
			m_windowHeight = 0;
			m_channelWidth = 0;
			m_engineType = EngineType.Standard;
			m_profile = null;
			TreatAsLocal = false;
			m_audioDeviceIndex = -1;
			m_audioDeviceVolume = 0;
			UserData = null;
			m_key = Host.GetUniqueKey();
			m_channels = new List<Channel>();
			m_plugInData = new SetupData();
			m_loadableData = new LoadableData();
			m_sortOrders = new SortOrders();
			m_extensions = new SequenceExtensions();
			if (preferences != null)
			{
				m_eventPeriod = preferences.GetInteger("EventPeriod");
				m_minimumLevel = (byte) preferences.GetInteger("MinimumLevel");
				m_maximumLevel = (byte) preferences.GetInteger("MaximumLevel");
				string profileName = preferences.GetString("DefaultProfile");
				if (profileName.Length > 0)
				{
					AttachToProfile(profileName);
				}
				m_audioDeviceIndex = preferences.GetInteger("DefaultSequenceAudioDevice");
			}
			else
			{
				m_eventPeriod = 100;
				m_minimumLevel = 0;
				m_maximumLevel = 0xff;
				m_audioDeviceIndex = -1;
			}
			Time = 0xea60;
		}

		public Audio Audio
		{
			get { return m_audio; }
			set { m_audio = value; }
		}

		public int ChannelCount
		{
			get
			{
				if (m_profile != null)
				{
					return m_profile.Channels.Count;
				}
				return m_channels.Count;
			}
			set
			{
				while (m_channels.Count > value)
				{
					m_channels.RemoveAt(value);
				}
				for (int i = m_channels.Count + 1; m_channels.Count < value; i++)
				{
					m_channels.Add(new Channel("Channel " + i.ToString(), i - 1, true));
				}
				UpdateEventValueArray();
				m_sortOrders.UpdateChannelCounts(value);
			}
		}

		public int ChannelWidth
		{
			get { return m_channelWidth; }
			set { m_channelWidth = value; }
		}

		public EngineType EngineType
		{
			get { return m_engineType; }
			set { m_engineType = value; }
		}

		public int EventPeriod
		{
			get { return m_eventPeriod; }
			set
			{
				m_eventPeriod = value;
				UpdateEventValueArray(true);
			}
		}

		public byte[,] EventValues
		{
			get { return m_eventValues; }
			set { m_eventValues = value; }
		}

		public SequenceExtensions Extensions
		{
			get { return m_extensions; }
		}

		public int LastSort
		{
			get
			{
				if (m_profile == null)
				{
					return m_sortOrders.LastSort;
				}
				return m_profile.Sorts.LastSort;
			}
			set
			{
				if (m_profile == null)
				{
					m_sortOrders.LastSort = value;
				}
				else
				{
					m_profile.Sorts.LastSort = value;
				}
			}
		}

		public LoadableData LoadableData
		{
			get { return m_loadableData; }
		}

		public byte MaximumLevel
		{
			get { return m_maximumLevel; }
			set { m_maximumLevel = value; }
		}

		public byte MinimumLevel
		{
			get { return m_minimumLevel; }
			set { m_minimumLevel = value; }
		}

		public Profile Profile
		{
			get { return m_profile; }
			set
			{
				if ((value == null) && (m_profile != null))
				{
					DetachFromProfile();
				}
				else if (m_profile != value)
				{
					AttachToProfile(value);
				}
			}
		}

		public SortOrders Sorts
		{
			get
			{
				if (m_profile == null)
				{
					return m_sortOrders;
				}
				return m_profile.Sorts;
			}
		}

		public int Time
		{
			get { return m_time; }
			set { SetTime(value); }
		}

		public int TotalEventPeriods
		{
			get { return m_totalEventPeriods; }
		}

		public int WindowHeight
		{
			get { return m_windowHeight; }
			set { m_windowHeight = value; }
		}

		public int WindowWidth
		{
			get { return m_windowWidth; }
			set { m_windowWidth = value; }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public int AudioDeviceIndex
		{
			get { return m_audioDeviceIndex; }
			set { m_audioDeviceIndex = value; }
		}

		public int AudioDeviceVolume
		{
			get { return m_audioDeviceVolume; }
			set { m_audioDeviceVolume = value; }
		}

		public bool CanBePlayed
		{
			get { return true; }
		}

		public List<Channel> Channels
		{
			get
			{
				if (m_profile != null)
				{
					return m_profile.Channels;
				}
				return m_channels;
			}
			set { AssignChannelArray(value); }
		}

		public string FileName
		{
			get { return m_sourceFileName; }
			set { m_sourceFileName = value; }
		}

		public ulong Key
		{
			get { return m_key; }
		}

		public int Length
		{
			get { return m_time; }
		}

		public byte[][] Mask
		{
			get
			{
				if (m_profile == null)
				{
					var buffer = new byte[m_channels.Count];
					for (int i = 0; i < m_channels.Count; i++)
					{
						buffer[i] = m_channels[i].Enabled ? ((byte) 0xff) : ((byte) 0);
					}
					return new[] {buffer};
				}
				return m_profile.Mask;
			}
			set
			{
				if (m_profile == null)
				{
					for (int i = 0; i < m_channels.Count; i++)
					{
						m_channels[i].Enabled = value[0][i] == 0xff;
					}
				}
			}
		}

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(m_sourceFileName); }
			set
			{
				string extension = ".vix";
				if ((m_sourceFileName != null) && (m_sourceFileName != string.Empty))
				{
					extension = Path.GetExtension(m_sourceFileName);
				}
				else if (Path.HasExtension(value))
				{
					extension = Path.GetExtension(value);
				}
				value = Path.ChangeExtension(value, extension.ToLower());
				if (Path.IsPathRooted(value))
				{
					m_sourceFileName = value;
				}
				else
				{
					string str2 = ((m_sourceFileName == null) || (m_sourceFileName.Length == 0))
						              ? null
						              : Path.GetDirectoryName(m_sourceFileName);
					if ((str2 != null) && (str2 != string.Empty))
					{
						m_sourceFileName = Path.Combine(str2, value);
					}
					else
					{
						m_sourceFileName = Path.Combine(Paths.SequencePath, value);
					}
				}
			}
		}

		public List<Channel> OutputChannels
		{
			get
			{
				var list = new List<Channel>(m_channels);
				foreach (Channel channel in m_channels)
				{
					list[channel.OutputChannel] = channel;
				}
				return list;
			}
		}

		public SetupData PlugInData
		{
			get { return m_plugInData; }
			set { m_plugInData = value; }
		}

		public bool TreatAsLocal { get; set; }

		public object UserData { get; set; }

		private void AssignChannelArray(List<Channel> channels)
		{
			m_channels = channels;
			if (m_channels.Count != m_eventValues.GetLength(0))
			{
				UpdateEventValueArray(true);
			}
			m_sortOrders.UpdateChannelCounts(m_channels.Count);
		}

		private void AttachToProfile(string profileName)
		{
			string path = Path.Combine(Paths.ProfilePath, profileName + ".pro");
			if (File.Exists(path))
			{
				AttachToProfile(new Profile(path));
			}
			else
			{
				LoadEmbeddedData(FileName);
			}
		}

		private void AttachToProfile(Profile profile)
		{
			m_profile = profile;
			m_profile.Freeze();
			LoadFromProfile();
		}

		public void CopyChannel(Channel source, Channel dest)
		{
			int index = m_channels.IndexOf(source);
			int num2 = m_channels.IndexOf(dest);
			for (int i = 0; i < TotalEventPeriods; i++)
			{
				m_eventValues[num2, i] = m_eventValues[index, i];
			}
		}

		public void DeleteChannel(ulong channelId)
		{
			int index = Channels.IndexOf(FindChannel(channelId));
			Channels.RemoveAt(index);
			var buffer = new byte[ChannelCount,TotalEventPeriods];
			int num3 = 0;
			for (int i = 0; i < m_eventValues.GetLength(0); i++)
			{
				if (i != index)
				{
					for (int j = 0; j < TotalEventPeriods; j++)
					{
						buffer[num3, j] = m_eventValues[i, j];
					}
					num3++;
				}
			}
			m_eventValues = buffer;
			m_sortOrders.DeleteChannel(index);
		}

		private void DetachFromProfile()
		{
			LoadEmbeddedData(FileName);
			if (((m_profile.Channels.Count > m_channels.Count) && HasData()) &&
			    (MessageBox.Show(
				    "The sequence does not contain as many channels as the profile you are detaching from.\nDo you want to increase the channel count to prevent any possible data loss?",
				    Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
			{
				while (m_channels.Count < m_profile.Channels.Count)
				{
					m_channels.Add(m_profile.Channels[m_channels.Count]);
				}
			}
			m_profile = null;
			UpdateEventValueArray();
		}

		private void Dispose(bool disposing)
		{
		}

		private int ExtentOfAudio()
		{
			if (m_audio != null)
			{
				return m_audio.Duration;
			}
			return -2147483648;
		}

		~EventSequence()
		{
			Dispose(false);
		}

		public Channel FindChannel(ulong id)
		{
			return Channels.Find(delegate(Channel c) { return c.Id == id; });
		}

		private bool HasData()
		{
			for (int i = 0; i < m_eventValues.GetLength(0); i++)
			{
				for (int j = 0; j < m_eventValues.GetLength(1); j++)
				{
					if (m_eventValues[i, j] != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public int InsertChannel(int sortedIndex)
		{
			int count = LastSort >= 0 ? m_channels.Count : sortedIndex;
			if (count > m_channels.Count)
			{
				count = m_channels.Count;
			}
			if (sortedIndex > m_channels.Count)
			{
				sortedIndex = m_channels.Count;
			}
			int outputChannel = count;
			foreach (Channel channel in m_channels)
			{
				if (channel.OutputChannel >= outputChannel)
				{
					channel.OutputChannel++;
				}
			}
			int num7 = sortedIndex + 1;
			m_channels.Insert(count, new Channel("Channel " + num7.ToString(), outputChannel, true));
			var buffer = new byte[m_channels.Count,TotalEventPeriods];
			for (int i = 0; i < m_eventValues.GetLength(0); i++)
			{
				int num4 = (i >= count) ? (i + 1) : i;
				for (int j = 0; j < TotalEventPeriods; j++)
				{
					buffer[num4, j] = m_eventValues[i, j];
				}
			}
			m_eventValues = buffer;
			m_sortOrders.InsertChannel(count, sortedIndex);
			return count;
		}

		private void LoadEmbeddedData(string fileName)
		{
			if (((fileName != null) && (fileName != string.Empty)) && File.Exists(fileName))
			{
				var document = new XmlDocument();
				document.Load(fileName);
				LoadEmbeddedData(document.SelectSingleNode("//Program"));
			}
			else
			{
				m_plugInData = new SetupData();
			}
		}

		private void LoadEmbeddedData(XmlNode contextNode)
		{
			m_channels.Clear();
			foreach (XmlNode node in contextNode.SelectNodes("Channels/Channel"))
			{
				m_channels.Add(new Channel(node));
			}
			m_plugInData = new SetupData();
			m_plugInData.LoadFromXml(contextNode);
			m_sortOrders = new SortOrders();
			m_sortOrders.LoadFromXml(contextNode);
		}

		private void LoadFromProfile()
		{
			m_plugInData = m_profile.PlugInData;
			UpdateEventValueArray();
		}

		private void LoadFromXml(XmlNode contextNode)
		{
			XmlNode requiredNode = Xml.GetRequiredNode(contextNode, "Program");
			m_channels = new List<Channel>();
			m_plugInData = new SetupData();
			m_loadableData = new LoadableData();
			m_extensions = new SequenceExtensions();
			m_sortOrders = new SortOrders();
			Time = Convert.ToInt32(requiredNode.SelectSingleNode("Time").InnerText);
			m_eventPeriod = Convert.ToInt32(requiredNode.SelectSingleNode("EventPeriodInMilliseconds").InnerText);
			m_minimumLevel = (byte) Convert.ToInt32(requiredNode.SelectSingleNode("MinimumLevel").InnerText);
			m_maximumLevel = (byte) Convert.ToInt32(requiredNode.SelectSingleNode("MaximumLevel").InnerText);
			m_audioDeviceIndex = int.Parse(requiredNode.SelectSingleNode("AudioDevice").InnerText);
			m_audioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText);
			XmlNode node2 = requiredNode.SelectSingleNode("Profile");
			if (node2 == null)
			{
				LoadEmbeddedData(requiredNode);
			}
			else
			{
				AttachToProfile(node2.InnerText);
			}
			UpdateEventValueArray();
			XmlNode node3 = requiredNode.SelectSingleNode("Audio");
			if (node3 != null)
			{
				m_audio = new Audio(node3.InnerText, node3.Attributes["filename"].Value,
				                    Convert.ToInt32(node3.Attributes["duration"].Value));
			}
			int count = Channels.Count;
			XmlNode node4 = requiredNode.SelectSingleNode("EventValues");
			if (node4 != null)
			{
				byte[] buffer = Convert.FromBase64String(node4.InnerText);
				int num3 = 0;
				for (int i = 0; (i < count) && (num3 < buffer.Length); i++)
				{
					for (int j = 0; (j < m_totalEventPeriods) && (num3 < buffer.Length); j++)
					{
						m_eventValues[i, j] = buffer[num3++];
					}
				}
			}
			XmlNode node5 = requiredNode.SelectSingleNode("WindowSize");
			if (node5 != null)
			{
				string[] strArray = node5.InnerText.Split(new[] {','});
				try
				{
					m_windowWidth = Convert.ToInt32(strArray[0]);
				}
				catch
				{
					m_windowWidth = 0;
				}
				try
				{
					m_windowHeight = Convert.ToInt32(strArray[1]);
				}
				catch
				{
					m_windowHeight = 0;
				}
			}
			node5 = requiredNode.SelectSingleNode("ChannelWidth");
			if (node5 != null)
			{
				try
				{
					m_channelWidth = Convert.ToInt32(node5.InnerText);
				}
				catch
				{
					m_channelWidth = 0;
				}
			}
			XmlNode node6 = requiredNode.SelectSingleNode("EngineType");
			if (node6 != null)
			{
				try
				{
					m_engineType = (EngineType) Enum.Parse(typeof (EngineType), node6.InnerText);
				}
				catch
				{
				}
			}
			m_loadableData.LoadFromXml(requiredNode);
			m_extensions.LoadFromXml(requiredNode);
		}

		public void ReloadProfile()
		{
			if (m_profile != null)
			{
				m_profile.Reload();
				LoadFromProfile();
			}
		}

		public void Save()
		{
			if (!Directory.Exists(Path.GetDirectoryName(m_sourceFileName)))
			{
				throw new Exception("Attemped to save to non-existent file path:\n" + m_sourceFileName);
			}
			SaveTo(m_sourceFileName);
		}

		public void SaveTo(string fileName)
		{
			SaveTo(fileName, true);
		}

		public void SaveTo(string fileName, bool setSequenceFileName)
		{
			XmlDocument contextNode = Xml.CreateXmlDocument();
			SaveToXml(contextNode);
			if (setSequenceFileName)
			{
				FileName = fileName;
			}
			contextNode.Save(fileName);
		}

		private void SaveToXml(XmlNode contextNode)
		{
			XmlDocument doc = contextNode.OwnerDocument ?? ((XmlDocument) contextNode);
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
			Xml.SetValue(emptyNodeAlways, "Time", m_time.ToString());
			Xml.SetValue(emptyNodeAlways, "EventPeriodInMilliseconds", m_eventPeriod.ToString());
			Xml.SetValue(emptyNodeAlways, "MinimumLevel", m_minimumLevel.ToString());
			Xml.SetValue(emptyNodeAlways, "MaximumLevel", m_maximumLevel.ToString());
			Xml.SetValue(emptyNodeAlways, "AudioDevice", m_audioDeviceIndex.ToString());
			Xml.SetValue(emptyNodeAlways, "AudioVolume", m_audioDeviceVolume.ToString());
			XmlNode node2 = Xml.GetEmptyNodeAlways(emptyNodeAlways, "Channels");
			foreach (Channel channel in m_channels)
			{
				node2.AppendChild(channel.SaveToXml(doc));
			}
			emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(m_plugInData.RootNode, true));
			m_sortOrders.SaveToXml(emptyNodeAlways);
			if (m_profile != null)
			{
				Xml.SetValue(emptyNodeAlways, "Profile", m_profile.Name);
			}
			if (m_audio != null)
			{
				XmlNode node = Xml.SetNewValue(emptyNodeAlways, "Audio", m_audio.Name);
				Xml.SetAttribute(node, "filename", m_audio.FileName);
				Xml.SetAttribute(node, "duration", m_audio.Duration.ToString());
			}
			int count = Channels.Count;
			int totalEventPeriods = m_totalEventPeriods;
			var inArray = new byte[count*totalEventPeriods];
			int num4 = 0;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < totalEventPeriods; j++)
				{
					inArray[num4++] = m_eventValues[i, j];
				}
			}
			Xml.GetNodeAlways(emptyNodeAlways, "EventValues").InnerText = Convert.ToBase64String(inArray);
			emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(m_loadableData.RootNode, true));
			Xml.SetValue(emptyNodeAlways, "EngineType", m_engineType.ToString());
			emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(m_extensions.RootNode, true));
		}

		private void SetTime(int milliseconds)
		{
			if (milliseconds < ExtentOfAudio())
			{
				throw new Exception("Cannot set the sequence length.\nThere is audio associated which would exceed that length.");
			}
			if ((m_eventValues == null) || (milliseconds != (m_eventValues.GetLength(1)*m_eventPeriod)))
			{
				m_time = milliseconds;
				UpdateEventValueArray();
			}
		}

		public override string ToString()
		{
			return Name;
		}

		private void UpdateEventValueArray(bool dataExtrapolation = false)
		{
			int length = 0;
			List<Channel> list = (m_profile == null) ? m_channels : m_profile.Channels;
			if (m_eventValues != null)
			{
				length = m_eventValues.GetLength(0);
			}
			if (!dataExtrapolation)
			{
				byte[,] eventValues = m_eventValues;
				m_eventValues = new byte[list.Count,(int) Math.Ceiling(((m_time)/((float) m_eventPeriod)))];
				if (eventValues != null)
				{
					int num2 = Math.Min(eventValues.GetLength(1), m_eventValues.GetLength(1));
					int num3 = Math.Min(eventValues.GetLength(0), m_eventValues.GetLength(0));
					for (int i = 0; i < num3; i++)
					{
						for (int j = 0; j < num2; j++)
						{
							m_eventValues[i, j] = eventValues[i, j];
						}
					}
				}
			}
			else
			{
				var buffer2 = new byte[list.Count,(int) Math.Ceiling(((m_time)/((float) m_eventPeriod)))];
				if (((m_eventValues != null) && (m_eventValues.GetLength(0) != 0)) && (m_eventValues.GetLength(1) != 0))
				{
					double num6 = (buffer2.GetLength(1))/((double) m_eventValues.GetLength(1));
					var num7 = (float) (1000.0/(m_eventPeriod*num6));
					float num8 = 1000f/(m_eventPeriod);
					int num9 = buffer2.Length/list.Count;
					int num10 = m_eventValues.Length/list.Count;
					float num12 = Math.Min(num7, num8);
					float num13 = num7/num12;
					float num14 = num8/num12;
					var num15 = (int) Math.Min(((num10)/num13), ((num9)/num14));
					int num19 = Math.Min(list.Count, m_eventValues.GetLength(0));
					for (int k = 0; k < num19; k++)
					{
						for (float m = 0f; m < num15; m++)
						{
							byte num18 = 0;
							for (float n = 0f; n < num13; n++)
							{
								num18 = Math.Max(num18, m_eventValues[k, (int) ((m*num13) + n)]);
							}
							for (float num17 = 0f; num17 < num14; num17++)
							{
								buffer2[k, (int) ((m*num14) + num17)] = num18;
							}
						}
					}
				}
				m_eventValues = buffer2;
			}
			m_totalEventPeriods = m_eventValues.GetLength(1);
			foreach (XmlNode node in m_plugInData.GetAllPluginData(SetupData.PluginType.Output))
			{
				if (int.Parse(node.Attributes["from"].Value) > list.Count)
				{
					node.Attributes["from"].Value = list.Count.ToString();
				}
				int num21 = int.Parse(node.Attributes["to"].Value);
				if ((num21 == length) || (num21 > list.Count))
				{
					node.Attributes["to"].Value = list.Count.ToString();
				}
			}
		}

		public void UpdateMetrics(int windowWidth, int windowHeight, int channelWidth)
		{
			var document = new XmlDocument();
			if (File.Exists(m_sourceFileName) && ((File.GetAttributes(m_sourceFileName) & FileAttributes.ReadOnly) == 0))
			{
				document.Load(m_sourceFileName);
				XmlNode contextNode = document.SelectSingleNode("//Program");
				Xml.SetValue(contextNode, "WindowSize", string.Format("{0},{1}", windowWidth, windowHeight));
				Xml.SetValue(contextNode, "ChannelWidth", channelWidth.ToString());
				document.Save(m_sourceFileName);
			}
		}
	}
}