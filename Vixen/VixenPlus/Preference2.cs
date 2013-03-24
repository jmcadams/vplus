using System;
using System.IO;
using System.Xml;

namespace VixenPlus
{
	public class Preference2
	{
		public delegate void OnPreferenceChange(string preferenceName);

		private const string PREFERENCES_FILE_NAME = "preferences";

		private static Preference2 m_instance;
		private readonly XmlDocument m_doc;
		private string m_preferenceFileName;

		private Preference2(string preferenceFilePath)
		{
			if (!Directory.Exists(Paths.DataPath))
			{
				Directory.CreateDirectory(Paths.DataPath);
			}
			m_doc = new XmlDocument();
			m_preferenceFileName = preferenceFilePath;
			if (File.Exists(m_preferenceFileName))
			{
				m_doc.Load(m_preferenceFileName);
			}
			else
			{
				XmlDeclaration newChild = m_doc.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
				m_doc.AppendChild(newChild);
				m_doc.AppendChild(m_doc.CreateElement("User"));
			}
			bool flag = false;
			flag |= VerifyPreference("EventPeriod", 100);
			flag |= VerifyPreference("MinimumLevel", 0);
			flag |= VerifyPreference("MaximumLevel", 0xff);
			flag |= VerifyPreference("MaxColumnWidth", 30);
			flag |= VerifyPreference("MaxRowHeight", 20);
			flag |= VerifyPreference("ShowPositionMarker", true);
			flag |= VerifyPreference("AutoScrolling", true);
			flag |= VerifyPreference("SavePlugInDialogPositions", true);
			flag |= VerifyPreference("EventSequenceAutoSize", true);
			flag |= VerifyPreference("WizardForNewSequences", true);
			flag |= VerifyPreference("SaveZoomLevels", true);
			flag |= VerifyPreference("SaveZoomLevels", "column", "100%");
			flag |= VerifyPreference("SaveZoomLevels", "row", "100%");
			flag |= VerifyPreference("ShowSaveConfirmation", true);
			flag |= VerifyPreference("ClearAtEndOfSequence", true);
			flag |= VerifyPreference("TimerCheckFrequency", 10);
			flag |= VerifyPreference("BackgroundSequence", string.Empty);
			flag |= VerifyPreference("EnableBackgroundSequence", false);
			flag |= VerifyPreference("BackgroundSequenceDelay", 10);
			flag |= VerifyPreference("SecondaryEngine", "ScriptEngine.dll");
			flag |= VerifyPreference("MouseWheelVerticalDelta", 2);
			flag |= VerifyPreference("MouseWheelHorizontalDelta", 10);
			flag |= VerifyPreference("IntensityLargeDelta", 5);
			flag |= VerifyPreference("EnableBackgroundMusic", false);
			flag |= VerifyPreference("BackgroundMusicDelay", 10);
			flag |= VerifyPreference("EnableMusicFade", false);
			flag |= VerifyPreference("MusicFadeDuration", 5);
			flag |= VerifyPreference("ClientName", Vendor.ProductName);
			flag |= VerifyPreference("ResetAtStartup", false);
			flag |= VerifyPreference("DefaultProfile", string.Empty);
			flag |= VerifyPreference("PreferredSequenceType", ".vix");
			flag |= VerifyPreference("SynchronousData", "Embedded");
			flag |= VerifyPreference("AsynchronousData", "Sync");
			flag |= VerifyPreference("ShutdownTime", string.Empty);
			flag |= VerifyPreference("CustomColors",
			                         "16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215");
			flag |= VerifyPreference("ActualLevels", false);
			flag |= VerifyPreference("ShowNaturalChannelNumber", false);
			flag |= VerifyPreference("DefaultSequenceAudioDevice", -1);
			flag |= VerifyPreference("HistoryImages", 0);
			flag |= VerifyPreference("LogAudioManual", false);
			flag |= VerifyPreference("LogAudioScheduled", false);
			flag |= VerifyPreference("LogAudioMusicPlayer", false);
			flag |= VerifyPreference("AudioLogFilePath", string.Empty);
			flag |= VerifyPreference("BarLevels", false);
			flag |= VerifyPreference("FlipScrollBehavior", false);
			flag |= VerifyPreference("RemoteLibraryFTPURL", "");
			flag |= VerifyPreference("RemoteLibraryHTTPURL", "");
			flag |= VerifyPreference("RemoteLibraryFileName", "");
			if (flag | VerifyPreference("DefaultSequenceDirectory", ""))
			{
				Flush();
			}
		}

		public string FileName
		{
			get { return m_preferenceFileName; }
			set { m_preferenceFileName = value; }
		}

		public XmlDocument XmlDoc
		{
			get { return m_doc; }
		}

		public event OnPreferenceChange PreferenceChange;

		public void Flush()
		{
			m_doc.Save(m_preferenceFileName);
		}

		public bool GetBoolean(string name)
		{
			string str = GetString(name);
			if (str == string.Empty)
			{
				return false;
			}
			return Convert.ToBoolean(str);
		}

		public string GetChildString(string parentName, string name)
		{
			XmlNode node = m_doc.DocumentElement.SelectSingleNode(parentName);
			if (node != null)
			{
				XmlAttribute attribute = node.Attributes[name];
				if (attribute == null)
				{
					return string.Empty;
				}
				return attribute.Value;
			}
			return string.Empty;
		}

		public static Preference2 GetInstance()
		{
			return m_instance ?? (m_instance = new Preference2(Path.Combine(Paths.DataPath, "preferences")));
		}

		public int GetInteger(string name)
		{
			string str = GetString(name);
			if (str == string.Empty)
			{
				return 0;
			}
			return Convert.ToInt32(str);
		}

		public string GetString(string name)
		{
			XmlNode node = m_doc.DocumentElement.SelectSingleNode(name);
			if (node == null)
			{
				return string.Empty;
			}
			return node.InnerText;
		}

		public void Reload()
		{
			m_doc.Load(m_preferenceFileName);
		}

		public void SetBoolean(string name, bool value)
		{
			SetString(name, value.ToString());
		}

		public void SetBoolean(string name, bool value, bool defaultValue)
		{
			SetString(name, value.ToString(), defaultValue.ToString());
		}

		public void SetChildBoolean(string parentName, string name, bool value)
		{
			SetChildString(parentName, name, value.ToString());
		}

		public void SetChildInteger(string parentName, string name, int value)
		{
			SetChildString(parentName, name, value.ToString());
		}

		public void SetChildString(string parentName, string name, string value)
		{
			var element = (XmlElement) m_doc.DocumentElement.SelectSingleNode(parentName);
			if (element != null)
			{
				XmlAttribute attribute = element.Attributes[name];
				if (attribute == null)
				{
					element.Attributes.Append(attribute = m_doc.CreateAttribute(name));
				}
				attribute.Value = value;
			}
		}

		public void SetInteger(string name, int value)
		{
			SetString(name, value.ToString());
		}

		public void SetInteger(string name, int value, int defaultValue)
		{
			SetString(name, value.ToString(), defaultValue.ToString());
		}

		public void SetString(string name, string value)
		{
			SetString(name, value, null);
		}

		public void SetString(string name, string value, string defaultValue)
		{
			bool flag = GetString(name) != value;
			SetValue(m_doc.DocumentElement, name, value, defaultValue);
			if (flag && (PreferenceChange != null))
			{
				PreferenceChange(name);
			}
		}

		public void SetValue(XmlNode parentNode, string name, string value, string defaultValue)
		{
			var newChild = (XmlElement) parentNode.SelectSingleNode(name);
			if (newChild == null)
			{
				newChild = m_doc.CreateElement(name);
				if (defaultValue != null)
				{
					XmlAttribute node = m_doc.CreateAttribute("default");
					newChild.Attributes.Append(node);
				}
				parentNode.AppendChild(newChild);
			}
			if (defaultValue != null)
			{
				newChild.Attributes["default"].Value = defaultValue;
			}
			newChild.InnerText = value;
		}

		private bool VerifyPreference(string name, object defaultValue)
		{
			XmlNode node = m_doc.DocumentElement.SelectSingleNode(name);
			bool flag = false;
			if (node == null)
			{
				flag = true;
				if (defaultValue is bool)
				{
					SetBoolean(name, (bool) defaultValue, (bool) defaultValue);
					return flag;
				}
				if (defaultValue is int)
				{
					SetInteger(name, (int) defaultValue, (int) defaultValue);
					return flag;
				}
				SetString(name, defaultValue.ToString(), defaultValue.ToString());
			}
			return flag;
		}

		private bool VerifyPreference(string parentName, string name, object defaultValue)
		{
			bool flag = false;
			if (GetChildString(parentName, name) == string.Empty)
			{
				flag = true;
				if (defaultValue is bool)
				{
					SetChildBoolean(parentName, name, (bool) defaultValue);
					return flag;
				}
				if (defaultValue is int)
				{
					SetChildInteger(parentName, name, (int) defaultValue);
					return flag;
				}
				SetChildString(parentName, name, defaultValue.ToString());
			}
			return flag;
		}
	}
}