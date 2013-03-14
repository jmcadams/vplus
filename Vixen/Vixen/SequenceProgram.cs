namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class SequenceProgram : IScheduledObject, IExecutable, IMaskable, IDisposable
    {
        private int m_crossFadeLength;
        private List<EventSequenceStub> m_eventSequences;
        private string m_fileName;
        private ulong m_key;
        private bool m_loop;
        private byte[][] m_mask;
        private Vixen.Profile m_profile;
        private Vixen.SetupData m_setupData;
        private bool m_treatAsLocal;
        private object m_userData;
        private bool m_useSequencePluginData;

        public SequenceProgram()
        {
            this.m_loop = false;
            this.m_profile = null;
            this.m_useSequencePluginData = false;
            this.m_treatAsLocal = false;
            this.m_userData = null;
            this.m_crossFadeLength = 0;
            this.m_key = Host.GetUniqueKey();
            this.m_mask = null;
            this.m_fileName = string.Empty;
            this.ConstructUsing();
            this.m_setupData = new Vixen.SetupData();
        }

        public SequenceProgram(string fileName)
        {
            this.m_loop = false;
            this.m_profile = null;
            this.m_useSequencePluginData = false;
            this.m_treatAsLocal = false;
            this.m_userData = null;
            this.m_crossFadeLength = 0;
            this.m_key = Host.GetUniqueKey();
            this.m_mask = null;
            this.m_fileName = fileName;
            this.ConstructUsing();
            this.m_setupData = new Vixen.SetupData();
            this.LoadFromXml(Xml.LoadDocument(Path.Combine(Paths.ProgramPath, fileName)));
        }

        public SequenceProgram(EventSequence sequence)
        {
            this.m_loop = false;
            this.m_profile = null;
            this.m_useSequencePluginData = false;
            this.m_treatAsLocal = false;
            this.m_userData = null;
            this.m_crossFadeLength = 0;
            this.m_key = Host.GetUniqueKey();
            this.m_mask = null;
            this.m_fileName = sequence.FileName;
            this.ConstructUsing();
            this.m_setupData = sequence.PlugInData;
            this.m_eventSequences.Add(new EventSequenceStub(sequence));
        }

        public void AddSequence(string sequenceFileName)
        {
            this.m_eventSequences.Add(new EventSequenceStub(Path.Combine(Paths.SequencePath, Path.GetFileName(sequenceFileName)), true));
        }

        public void AddSequence(EventSequence sequence)
        {
            this.m_eventSequences.Add(new EventSequenceStub(sequence));
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
                MessageBox.Show(this.Name + "\n\nThe referenced profile does not exist.\nWill use sequence data instead.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.LoadEmbeddedData(this.FileName);
            }
        }

        private void AttachToProfile(Vixen.Profile profile)
        {
            this.m_profile = profile;
            this.ReloadProfile();
        }

        public void ClearSequences()
        {
            this.m_eventSequences.Clear();
        }

        private void ConstructUsing()
        {
            this.m_eventSequences = new List<EventSequenceStub>();
        }

        private void DetachFromProfile()
        {
            this.m_profile = null;
            this.LoadEmbeddedData(this.FileName);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            foreach (EventSequenceStub stub in this.m_eventSequences)
            {
                stub.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        ~SequenceProgram()
        {
            this.Dispose(false);
        }

        private void LoadEmbeddedData(string fileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            this.LoadEmbeddedData(document.SelectSingleNode("//Program"));
        }

        private void LoadEmbeddedData(XmlNode contextNode)
        {
            this.m_setupData = new Vixen.SetupData();
            this.m_setupData.LoadFromXml(contextNode);
        }

        private void LoadFromXml(XmlNode contextNode)
        {
            XmlNode node = contextNode.SelectSingleNode("Program");
            if (node.Attributes["useSequencePluginData"] != null)
            {
                this.m_useSequencePluginData = bool.Parse(node.Attributes["useSequencePluginData"].Value);
            }
            this.m_eventSequences.Clear();
            foreach (XmlNode node2 in node.SelectNodes("Sequence"))
            {
                string path = Path.Combine(Paths.SequencePath, node2.InnerText);
                if (File.Exists(path))
                {
                    this.m_eventSequences.Add(new EventSequenceStub(path, true));
                }
                else
                {
                    node.RemoveChild(node2);
                }
            }
            XmlNode node3 = node.SelectSingleNode("Profile");
            if (node3 == null)
            {
                this.LoadEmbeddedData(node);
            }
            else
            {
                this.AttachToProfile(node3.InnerText);
            }
            this.m_crossFadeLength = int.Parse(Xml.GetNodeAlways(node, "CrossFadeLength", "0").InnerText);
        }

        public void Refresh()
        {
            foreach (EventSequenceStub stub in this.m_eventSequences)
            {
                if ((stub.FileName == null) || (stub.FileName == string.Empty))
                {
                    throw new Exception("The program has at least one sequence that has not been saved.");
                }
                using (EventSequence sequence = new EventSequence(stub.FileName))
                {
                    stub.Name = sequence.Name;
                    stub.Length = sequence.Time;
                }
            }
        }

        public void ReloadProfile()
        {
            this.m_setupData = this.m_profile.PlugInData;
        }

        public void SaveTo(string filePath)
        {
            XmlDocument contextNode = Xml.CreateXmlDocument();
            this.SaveToXml(contextNode);
            contextNode.Save(filePath);
        }

        private void SaveToXml(XmlNode contextNode)
        {
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
            if (this.m_useSequencePluginData)
            {
                Xml.SetAttribute(emptyNodeAlways, "useSequencePluginData", this.m_useSequencePluginData.ToString());
            }
            foreach (EventSequenceStub stub in this.m_eventSequences)
            {
                if ((stub.FileName == null) || (stub.FileName == string.Empty))
                {
                    throw new Exception("Before a program can be saved, the contained sequences need to be saved.");
                }
                Xml.SetNewValue(emptyNodeAlways, "Sequence", Path.GetFileName(stub.FileName));
            }
            if (this.m_profile == null)
            {
                emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(this.m_setupData.RootNode, true));
            }
            else
            {
                Xml.SetValue(emptyNodeAlways, "Profile", this.m_profile.Name);
            }
            Xml.SetValue(emptyNodeAlways, "CrossFadeLength", this.m_crossFadeLength.ToString());
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
                return true;
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
                return new List<Channel>();
            }
        }

        public int CrossFadeLength
        {
            get
            {
                return this.m_crossFadeLength;
            }
            set
            {
                this.m_crossFadeLength = value;
            }
        }

        public List<string> EventSequenceFileNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (EventSequenceStub stub in this.m_eventSequences)
                {
                    list.Add(Path.GetFileName(stub.FileName));
                }
                return list;
            }
        }

        internal List<EventSequenceStub> EventSequences
        {
            get
            {
                return this.m_eventSequences;
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

        public int Length
        {
            get
            {
                int num = 0;
                foreach (EventSequenceStub stub in this.m_eventSequences)
                {
                    num += stub.Length;
                }
                return num;
            }
        }

        public bool Loop
        {
            get
            {
                return this.m_loop;
            }
            set
            {
                this.m_loop = value;
            }
        }

        public byte[][] Mask
        {
            get
            {
                if (this.m_profile == null)
                {
                    if (this.m_mask == null)
                    {
                        this.m_mask = new byte[this.m_eventSequences.Count][];
                        for (int i = 0; i < this.m_eventSequences.Count; i++)
                        {
                            this.m_mask[i] = this.m_eventSequences[i].Mask[0];
                        }
                    }
                    return this.m_mask;
                }
                return this.m_profile.Mask;
            }
            set
            {
                if (this.m_profile == null)
                {
                    for (int i = 0; i < this.m_eventSequences.Count; i++)
                    {
                        this.m_eventSequences[i].Mask[0] = value[0];
                    }
                }
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
                this.m_fileName = Path.ChangeExtension(value, ".vpr");
            }
        }

        public List<Channel> OutputChannels
        {
            get
            {
                if (this.m_profile != null)
                {
                    return this.m_profile.OutputChannels;
                }
                return new List<Channel>();
            }
        }

        public Vixen.SetupData PlugInData
        {
            get
            {
                if (this.m_profile != null)
                {
                    return this.m_profile.PlugInData;
                }
                return this.m_setupData;
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

        public Vixen.SetupData SetupData
        {
            get
            {
                return this.m_setupData;
            }
            set
            {
                this.m_setupData = value;
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

        public bool UseSequencePluginData
        {
            get
            {
                return this.m_useSequencePluginData;
            }
            set
            {
                this.m_useSequencePluginData = value;
            }
        }
    }
}

