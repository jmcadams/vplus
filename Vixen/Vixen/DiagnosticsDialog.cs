namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class DiagnosticsDialog : Form
    {
        private Timers m_timers;

        public DiagnosticsDialog(Timers timers)
        {
            this.InitializeComponent();
            this.m_timers = timers;
            if (Host.GetDebugValue("TraceStart") != null)
            {
                this.dateTimePickerTimerTraceFrom.Value = DateTime.Parse(Host.GetDebugValue("TraceStart"));
            }
            if (Host.GetDebugValue("TraceEnd") != null)
            {
                this.dateTimePickerTimerTraceTo.Value = DateTime.Parse(Host.GetDebugValue("TraceEnd"));
            }
            this.checkBoxTraceTimers.Checked = Host.GetDebugValue("TraceTimers") == bool.TrueString;
            if (Host.GetDebugValue("EventAverages") != null)
            {
                this.checkBoxGetEventAverages.Checked = true;
            }
            else
            {
                this.checkBoxGetEventAverages.Checked = false;
            }
            this.buttonShowOutputPluginDurations.Enabled = Host.GetDebugValue("event_average_0") != null;
        }

        private void buttonDumpTimers_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            string path = Path.Combine(Paths.DataPath, "timers.dump");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            StreamWriter writer = new StreamWriter(path);
            try
            {
                List<Vixen.Timer> list = new List<Vixen.Timer>(this.m_timers.TimerArray);
                writer.WriteLine("Timers dumped at " + DateTime.Now.ToString());
                writer.WriteLine();
                writer.WriteLine("(Starting timers)");
                writer.WriteLine();
                foreach (Vixen.Timer timer in this.m_timers.StartingTimers())
                {
                    Host.DumpTimer(writer, timer);
                    list.Remove(timer);
                }
                writer.WriteLine("(Currently effective timers)");
                writer.WriteLine();
                foreach (Vixen.Timer timer in this.m_timers.CurrentlyEffectiveTimers())
                {
                    Host.DumpTimer(writer, timer);
                    list.Remove(timer);
                }
                writer.WriteLine("(Other timers)");
                writer.WriteLine();
                foreach (Vixen.Timer timer in list)
                {
                    Host.DumpTimer(writer, timer);
                }
            }
            catch (Exception exception)
            {
                text = exception.Message;
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
            if (text != string.Empty)
            {
                MessageBox.Show(text, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Dump file written to " + path, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void buttonShowOutputPluginDurations_Click(object sender, EventArgs e)
        {
            EventAverageDialog dialog = new EventAverageDialog();
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void checkBoxGetEventAverages_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxGetEventAverages.Checked)
            {
                Host.SetDebugValue("EventAverages");
            }
            else
            {
                Host.ResetDebugValue("EventAverages");
            }
        }

        private void checkBoxTraceTimers_CheckedChanged(object sender, EventArgs e)
        {
            Host.SetDebugValue("TraceTimers", this.checkBoxTraceTimers.Checked.ToString());
            if (this.checkBoxTraceTimers.Checked)
            {
                DateTime time = DateTime.Today + this.dateTimePickerTimerTraceFrom.Value.TimeOfDay;
                Host.SetDebugValue("TraceStart", time.ToString());
                Host.SetDebugValue("TraceEnd", (DateTime.Today + this.dateTimePickerTimerTraceTo.Value.TimeOfDay).ToString());
            }
            else
            {
                Host.ResetDebugValue("TraceStart");
                Host.ResetDebugValue("TraceEnd");
            }
        }

        

        
    }
}

