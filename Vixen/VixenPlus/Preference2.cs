using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus {
    public class Preference2 {
        public delegate void OnPreferenceChange(string preferenceName);

        private static Preference2 _preference2;
        private readonly XmlDocument _preferencesDoc;


        private Preference2(string preferenceFilePath) {
            if (!Directory.Exists(Paths.DataPath)) {
                Directory.CreateDirectory(Paths.DataPath);
            }
            _preferencesDoc = new XmlDocument();
            FileName = preferenceFilePath;
            if (File.Exists(FileName)) {
                _preferencesDoc.Load(FileName);
            }
            else {
                var newChild = _preferencesDoc.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
                _preferencesDoc.AppendChild(newChild);
                _preferencesDoc.AppendChild(_preferencesDoc.CreateElement("User"));
            }
            var isDirty = false;
            //General - missing no.update and redirec.data
            isDirty |= CreateIfMissing("TimerCheckFrequency", 10);
            isDirty |= CreateIfMissing("MouseWheelVerticalDelta", 2);
            isDirty |= CreateIfMissing("MouseWheelHorizontalDelta", 10);
            isDirty |= CreateIfMissing("ClientName", Vendor.ProductName);
            isDirty |= CreateIfMissing("ResetAtStartup", false);
            isDirty |= CreateIfMissing("PreferredSequenceType", ".vix");
            isDirty |= CreateIfMissing("ShutdownTime", string.Empty);
            isDirty |= CreateIfMissing("HistoryImages", 0);

            // Screen & Colors
            isDirty |= CreateIfMissing("PrimaryDisplay", Screen.AllScreens[0].DeviceName);
            isDirty |= CreateIfMissing("Waveform", Color.White.ToArgb().ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("WaveformBackground", Color.Black.ToArgb().ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("WaveformZeroLine", Color.Red.ToArgb().ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("Crosshair", Color.Yellow.ToArgb().ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("MouseCaret", Color.Gray.ToArgb().ToString(CultureInfo.InvariantCulture));

            //New Sequence Settings
            isDirty |= CreateIfMissing("EventPeriod", 100);
            isDirty |= CreateIfMissing("MinimumLevel", 0);
            isDirty |= CreateIfMissing("MaximumLevel", 255);
            isDirty |= CreateIfMissing("WizardForNewSequences", true);
            isDirty |= CreateIfMissing("DefaultProfile", string.Empty);
            isDirty |= CreateIfMissing("DefaultSequenceAudioDevice", -1);

            //Sequence Editing
            isDirty |= CreateIfMissing("MaxColumnWidth", 30);
            isDirty |= CreateIfMissing("MaxRowHeight", 20);
            isDirty |= CreateIfMissing("IntensityLargeDelta", 5);
            isDirty |= CreateIfMissing("EventSequenceAutoSize", true);
            isDirty |= CreateIfMissing("SaveZoomLevels", true);
            isDirty |= CreateIfMissing("SaveZoomLevels", "column", "100%");
            isDirty |= CreateIfMissing("SaveZoomLevels", "row", "100%");
            isDirty |= CreateIfMissing("ShowSaveConfirmation", true);
            isDirty |= CreateIfMissing("ShowNaturalChannelNumber", false);
            isDirty |= CreateIfMissing("FlipScrollBehavior", false);
            isDirty |= CreateIfMissing("RemoteLibraryFTPURL", "");
            isDirty |= CreateIfMissing("RemoteLibraryHTTPURL", "");
            isDirty |= CreateIfMissing("RemoteLibraryFileName", "");
            isDirty |= CreateIfMissing("DefaultSequenceDirectory", "");
            isDirty |= CreateIfMissing("ShowWaveformZeroLine", true);

            //Sequence Execution
            isDirty |= CreateIfMissing("ShowPositionMarker", true);
            isDirty |= CreateIfMissing("AutoScrolling", true);
            isDirty |= CreateIfMissing("SavePlugInDialogPositions", true);
            isDirty |= CreateIfMissing("ClearAtEndOfSequence", true);
            isDirty |= CreateIfMissing("LogAudioManual", false);
            isDirty |= CreateIfMissing("LogAudioScheduled", false);
            isDirty |= CreateIfMissing("LogAudioMusicPlayer", false);
            isDirty |= CreateIfMissing("AudioLogFilePath", string.Empty);

            //Background
            isDirty |= CreateIfMissing("EnableBackgroundSequence", false);
            isDirty |= CreateIfMissing("BackgroundSequenceDelay", 10);
            isDirty |= CreateIfMissing("EnableBackgroundMusic", false);
            isDirty |= CreateIfMissing("BackgroundMusicDelay", 10);
            isDirty |= CreateIfMissing("EnableMusicFade", false);
            isDirty |= CreateIfMissing("MusicFadeDuration", 5);

            //Remote Execution
            isDirty |= CreateIfMissing("SynchronousData", "Embedded");
            isDirty |= CreateIfMissing("AsynchronousData", "Sync");

            //Engine
            isDirty |= CreateIfMissing("SecondaryEngine", "ScriptEngine.dll");

            //Set in various Application classes
            isDirty |= CreateIfMissing("CustomColors",
                                       "16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215");
            isDirty |= CreateIfMissing("ActualLevels", false);
            isDirty |= CreateIfMissing("BackgroundSequence", string.Empty);
            isDirty |= CreateIfMissing("BarLevels", false);

            if (isDirty) {
                SaveSettings();
            }
        }


        public string FileName { get; set; }

        public XmlDocument XmlDoc {
            get { return _preferencesDoc; }
        }

        public event OnPreferenceChange PreferenceChange;


        public void SaveSettings() {
            _preferencesDoc.Save(FileName);
        }


        public bool GetBoolean(string name) {
            var str = GetString(name);
            return str != string.Empty && Convert.ToBoolean(str);
        }


        public string GetChildString(string parentName, string name) {
            var value = string.Empty;
            // ReSharper disable PossibleNullReferenceException
            var parentNode = _preferencesDoc.DocumentElement.SelectSingleNode(parentName);
            if (parentNode != null) {
                var attribute = parentNode.Attributes[name];
                if (attribute != null) {
                    value = attribute.Value;
                }
            }
            // ReSharper restore PossibleNullReferenceException
            return value;
        }


        public static Preference2 GetInstance() {
            return _preference2 ?? (_preference2 = new Preference2(Path.Combine(Paths.DataPath, "preferences")));
        }


        public int GetInteger(string name) {
            var value = 0;
            var str = GetString(name);
            if (!String.IsNullOrEmpty(str)) {
                value = Convert.ToInt32(str);
            }
            return value;
        }


        public string GetString(string name) {
            var value = string.Empty;
            // ReSharper disable PossibleNullReferenceException
            var node = _preferencesDoc.DocumentElement.SelectSingleNode(name);
            if (node != null) {
                value = node.InnerText;
            }
            // ReSharper restore PossibleNullReferenceException
            return value;
        }


        public void Reload() {
            _preferencesDoc.Load(FileName);
        }


        public void SetBoolean(string name, bool value) {
            SetString(name, value.ToString());
        }


        public void SetBoolean(string name, bool value, bool defaultValue) {
            SetString(name, value.ToString(), defaultValue.ToString());
        }


        public void SetChildBoolean(string parentName, string name, bool value) {
            SetChildString(parentName, name, value.ToString());
        }


        public void SetChildInteger(string parentName, string name, int value) {
            SetChildString(parentName, name, value.ToString(CultureInfo.InvariantCulture));
        }


        public void SetChildString(string parentName, string name, string value) {
            // ReSharper disable PossibleNullReferenceException
            var element = (XmlElement)_preferencesDoc.DocumentElement.SelectSingleNode(parentName);
            // ReSharper restore PossibleNullReferenceException
            if (element == null) {
                return;
            }

            var attribute = element.Attributes[name];
            if (attribute == null) {
                element.Attributes.Append(attribute = _preferencesDoc.CreateAttribute(name));
            }
            attribute.Value = value;
        }


        public void SetInteger(string name, int value) {
            SetString(name, value.ToString(CultureInfo.InvariantCulture));
        }


        public void SetInteger(string name, int value, int defaultValue) {
            SetString(name, value.ToString(CultureInfo.InvariantCulture), defaultValue.ToString(CultureInfo.InvariantCulture));
        }


        public void SetString(string name, string value) {
            SetString(name, value, null);
        }


        public void SetString(string name, string value, string defaultValue) {
            var flag = GetString(name) != value;
            SetValue(_preferencesDoc.DocumentElement, name, value, defaultValue);
            if (flag && (PreferenceChange != null)) {
                PreferenceChange(name);
            }
        }


        public void SetValue(XmlNode parentNode, string name, string value, string defaultValue) {
            var newChild = (XmlElement) parentNode.SelectSingleNode(name);
            if (newChild == null) {
                newChild = _preferencesDoc.CreateElement(name);
                if (defaultValue != null) {
                    var node = _preferencesDoc.CreateAttribute("default");
                    newChild.Attributes.Append(node);
                }
                parentNode.AppendChild(newChild);
            }
            if (defaultValue != null) {
                newChild.Attributes["default"].Value = defaultValue;
            }
            newChild.InnerText = value;
        }


        private bool CreateIfMissing(string name, object defaultValue) {
            // ReSharper disable PossibleNullReferenceException
            var node = _preferencesDoc.DocumentElement.SelectSingleNode(name);
            // ReSharper restore PossibleNullReferenceException
            var isMissing = false;
            if (node == null) {
                isMissing = true;
                if (defaultValue is bool) {
                    SetBoolean(name, (bool) defaultValue, (bool) defaultValue);
                }
                else if (defaultValue is int) {
                    SetInteger(name, (int) defaultValue, (int) defaultValue);
                }
                else {
                    SetString(name, defaultValue.ToString(), defaultValue.ToString());
                }
            }
            return isMissing;
        }


        private bool CreateIfMissing(string parentName, string name, object defaultValue) {
            var isMissing = false;
            if (GetChildString(parentName, name) == string.Empty) {
                isMissing = true;
                if (defaultValue is bool) {
                    SetChildBoolean(parentName, name, (bool) defaultValue);
                }
                else if (defaultValue is int) {
                    SetChildInteger(parentName, name, (int) defaultValue);
                }
                else {
                    SetChildString(parentName, name, defaultValue.ToString());
                }
            }
            return isMissing;
        }


        public Screen GetScreen(string displayName) {
            var value = Screen.AllScreens[0];

            foreach (var s in Screen.AllScreens) {
                if (!s.DeviceName.Equals(displayName)) {
                    continue;
                }
                value = s;
                break;
            }

            return value;
        }
    }
}
