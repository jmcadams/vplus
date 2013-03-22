namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Timers;
    using System.Windows.Forms;

    internal class Host : IQueryable
    {
        public static byte[,] Clipboard = null;
        internal static Dictionary<string, object> Communication = new Dictionary<string, object>();
        private int m_backgroundExecutionContextHandle = 0;
        private System.Timers.Timer m_backgroundMusicDelayTimer;
        private ToolStripLabel m_backgroundMusicLabel = null;
        private ToolStripProgressBar m_backgroundProgressBar = null;
        private EventSequence m_backgroundSequence = null;
        private System.Timers.Timer m_backgroundSequenceDelayTimer;
        private IExecution m_executionInterface = null;
        private Form m_hostForm = null;
        private static ulong m_lastKey = 0L;
        private Vixen.MusicPlayer m_musicPlayer;
        private static Preference2 m_preferences = Preference2.GetInstance();
        private static Dictionary<string, string> m_properties = new Dictionary<string, string>();
        private static PlugInRouter m_singletonRouter = null;

        public Host(Form hostForm)
        {
            this.m_hostForm = hostForm;
            m_singletonRouter = PlugInRouter.GetInstance();
            this.m_backgroundSequenceDelayTimer = new System.Timers.Timer();
            this.m_backgroundSequenceDelayTimer.Elapsed += new ElapsedEventHandler(this.m_backgroundSequenceDelayTimer_Elapsed);
            this.m_backgroundMusicDelayTimer = new System.Timers.Timer();
            this.m_backgroundMusicDelayTimer.Elapsed += new ElapsedEventHandler(this.m_backgroundMusicDelayTimer_Elapsed);
            StatusStrip strip = (StatusStrip) this.m_hostForm.Controls.Find("statusStrip", true)[0];
            this.m_backgroundProgressBar = (ToolStripProgressBar) strip.Items.Find("toolStripProgressBarBackgroundSequenceRunning", false)[0];
            this.m_backgroundMusicLabel = (ToolStripLabel) strip.Items.Find("toolStripStatusLabelMusic", false)[0];
            this.m_musicPlayer = new Vixen.MusicPlayer();
            this.m_musicPlayer.SongChange += new Vixen.MusicPlayer.OnSongChange(this.m_musicPlayer_SongChange);
        }

        public static void BeginInvoke(Delegate method, params object[] args)
        {
            Application.OpenForms[0].BeginInvoke(method, args);
        }

        public static void ClearLog(string filePath)
        {
            File.Delete(filePath);
        }

        private void CreateBackgroundContext()
        {
            if (this.m_executionInterface == null)
            {
                this.m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
            }
            if (this.m_backgroundExecutionContextHandle == 0)
            {
                this.m_backgroundExecutionContextHandle = this.m_executionInterface.RequestContext(true, false, null);
                this.m_executionInterface.SetSynchronousContext(this.m_backgroundExecutionContextHandle, this.m_backgroundSequence);
            }
        }

        public void DelegateNullMethod(MethodInvoker method)
        {
            if (this.m_hostForm.InvokeRequired)
            {
                this.m_hostForm.BeginInvoke(method);
            }
            else
            {
                method();
            }
        }

        public static void DumpTimer(StreamWriter writer, Vixen.Timer timer)
        {
            writer.WriteLine("[Timer for {0}]", Path.GetFileName(timer.ProgramFileName));
            writer.WriteLine("Executing? " + timer.IsExecuting.ToString());
            writer.WriteLine("Last execution: " + timer.LastExecution.ToString());
            writer.WriteLine("Not valid until: " + timer.NotValidUntil.ToString());
            writer.WriteLine("Object length: " + timer.ObjectLength.ToString());
            writer.WriteLine("Recurrence: {0} ({1})", timer.Recurrence, timer.RecurrenceData);
            writer.WriteLine("Recurrence start: " + timer.RecurrenceStart.ToString());
            writer.WriteLine("Recurrence start date/time: " + timer.RecurrenceStartDateTime.ToString());
            writer.WriteLine("Recurrence span: " + timer.RecurrenceSpan.ToString());
            writer.WriteLine("Recurrence end: " + timer.RecurrenceEnd.ToString());
            writer.WriteLine("Recurrence end date/time: " + timer.RecurrenceEndDateTime.ToString());
            writer.WriteLine("Repeat interval: " + timer.RepeatInterval.ToString());
            writer.WriteLine("Start date: " + timer.StartDate.ToString());
            writer.WriteLine("Start time: " + timer.StartTime.ToString());
            writer.WriteLine("Start date/time: " + timer.StartDateTime.ToString());
            writer.WriteLine("Timer length: " + timer.TimerLength.ToString());
            writer.WriteLine("End date: " + timer.EndDate.ToString());
            writer.WriteLine("End time: " + timer.EndTime.ToString());
            writer.WriteLine("End date/time: " + timer.EndDateTime.ToString());
            writer.WriteLine();
        }

        public void ExecuteBackgroundSequence()
        {
            if ((((this.m_executionInterface != null) && (this.m_backgroundExecutionContextHandle != 0)) && (this.m_backgroundSequence != null)) && !this.m_executionInterface.ExecutePlay(this.m_backgroundExecutionContextHandle, 0, 0, m_preferences.GetBoolean("LogAudioScheduled")))
            {
                MessageBox.Show("There was a problem starting the background sequence.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static string GetDebugValue(string name)
        {
            string str;
            m_properties.TryGetValue(name, out str);
            return str;
        }

        public static ulong GetUniqueKey()
        {
            ulong num;
            do
            {
                num = BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
            }
            while (num == m_lastKey);
            return (m_lastKey = num);
        }

        public static void Invoke(Delegate method, params object[] args)
        {
            Application.OpenForms[0].Invoke(method, args);
        }

        public bool IsBackgroundExecutionEngineInstance(Engine8 engine)
        {
            return (((this.m_executionInterface != null) && (this.m_backgroundExecutionContextHandle != 0)) && (this.m_executionInterface.FindExecutionContextHandle(engine) == this.m_backgroundExecutionContextHandle));
        }

        public static void LogAudio(string source, string sourceNote, string audioFileName, int lengthInMilliseconds)
        {
            string path = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetString("AudioLogFilePath");
            if (path.Trim().Length == 0)
            {
                ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioManual", false);
                ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioScheduled", false);
                ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioMusicPlayer", false);
                MessageBox.Show("Audio logging is enabled but no log file is specified.\n\nAudio logging has been turned off.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    string str2;
                    TimeSpan span = new TimeSpan(0, 0, 0, 0, lengthInMilliseconds);
                    if ((sourceNote != null) && (sourceNote.Length != 0))
                    {
                        str2 = string.Format("{0} [{1} - {2}]   {3} ({4})\n", new object[] { DateTime.Now, source, sourceNote, audioFileName, span });
                    }
                    else
                    {
                        str2 = string.Format("{0} [{1}]   {2} ({3})\n", new object[] { DateTime.Now, source, audioFileName, span });
                    }
                    File.AppendAllText(path, str2);
                }
                catch (Exception exception)
                {
                    ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioManual", false);
                    ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioScheduled", false);
                    ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioMusicPlayer", false);
                    MessageBox.Show(string.Format("An exception occurred when trying to log the use of an audio file:\n\n{0}\n\nAudio logging has been turned off.", exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public static void LogTo(string filePath, string message)
        {
            File.AppendAllText(filePath, message + "\n");
        }

        private void m_backgroundMusicDelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.m_backgroundMusicDelayTimer.Enabled = false;
            this.m_hostForm.BeginInvoke(new MethodInvoker(this.ShowBackgroundMusicThumbSucker));
            this.m_hostForm.BeginInvoke(new MethodInvoker(this.m_musicPlayer.Start));
        }

        private void m_backgroundSequenceDelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.m_backgroundSequenceDelayTimer.Enabled = false;
            this.m_hostForm.BeginInvoke(new MethodInvoker(this.ShowBackgroundSequenceThumbSucker));
            this.ExecuteBackgroundSequence();
        }

        private void m_musicPlayer_SongChange(string songName)
        {
            this.m_backgroundMusicLabel.Text = songName;
        }

        public string QueryInstance(int index)
        {
            StringBuilder builder = new StringBuilder();
            if (index == 0)
            {
                builder.AppendLine("(Background Sequence)");
                builder.AppendLine("Execution handle: " + this.m_backgroundExecutionContextHandle);
                builder.AppendLine("Sequence: " + ((this.m_backgroundSequence == null) ? "(null)" : this.m_backgroundSequence.Name));
            }
            else
            {
                builder.AppendLine("(Music Player)");
                builder.AppendLine("Song count: " + this.m_musicPlayer.SongCount);
                builder.AppendLine("Playing: " + this.m_musicPlayer.IsPlaying);
                if (this.m_musicPlayer.IsPlaying)
                {
                    builder.AppendLine("Song name: " + this.m_musicPlayer.CurrentSongName);
                    builder.AppendLine("Song length: " + this.m_musicPlayer.CurrentSongLength);
                }
            }
            return builder.ToString();
        }

        public static void ResetDebugValue(string name)
        {
            if (m_properties.ContainsKey(name))
            {
                m_properties.Remove(name);
            }
        }

        public static void SetDebugValue(string name)
        {
            m_properties[name] = string.Empty;
        }

        public static void SetDebugValue(string name, string value)
        {
            m_properties[name] = value;
        }

        private void ShowBackgroundMusicThumbSucker()
        {
            if (!this.m_backgroundMusicLabel.Visible)
            {
                this.m_backgroundMusicLabel.Text = string.Empty;
                this.m_backgroundMusicLabel.Visible = true;
            }
        }

        private void ShowBackgroundSequenceThumbSucker()
        {
            if (!this.m_backgroundProgressBar.Visible)
            {
                this.m_backgroundProgressBar.ToolTipText = this.m_backgroundSequence.Name + " is running";
                this.m_backgroundProgressBar.Visible = true;
                this.m_backgroundProgressBar.Enabled = true;
            }
        }

        public void StartBackgroundMusic()
        {
            if ((this.m_musicPlayer.SongCount != 0) && m_preferences.GetBoolean("EnableBackgroundMusic"))
            {
                this.m_backgroundMusicDelayTimer.Interval = m_preferences.GetInteger("BackgroundMusicDelay") * 0x3e8;
                this.m_backgroundMusicDelayTimer.Enabled = true;
            }
        }

        public void StartBackgroundObjects()
        {
            this.StartBackgroundSequence();
            this.StartBackgroundMusic();
        }

        public void StartBackgroundSequence()
        {
            if ((this.m_backgroundSequence != null) && m_preferences.GetBoolean("EnableBackgroundSequence"))
            {
                this.CreateBackgroundContext();
                if (this.m_executionInterface.EngineStatus(this.m_backgroundExecutionContextHandle) == 0)
                {
                    this.m_backgroundSequenceDelayTimer.Interval = m_preferences.GetInteger("BackgroundSequenceDelay") * 0x3e8;
                    this.m_backgroundSequenceDelayTimer.Enabled = true;
                }
            }
        }

        public void StopBackgroundMusic()
        {
            this.m_musicPlayer.Stop();
            this.m_backgroundMusicDelayTimer.Enabled = false;
            this.m_backgroundMusicLabel.Visible = false;
        }

        public void StopBackgroundObjects()
        {
            this.StopBackgroundSequence();
            this.StopBackgroundMusic();
        }

        public void StopBackgroundSequence()
        {
            this.StopBackgroundSequenceUI();
            this.StopBackgroundSequenceExecution();
        }

        public void StopBackgroundSequenceExecution()
        {
            if (this.m_backgroundExecutionContextHandle != 0)
            {
                this.m_executionInterface.ExecuteStop(this.m_backgroundExecutionContextHandle);
                this.m_executionInterface.ReleaseContext(this.m_backgroundExecutionContextHandle);
                this.m_backgroundExecutionContextHandle = 0;
            }
        }

        public void StopBackgroundSequenceUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(this.StopBackgroundSequenceUI), new object[0]);
            }
            else
            {
                this.m_backgroundSequenceDelayTimer.Enabled = false;
                this.m_backgroundProgressBar.Visible = false;
                this.m_backgroundProgressBar.Enabled = false;
            }
        }

        public string BackgroundSequenceName
        {
            get
            {
                if (this.m_backgroundSequence != null)
                {
                    return this.m_backgroundSequence.FileName;
                }
                return null;
            }
            set
            {
                this.StopBackgroundSequence();
                if ((value == null) || (value == string.Empty))
                {
                    if (this.m_backgroundSequence != null)
                    {
                        this.m_backgroundSequence.Dispose();
                        this.m_backgroundSequence = null;
                    }
                }
                else if (!File.Exists(value))
                {
                    if (m_preferences.GetBoolean("EnableBackgroundSequence"))
                    {
                        MessageBox.Show("A background sequence has been specified, but it does not exist.\nThis message will show each time you start the application and this situation exists.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    this.m_backgroundSequence = new EventSequence(value);
                }
            }
        }

        public int Count
        {
            get
            {
                return 2;
            }
        }

        public static bool InvokeRequired
        {
            get
            {
                return Application.OpenForms[0].InvokeRequired;
            }
        }

        public Vixen.MusicPlayer MusicPlayer
        {
            get
            {
                return this.m_musicPlayer;
            }
        }

        public static Preference2 Preferences
        {
            get
            {
                return m_preferences;
            }
        }

        public static PlugInRouter Router
        {
            get
            {
                return m_singletonRouter;
            }
        }
    }
}

