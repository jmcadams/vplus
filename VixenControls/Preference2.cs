using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Common {
    public class Preference2 {
        public delegate void OnPreferenceChange(string preferenceName);

        private static Preference2 _preference2;
        private readonly XmlDocument _preferencesDoc;

        public const string CustomColorsPreference = "CustomColors";

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
            //General
            isDirty |= CreateIfMissing("MouseWheelVerticalDelta", 2);
            isDirty |= CreateIfMissing("MouseWheelHorizontalDelta", 10);
            isDirty |= CreateIfMissing("PreferredSequenceType", ".vix");
            isDirty |= CreateIfMissing("ShutdownTime", string.Empty);
            isDirty |= CreateIfMissing("HistoryImages", 0);
            isDirty |= CreateIfMissing("AutoSaveToolbars", false);
            isDirty |= CreateIfMissing("RecentFiles", 7);
            isDirty |= CreateIfMissing("AutoUpdateCheckFreq", "On Startup");
            isDirty |= CreateIfMissing("LastUpdateCheck", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("SkippedVersion", string.Empty);

            // Screen & Colors
            isDirty |= CreateIfMissing("PrimaryDisplay", FixDeviceName(Screen.AllScreens[0].DeviceName));
            isDirty |= CreateIfMissing("ChannelBackground", "-1");
            isDirty |= CreateIfMissing("Crosshair", "-256");
            isDirty |= CreateIfMissing("GridBackground", "-11513776");
            isDirty |= CreateIfMissing("GridLines", "-16777216");
            isDirty |= CreateIfMissing("MouseCaret", "-65536");
            isDirty |= CreateIfMissing("Waveform", "-1");
            isDirty |= CreateIfMissing("WaveformBackground", "-16777216");
            isDirty |= CreateIfMissing("WaveformZeroLine", Color.Red.ToArgb().ToString(CultureInfo.InvariantCulture));
            isDirty |= CreateIfMissing("RoutineBitmap", Color.LightBlue.ToArgb().ToString(CultureInfo.InvariantCulture));

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
            isDirty |= CreateIfMissing("DefaultSequenceDirectory", "");
            isDirty |= CreateIfMissing("ShowWaveformZeroLine", true);
            isDirty |= CreateIfMissing("SilenceProfileErrors", false);

            //Sequence Execution
            isDirty |= CreateIfMissing("ShowPositionMarker", true);
            isDirty |= CreateIfMissing("AutoScrolling", true);
            isDirty |= CreateIfMissing("SavePlugInDialogPositions", true);
            isDirty |= CreateIfMissing("ClearAtEndOfSequence", true);

            //Background
            isDirty |= CreateIfMissing("EnableBackgroundSequence", false);
            isDirty |= CreateIfMissing("BackgroundSequenceDelay", 10);
            isDirty |= CreateIfMissing("EnableBackgroundMusic", false);
            isDirty |= CreateIfMissing("BackgroundMusicDelay", 10);
            isDirty |= CreateIfMissing("EnableMusicFade", false);
            isDirty |= CreateIfMissing("MusicFadeDuration", 5);

            //Set in various Application classes
            isDirty |= CreateIfMissing(CustomColorsPreference,
                "16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215,16777215");
            isDirty |= CreateIfMissing("ActualLevels", false);
            isDirty |= CreateIfMissing("BackgroundSequence", string.Empty);
            isDirty |= CreateIfMissing("BarLevels", false);

            if (isDirty) {
                SaveSettings();
            }
        }


        private string FileName { get; set; }

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
            if (parentNode == null) {
                return value;
            }
            var attribute = parentNode.Attributes[name];
            if (attribute != null) {
                value = attribute.Value;
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


        private void SetBoolean(string name, bool value, bool defaultValue) {
            SetString(name, value.ToString(), defaultValue.ToString());
        }


        private void SetChildBoolean(string parentName, string name, bool value) {
            SetChildString(parentName, name, value.ToString());
        }


        private void SetChildInteger(string parentName, string name, int value) {
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


        public void SetString(string name, string value, string defaultValue = null) {
            var flag = GetString(name) != value;
            SetValue(_preferencesDoc.DocumentElement, name, value, defaultValue);
            if (flag && (PreferenceChange != null)) {
                PreferenceChange(name);
            }
        }


        private void SetValue(XmlNode parentNode, string name, string value, string defaultValue) {
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
            if (node != null) {
                return false;
            }
            if (defaultValue is bool) {
                SetBoolean(name, (bool) defaultValue, (bool) defaultValue);
            }
            else if (defaultValue is int) {
                SetInteger(name, (int) defaultValue, (int) defaultValue);
            }
            else {
                SetString(name, defaultValue.ToString(), defaultValue.ToString());
            }
            return true;
        }


        private bool CreateIfMissing(string parentName, string name, object defaultValue) {
            if (GetChildString(parentName, name) != string.Empty) {
                return false;
            }
            if (defaultValue is bool) {
                SetChildBoolean(parentName, name, (bool) defaultValue);
            }
            else if (defaultValue is int) {
                SetChildInteger(parentName, name, (int) defaultValue);
            }
            else {
                SetChildString(parentName, name, defaultValue.ToString());
            }
            return true;
        }


        public static Screen GetScreen(string displayName) {
            var value = Screen.AllScreens[0];

            foreach (var s in Screen.AllScreens.Where(s => FixDeviceName(s.DeviceName).Equals(displayName))) {
                value = s;
                break;
            }

            return value;
        }


        public static string FixDeviceName(string deviceName)
        {
            var garbageStart = deviceName.IndexOf("\0", StringComparison.Ordinal);
            return garbageStart < 0 ? deviceName : deviceName.Substring(0, garbageStart);
        }

        //TODO Refactor this when we move all color dialogs to our own.
        public int[] CustomColors {
            get {
                var loadCustomColors = GetString(CustomColorsPreference).Split(new[] { ',' });
                var numArray = new int[loadCustomColors.Length];
                for (var i = 0; i < loadCustomColors.Length; i++) {
                    numArray[i] = int.Parse(loadCustomColors[i]);
                }
                return numArray;
            }
            set {
                var saveCustomColors = new string[value.Length];
                for (var i = 0; i < saveCustomColors.Length; i++) {
                    saveCustomColors[i] = value[i].ToString(CultureInfo.InvariantCulture);
                }
                SetString(CustomColorsPreference, string.Join(",", saveCustomColors));
            }
        }
    }
}