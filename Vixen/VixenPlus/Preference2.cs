using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	public class Preference2
	{
		public delegate void OnPreferenceChange(string preferenceName);

		private static Preference2 _preference2;
		private readonly XmlDocument _xmlDocument;
		private string _preferenceFileName;

		private Preference2(string preferenceFilePath)
		{
			if (!Directory.Exists(Paths.DataPath))
			{
				Directory.CreateDirectory(Paths.DataPath);
			}
			_xmlDocument = new XmlDocument();
			_preferenceFileName = preferenceFilePath;
			if (File.Exists(_preferenceFileName))
			{
				_xmlDocument.Load(_preferenceFileName);
			}
			else
			{
				XmlDeclaration newChild = _xmlDocument.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
				_xmlDocument.AppendChild(newChild);
				_xmlDocument.AppendChild(_xmlDocument.CreateElement("User"));
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
				SaveSettings();
			}
		}

		public string FileName
		{
			get { return _preferenceFileName; }
			set { _preferenceFileName = value; }
		}

		public XmlDocument XmlDoc
		{
			get { return _xmlDocument; }
		}

		public event OnPreferenceChange PreferenceChange;

		public void SaveSettings()
		{
			_xmlDocument.Save(_preferenceFileName);
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

		// TODO This can be refactored
		public string GetChildString(string parentName, string name)
		{
			XmlNode node = _xmlDocument.DocumentElement.SelectSingleNode(parentName);
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
			return _preference2 ?? (_preference2 = new Preference2(Path.Combine(Paths.DataPath, "preferences")));
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

		// TODO This can be refactored to eliminte the potential nullreference
		public string GetString(string name)
		{
			XmlNode node = _xmlDocument.DocumentElement.SelectSingleNode(name);
			if (node == null)
			{
				return string.Empty;
			}
			return node.InnerText;
		}

		public void Reload()
		{
			_xmlDocument.Load(_preferenceFileName);
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
			SetChildString(parentName, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void SetChildString(string parentName, string name, string value)
		{
			var element = (XmlElement) _xmlDocument.DocumentElement.SelectSingleNode(parentName);
			if (element != null)
			{
				XmlAttribute attribute = element.Attributes[name];
				if (attribute == null)
				{
					element.Attributes.Append(attribute = _xmlDocument.CreateAttribute(name));
				}
				attribute.Value = value;
			}
		}

		public void SetInteger(string name, int value)
		{
			SetString(name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void SetInteger(string name, int value, int defaultValue)
		{
			SetString(name, value.ToString(CultureInfo.InvariantCulture), defaultValue.ToString(CultureInfo.InvariantCulture));
		}

		public void SetString(string name, string value)
		{
			SetString(name, value, null);
		}

		public void SetString(string name, string value, string defaultValue)
		{
			bool flag = GetString(name) != value;
			SetValue(_xmlDocument.DocumentElement, name, value, defaultValue);
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
				newChild = _xmlDocument.CreateElement(name);
				if (defaultValue != null)
				{
					XmlAttribute node = _xmlDocument.CreateAttribute("default");
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
			XmlNode node = _xmlDocument.DocumentElement.SelectSingleNode(name);
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

        public Screen GetScreen(string displayName) {
            var screen = Screen.AllScreens[0];

            foreach (var s in Screen.AllScreens) {
                if (!s.DeviceName.Equals(displayName)) {
                    continue;
                }
                screen = s;
                break;
            }

            return screen;
        }
	}
}