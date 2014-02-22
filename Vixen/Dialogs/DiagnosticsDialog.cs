using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;



using VixenPlus.Properties;
//TODO Remove this and then we can remove the timers as well.
using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    internal partial class DiagnosticsDialog : Form {
        private readonly Timers _timers;


        public DiagnosticsDialog(Timers timers) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            _timers = timers;
            if (Host.GetDebugValue("TraceStart") != null) {
                dateTimePickerTimerTraceFrom.Value = DateTime.Parse(Host.GetDebugValue("TraceStart"));
            }
            if (Host.GetDebugValue("TraceEnd") != null) {
                dateTimePickerTimerTraceTo.Value = DateTime.Parse(Host.GetDebugValue("TraceEnd"));
            }
            checkBoxTraceTimers.Checked = Host.GetDebugValue("TraceTimers") == bool.TrueString;
            checkBoxGetEventAverages.Checked = Host.GetDebugValue("EventAverages") != null;
            buttonShowOutputPluginDurations.Enabled = Host.GetDebugValue("event_average_0") != null;
        }


        private void buttonDumpTimers_Click(object sender, EventArgs e) {
            var text = string.Empty;
            var path = Path.Combine(Paths.DataPath, "timers.dump");
            if (File.Exists(path)) {
                File.Delete(path);
            }
            using (var writer = new StreamWriter(path)) {
                try {
                    var list = new List<Timer>(_timers.TimerArray);
                    writer.WriteLine("Timers dumped at " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine();
                    writer.WriteLine("(Starting timers)");
                    writer.WriteLine();
                    foreach (var timer in _timers.StartingTimers()) {
                        Host.DumpTimer(writer, timer);
                        list.Remove(timer);
                    }
                    writer.WriteLine("(Currently effective timers)");
                    writer.WriteLine();
                    foreach (var timer in _timers.CurrentlyEffectiveTimers()) {
                        Host.DumpTimer(writer, timer);
                        list.Remove(timer);
                    }
                    writer.WriteLine("(Other timers)");
                    writer.WriteLine();
                    foreach (var timer in list) {
                        Host.DumpTimer(writer, timer);
                    }
                }
                catch (Exception exception) {
                    text = exception.Message;
                }
            }
            if (text != string.Empty) {
                MessageBox.Show(text, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                MessageBox.Show(Resources.DumpFileWritten + path, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        private void buttonShowOutputPluginDurations_Click(object sender, EventArgs e) {
            var dialog = new EventAverageDialog();
            dialog.ShowDialog();
            dialog.Dispose();
        }


        private void checkBoxGetEventAverages_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxGetEventAverages.Checked) {
                Host.SetDebugValue("EventAverages");
            }
            else {
                Host.ResetDebugValue("EventAverages");
            }
        }


        private void checkBoxTraceTimers_CheckedChanged(object sender, EventArgs e) {
            Host.SetDebugValue("TraceTimers", checkBoxTraceTimers.Checked.ToString());
            if (checkBoxTraceTimers.Checked) {
                var time = DateTime.Today + dateTimePickerTimerTraceFrom.Value.TimeOfDay;
                Host.SetDebugValue("TraceStart", time.ToString(CultureInfo.InvariantCulture));
                Host.SetDebugValue("TraceEnd", (DateTime.Today + dateTimePickerTimerTraceTo.Value.TimeOfDay).ToString(CultureInfo.InvariantCulture));
            }
            else {
                Host.ResetDebugValue("TraceStart");
                Host.ResetDebugValue("TraceEnd");
            }
        }
    }
}
