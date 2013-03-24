using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	public class SequenceProgram : IScheduledObject, IExecutable, IMaskable, IDisposable
	{
		private readonly ulong m_key;
		private int m_crossFadeLength;
		private List<EventSequenceStub> m_eventSequences;
		private string m_fileName;
		private byte[][] m_mask;
		private Profile m_profile;
		private SetupData m_setupData;
		private bool m_useSequencePluginData;

		public SequenceProgram()
		{
			Loop = false;
			m_profile = null;
			m_useSequencePluginData = false;
			TreatAsLocal = false;
			UserData = null;
			m_crossFadeLength = 0;
			m_key = Host.GetUniqueKey();
			m_mask = null;
			m_fileName = string.Empty;
			ConstructUsing();
			m_setupData = new SetupData();
		}

		public SequenceProgram(string fileName)
		{
			Loop = false;
			m_profile = null;
			m_useSequencePluginData = false;
			TreatAsLocal = false;
			UserData = null;
			m_crossFadeLength = 0;
			m_key = Host.GetUniqueKey();
			m_mask = null;
			m_fileName = fileName;
			ConstructUsing();
			m_setupData = new SetupData();
			LoadFromXml(Xml.LoadDocument(Path.Combine(Paths.ProgramPath, fileName)));
		}

		public SequenceProgram(EventSequence sequence)
		{
			Loop = false;
			m_profile = null;
			m_useSequencePluginData = false;
			TreatAsLocal = false;
			UserData = null;
			m_crossFadeLength = 0;
			m_key = Host.GetUniqueKey();
			m_mask = null;
			m_fileName = sequence.FileName;
			ConstructUsing();
			m_setupData = sequence.PlugInData;
			m_eventSequences.Add(new EventSequenceStub(sequence));
		}

		public int CrossFadeLength
		{
			get { return m_crossFadeLength; }
			set { m_crossFadeLength = value; }
		}

		public List<string> EventSequenceFileNames
		{
			get
			{
				var list = new List<string>();
				foreach (EventSequenceStub stub in m_eventSequences)
				{
					list.Add(Path.GetFileName(stub.FileName));
				}
				return list;
			}
		}

		internal List<EventSequenceStub> EventSequences
		{
			get { return m_eventSequences; }
		}

		public bool Loop { get; set; }

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

		public SetupData SetupData
		{
			get { return m_setupData; }
			set { m_setupData = value; }
		}

		public bool UseSequencePluginData
		{
			get { return m_useSequencePluginData; }
			set { m_useSequencePluginData = value; }
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
				return new List<Channel>();
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

		public int Length
		{
			get
			{
				int num = 0;
				foreach (EventSequenceStub stub in m_eventSequences)
				{
					num += stub.Length;
				}
				return num;
			}
		}

		public byte[][] Mask
		{
			get
			{
				if (m_profile == null)
				{
					if (m_mask == null)
					{
						m_mask = new byte[m_eventSequences.Count][];
						for (int i = 0; i < m_eventSequences.Count; i++)
						{
							m_mask[i] = m_eventSequences[i].Mask[0];
						}
					}
					return m_mask;
				}
				return m_profile.Mask;
			}
			set
			{
				if (m_profile == null)
				{
					for (int i = 0; i < m_eventSequences.Count; i++)
					{
						m_eventSequences[i].Mask[0] = value[0];
					}
				}
			}
		}

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(m_fileName); }
			set { m_fileName = Path.ChangeExtension(value, ".vpr"); }
		}

		public List<Channel> OutputChannels
		{
			get
			{
				if (m_profile != null)
				{
					return m_profile.OutputChannels;
				}
				return new List<Channel>();
			}
		}

		public SetupData PlugInData
		{
			get
			{
				if (m_profile != null)
				{
					return m_profile.PlugInData;
				}
				return m_setupData;
			}
		}

		public bool TreatAsLocal { get; set; }

		public object UserData { get; set; }

		public void AddSequence(string sequenceFileName)
		{
			m_eventSequences.Add(new EventSequenceStub(Path.Combine(Paths.SequencePath, Path.GetFileName(sequenceFileName)), true));
		}

		public void AddSequence(EventSequence sequence)
		{
			m_eventSequences.Add(new EventSequenceStub(sequence));
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
				MessageBox.Show(Name + "\n\nThe referenced profile does not exist.\nWill use sequence data instead.",
				                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				LoadEmbeddedData(FileName);
			}
		}

		private void AttachToProfile(Profile profile)
		{
			m_profile = profile;
			ReloadProfile();
		}

		public void ClearSequences()
		{
			m_eventSequences.Clear();
		}

		private void ConstructUsing()
		{
			m_eventSequences = new List<EventSequenceStub>();
		}

		private void DetachFromProfile()
		{
			m_profile = null;
			LoadEmbeddedData(FileName);
		}

		public void Dispose(bool disposing)
		{
			foreach (EventSequenceStub stub in m_eventSequences)
			{
				stub.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		~SequenceProgram()
		{
			Dispose(false);
		}

		private void LoadEmbeddedData(string fileName)
		{
			var document = new XmlDocument();
			document.Load(fileName);
			LoadEmbeddedData(document.SelectSingleNode("//Program"));
		}

		private void LoadEmbeddedData(XmlNode contextNode)
		{
			m_setupData = new SetupData();
			m_setupData.LoadFromXml(contextNode);
		}

		private void LoadFromXml(XmlNode contextNode)
		{
			XmlNode node = contextNode.SelectSingleNode("Program");
			if (node.Attributes["useSequencePluginData"] != null)
			{
				m_useSequencePluginData = bool.Parse(node.Attributes["useSequencePluginData"].Value);
			}
			m_eventSequences.Clear();
			foreach (XmlNode node2 in node.SelectNodes("Sequence"))
			{
				string path = Path.Combine(Paths.SequencePath, node2.InnerText);
				if (File.Exists(path))
				{
					m_eventSequences.Add(new EventSequenceStub(path, true));
				}
				else
				{
					node.RemoveChild(node2);
				}
			}
			XmlNode node3 = node.SelectSingleNode("Profile");
			if (node3 == null)
			{
				LoadEmbeddedData(node);
			}
			else
			{
				AttachToProfile(node3.InnerText);
			}
			m_crossFadeLength = int.Parse(Xml.GetNodeAlways(node, "CrossFadeLength", "0").InnerText);
		}

		public void Refresh()
		{
			foreach (EventSequenceStub stub in m_eventSequences)
			{
				if ((stub.FileName == null) || (stub.FileName == string.Empty))
				{
					throw new Exception("The program has at least one sequence that has not been saved.");
				}
				using (var sequence = new EventSequence(stub.FileName))
				{
					stub.Name = sequence.Name;
					stub.Length = sequence.Time;
				}
			}
		}

		public void ReloadProfile()
		{
			m_setupData = m_profile.PlugInData;
		}

		public void SaveTo(string filePath)
		{
			XmlDocument contextNode = Xml.CreateXmlDocument();
			SaveToXml(contextNode);
			contextNode.Save(filePath);
		}

		private void SaveToXml(XmlNode contextNode)
		{
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
			if (m_useSequencePluginData)
			{
				Xml.SetAttribute(emptyNodeAlways, "useSequencePluginData", m_useSequencePluginData.ToString());
			}
			foreach (EventSequenceStub stub in m_eventSequences)
			{
				if ((stub.FileName == null) || (stub.FileName == string.Empty))
				{
					throw new Exception("Before a program can be saved, the contained sequences need to be saved.");
				}
				Xml.SetNewValue(emptyNodeAlways, "Sequence", Path.GetFileName(stub.FileName));
			}
			if (m_profile == null)
			{
				emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(m_setupData.RootNode, true));
			}
			else
			{
				Xml.SetValue(emptyNodeAlways, "Profile", m_profile.Name);
			}
			Xml.SetValue(emptyNodeAlways, "CrossFadeLength", m_crossFadeLength.ToString());
		}

		public override string ToString()
		{
			return Name;
		}
	}
}