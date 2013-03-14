namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class Profile : IExecutable, IMaskable, IDisposable
    {
        private List<Channel> m_channelObjects;
        private List<int> m_channelOutputs;
        private string m_fileName;
        private List<Channel> m_frozenChannelList;
        private byte[][] m_frozenMask;
        private List<Channel> m_frozenOutputChannelList;
        private bool m_isFrozen;
        private ulong m_key;
        private SetupData m_plugInData;
        private SortOrders m_sortOrders;
        private bool m_treatAsLocal;
        private object m_userData;

        public Profile()
        {
            this.m_fileName = string.Empty;
            this.m_isFrozen = false;
            this.m_treatAsLocal = false;
            this.m_userData = null;
            this.m_key = Host.GetUniqueKey();
            this.m_channelObjects = new List<Channel>();
            this.m_channelOutputs = new List<int>();
            this.m_plugInData = new SetupData();
            this.m_sortOrders = new SortOrders();
        }

        public Profile(string fileName)
        {
            this.m_fileName = string.Empty;
            this.m_isFrozen = false;
            this.m_treatAsLocal = false;
            this.m_userData = null;
            this.m_key = Host.GetUniqueKey();
            this.m_channelObjects = new List<Channel>();
            this.m_channelOutputs = new List<int>();
            this.m_plugInData = new SetupData();
            this.m_sortOrders = new SortOrders();
            this.ReloadFrom(fileName);
        }

        public void AddChannelObject(Channel channelObject)
        {
            this.m_channelObjects.Add(channelObject);
            this.m_channelOutputs.Add(this.m_channelOutputs.Count);
            this.m_sortOrders.UpdateChannelCounts(this.Channels.Count);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            foreach (Channel channel in this.m_channelObjects)
            {
                channel.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        ~Profile()
        {
            this.Dispose(false);
        }

        public void Freeze()
        {
            if (!this.m_isFrozen)
            {
                this.m_frozenChannelList = this.Channels;
                this.m_frozenOutputChannelList = this.OutputChannels;
                this.m_frozenMask = this.Mask;
                this.m_isFrozen = true;
            }
        }

        public void InheritChannelsFrom(EventSequence sequence)
        {
            this.m_channelObjects = sequence.Channels;
            this.m_channelOutputs.Clear();
            foreach (Channel channel in sequence.Channels)
            {
                this.m_channelOutputs.Add(channel.OutputChannel);
            }
            this.m_sortOrders.UpdateChannelCounts(this.Channels.Count);
        }

        public void InheritPlugInDataFrom(EventSequence sequence)
        {
            this.m_plugInData.LoadFromXml(sequence.PlugInData.RootNode.ParentNode);
        }

        public void InheritSortsFrom(EventSequence sequence)
        {
            this.m_sortOrders = (sequence.Sorts == null) ? null : sequence.Sorts.Clone();
        }

        public void MoveChannelObject(int oldIndex, int newIndex)
        {
            Channel item = this.m_channelObjects[oldIndex];
            this.m_channelObjects.RemoveAt(oldIndex);
            this.m_channelObjects.Insert(newIndex, item);
        }

        private void RedirectAndRemoveOutput(int channelObjectIndex, int channelObjectOutputIndex)
        {
            int num = this.m_channelOutputs[channelObjectIndex];
            int index = this.m_channelOutputs.IndexOf(channelObjectOutputIndex);
            this.m_channelOutputs[index] = num;
            this.m_channelOutputs.RemoveAt(channelObjectIndex);
        }

        public void Reload()
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.m_fileName);
            XmlNode documentElement = document.DocumentElement;
            this.m_channelObjects.Clear();
            foreach (XmlNode node2 in documentElement.SelectNodes("ChannelObjects/*"))
            {
                this.m_channelObjects.Add(new Channel(node2));
            }
            this.m_channelOutputs.Clear();
            foreach (string str in documentElement.SelectSingleNode("Outputs").InnerText.Split(new char[] { ',' }))
            {
                if (str.Length > 0)
                {
                    this.m_channelOutputs.Add(Convert.ToInt32(str));
                }
            }
            this.m_plugInData.LoadFromXml(documentElement);
            this.m_sortOrders.LoadFromXml(documentElement);
            List<Channel> channels = this.Channels;
            foreach (string str2 in documentElement.SelectSingleNode("DisabledChannels").InnerText.Split(new char[] { ',' }))
            {
                if (!(str2 == string.Empty))
                {
                    channels[Convert.ToInt32(str2)].Enabled = false;
                }
            }
            if (this.m_isFrozen)
            {
                this.m_isFrozen = false;
                this.Freeze();
            }
        }

        public void ReloadFrom(string fileName)
        {
            this.m_fileName = fileName;
            this.Reload();
        }

        public void RemoveChannelObject(Channel channelObject)
        {
            int index = this.m_channelObjects.IndexOf(channelObject);
            this.RedirectAndRemoveOutput(index, index);
            this.m_channelObjects.Remove(channelObject);
            this.m_sortOrders.UpdateChannelCounts(this.Channels.Count);
        }

        public void SaveToFile()
        {
            this.SaveToXml(null).OwnerDocument.Save(this.m_fileName);
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
            foreach (Channel channel in this.m_channelObjects)
            {
                emptyNodeAlways.AppendChild(channel.SaveToXml(doc));
            }
            StringBuilder builder = new StringBuilder();
            foreach (int num in this.m_channelOutputs)
            {
                builder.AppendFormat("{0},", num);
            }
            Xml.GetEmptyNodeAlways(documentElement, "Outputs").InnerText = builder.ToString().TrimEnd(new char[] { ',' });
            documentElement.AppendChild(doc.ImportNode(this.m_plugInData.RootNode, true));
            this.m_sortOrders.SaveToXml(documentElement);
            List<string> list = new List<string>();
            List<Channel> channels = this.Channels;
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
            return this.Name;
        }

        public int AudioDeviceIndex
        {
            get
            {
                return -1;
            }
        }

        public int AudioDeviceVolume
        {
            get
            {
                return 100;
            }
        }

        public bool CanBePlayed
        {
            get
            {
                return false;
            }
        }

        public List<Channel> Channels
        {
            get
            {
                if (this.m_isFrozen)
                {
                    return this.m_frozenChannelList;
                }
                List<Channel> list = new List<Channel>(this.m_channelObjects);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].OutputChannel = this.m_channelOutputs[i];
                }
                return list;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_fileName;
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
                return this.m_sortOrders.LastSort;
            }
            set
            {
                this.m_sortOrders.LastSort = value;
            }
        }

        public byte[][] Mask
        {
            get
            {
                if (this.m_isFrozen)
                {
                    return this.m_frozenMask;
                }
                List<Channel> channels = this.Channels;
                byte[] buffer = new byte[channels.Count];
                for (int i = 0; i < channels.Count; i++)
                {
                    buffer[i] = channels[i].Enabled ? ((byte) 0xff) : ((byte) 0);
                }
                return new byte[][] { buffer };
            }
            set
            {
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.m_fileName);
            }
            set
            {
                if (this.m_fileName.Length == 0)
                {
                    this.m_fileName = Path.Combine(Paths.ProfilePath, value + ".pro");
                }
                else
                {
                    this.m_fileName = Path.Combine(Path.GetDirectoryName(this.m_fileName), value + ".pro");
                }
            }
        }

        public List<Channel> OutputChannels
        {
            get
            {
                if (this.m_isFrozen)
                {
                    return this.m_frozenOutputChannelList;
                }
                List<Channel> channels = this.Channels;
                List<Channel> list2 = new List<Channel>(channels);
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[this.m_channelOutputs[i]] = channels[i];
                }
                return list2;
            }
            set
            {
                List<Channel> channels = this.Channels;
                for (int i = 0; i < channels.Count; i++)
                {
                    this.m_channelOutputs[i] = value.IndexOf(channels[i]);
                }
            }
        }

        public SetupData PlugInData
        {
            get
            {
                return this.m_plugInData;
            }
        }

        public SortOrders Sorts
        {
            get
            {
                return this.m_sortOrders;
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
    }
}

