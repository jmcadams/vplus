namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class EventSequence : IScheduledObject, IExecutable, IMaskable, IDisposable
    {
        private Vixen.Audio m_audio;
        private int m_audioDeviceIndex;
        private int m_audioDeviceVolume;
        private List<Channel> m_channels;
        private int m_channelWidth;
        private Vixen.EngineType m_engineType;
        private int m_eventPeriod;
        private byte[,] m_eventValues;
        private SequenceExtensions m_extensions;
        private ulong m_key;
        private Vixen.LoadableData m_loadableData;
        private byte m_maximumLevel;
        private byte m_minimumLevel;
        private SetupData m_plugInData;
        private Vixen.Profile m_profile;
        private SortOrders m_sortOrders;
        private string m_sourceFileName;
        private int m_time;
        private int m_totalEventPeriods;
        private bool m_treatAsLocal;
        private object m_userData;
        private int m_windowHeight;
        private int m_windowWidth;

        public EventSequence(string fileName)
        {
            this.m_eventValues = null;
            this.m_eventPeriod = 100;
            this.m_minimumLevel = 0;
            this.m_maximumLevel = 0xff;
            this.m_audio = null;
            this.m_totalEventPeriods = 0;
            this.m_windowWidth = 0;
            this.m_windowHeight = 0;
            this.m_channelWidth = 0;
            this.m_engineType = Vixen.EngineType.Standard;
            this.m_profile = null;
            this.m_treatAsLocal = false;
            this.m_audioDeviceIndex = -1;
            this.m_audioDeviceVolume = 0;
            this.m_userData = null;
            this.m_key = Host.GetUniqueKey();
            XmlDocument contextNode = new XmlDocument();
            contextNode.Load(fileName);
            this.m_sourceFileName = fileName;
            this.LoadFromXml(contextNode);
        }

        public EventSequence(Preference2 preferences)
        {
            this.m_eventValues = null;
            this.m_eventPeriod = 100;
            this.m_minimumLevel = 0;
            this.m_maximumLevel = 0xff;
            this.m_audio = null;
            this.m_totalEventPeriods = 0;
            this.m_windowWidth = 0;
            this.m_windowHeight = 0;
            this.m_channelWidth = 0;
            this.m_engineType = Vixen.EngineType.Standard;
            this.m_profile = null;
            this.m_treatAsLocal = false;
            this.m_audioDeviceIndex = -1;
            this.m_audioDeviceVolume = 0;
            this.m_userData = null;
            this.m_key = Host.GetUniqueKey();
            this.m_channels = new List<Channel>();
            this.m_plugInData = new SetupData();
            this.m_loadableData = new Vixen.LoadableData();
            this.m_sortOrders = new SortOrders();
            this.m_extensions = new SequenceExtensions();
            if (preferences != null)
            {
                this.m_eventPeriod = preferences.GetInteger("EventPeriod");
                this.m_minimumLevel = (byte) preferences.GetInteger("MinimumLevel");
                this.m_maximumLevel = (byte) preferences.GetInteger("MaximumLevel");
                string profileName = preferences.GetString("DefaultProfile");
                if (profileName.Length > 0)
                {
                    this.AttachToProfile(profileName);
                }
                this.m_audioDeviceIndex = preferences.GetInteger("DefaultSequenceAudioDevice");
            }
            else
            {
                this.m_eventPeriod = 100;
                this.m_minimumLevel = 0;
                this.m_maximumLevel = 0xff;
                this.m_audioDeviceIndex = -1;
            }
            this.Time = 0xea60;
        }

        private void AssignChannelArray(List<Channel> channels)
        {
            this.m_channels = channels;
            if (this.m_channels.Count != this.m_eventValues.GetLength(0))
            {
                this.UpdateEventValueArray(true);
            }
            this.m_sortOrders.UpdateChannelCounts(this.m_channels.Count);
        }

        private void AttachToProfile(string profileName)
        {
            string path = Path.Combine(Paths.ProfilePath, profileName + ".pro");
            if (File.Exists(path))
            {
                this.AttachToProfile(new Vixen.Profile(path));
            }
            else
            {
                this.LoadEmbeddedData(this.FileName);
            }
        }

        private void AttachToProfile(Vixen.Profile profile)
        {
            this.m_profile = profile;
            this.m_profile.Freeze();
            this.LoadFromProfile();
        }

        public void CopyChannel(Channel source, Channel dest)
        {
            int index = this.m_channels.IndexOf(source);
            int num2 = this.m_channels.IndexOf(dest);
            for (int i = 0; i < this.TotalEventPeriods; i++)
            {
                this.m_eventValues[num2, i] = this.m_eventValues[index, i];
            }
        }

        public void DeleteChannel(ulong channelId)
        {
            int index = this.Channels.IndexOf(this.FindChannel(channelId));
            this.Channels.RemoveAt(index);
            byte[,] buffer = new byte[this.ChannelCount, this.TotalEventPeriods];
            int num3 = 0;
            for (int i = 0; i < this.m_eventValues.GetLength(0); i++)
            {
                if (i != index)
                {
                    for (int j = 0; j < this.TotalEventPeriods; j++)
                    {
                        buffer[num3, j] = this.m_eventValues[i, j];
                    }
                    num3++;
                }
            }
            this.m_eventValues = buffer;
            this.m_sortOrders.DeleteChannel(index);
        }

        private void DetachFromProfile()
        {
            this.LoadEmbeddedData(this.FileName);
            if (((this.m_profile.Channels.Count > this.m_channels.Count) && this.HasData()) && (MessageBox.Show("The sequence does not contain as many channels as the profile you are detaching from.\nDo you want to increase the channel count to prevent any possible data loss?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                while (this.m_channels.Count < this.m_profile.Channels.Count)
                {
                    this.m_channels.Add(this.m_profile.Channels[this.m_channels.Count]);
                }
            }
            this.m_profile = null;
            this.UpdateEventValueArray();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
        }

        private int ExtentOfAudio()
        {
            if (this.m_audio != null)
            {
                return this.m_audio.Duration;
            }
            return -2147483648;
        }

        ~EventSequence()
        {
            this.Dispose(false);
        }

        public Channel FindChannel(ulong id)
        {
            return this.Channels.Find(delegate (Channel c) {
                return c.ID == id;
            });
        }

        private bool HasData()
        {
            for (int i = 0; i < this.m_eventValues.GetLength(0); i++)
            {
                for (int j = 0; j < this.m_eventValues.GetLength(1); j++)
                {
                    if (this.m_eventValues[i, j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int InsertChannel(int sortedIndex)
        {
            int count;
            if (this.LastSort >= 0)
            {
                count = this.m_channels.Count;
            }
            else
            {
                count = sortedIndex;
            }
            if (count > this.m_channels.Count)
            {
                count = this.m_channels.Count;
            }
            if (sortedIndex > this.m_channels.Count)
            {
                sortedIndex = this.m_channels.Count;
            }
            int outputChannel = count;
            foreach (Channel channel in this.m_channels)
            {
                if (channel.OutputChannel >= outputChannel)
                {
                    channel.OutputChannel++;
                }
            }
            int num7 = sortedIndex + 1;
            this.m_channels.Insert(count, new Channel("Channel " + num7.ToString(), outputChannel, true));
            byte[,] buffer = new byte[this.m_channels.Count, this.TotalEventPeriods];
            for (int i = 0; i < this.m_eventValues.GetLength(0); i++)
            {
                int num4 = (i >= count) ? (i + 1) : i;
                for (int j = 0; j < this.TotalEventPeriods; j++)
                {
                    buffer[num4, j] = this.m_eventValues[i, j];
                }
            }
            this.m_eventValues = buffer;
            this.m_sortOrders.InsertChannel(count, sortedIndex);
            return count;
        }

        private void LoadEmbeddedData(string fileName)
        {
            if (((fileName != null) && (fileName != string.Empty)) && File.Exists(fileName))
            {
                XmlDocument document = new XmlDocument();
                document.Load(fileName);
                this.LoadEmbeddedData(document.SelectSingleNode("//Program"));
            }
            else
            {
                this.m_plugInData = new SetupData();
            }
        }

        private void LoadEmbeddedData(XmlNode contextNode)
        {
            this.m_channels.Clear();
            foreach (XmlNode node in contextNode.SelectNodes("Channels/Channel"))
            {
                this.m_channels.Add(new Channel(node));
            }
            this.m_plugInData = new SetupData();
            this.m_plugInData.LoadFromXml(contextNode);
            this.m_sortOrders = new SortOrders();
            this.m_sortOrders.LoadFromXml(contextNode);
        }

        private void LoadFromProfile()
        {
            this.m_plugInData = this.m_profile.PlugInData;
            this.UpdateEventValueArray();
        }

        private void LoadFromXml(XmlNode contextNode)
        {
            XmlNode requiredNode = Xml.GetRequiredNode(contextNode, "Program");
            this.m_channels = new List<Channel>();
            this.m_plugInData = new SetupData();
            this.m_loadableData = new Vixen.LoadableData();
            this.m_extensions = new SequenceExtensions();
            this.m_sortOrders = new SortOrders();
            this.Time = Convert.ToInt32(requiredNode.SelectSingleNode("Time").InnerText);
            this.m_eventPeriod = Convert.ToInt32(requiredNode.SelectSingleNode("EventPeriodInMilliseconds").InnerText);
            this.m_minimumLevel = (byte) Convert.ToInt32(requiredNode.SelectSingleNode("MinimumLevel").InnerText);
            this.m_maximumLevel = (byte) Convert.ToInt32(requiredNode.SelectSingleNode("MaximumLevel").InnerText);
            this.m_audioDeviceIndex = int.Parse(requiredNode.SelectSingleNode("AudioDevice").InnerText);
            this.m_audioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText);
            XmlNode node2 = requiredNode.SelectSingleNode("Profile");
            if (node2 == null)
            {
                this.LoadEmbeddedData(requiredNode);
            }
            else
            {
                this.AttachToProfile(node2.InnerText);
            }
            this.UpdateEventValueArray();
            XmlNode node3 = requiredNode.SelectSingleNode("Audio");
            if (node3 != null)
            {
                this.m_audio = new Vixen.Audio(node3.InnerText, node3.Attributes["filename"].Value, Convert.ToInt32(node3.Attributes["duration"].Value));
            }
            int count = this.Channels.Count;
            XmlNode node4 = requiredNode.SelectSingleNode("EventValues");
            if (node4 != null)
            {
                byte[] buffer = Convert.FromBase64String(node4.InnerText);
                int num3 = 0;
                for (int i = 0; (i < count) && (num3 < buffer.Length); i++)
                {
                    for (int j = 0; (j < this.m_totalEventPeriods) && (num3 < buffer.Length); j++)
                    {
                        this.m_eventValues[i, j] = buffer[num3++];
                    }
                }
            }
            XmlNode node5 = requiredNode.SelectSingleNode("WindowSize");
            if (node5 != null)
            {
                string[] strArray = node5.InnerText.Split(new char[] { ',' });
                try
                {
                    this.m_windowWidth = Convert.ToInt32(strArray[0]);
                }
                catch
                {
                    this.m_windowWidth = 0;
                }
                try
                {
                    this.m_windowHeight = Convert.ToInt32(strArray[1]);
                }
                catch
                {
                    this.m_windowHeight = 0;
                }
            }
            node5 = requiredNode.SelectSingleNode("ChannelWidth");
            if (node5 != null)
            {
                try
                {
                    this.m_channelWidth = Convert.ToInt32(node5.InnerText);
                }
                catch
                {
                    this.m_channelWidth = 0;
                }
            }
            XmlNode node6 = requiredNode.SelectSingleNode("EngineType");
            if (node6 != null)
            {
                try
                {
                    this.m_engineType = (Vixen.EngineType) Enum.Parse(typeof(Vixen.EngineType), node6.InnerText);
                }
                catch
                {
                }
            }
            this.m_loadableData.LoadFromXml(requiredNode);
            this.m_extensions.LoadFromXml(requiredNode);
        }

        public void ReloadProfile()
        {
            if (this.m_profile != null)
            {
                this.m_profile.Reload();
                this.LoadFromProfile();
            }
        }

        public void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(this.m_sourceFileName)))
            {
                throw new Exception("Attemped to save to non-existent file path:\n" + this.m_sourceFileName);
            }
            this.SaveTo(this.m_sourceFileName);
        }

        public void SaveTo(string fileName)
        {
            this.SaveTo(fileName, true);
        }

        public void SaveTo(string fileName, bool setSequenceFileName)
        {
            XmlDocument contextNode = Xml.CreateXmlDocument();
            this.SaveToXml(contextNode);
            if (setSequenceFileName)
            {
                this.FileName = fileName;
            }
            contextNode.Save(fileName);
        }

        private void SaveToXml(XmlNode contextNode)
        {
            XmlDocument doc = (contextNode.OwnerDocument == null) ? ((XmlDocument) contextNode) : contextNode.OwnerDocument;
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
            Xml.SetValue(emptyNodeAlways, "Time", this.m_time.ToString());
            Xml.SetValue(emptyNodeAlways, "EventPeriodInMilliseconds", this.m_eventPeriod.ToString());
            Xml.SetValue(emptyNodeAlways, "MinimumLevel", this.m_minimumLevel.ToString());
            Xml.SetValue(emptyNodeAlways, "MaximumLevel", this.m_maximumLevel.ToString());
            Xml.SetValue(emptyNodeAlways, "AudioDevice", this.m_audioDeviceIndex.ToString());
            Xml.SetValue(emptyNodeAlways, "AudioVolume", this.m_audioDeviceVolume.ToString());
            XmlNode node2 = Xml.GetEmptyNodeAlways(emptyNodeAlways, "Channels");
            foreach (Channel channel in this.m_channels)
            {
                node2.AppendChild(channel.SaveToXml(doc));
            }
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(this.m_plugInData.RootNode, true));
            this.m_sortOrders.SaveToXml(emptyNodeAlways);
            if (this.m_profile != null)
            {
                Xml.SetValue(emptyNodeAlways, "Profile", this.m_profile.Name);
            }
            if (this.m_audio != null)
            {
                XmlNode node = Xml.SetNewValue(emptyNodeAlways, "Audio", this.m_audio.Name);
                Xml.SetAttribute(node, "filename", this.m_audio.FileName);
                Xml.SetAttribute(node, "duration", this.m_audio.Duration.ToString());
            }
            int count = this.Channels.Count;
            int totalEventPeriods = this.m_totalEventPeriods;
            byte[] inArray = new byte[count * totalEventPeriods];
            int num4 = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < totalEventPeriods; j++)
                {
                    inArray[num4++] = this.m_eventValues[i, j];
                }
            }
            Xml.GetNodeAlways(emptyNodeAlways, "EventValues").InnerText = Convert.ToBase64String(inArray);
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(this.m_loadableData.RootNode, true));
            Xml.SetValue(emptyNodeAlways, "EngineType", this.m_engineType.ToString());
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(this.m_extensions.RootNode, true));
        }

        private void SetTime(int milliseconds)
        {
            if (milliseconds < this.ExtentOfAudio())
            {
                throw new Exception("Cannot set the sequence length.\nThere is audio associated which would exceed that length.");
            }
            if ((this.m_eventValues == null) || (milliseconds != (this.m_eventValues.GetLength(1) * this.m_eventPeriod)))
            {
                this.m_time = milliseconds;
                this.UpdateEventValueArray();
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private void UpdateEventValueArray()
        {
            this.UpdateEventValueArray(false);
        }

        private void UpdateEventValueArray(bool dataExtrapolation)
        {
            int length = 0;
            List<Channel> list = (this.m_profile == null) ? this.m_channels : this.m_profile.Channels;
            if (this.m_eventValues != null)
            {
                length = this.m_eventValues.GetLength(0);
            }
            if (!dataExtrapolation)
            {
                byte[,] eventValues = this.m_eventValues;
                this.m_eventValues = new byte[list.Count, (int) Math.Ceiling((double) (((float) this.m_time) / ((float) this.m_eventPeriod)))];
                if (eventValues != null)
                {
                    int num2 = Math.Min(eventValues.GetLength(1), this.m_eventValues.GetLength(1));
                    int num3 = Math.Min(eventValues.GetLength(0), this.m_eventValues.GetLength(0));
                    for (int i = 0; i < num3; i++)
                    {
                        for (int j = 0; j < num2; j++)
                        {
                            this.m_eventValues[i, j] = eventValues[i, j];
                        }
                    }
                }
            }
            else
            {
                byte[,] buffer2 = new byte[list.Count, (int) Math.Ceiling((double) (((float) this.m_time) / ((float) this.m_eventPeriod)))];
                if (((this.m_eventValues != null) && (this.m_eventValues.GetLength(0) != 0)) && (this.m_eventValues.GetLength(1) != 0))
                {
                    double num6 = ((double) buffer2.GetLength(1)) / ((double) this.m_eventValues.GetLength(1));
                    float num7 = (float) (1000.0 / (this.m_eventPeriod * num6));
                    float num8 = 1000f / ((float) this.m_eventPeriod);
                    int num9 = buffer2.Length / list.Count;
                    int num10 = this.m_eventValues.Length / list.Count;
                    float num12 = Math.Min(num7, num8);
                    float num13 = num7 / num12;
                    float num14 = num8 / num12;
                    int num15 = (int) Math.Min((float) (((float) num10) / num13), (float) (((float) num9) / num14));
                    int num19 = Math.Min(list.Count, this.m_eventValues.GetLength(0));
                    for (int k = 0; k < num19; k++)
                    {
                        for (float m = 0f; m < num15; m++)
                        {
                            byte num18 = 0;
                            for (float n = 0f; n < num13; n++)
                            {
                                num18 = Math.Max(num18, this.m_eventValues[k, (int) ((m * num13) + n)]);
                            }
                            for (float num17 = 0f; num17 < num14; num17++)
                            {
                                buffer2[k, (int) ((m * num14) + num17)] = num18;
                            }
                        }
                    }
                }
                this.m_eventValues = buffer2;
            }
            this.m_totalEventPeriods = this.m_eventValues.GetLength(1);
            foreach (XmlNode node in this.m_plugInData.GetAllPluginData(SetupData.PluginType.Output))
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
            XmlDocument document = new XmlDocument();
            if (File.Exists(this.m_sourceFileName) && ((File.GetAttributes(this.m_sourceFileName) & FileAttributes.ReadOnly) == 0))
            {
                document.Load(this.m_sourceFileName);
                XmlNode contextNode = document.SelectSingleNode("//Program");
                Xml.SetValue(contextNode, "WindowSize", string.Format("{0},{1}", windowWidth, windowHeight));
                Xml.SetValue(contextNode, "ChannelWidth", channelWidth.ToString());
                document.Save(this.m_sourceFileName);
            }
        }

        public Vixen.Audio Audio
        {
            get
            {
                return this.m_audio;
            }
            set
            {
                this.m_audio = value;
            }
        }

        public int AudioDeviceIndex
        {
            get
            {
                return this.m_audioDeviceIndex;
            }
            set
            {
                this.m_audioDeviceIndex = value;
            }
        }

        public int AudioDeviceVolume
        {
            get
            {
                return this.m_audioDeviceVolume;
            }
            set
            {
                this.m_audioDeviceVolume = value;
            }
        }

        public bool CanBePlayed
        {
            get
            {
                return true;
            }
        }

        public int ChannelCount
        {
            get
            {
                if (this.m_profile != null)
                {
                    return this.m_profile.Channels.Count;
                }
                return this.m_channels.Count;
            }
            set
            {
                while (this.m_channels.Count > value)
                {
                    this.m_channels.RemoveAt(value);
                }
                for (int i = this.m_channels.Count + 1; this.m_channels.Count < value; i++)
                {
                    this.m_channels.Add(new Channel("Channel " + i.ToString(), i - 1, true));
                }
                this.UpdateEventValueArray();
                this.m_sortOrders.UpdateChannelCounts(value);
            }
        }

        public List<Channel> Channels
        {
            get
            {
                if (this.m_profile != null)
                {
                    return this.m_profile.Channels;
                }
                return this.m_channels;
            }
            set
            {
                this.AssignChannelArray(value);
            }
        }

        public int ChannelWidth
        {
            get
            {
                return this.m_channelWidth;
            }
            set
            {
                this.m_channelWidth = value;
            }
        }

        public Vixen.EngineType EngineType
        {
            get
            {
                return this.m_engineType;
            }
            set
            {
                this.m_engineType = value;
            }
        }

        public int EventPeriod
        {
            get
            {
                return this.m_eventPeriod;
            }
            set
            {
                this.m_eventPeriod = value;
                this.UpdateEventValueArray(true);
            }
        }

        public byte[,] EventValues
        {
            get
            {
                return this.m_eventValues;
            }
            set
            {
                this.m_eventValues = value;
            }
        }

        public SequenceExtensions Extensions
        {
            get
            {
                return this.m_extensions;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_sourceFileName;
            }
            set
            {
                this.m_sourceFileName = value;
            }
        }

        public ulong Key
        {
            get
            {
                return this.m_key;
            }
        }

        public int LastSort
        {
            get
            {
                if (this.m_profile == null)
                {
                    return this.m_sortOrders.LastSort;
                }
                return this.m_profile.Sorts.LastSort;
            }
            set
            {
                if (this.m_profile == null)
                {
                    this.m_sortOrders.LastSort = value;
                }
                else
                {
                    this.m_profile.Sorts.LastSort = value;
                }
            }
        }

        public int Length
        {
            get
            {
                return this.m_time;
            }
        }

        public Vixen.LoadableData LoadableData
        {
            get
            {
                return this.m_loadableData;
            }
        }

        public byte[][] Mask
        {
            get
            {
                if (this.m_profile == null)
                {
                    byte[] buffer = new byte[this.m_channels.Count];
                    for (int i = 0; i < this.m_channels.Count; i++)
                    {
                        buffer[i] = this.m_channels[i].Enabled ? ((byte) 0xff) : ((byte) 0);
                    }
                    return new byte[][] { buffer };
                }
                return this.m_profile.Mask;
            }
            set
            {
                if (this.m_profile == null)
                {
                    for (int i = 0; i < this.m_channels.Count; i++)
                    {
                        this.m_channels[i].Enabled = value[0][i] == 0xff;
                    }
                }
            }
        }

        public byte MaximumLevel
        {
            get
            {
                return this.m_maximumLevel;
            }
            set
            {
                this.m_maximumLevel = value;
            }
        }

        public byte MinimumLevel
        {
            get
            {
                return this.m_minimumLevel;
            }
            set
            {
                this.m_minimumLevel = value;
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.m_sourceFileName);
            }
            set
            {
                string extension = ".vix";
                if ((this.m_sourceFileName != null) && (this.m_sourceFileName != string.Empty))
                {
                    extension = Path.GetExtension(this.m_sourceFileName);
                }
                else if (Path.HasExtension(value))
                {
                    extension = Path.GetExtension(value);
                }
                value = Path.ChangeExtension(value, extension.ToLower());
                if (Path.IsPathRooted(value))
                {
                    this.m_sourceFileName = value;
                }
                else
                {
                    string str2 = ((this.m_sourceFileName == null) || (this.m_sourceFileName.Length == 0)) ? null : Path.GetDirectoryName(this.m_sourceFileName);
                    if ((str2 != null) && (str2 != string.Empty))
                    {
                        this.m_sourceFileName = Path.Combine(str2, value);
                    }
                    else
                    {
                        this.m_sourceFileName = Path.Combine(Paths.SequencePath, value);
                    }
                }
            }
        }

        public List<Channel> OutputChannels
        {
            get
            {
                List<Channel> list = new List<Channel>(this.m_channels);
                foreach (Channel channel in this.m_channels)
                {
                    list[channel.OutputChannel] = channel;
                }
                return list;
            }
        }

        public SetupData PlugInData
        {
            get
            {
                return this.m_plugInData;
            }
            set
            {
                this.m_plugInData = value;
            }
        }

        public Vixen.Profile Profile
        {
            get
            {
                return this.m_profile;
            }
            set
            {
                if ((value == null) && (this.m_profile != null))
                {
                    this.DetachFromProfile();
                }
                else if (this.m_profile != value)
                {
                    this.AttachToProfile(value);
                }
            }
        }

        public SortOrders Sorts
        {
            get
            {
                if (this.m_profile == null)
                {
                    return this.m_sortOrders;
                }
                return this.m_profile.Sorts;
            }
        }

        public int Time
        {
            get
            {
                return this.m_time;
            }
            set
            {
                this.SetTime(value);
            }
        }

        public int TotalEventPeriods
        {
            get
            {
                return this.m_totalEventPeriods;
            }
        }

        public bool TreatAsLocal
        {
            get
            {
                return this.m_treatAsLocal;
            }
            set
            {
                this.m_treatAsLocal = value;
            }
        }

        public object UserData
        {
            get
            {
                return this.m_userData;
            }
            set
            {
                this.m_userData = value;
            }
        }

        public int WindowHeight
        {
            get
            {
                return this.m_windowHeight;
            }
            set
            {
                this.m_windowHeight = value;
            }
        }

        public int WindowWidth
        {
            get
            {
                return this.m_windowWidth;
            }
            set
            {
                this.m_windowWidth = value;
            }
        }
    }
}

