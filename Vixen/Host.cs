using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus {
    internal class Host {
        public static byte[,] Clipboard;
        internal static readonly Dictionary<string, object> Communication = new Dictionary<string, object>();
        private static ulong _lastKey;
        private readonly Form _hostForm;


        public Host(Form hostForm) {
            _hostForm = hostForm;
            Router = PlugInRouter.GetInstance();
        }


        public static bool InvokeRequired {
            get { return Application.OpenForms.Count > 0 && Application.OpenForms[0].InvokeRequired; }
        }

        public static PlugInRouter Router { get; private set; }

        public void DelegateNullMethod(MethodInvoker method) {
            if (_hostForm.InvokeRequired) {
                _hostForm.BeginInvoke(method);
            }
            else {
                method();
            }
        }


        public static ulong GetUniqueKey() {
            ulong num;
            do {
                num = BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
            } while (num == _lastKey);
            return (_lastKey = num);
        }


        public static void Invoke(Delegate method, params object[] args) {
            Application.OpenForms[0].Invoke(method, args);
        }


        public static void LogAudio(string source, string sourceNote, string audioFileName, int lengthInMilliseconds) {
            var path = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetString("AudioLogFilePath");
            if (path.Trim().Length == 0) {
                SetInterfacePrefs();
                MessageBox.Show(Resources.Host_LogAudioFailed, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                try {
                    var span = new TimeSpan(0, 0, 0, 0, lengthInMilliseconds);
                    var str2 = !string.IsNullOrEmpty(sourceNote)
                        ? string.Format("{0} [{1} - {2}]   {3} ({4})\n", DateTime.Now, source, sourceNote, audioFileName, span)
                        : string.Format("{0} [{1}]   {2} ({3})\n", DateTime.Now, source, audioFileName, span);
                    File.AppendAllText(path, str2);
                }
                catch (Exception exception) {
                    SetInterfacePrefs();
                    MessageBox.Show(
                        string.Format(
                            "An exception occurred when trying to log the use of an audio file:\n\n{0}\n\nAudio logging has been turned off.",
                            exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        private static void SetInterfacePrefs() {
            ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioManual", false);
            ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioScheduled", false);
            ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioMusicPlayer", false);
        }
    }
}