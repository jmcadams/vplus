using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using FMOD;

namespace VixenPlus
{
	internal class Engine8 : IDisposable, IQueryable
	{
		public delegate void ProgramEndDelegate();

		public delegate void SequenceChangeDelegate();

		private const string INSTANCE_ID_ROOT = "engine_";
		private static readonly List<Engine8> m_instanceList = new List<Engine8>();
		private static int m_nextInstanceId;
		private readonly object m_runLock;
		private float m_audioSpeed;
		private XmlDocument m_commDoc;
		private EngineContext[] m_contexts;
		private System.Timers.Timer m_eventTimer;
		private fmod m_fmod;
		private HardwareUpdateDelegate m_hardwareUpdate;
		private Host m_host;
		private int m_instanceId;
		private bool m_isPaused;
		private bool m_loggingEnabled;
		private bool m_loop;
		private EngineMode m_mode;
		private int m_primaryContext;
		private SequenceProgram m_program;
		private PlugInRouter m_router;
		private bool m_running;
		private int m_secondaryContext;
		private IEngine m_secondaryEngine;
		private bool m_stopping;
		private EngineTimer m_surfacedTimer;
		private bool m_useSequencePluginData;

		internal Engine8(Host host, int audioDeviceIndex)
		{
			m_isPaused = false;
			m_loop = false;
			m_secondaryEngine = null;
			m_running = false;
			m_runLock = new object();
			m_useSequencePluginData = false;
			m_primaryContext = 0;
			m_secondaryContext = 1;
			m_audioSpeed = 1f;
			m_loggingEnabled = false;
			m_stopping = false;
			ConstructUsing(EngineMode.Synchronous, host, audioDeviceIndex);
		}

		internal Engine8(EngineMode mode, Host host, int audioDeviceIndex)
		{
			m_isPaused = false;
			m_loop = false;
			m_secondaryEngine = null;
			m_running = false;
			m_runLock = new object();
			m_useSequencePluginData = false;
			m_primaryContext = 0;
			m_secondaryContext = 1;
			m_audioSpeed = 1f;
			m_loggingEnabled = false;
			m_stopping = false;
			ConstructUsing(mode, host, audioDeviceIndex);
		}

		public float AudioSpeed
		{
			get { return m_audioSpeed; }
			set { m_audioSpeed = value; }
		}

		public XmlDocument CommDoc
		{
			set { m_commDoc = value; }
		}

		public IExecutable CurrentObject
		{
			get { return m_program; }
		}

		public string ExecutingProgram
		{
			get
			{
				if ((m_mode != EngineMode.Asynchronous) && (IsRunning || IsPaused))
				{
					return m_program.Name;
				}
				return string.Empty;
			}
		}

		public string ExecutingSequence
		{
			get
			{
				if (m_mode == EngineMode.Asynchronous)
				{
					return string.Empty;
				}
				if (IsRunning || IsPaused)
				{
					return m_contexts[m_primaryContext].m_currentSequence.Name;
				}
				return "None";
			}
		}

		public bool IsPaused
		{
			get { return m_isPaused; }
		}

		public bool IsRunning
		{
			get
			{
				if (m_secondaryEngine != null)
				{
					return m_secondaryEngine.IsRunning;
				}
				return m_running;
			}
		}

		public string LoadedProgram
		{
			get
			{
				if (m_mode == EngineMode.Asynchronous)
				{
					return string.Empty;
				}
				if (m_program == null)
				{
					return string.Empty;
				}
				return m_program.Name;
			}
		}

		public int LoadedProgramLength
		{
			get
			{
				if (m_mode == EngineMode.Asynchronous)
				{
					return 0;
				}
				if (m_program == null)
				{
					return 0;
				}
				return m_program.Length;
			}
		}

		public string LoadedSequence
		{
			get
			{
				if (m_mode == EngineMode.Asynchronous)
				{
					return string.Empty;
				}
				if (m_contexts[m_primaryContext].m_currentSequence == null)
				{
					return string.Empty;
				}
				return m_contexts[m_primaryContext].m_currentSequence.Name;
			}
		}

		public int LoadedSequenceLength
		{
			get
			{
				if (m_mode == EngineMode.Asynchronous)
				{
					return 0;
				}
				if (m_contexts[m_primaryContext].m_currentSequence == null)
				{
					return 0;
				}
				return m_contexts[m_primaryContext].m_currentSequence.Length;
			}
		}

		public bool Loop
		{
			get { return m_loop; }
			set { m_loop = value; }
		}

		public EngineMode Mode
		{
			get { return m_mode; }
		}

		public int ObjectPosition
		{
			get
			{
				EngineContext context = m_contexts[m_primaryContext];
				int tickCount = context.m_tickCount;
				for (int i = 0; i < context.m_sequenceIndex; i++)
				{
					tickCount += m_program.EventSequences[i].Length;
				}
				return tickCount;
			}
		}

		public int Position
		{
			get { return m_contexts[m_primaryContext].m_tickCount; }
		}

		public void Dispose()
		{
			ReleaseSecondaryEngine();
			if (m_mode == EngineMode.Synchronous)
			{
				if (m_eventTimer != null)
				{
					m_eventTimer.Stop();
					m_eventTimer.Elapsed -= m_eventTimer_Elapsed;
					m_eventTimer.Dispose();
					m_eventTimer = null;
				}
				m_fmod.Stop(m_contexts[m_primaryContext].m_soundChannel);
				if (m_contexts[m_secondaryContext] != null)
				{
					m_fmod.Stop(m_contexts[m_secondaryContext].m_soundChannel);
				}
				m_fmod.Shutdown();
			}
			if (m_router != null)
			{
				try
				{
					m_router.Shutdown(m_contexts[m_primaryContext].m_routerContext);
					if (m_contexts[m_secondaryContext] != null)
					{
						m_router.Shutdown(m_contexts[m_secondaryContext].m_routerContext);
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show("Error when shutting down the plugin:\n" + exception.Message, Vendor.ProductName,
					                MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			if (m_surfacedTimer != null)
			{
				m_surfacedTimer.Dispose();
				m_surfacedTimer = null;
			}
			ProgramEnd = null;
			SequenceChange = null;
			m_hardwareUpdate = null;
			if (m_program != null)
			{
				m_program.Dispose();
			}
			m_instanceList.Remove(this);
			GC.SuppressFinalize(this);
		}

		public string QueryInstance(int index)
		{
			var builder = new StringBuilder();
			if ((index >= 0) && (index < m_instanceList.Count))
			{
				Engine8 engine = m_instanceList[index];
				if (engine.m_program != null)
				{
					builder.AppendLine("Program: " + engine.m_program.Name);
					builder.AppendLine("Sequence count: " + engine.m_program.EventSequences.Count);
					builder.AppendLine("Sequences:");
					foreach (EventSequenceStub stub in engine.m_program.EventSequences)
					{
						builder.AppendLine("   " + stub.FileName);
					}
				}
				else
				{
					builder.AppendLine("Program: (null)");
				}
				builder.AppendLine("Primary context: " + engine.m_primaryContext);
				builder.Append(QueryContext(engine.m_contexts[engine.m_primaryContext]));
				builder.AppendLine("Secondary context: " + engine.m_secondaryContext);
				builder.Append(QueryContext(engine.m_contexts[engine.m_secondaryContext]));
				builder.AppendLine("Loop: " + engine.m_loop);
				builder.AppendLine("Paused: " + engine.m_isPaused);
				builder.AppendLine("Running: " + engine.m_running);
				builder.AppendLine("Use sequence plugin data: " + engine.m_useSequencePluginData);
				builder.AppendLine("Mode: " + engine.m_mode);
			}
			return builder.ToString();
		}

		public int Count
		{
			get { return m_instanceList.Count; }
		}

		public event ProgramEndDelegate ProgramEnd;

		public event SequenceChangeDelegate SequenceChange;

		private int CalcContainingSequence(int milliseconds)
		{
			int num = 0;
			for (int i = 0; i < m_program.EventSequences.Count; i++)
			{
				num += m_program.EventSequences[i].Length;
				if (milliseconds <= num)
				{
					return i;
				}
			}
			return -1;
		}

		private int CalcSequenceStart(int index)
		{
			int num = 0;
			while (index-- > 0)
			{
				num += m_program.EventSequences[index].Length;
			}
			return num;
		}

		private void ConstructUsing(EngineMode mode, Host host, int audioDeviceIndex)
		{
			m_instanceId = m_nextInstanceId++;
			m_mode = mode;
			m_host = host;
			m_router = Host.Router;
			if (mode == EngineMode.Synchronous)
			{
				m_eventTimer = new System.Timers.Timer(1.0);
				m_eventTimer.Elapsed += m_eventTimer_Elapsed;
				m_fmod = fmod.GetInstance(audioDeviceIndex);
				m_surfacedTimer = (m_mode == EngineMode.Synchronous) ? new EngineTimer(CurrentTime) : null;
			}
			else
			{
				m_eventTimer = null;
				m_fmod = null;
				m_surfacedTimer = null;
			}
			m_hardwareUpdate = HardwareUpdate;
			m_contexts = new[] {new EngineContext(), new EngineContext()};
			m_instanceList.Add(this);
		}

		private void CreateScriptEngine(EventSequence sequence)
		{
			if ((sequence.EngineType == EngineType.Procedural) && (m_secondaryEngine == null))
			{
				LoadSecondaryEngine(Path.Combine(Paths.BinaryPath,
				                                 Path.GetFileName(
					                                 ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetString(
						                                 "SecondaryEngine"))));
				m_secondaryEngine.HardwareUpdate = m_hardwareUpdate;
				m_secondaryEngine.CommDoc = m_commDoc;
			}
		}

		private int CurrentTime()
		{
			if ((m_contexts[m_primaryContext].m_soundChannel != null) && m_contexts[m_primaryContext].m_soundChannel.IsPlaying)
			{
				return (int) m_contexts[m_primaryContext].m_soundChannel.Position;
			}
			return (m_contexts[m_primaryContext].m_startOffset +
			        ((int) m_contexts[m_primaryContext].m_timekeeper.ElapsedMilliseconds));
		}

		private int DetermineSecondarySequenceIndex()
		{
			int sequenceIndex = m_contexts[m_primaryContext].m_sequenceIndex;
			if ((sequenceIndex + 1) == m_program.EventSequences.Count)
			{
				if (m_loop)
				{
					return 0;
				}
				return -1;
			}
			return (sequenceIndex + 1);
		}

		private void ExecutionStopThread()
		{
			if (m_secondaryEngine != null)
			{
				lock (m_secondaryEngine)
				{
					if (m_secondaryEngine.IsRunning)
					{
						lock (m_runLock)
						{
							m_running = false;
						}
						m_secondaryEngine.Stop();
					}
				}
			}
			else if (m_running && (m_router != null))
			{
				StopExecution();
				m_contexts[m_primaryContext].m_currentSequence = null;
				OnProgramEnd(true);
			}
			m_stopping = false;
		}

		~Engine8()
		{
			Dispose();
		}

		private void FinalizeEngineContext(EngineContext context, bool shutdownRouterContext = true)
		{
			if (context != null)
			{
				if (shutdownRouterContext && (context.m_routerContext != null))
				{
					m_router.Shutdown(context.m_routerContext);
					context.m_routerContext = null;
				}
				if (context.m_soundChannel != null)
				{
					m_fmod.ReleaseSound(context.m_soundChannel);
					context.m_soundChannel = null;
				}
				if (context.m_timekeeper.IsRunning)
				{
					context.m_timekeeper.Stop();
					context.m_timekeeper.Reset();
				}
			}
		}

		private bool FindEventSequence(object uniqueReference)
		{
			foreach (EventSequenceStub stub in m_program.EventSequences)
			{
				if (stub.Sequence == uniqueReference)
				{
					return true;
				}
			}
			return false;
		}

		private bool FindOutputPlugIn(object uniqueReference)
		{
			foreach (MappedOutputPlugIn @in in m_contexts[m_primaryContext].m_routerContext.OutputPluginList)
			{
				if (@in.PlugIn == uniqueReference)
				{
					return true;
				}
			}
			return false;
		}

		private void FireEvent(EngineContext context, int index)
		{
			if ((context.m_timekeeper.IsRunning && m_eventTimer.Enabled) && !m_stopping)
			{
				for (int i = 0; i < context.m_currentSequence.Channels.Count; i++)
				{
					context.m_routerContext.EngineBuffer[i] = context.m_data[i, index];
				}
				HardwareUpdate(context.m_routerContext.EngineBuffer, index);
			}
		}

		public void HardwareUpdate(byte[] values)
		{
			HardwareUpdate(values, -1);
		}

		public void HardwareUpdate(byte[] values, int eventIndex)
		{
			if (m_running && !m_stopping)
			{
				lock (m_runLock)
				{
					if (m_running && !m_stopping)
					{
						int num;
						EngineContext context = m_contexts[m_primaryContext];
						byte[] engineBuffer = context.m_routerContext.EngineBuffer;
						values.CopyTo(engineBuffer, 0);
						m_router.BeginUpdate();
						m_router.GetSequenceInputs(context.m_routerContext.ExecutableObject, engineBuffer, true, false);
						bool flag = context.m_lastPeriod == null;
						for (num = 0; (num < engineBuffer.Length) && !flag; num++)
						{
							flag |= engineBuffer[num] != context.m_lastPeriod[num];
						}
						if (!flag)
						{
							m_router.CancelUpdate();
						}
						else
						{
							if ((context.m_lastPeriod != null) && (engineBuffer.Length == context.m_lastPeriod.Length))
							{
								engineBuffer.CopyTo(context.m_lastPeriod, 0);
							}
							IExecutable executableObject = context.m_routerContext.ExecutableObject;
							int count = executableObject.Channels.Count;
							for (num = 0; num < count; num++)
							{
								Channel channel = executableObject.Channels[num];
								if (channel.DimmingCurve != null)
								{
									engineBuffer[num] = channel.DimmingCurve[engineBuffer[num]];
								}
							}
							for (num = 0; num < context.m_channelMask.Length; num++)
							{
								engineBuffer[num] = (byte) (engineBuffer[num] & context.m_channelMask[num]);
							}
							try
							{
								m_router.EndUpdate();
							}
							catch (Exception exception)
							{
								string stackTrace = exception.StackTrace;
								StopExecution();
								MessageBox.Show(exception.Message + "\n\nExecution has been stopped.", "Plugin error", MessageBoxButtons.OK,
								                MessageBoxIcon.Exclamation);
							}
						}
						if ((!m_stopping && m_running) && (eventIndex != -1))
						{
							engineBuffer = new byte[engineBuffer.Length];
							int num3 = context.m_currentSequence.Channels.Count;
							byte[,] eventValues = context.m_currentSequence.EventValues;
							if (m_router.GetSequenceInputs(context.m_routerContext.ExecutableObject, engineBuffer, false, true))
							{
								for (int i = 0; i < num3; i++)
								{
									eventValues[i, eventIndex] = Math.Max(eventValues[i, eventIndex], engineBuffer[i]);
								}
							}
						}
					}
				}
			}
		}

		private void InitEngineContext(ref EngineContext context, int sequenceIndex)
		{
			if (context != null)
			{
				context.m_timekeeper.Stop();
				if (Host.InvokeRequired)
				{
					Host.Invoke(new InitEngineContextDelegate(InitEngineContext), new object[] {context, sequenceIndex});
				}
				else if ((sequenceIndex == -1) || (m_program.EventSequences.Count <= sequenceIndex))
				{
					FinalizeEngineContext(context);
				}
				else
				{
					EventSequence executableObject = m_program.EventSequences[sequenceIndex].Sequence;
					if (context.m_routerContext == null)
					{
						context.m_routerContext = m_router.CreateContext(new byte[executableObject.ChannelCount],
						                                                 m_useSequencePluginData
							                                                 ? executableObject.PlugInData
							                                                 : m_program.SetupData, executableObject, m_surfacedTimer);
					}
					if (m_program.Mask.Length > sequenceIndex)
					{
						context.m_channelMask = m_program.Mask[sequenceIndex];
					}
					else if (context == m_contexts[m_primaryContext])
					{
						context.m_channelMask = m_contexts[m_secondaryContext].m_channelMask;
					}
					else
					{
						context.m_channelMask = m_contexts[m_primaryContext].m_channelMask;
					}
					context.m_sequenceIndex = sequenceIndex;
					context.m_tickCount = 0;
					context.m_lastIndex = -1;
					context.m_sequenceTickLength = executableObject.Time;
					context.m_fadeStartTickCount = ((m_program.CrossFadeLength == 0) || (m_program.EventSequences.Count == 1))
						                               ? 0
						                               : ((m_loop || (m_program.EventSequences.Count > (sequenceIndex + 1)))
							                                  ? (executableObject.Time - (m_program.CrossFadeLength*0x3e8))
							                                  : 0);
					context.m_startOffset = 0;
					if (executableObject.Audio != null)
					{
						context.m_soundChannel = m_fmod.LoadSound(Path.Combine(Paths.AudioPath, executableObject.Audio.FileName),
						                                          context.m_soundChannel);
					}
					else
					{
						context.m_soundChannel = m_fmod.LoadSound(null);
					}
					if ((m_program.CrossFadeLength != 0) && (context.m_soundChannel != null))
					{
						if (m_loop || (sequenceIndex > 0))
						{
							context.m_soundChannel.SetEntryFade(m_program.CrossFadeLength);
						}
						if (m_loop || (m_program.EventSequences.Count > (sequenceIndex + 1)))
						{
							context.m_soundChannel.SetExitFade(m_program.CrossFadeLength);
						}
					}
					context.m_currentSequence = executableObject;
					context.m_maxEvent = context.m_currentSequence.TotalEventPeriods;
					context.m_lastPeriod = new byte[context.m_currentSequence.Channels.Count];
					for (int i = 0; i < context.m_lastPeriod.Length; i++)
					{
						context.m_lastPeriod[i] = (byte) i;
					}
					context.m_data = ReconfigureSourceData(executableObject);
				}
			}
		}

		public void Initialize(EventSequence sequence)
		{
			if (m_mode == EngineMode.Asynchronous)
			{
				InitializeForAsynchronous(sequence);
			}
			else
			{
				Initialize(new SequenceProgram(sequence));
			}
		}

		public void Initialize(IExecutable obj)
		{
			if (obj is SequenceProgram)
			{
				Initialize((SequenceProgram) obj);
			}
			else if (obj is EventSequence)
			{
				Initialize((EventSequence) obj);
			}
			else
			{
				if (!(obj is Profile))
				{
					throw new Exception("Trying to initialize the engine with an unknown object type.\nType: " + obj.GetType());
				}
				Initialize((Profile) obj);
			}
		}

		public void Initialize(Profile profile)
		{
			if (m_mode == EngineMode.Synchronous)
			{
				throw new Exception("Only an asynchronous engine instance can be initialized with a profile.");
			}
			profile.Freeze();
			InitializeForAsynchronous(profile);
		}

		public void Initialize(SequenceProgram program)
		{
			if (m_mode == EngineMode.Asynchronous)
			{
				InitializeForAsynchronous(program);
			}
			else
			{
				if (program.EventSequences.Count == 0)
				{
					throw new Exception("Cannot execute a program that has no sequences.");
				}
				m_program = program;
				m_useSequencePluginData = m_program.UseSequencePluginData;
				EventSequence sequence = m_program.EventSequences[0].Sequence;
				if (m_program.EventSequences.Count > 1)
				{
					if (m_contexts[m_secondaryContext] == null)
					{
						m_contexts[m_secondaryContext] = new EngineContext();
					}
				}
				else
				{
					m_contexts[m_secondaryContext] = null;
				}
			}
		}

		private void InitializeForAsynchronous(IExecutable executableObject)
		{
			List<Channel> channels = executableObject.Channels;
			if (channels.Count == 0)
			{
				throw new Exception("Trying to setup for asynchronous operation with no channels?");
			}
			if ((m_contexts[m_primaryContext].m_routerContext != null) &&
			    m_contexts[m_primaryContext].m_routerContext.Initialized)
			{
				m_router.Shutdown(m_contexts[m_primaryContext].m_routerContext);
			}
			m_contexts[m_primaryContext].m_routerContext = m_router.CreateContext(new byte[channels.Count],
			                                                                      executableObject.PlugInData, executableObject,
			                                                                      null);
			m_contexts[m_primaryContext].m_channelMask = executableObject.Mask[0];
			m_contexts[m_secondaryContext] = null;
			Host.Communication["CurrentObject"] = m_program;
			m_router.Startup(m_contexts[m_primaryContext].m_routerContext);
			m_running = true;
			Host.Communication["CurrentObject"] = null;
		}

		private bool LoadSecondaryEngine(string enginePath)
		{
			if (m_mode == EngineMode.Asynchronous)
			{
				return false;
			}
			if (enginePath == null)
			{
				ReleaseSecondaryEngine();
				return true;
			}
			IEngine engine = null;
			try
			{
				Assembly assembly = Assembly.LoadFile(enginePath);
				foreach (Type type in assembly.GetExportedTypes())
				{
					foreach (Type type2 in type.GetInterfaces())
					{
						if (type2.Name == "IEngine")
						{
							engine = (IEngine) Activator.CreateInstance(type);
							break;
						}
					}
					if (engine != null)
					{
						goto Label_00EB;
					}
				}
			}
			catch
			{
				MessageBox.Show(Path.GetFileName(enginePath) + " is missing or does not contain a valid engine");
				return false;
			}
			Label_00EB:
			m_secondaryEngine = engine;
			if (engine != null)
			{
				m_secondaryEngine.EngineError += SecondaryEngineError;
				m_secondaryEngine.EngineStopped += SecondaryEngineStopped;
				return true;
			}
			return false;
		}

		private void LogAudio(EventSequence sequence)
		{
			if (m_loggingEnabled && (sequence.Audio != null))
			{
				Host.LogAudio("Sequence", sequence.Name, sequence.Audio.FileName, sequence.Audio.Duration);
			}
		}

		private void m_eventTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			TimerTick(m_contexts[m_primaryContext]);
			if (((m_contexts[m_secondaryContext] != null) && (m_contexts[m_secondaryContext].m_routerContext != null)) &&
			    m_contexts[m_secondaryContext].m_routerContext.Initialized)
			{
				TimerTick(m_contexts[m_secondaryContext]);
			}
		}

		protected virtual void OnProgramEnd(bool restartBackgroundObjects)
		{
			if (ProgramEnd != null)
			{
				m_host.DelegateNullMethod(ProgramEnd.Invoke);
			}
			string key = CurrentObject.Key.ToString();
			bool flag = Host.Communication.ContainsKey(key);
			int count = Host.Communication.Count;
			Host.Communication.Remove("KeyInterceptor_" + key);
			Host.Communication.Remove("ExecutionContext_" + key);
			if (restartBackgroundObjects)
			{
				RestartBackgroundObjects();
			}
		}

		protected virtual void OnSequenceChange()
		{
			if (SequenceChange != null)
			{
				SequenceChange();
			}
		}

		public void Pause()
		{
			if (((m_mode != EngineMode.Asynchronous) && IsRunning) && !m_isPaused)
			{
				lock (m_runLock)
				{
					if (m_secondaryEngine != null)
					{
						m_secondaryEngine.Pause();
						m_isPaused = true;
						m_contexts[m_primaryContext].m_timekeeper.Stop();
						m_running = false;
					}
					else
					{
						m_eventTimer.Stop();
						if (m_contexts[m_primaryContext].m_soundChannel != null)
						{
							m_contexts[m_primaryContext].m_soundChannel.Paused = true;
						}
						m_contexts[m_primaryContext].m_timekeeper.Stop();
						if (m_contexts[m_secondaryContext] != null)
						{
							m_contexts[m_secondaryContext].m_timekeeper.Stop();
							if (m_contexts[m_secondaryContext].m_soundChannel != null)
							{
								m_contexts[m_secondaryContext].m_soundChannel.Paused = true;
							}
						}
						m_isPaused = true;
						m_running = false;
					}
				}
			}
		}

		public bool Play(int startMillisecond, int endMillisecond, bool logAudio)
		{
			if (m_mode == EngineMode.Asynchronous)
			{
				return false;
			}
			try
			{
				m_loggingEnabled = logAudio;
				if (m_eventTimer.Enabled)
				{
					return false;
				}
				StopBackgroundObjects();
				CreateScriptEngine(m_program.EventSequences[0].Sequence);
				if (m_secondaryEngine != null)
				{
					if (m_isPaused)
					{
						m_isPaused = false;
					}
					else
					{
						ResetScriptEngineContext();
						OnSequenceChange();
						Host.Communication["CurrentObject"] = m_program;
						m_router.Startup(m_contexts[m_primaryContext].m_routerContext);
						Host.Communication["CurrentObject"] = null;
					}
					StartTimekeepers();
					return m_secondaryEngine.Play();
				}
				if (!m_isPaused)
				{
					InitEngineContext(ref m_contexts[m_primaryContext], CalcContainingSequence(startMillisecond));
					LogAudio(m_contexts[m_primaryContext].m_currentSequence);
					m_contexts[m_primaryContext].m_startOffset = startMillisecond;
					InitEngineContext(ref m_contexts[m_secondaryContext], DetermineSecondarySequenceIndex());
					m_contexts[m_primaryContext].m_maxEvent = (endMillisecond == 0)
						                                          ? m_contexts[m_primaryContext].m_currentSequence.TotalEventPeriods
						                                          : (endMillisecond/
						                                             m_contexts[m_primaryContext].m_currentSequence.EventPeriod);
				}
				int millisecondPosition = (m_contexts[m_primaryContext].m_currentSequence.Audio != null) ? startMillisecond : -1;
				PrepareAudio(m_contexts[m_primaryContext], millisecondPosition);
				if (m_isPaused)
				{
					m_isPaused = false;
				}
				else
				{
					OnSequenceChange();
					Host.Communication["CurrentObject"] = m_program;
					m_router.Startup(m_contexts[m_primaryContext].m_routerContext);
					Host.Communication["CurrentObject"] = null;
				}
				StartAudio(m_contexts[m_primaryContext]);
				StartTimekeepers();
				m_eventTimer.Start();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void PrepareAudio(EngineContext context, int millisecondPosition)
		{
			if (((m_mode != EngineMode.Asynchronous) && (context.m_soundChannel != null)) && (millisecondPosition != -1))
			{
				m_fmod.Play(context.m_soundChannel, true);
				context.m_soundChannel.Position = (uint) millisecondPosition;
				context.m_soundChannel.Frequency = m_audioSpeed;
			}
		}

		private string QueryContext(EngineContext context)
		{
			if (context == null)
			{
				return "   (null)\n";
			}
			var builder = new StringBuilder();
			builder.AppendLine("   Sequence: " +
			                   ((context.m_currentSequence != null) ? context.m_currentSequence.Name : "(null)"));
			builder.AppendLine("   Sequence index: " + context.m_sequenceIndex);
			builder.AppendLine("   Sequence tick length: " + context.m_sequenceTickLength);
			builder.AppendLine("   Tick count: " + context.m_tickCount);
			if (context.m_routerContext != null)
			{
				builder.AppendLine("   Router context:");
				builder.AppendLine("      Mapped plugin count: " + context.m_routerContext.OutputPluginList.Count);
				builder.AppendLine("      Mapped plugins: " + context.m_routerContext.OutputPluginList.Count);
				foreach (MappedOutputPlugIn @in in context.m_routerContext.OutputPluginList)
				{
					builder.Append(QueryMappedPlugIn(@in));
				}
			}
			else
			{
				builder.AppendLine("   Router context: (null)");
			}
			return builder.ToString();
		}

		private string QueryMappedPlugIn(MappedOutputPlugIn mappedPlugIn)
		{
			var builder = new StringBuilder();
			builder.AppendLine("         Name: " + mappedPlugIn.PlugIn.Name);
			builder.AppendLine("         From: " + mappedPlugIn.From);
			builder.AppendLine("         To: " + mappedPlugIn.To);
			builder.AppendLine("         Enabled: " + mappedPlugIn.Enabled);
			return builder.ToString();
		}

		private byte[,] ReconfigureSourceData(EventSequence sequence)
		{
			var list = new List<int>();
			var buffer = new byte[sequence.ChannelCount,sequence.TotalEventPeriods];
			foreach (Channel channel in sequence.Channels)
			{
				list.Add(channel.OutputChannel);
			}
			for (int i = 0; i < sequence.ChannelCount; i++)
			{
				int num3 = list[i];
				for (int j = 0; j < sequence.TotalEventPeriods; j++)
				{
					buffer[num3, j] = sequence.EventValues[i, j];
				}
			}
			return buffer;
		}

		private void ReleaseSecondaryEngine()
		{
			if (m_secondaryEngine != null)
			{
				lock (m_secondaryEngine)
				{
					m_secondaryEngine.Stop();
					m_secondaryEngine.HardwareUpdate = null;
					m_secondaryEngine.EngineError -= SecondaryEngineError;
					m_secondaryEngine.EngineStopped -= SecondaryEngineStopped;
					m_secondaryEngine.Dispose();
					m_secondaryEngine = null;
					m_secondaryContext = 0;
				}
			}
		}

		private void ResetScriptEngineContext()
		{
			InitEngineContext(ref m_contexts[m_primaryContext], 0);
			LogAudio(m_contexts[m_primaryContext].m_currentSequence);
			if (m_contexts[m_primaryContext].m_soundChannel != null)
			{
				m_contexts[m_primaryContext].m_soundChannel.SetEntryFade(0);
			}
			m_secondaryEngine.Initialize(m_contexts[m_primaryContext].m_currentSequence);
		}

		private void RestartBackgroundObjects()
		{
			Engine8 engine = null;
			foreach (Engine8 engine2 in m_instanceList)
			{
				if (engine2.IsRunning)
				{
					engine = engine2;
					break;
				}
			}
			if (engine == null)
			{
				m_host.StartBackgroundObjects();
			}
		}

		private void SecondaryEngineError(string message, string stackTrace)
		{
			Stop();
			if (m_host.IsBackgroundExecutionEngineInstance(this))
			{
				m_host.StopBackgroundSequence();
			}
			MessageBox.Show(message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void SecondaryEngineStopped(object sender, EventArgs e)
		{
			if (m_running && m_loop)
			{
				FinalizeEngineContext(m_contexts[m_primaryContext], false);
				FinalizeEngineContext(m_contexts[m_secondaryContext], false);
				ResetScriptEngineContext();
				StartTimekeepers();
				OnSequenceChange();
				m_secondaryEngine.Play();
			}
			else
			{
				bool flag = m_host.IsBackgroundExecutionEngineInstance(this);
				if (flag)
				{
					m_host.StopBackgroundSequenceUI();
				}
				FinalizeEngineContext(m_contexts[m_primaryContext]);
				FinalizeEngineContext(m_contexts[m_secondaryContext]);
				OnProgramEnd(!flag);
			}
		}

		public void SetAudioDevice(int value)
		{
			m_fmod.DeviceIndex = value;
		}

		private void StartAudio(EngineContext context)
		{
			if (((m_mode != EngineMode.Asynchronous) && (context.m_soundChannel != null)) && context.m_soundChannel.Paused)
			{
				context.m_soundChannel.Paused = false;
			}
		}

		private void StartContextAudio(EngineContext context)
		{
			MethodInvoker method = null;
			if (context.m_currentSequence.Audio != null)
			{
				if (Host.InvokeRequired)
				{
					if (method == null)
					{
						method = delegate
							{
								PrepareAudio(context, context.m_startOffset);
								StartAudio(context);
							};
					}
					Host.Invoke(method, new object[0]);
				}
				else
				{
					PrepareAudio(context, context.m_startOffset);
					StartAudio(context);
				}
			}
		}

		private void StartTimekeepers()
		{
			lock (m_runLock)
			{
				m_running = true;
				m_contexts[m_primaryContext].m_timekeeper.Start();
				if ((m_contexts[m_primaryContext].m_fadeStartTickCount != 0) &&
				    (m_contexts[m_primaryContext].m_tickCount >= m_contexts[m_primaryContext].m_fadeStartTickCount))
				{
					m_contexts[m_secondaryContext].m_timekeeper.Start();
				}
			}
		}

		public void Stop()
		{
			if (!m_stopping)
			{
				m_stopping = true;
				if (m_mode != EngineMode.Asynchronous)
				{
					new Thread(ExecutionStopThread).Start();
				}
				else
				{
					FinalizeEngineContext(m_contexts[m_primaryContext], true);
					lock (m_runLock)
					{
						m_running = false;
					}
					m_stopping = false;
				}
			}
		}

		private void StopBackgroundObjects()
		{
			if (!m_host.IsBackgroundExecutionEngineInstance(this))
			{
				m_host.StopBackgroundObjects();
			}
		}

		private void StopExecution(bool shutdownPlugins = true)
		{
			lock (m_runLock)
			{
				m_running = false;
			}
			if (m_eventTimer != null)
			{
				m_eventTimer.Stop();
			}
			m_fmod.Stop(m_contexts[m_primaryContext].m_soundChannel);
			if (m_contexts[m_secondaryContext] != null)
			{
				m_fmod.Stop(m_contexts[m_secondaryContext].m_soundChannel);
			}
			m_isPaused = false;
			FinalizeEngineContext(m_contexts[m_primaryContext], shutdownPlugins);
			FinalizeEngineContext(m_contexts[m_secondaryContext], shutdownPlugins);
		}

		private void TimerTick(EngineContext context)
		{
			if (m_eventTimer.Enabled && !m_stopping)
			{
				if ((context.m_soundChannel != null) && context.m_soundChannel.IsPlaying)
				{
					context.m_tickCount = (int) context.m_soundChannel.Position;
				}
				else
				{
					context.m_tickCount = context.m_startOffset + ((int) context.m_timekeeper.ElapsedMilliseconds);
				}
				int num = context.m_tickCount/context.m_currentSequence.EventPeriod;
				if (((context.m_fadeStartTickCount != 0) && (context.m_tickCount >= context.m_fadeStartTickCount)) &&
				    !m_contexts[m_secondaryContext].m_timekeeper.IsRunning)
				{
					m_eventTimer.Enabled = false;
					Host.Communication["CurrentObject"] = m_program;
					m_router.Startup(m_contexts[m_secondaryContext].m_routerContext);
					Host.Communication["CurrentObject"] = null;
					LogAudio(m_contexts[m_secondaryContext].m_currentSequence);
					StartContextAudio(m_contexts[m_secondaryContext]);
					if (m_contexts[m_secondaryContext].m_soundChannel != null)
					{
						m_contexts[m_secondaryContext].m_soundChannel.Volume = 0f;
					}
					m_contexts[m_secondaryContext].m_timekeeper.Start();
					m_eventTimer.Enabled = true;
				}
				if ((context.m_tickCount >= context.m_sequenceTickLength) || (num >= context.m_maxEvent))
				{
					m_fmod.Stop(context.m_soundChannel);
					context.m_timekeeper.Stop();
					context.m_timekeeper.Reset();
					if ((m_contexts[m_secondaryContext] == null) || (m_contexts[m_secondaryContext].m_routerContext == null))
					{
						if (m_loop)
						{
							LogAudio(m_contexts[m_primaryContext].m_currentSequence);
							StartContextAudio(m_contexts[m_primaryContext]);
							context.m_timekeeper.Start();
							OnSequenceChange();
						}
						else
						{
							Host.Invoke(new MethodInvoker(Stop), new object[0]);
						}
					}
					else
					{
						if (m_contexts[m_secondaryContext].m_soundChannel != null)
						{
							m_contexts[m_secondaryContext].m_soundChannel.Volume = 1f;
						}
						if (m_contexts[m_secondaryContext] != null)
						{
							FinalizeEngineContext(m_contexts[m_primaryContext]);
							m_primaryContext ^= 1;
							m_secondaryContext ^= 1;
							InitEngineContext(ref m_contexts[m_secondaryContext], DetermineSecondarySequenceIndex());
						}
						if (!m_contexts[m_primaryContext].m_timekeeper.IsRunning)
						{
							Host.Communication["CurrentObject"] = m_program;
							m_router.Startup(m_contexts[m_primaryContext].m_routerContext);
							Host.Communication["CurrentObject"] = null;
							LogAudio(m_contexts[m_primaryContext].m_currentSequence);
							StartContextAudio(m_contexts[m_primaryContext]);
							m_contexts[m_primaryContext].m_timekeeper.Start();
						}
						OnSequenceChange();
					}
				}
				else if (num != context.m_lastIndex)
				{
					context.m_lastIndex = num;
					FireEvent(context, context.m_lastIndex);
				}
			}
		}

		public bool UsesObject(object uniqueReference)
		{
			if (uniqueReference is EventSequence)
			{
				return FindEventSequence(uniqueReference);
			}
			if (uniqueReference is SequenceProgram)
			{
				return (uniqueReference == m_program);
			}
			return ((uniqueReference is IOutputPlugIn) && FindOutputPlugIn(uniqueReference));
		}

		internal enum EngineMode
		{
			Synchronous,
			Asynchronous
		}

		private delegate void InitEngineContextDelegate(ref EngineContext context, int sequenceIndex);

		private delegate void ShutdownDelegate(RouterContext routerInstanceInfo);
	}
}