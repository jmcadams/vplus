namespace Vixen
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class Preference2
    {
        private XmlDocument m_doc;
        private static Preference2 m_instance = null;
        private string m_preferenceFileName;
        private const string PREFERENCES_FILE_NAME = "preferences";

        public event OnPreferenceChange PreferenceChange;

        private Preference2(string preferenceFilePath)
        {
            if (!Directory.Exists(Paths.DataPath))
            {
                Directory.CreateDirectory(Paths.DataPath);
            }
            this.m_doc = new XmlDocument();
            this.m_preferenceFileName = preferenceFilePath;
            if (File.Exists(this.m_preferenceFileName))
            {
                this.m_doc.Load(this.m_preferenceFileName);
            }
            else
            {
                XmlDeclaration newChild = this.m_doc.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
                this.m_doc.AppendChild(newChild);
                this.m_doc.AppendChild(this.m_doc.CreateElement("User"));
            }
            bool flag = false;
            flag |= this.VerifyPreference("EventPeriod", 100);
            flag |= this.VerifyPreference("MinimumLevel", 0);
            flag |= this.VerifyPreference("MaximumLevel", 0xff);
            flag |= this.VerifyPreference("MaxColumnWidth", 30);
            flag |= this.VerifyPreference("MaxRowHeight", 20);
            flag |= this.VerifyPreference("ShowPositionMarker", true);
            flag |= this.VerifyPreference("AutoScrolling", true);
            flag |= this.VerifyPreference("SavePlugInDialogPositions", true);
            flag |= this.VerifyPreference("EventSequenceAutoSize", true);
            flag |= this.VerifyPreference("WizardForNewSequences", true);
            flag |= this.VerifyPreference("SaveZoomLevels", true);
            flag |= this.VerifyPreference("SaveZoomLevels", "column", "100%");
            flag |= this.VerifyPreference("SaveZoomLevels", "row", "100%");
            flag |= this.VerifyPreference("ShowSaveConfirmation", true);
            flag |= this.VerifyPreference("ClearAtEndOfSequence", true);
            flag |= this.VerifyPreference("TimerCheckFrequency", 10);
            flag |= this.VerifyPreference("BackgroundSequence", string.Empty);
            flag |= this.VerifyPreference("EnableBackgroundSequence", false);
            flag |= this.VerifyPreference("BackgroundSequenceDelay", 10);
            flag |= this.VerifyPreference("SecondaryEngine", "ScriptEngine.dll");
            flag |= this.VerifyPreference("MouseWheelVerticalDelta", 2);
            flag |= this.VerifyPreference("MouseWheelHorizontalDelta", 10);
            flag |= this.VerifyPreference("IntensityLargeDelta", 5);
            flag |= this.VerifyPreference("EnableBackgroundMusic", false);
            flag |= this.VerifyPreference("BackgroundMusicDelay", 10);
            flag |= this.VerifyPreference("EnableMusicFade", false);
            flag |= this.VerifyPreference("MusicFadeDuration", 5);
            flag |= this.VerifyPreference("ClientName", Vendor.ProductName);
            flag |= this.VerifyPreference("ResetAtStartup", false);
            flag |= this.VerifyPreference("DefaultProfile", string.Empty);
            flag |= this.VerifyPreference("PreferredSequenceType", ".vix");
            flag |= this.VerifyPreference("SynchronousData", "Embedded");
            flag |= this.VerifyPreference("AsynchronousData", "Sync");
            flag |= this.VerifyPreference("ShutdownTime", string.Empty);
            flag |= this.VerifyPreference("CustomColors", "16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215");
            flag |= this.VerifyPreference("ActualLevels", false);
            flag |= this.VerifyPreference("ShowNaturalChannelNumber", false);
            flag |= this.VerifyPreference("DefaultSequenceAudioDevice", -1);
            flag |= this.VerifyPreference("HistoryImages", 0);
            flag |= this.VerifyPreference("LogAudioManual", false);
            flag |= this.VerifyPreference("LogAudioScheduled", false);
            flag |= this.VerifyPreference("LogAudioMusicPlayer", false);
            flag |= this.VerifyPreference("AudioLogFilePath", string.Empty);
            flag |= this.VerifyPreference("BarLevels", false);
            flag |= this.VerifyPreference("FlipScrollBehavior", false);
            flag |= this.VerifyPreference("RemoteLibraryFTPURL", "");
            flag |= this.VerifyPreference("RemoteLibraryHTTPURL", "");
            flag |= this.VerifyPreference("RemoteLibraryFileName", "");
            if (flag | this.VerifyPreference("DefaultSequenceDirectory", ""))
            {
                this.Flush();
            }
        }

        public void Flush()
        {
            this.m_doc.Save(this.m_preferenceFileName);
        }

        public bool GetBoolean(string name)
        {
            string str = this.GetString(name);
            if (str == string.Empty)
            {
                return false;
            }
            return Convert.ToBoolean(str);
        }

        public string GetChildString(string parentName, string name)
        {
            XmlNode node = this.m_doc.DocumentElement.SelectSingleNode(parentName);
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
            if (m_instance == null)
            {
                m_instance = new Preference2(Path.Combine(Paths.DataPath, "preferences"));
            }
            return m_instance;
        }

        public int GetInteger(string name)
        {
            string str = this.GetString(name);
            if (str == string.Empty)
            {
                return 0;
            }
            return Convert.ToInt32(str);
        }

        public string GetString(string name)
        {
            XmlNode node = this.m_doc.DocumentElement.SelectSingleNode(name);
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        public void Reload()
        {
            this.m_doc.Load(this.m_preferenceFileName);
        }

        public void SetBoolean(string name, bool value)
        {
            this.SetString(name, value.ToString());
        }

        public void SetBoolean(string name, bool value, bool defaultValue)
        {
            this.SetString(name, value.ToString(), defaultValue.ToString());
        }

        public void SetChildBoolean(string parentName, string name, bool value)
        {
            this.SetChildString(parentName, name, value.ToString());
        }

        public void SetChildInteger(string parentName, string name, int value)
        {
            this.SetChildString(parentName, name, value.ToString());
        }

        public void SetChildString(string parentName, string name, string value)
        {
            XmlElement element = (XmlElement) this.m_doc.DocumentElement.SelectSingleNode(parentName);
            if (element != null)
            {
                XmlAttribute attribute = element.Attributes[name];
                if (attribute == null)
                {
                    element.Attributes.Append(attribute = this.m_doc.CreateAttribute(name));
                }
                attribute.Value = value;
            }
        }

        public void SetInteger(string name, int value)
        {
            this.SetString(name, value.ToString());
        }

        public void SetInteger(string name, int value, int defaultValue)
        {
            this.SetString(name, value.ToString(), defaultValue.ToString());
        }

        public void SetString(string name, string value)
        {
            this.SetString(name, value, null);
        }

        public void SetString(string name, string value, string defaultValue)
        {
            bool flag = this.GetString(name) != value;
            this.SetValue(this.m_doc.DocumentElement, name, value, defaultValue);
            if (flag && (this.PreferenceChange != null))
            {
                this.PreferenceChange(name);
            }
        }

        public void SetValue(XmlNode parentNode, string name, string value, string defaultValue)
        {
            XmlElement newChild = (XmlElement) parentNode.SelectSingleNode(name);
            if (newChild == null)
            {
                newChild = this.m_doc.CreateElement(name);
                if (defaultValue != null)
                {
                    XmlAttribute node = this.m_doc.CreateAttribute("default");
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
            XmlNode node = this.m_doc.DocumentElement.SelectSingleNode(name);
            bool flag = false;
            if (node == null)
            {
                flag = true;
                if (defaultValue is bool)
                {
                    this.SetBoolean(name, (bool) defaultValue, (bool) defaultValue);
                    return flag;
                }
                if (defaultValue is int)
                {
                    this.SetInteger(name, (int) defaultValue, (int) defaultValue);
                    return flag;
                }
                this.SetString(name, defaultValue.ToString(), defaultValue.ToString());
            }
            return flag;
        }

        private bool VerifyPreference(string parentName, string name, object defaultValue)
        {
            bool flag = false;
            if (this.GetChildString(parentName, name) == string.Empty)
            {
                flag = true;
                if (defaultValue is bool)
                {
                    this.SetChildBoolean(parentName, name, (bool) defaultValue);
                    return flag;
                }
                if (defaultValue is int)
                {
                    this.SetChildInteger(parentName, name, (int) defaultValue);
                    return flag;
                }
                this.SetChildString(parentName, name, defaultValue.ToString());
            }
            return flag;
        }

        public string FileName
        {
            get
            {
                return this.m_preferenceFileName;
            }
            set
            {
                this.m_preferenceFileName = value;
            }
        }

        public XmlDocument XmlDoc
        {
            get
            {
                return this.m_doc;
            }
        }

        public delegate void OnPreferenceChange(string preferenceName);
    }
}

