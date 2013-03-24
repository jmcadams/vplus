using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace VixenPlus
{
	internal class Host : IQueryable
	{
		public static byte[,] Clipboard = null;
		internal static Dictionary<string, object> Communication = new Dictionary<string, object>();
		private static ulong m_lastKey;
		private static readonly Preference2 m_preferences = Preference2.GetInstance();
		private static readonly Dictionary<string, string> m_properties = new Dictionary<string, string>();
		private static PlugInRouter m_singletonRouter;
		private readonly System.Timers.Timer m_backgroundMusicDelayTimer;
		private readonly ToolStripLabel m_backgroundMusicLabel;
		private readonly ToolStripProgressBar m_backgroundProgressBar;
		private readonly System.Timers.Timer m_backgroundSequenceDelayTimer;
		private readonly Form m_hostForm;
		private readonly MusicPlayer m_musicPlayer;
		private int m_backgroundExecutionContextHandle;
		private EventSequence m_backgroundSequence;
		private IExecution m_executionInterface;

		public Host(Form hostForm)
		{
			m_hostForm = hostForm;
			m_singletonRouter = PlugInRouter.GetInstance();
			m_backgroundSequenceDelayTimer = new System.Timers.Timer();
			m_backgroundSequenceDelayTimer.Elapsed += m_backgroundSequenceDelayTimer_Elapsed;
			m_backgroundMusicDelayTimer = new System.Timers.Timer();
			m_backgroundMusicDelayTimer.Elapsed += m_backgroundMusicDelayTimer_Elapsed;
			var strip = (StatusStrip) m_hostForm.Controls.Find("statusStrip", true)[0];
			m_backgroundProgressBar =
				(ToolStripProgressBar) strip.Items.Find("toolStripProgressBarBackgroundSequenceRunning", false)[0];
			m_backgroundMusicLabel = (ToolStripLabel) strip.Items.Find("toolStripStatusLabelMusic", false)[0];
			m_musicPlayer = new MusicPlayer();
			m_musicPlayer.SongChange += m_musicPlayer_SongChange;
		}

		public string BackgroundSequenceName
		{
			get
			{
				if (m_backgroundSequence != null)
				{
					return m_backgroundSequence.FileName;
				}
				return null;
			}
			set
			{
				StopBackgroundSequence();
				if ((value == null) || (value == string.Empty))
				{
					if (m_backgroundSequence != null)
					{
						m_backgroundSequence.Dispose();
						m_backgroundSequence = null;
					}
				}
				else if (!File.Exists(value))
				{
					if (m_preferences.GetBoolean("EnableBackgroundSequence"))
					{
						MessageBox.Show(
							"A background sequence has been specified, but it does not exist.\nThis message will show each time you start the application and this situation exists.",
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					m_backgroundSequence = new EventSequence(value);
				}
			}
		}

		public static bool InvokeRequired
		{
			get { return Application.OpenForms[0].InvokeRequired; }
		}

		public MusicPlayer MusicPlayer
		{
			get { return m_musicPlayer; }
		}

		public static Preference2 Preferences
		{
			get { return m_preferences; }
		}

		public static PlugInRouter Router
		{
			get { return m_singletonRouter; }
		}

		public string QueryInstance(int index)
		{
			var builder = new StringBuilder();
			if (index == 0)
			{
				builder.AppendLine("(Background Sequence)");
				builder.AppendLine("Execution handle: " + m_backgroundExecutionContextHandle);
				builder.AppendLine("Sequence: " + ((m_backgroundSequence == null) ? "(null)" : m_backgroundSequence.Name));
			}
			else
			{
				builder.AppendLine("(Music Player)");
				builder.AppendLine("Song count: " + m_musicPlayer.SongCount);
				builder.AppendLine("Playing: " + m_musicPlayer.IsPlaying);
				if (m_musicPlayer.IsPlaying)
				{
					builder.AppendLine("Song name: " + m_musicPlayer.CurrentSongName);
					builder.AppendLine("Song length: " + m_musicPlayer.CurrentSongLength);
				}
			}
			return builder.ToString();
		}

		public int Count
		{
			get { return 2; }
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
			if (m_executionInterface == null)
			{
				m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			}
			if (m_backgroundExecutionContextHandle == 0)
			{
				m_backgroundExecutionContextHandle = m_executionInterface.RequestContext(true, false, null);
				m_executionInterface.SetSynchronousContext(m_backgroundExecutionContextHandle, m_backgroundSequence);
			}
		}

		public void DelegateNullMethod(MethodInvoker method)
		{
			if (m_hostForm.InvokeRequired)
			{
				m_hostForm.BeginInvoke(method);
			}
			else
			{
				method();
			}
		}

		public static void DumpTimer(StreamWriter writer, Timer timer)
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
			if ((((m_executionInterface != null) && (m_backgroundExecutionContextHandle != 0)) && (m_backgroundSequence != null)) &&
			    !m_executionInterface.ExecutePlay(m_backgroundExecutionContextHandle, 0, 0,
			                                      m_preferences.GetBoolean("LogAudioScheduled")))
			{
				MessageBox.Show("There was a problem starting the background sequence.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
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
			} while (num == m_lastKey);
			return (m_lastKey = num);
		}

		public static void Invoke(Delegate method, params object[] args)
		{
			Application.OpenForms[0].Invoke(method, args);
		}

		public bool IsBackgroundExecutionEngineInstance(Engine8 engine)
		{
			return (((m_executionInterface != null) && (m_backgroundExecutionContextHandle != 0)) &&
			        (m_executionInterface.FindExecutionContextHandle(engine) == m_backgroundExecutionContextHandle));
		}

		public static void LogAudio(string source, string sourceNote, string audioFileName, int lengthInMilliseconds)
		{
			string path = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetString("AudioLogFilePath");
			if (path.Trim().Length == 0)
			{
				((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioManual", false);
				((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioScheduled", false);
				((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioMusicPlayer", false);
				MessageBox.Show("Audio logging is enabled but no log file is specified.\n\nAudio logging has been turned off.",
				                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				try
				{
					string str2;
					var span = new TimeSpan(0, 0, 0, 0, lengthInMilliseconds);
					if ((sourceNote != null) && (sourceNote.Length != 0))
					{
						str2 = string.Format("{0} [{1} - {2}]   {3} ({4})\n",
						                     new object[] {DateTime.Now, source, sourceNote, audioFileName, span});
					}
					else
					{
						str2 = string.Format("{0} [{1}]   {2} ({3})\n", new object[] {DateTime.Now, source, audioFileName, span});
					}
					File.AppendAllText(path, str2);
				}
				catch (Exception exception)
				{
					((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioManual", false);
					((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioScheduled", false);
					((ISystem) Interfaces.Available["ISystem"]).UserPreferences.SetBoolean("LogAudioMusicPlayer", false);
					MessageBox.Show(
						string.Format(
							"An exception occurred when trying to log the use of an audio file:\n\n{0}\n\nAudio logging has been turned off.",
							exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		public static void LogTo(string filePath, string message)
		{
			File.AppendAllText(filePath, message + "\n");
		}

		private void m_backgroundMusicDelayTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			m_backgroundMusicDelayTimer.Enabled = false;
			m_hostForm.BeginInvoke(new MethodInvoker(ShowBackgroundMusicThumbSucker));
			m_hostForm.BeginInvoke(new MethodInvoker(m_musicPlayer.Start));
		}

		private void m_backgroundSequenceDelayTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			m_backgroundSequenceDelayTimer.Enabled = false;
			m_hostForm.BeginInvoke(new MethodInvoker(ShowBackgroundSequenceThumbSucker));
			ExecuteBackgroundSequence();
		}

		private void m_musicPlayer_SongChange(string songName)
		{
			m_backgroundMusicLabel.Text = songName;
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
			if (!m_backgroundMusicLabel.Visible)
			{
				m_backgroundMusicLabel.Text = string.Empty;
				m_backgroundMusicLabel.Visible = true;
			}
		}

		private void ShowBackgroundSequenceThumbSucker()
		{
			if (!m_backgroundProgressBar.Visible)
			{
				m_backgroundProgressBar.ToolTipText = m_backgroundSequence.Name + " is running";
				m_backgroundProgressBar.Visible = true;
				m_backgroundProgressBar.Enabled = true;
			}
		}

		public void StartBackgroundMusic()
		{
			if ((m_musicPlayer.SongCount != 0) && m_preferences.GetBoolean("EnableBackgroundMusic"))
			{
				m_backgroundMusicDelayTimer.Interval = m_preferences.GetInteger("BackgroundMusicDelay")*0x3e8;
				m_backgroundMusicDelayTimer.Enabled = true;
			}
		}

		public void StartBackgroundObjects()
		{
			StartBackgroundSequence();
			StartBackgroundMusic();
		}

		public void StartBackgroundSequence()
		{
			if ((m_backgroundSequence != null) && m_preferences.GetBoolean("EnableBackgroundSequence"))
			{
				CreateBackgroundContext();
				if (m_executionInterface.EngineStatus(m_backgroundExecutionContextHandle) == 0)
				{
					m_backgroundSequenceDelayTimer.Interval = m_preferences.GetInteger("BackgroundSequenceDelay")*0x3e8;
					m_backgroundSequenceDelayTimer.Enabled = true;
				}
			}
		}

		public void StopBackgroundMusic()
		{
			m_musicPlayer.Stop();
			m_backgroundMusicDelayTimer.Enabled = false;
			m_backgroundMusicLabel.Visible = false;
		}

		public void StopBackgroundObjects()
		{
			StopBackgroundSequence();
			StopBackgroundMusic();
		}

		public void StopBackgroundSequence()
		{
			StopBackgroundSequenceUI();
			StopBackgroundSequenceExecution();
		}

		public void StopBackgroundSequenceExecution()
		{
			if (m_backgroundExecutionContextHandle != 0)
			{
				m_executionInterface.ExecuteStop(m_backgroundExecutionContextHandle);
				m_executionInterface.ReleaseContext(m_backgroundExecutionContextHandle);
				m_backgroundExecutionContextHandle = 0;
			}
		}

		public void StopBackgroundSequenceUI()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(StopBackgroundSequenceUI), new object[0]);
			}
			else
			{
				m_backgroundSequenceDelayTimer.Enabled = false;
				m_backgroundProgressBar.Visible = false;
				m_backgroundProgressBar.Enabled = false;
			}
		}
	}
}