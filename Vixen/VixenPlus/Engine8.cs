namespace Vixen
{
    using FMOD;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;

    internal class Engine8 : IDisposable, IQueryable
    {
        private const string INSTANCE_ID_ROOT = "engine_";
        private float m_audioSpeed;
        private XmlDocument m_commDoc;
        private EngineContext[] m_contexts;
        private System.Timers.Timer m_eventTimer;
        private fmod m_fmod;
        private HardwareUpdateDelegate m_hardwareUpdate;
        private Host m_host;
        private int m_instanceId;
        private static List<Engine8> m_instanceList = new List<Engine8>();
        private bool m_isPaused;
        private bool m_loggingEnabled;
        private bool m_loop;
        private EngineMode m_mode;
        private static int m_nextInstanceId = 0;
        private int m_primaryContext;
        private SequenceProgram m_program;
        private PlugInRouter m_router;
        private object m_runLock;
        private bool m_running;
        private int m_secondaryContext;
        private IEngine m_secondaryEngine;
        private bool m_stopping;
        private EngineTimer m_surfacedTimer;
        private bool m_useSequencePluginData;

        public event ProgramEndDelegate ProgramEnd;

        public event SequenceChangeDelegate SequenceChange;

        internal Engine8(Host host, int audioDeviceIndex)
        {
            this.m_isPaused = false;
            this.m_loop = false;
            this.m_secondaryEngine = null;
            this.m_running = false;
            this.m_runLock = new object();
            this.m_useSequencePluginData = false;
            this.m_primaryContext = 0;
            this.m_secondaryContext = 1;
            this.m_audioSpeed = 1f;
            this.m_loggingEnabled = false;
            this.m_stopping = false;
            this.ConstructUsing(EngineMode.Synchronous, host, audioDeviceIndex);
        }

        internal Engine8(EngineMode mode, Host host, int audioDeviceIndex)
        {
            this.m_isPaused = false;
            this.m_loop = false;
            this.m_secondaryEngine = null;
            this.m_running = false;
            this.m_runLock = new object();
            this.m_useSequencePluginData = false;
            this.m_primaryContext = 0;
            this.m_secondaryContext = 1;
            this.m_audioSpeed = 1f;
            this.m_loggingEnabled = false;
            this.m_stopping = false;
            this.ConstructUsing(mode, host, audioDeviceIndex);
        }

        private int CalcContainingSequence(int milliseconds)
        {
            int num = 0;
            for (int i = 0; i < this.m_program.EventSequences.Count; i++)
            {
                num += this.m_program.EventSequences[i].Length;
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
                num += this.m_program.EventSequences[index].Length;
            }
            return num;
        }

        private void ConstructUsing(EngineMode mode, Host host, int audioDeviceIndex)
        {
            this.m_instanceId = m_nextInstanceId++;
            this.m_mode = mode;
            this.m_host = host;
            this.m_router = Host.Router;
            if (mode == EngineMode.Synchronous)
            {
                this.m_eventTimer = new System.Timers.Timer(1.0);
                this.m_eventTimer.Elapsed += new ElapsedEventHandler(this.m_eventTimer_Elapsed);
                this.m_fmod = fmod.GetInstance(audioDeviceIndex);
                this.m_surfacedTimer = (this.m_mode == EngineMode.Synchronous) ? new EngineTimer(new EngineTimer.TickCallDelegate(this.CurrentTime)) : null;
            }
            else
            {
                this.m_eventTimer = null;
                this.m_fmod = null;
                this.m_surfacedTimer = null;
            }
            this.m_hardwareUpdate = new HardwareUpdateDelegate(this.HardwareUpdate);
            this.m_contexts = new EngineContext[] { new EngineContext(), new EngineContext() };
            m_instanceList.Add(this);
        }

        private void CreateScriptEngine(EventSequence sequence)
        {
            if ((sequence.EngineType == EngineType.Procedural) && (this.m_secondaryEngine == null))
            {
                this.LoadSecondaryEngine(Path.Combine(Paths.BinaryPath, Path.GetFileName(((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetString("SecondaryEngine"))));
                this.m_secondaryEngine.HardwareUpdate = this.m_hardwareUpdate;
                this.m_secondaryEngine.CommDoc = this.m_commDoc;
            }
        }

        private int CurrentTime()
        {
            if ((this.m_contexts[this.m_primaryContext].m_soundChannel != null) && this.m_contexts[this.m_primaryContext].m_soundChannel.IsPlaying)
            {
                return (int) this.m_contexts[this.m_primaryContext].m_soundChannel.Position;
            }
            return (this.m_contexts[this.m_primaryContext].m_startOffset + ((int) this.m_contexts[this.m_primaryContext].m_timekeeper.ElapsedMilliseconds));
        }

        private int DetermineSecondarySequenceIndex()
        {
            int sequenceIndex = this.m_contexts[this.m_primaryContext].m_sequenceIndex;
            if ((sequenceIndex + 1) == this.m_program.EventSequences.Count)
            {
                if (this.m_loop)
                {
                    return 0;
                }
                return -1;
            }
            return (sequenceIndex + 1);
        }

        public void Dispose()
        {
            this.ReleaseSecondaryEngine();
            if (this.m_mode == EngineMode.Synchronous)
            {
                if (this.m_eventTimer != null)
                {
                    this.m_eventTimer.Stop();
                    this.m_eventTimer.Elapsed -= new ElapsedEventHandler(this.m_eventTimer_Elapsed);
                    this.m_eventTimer.Dispose();
                    this.m_eventTimer = null;
                }
                this.m_fmod.Stop(this.m_contexts[this.m_primaryContext].m_soundChannel);
                if (this.m_contexts[this.m_secondaryContext] != null)
                {
                    this.m_fmod.Stop(this.m_contexts[this.m_secondaryContext].m_soundChannel);
                }
                this.m_fmod.Shutdown();
            }
            if (this.m_router != null)
            {
                try
                {
                    this.m_router.Shutdown(this.m_contexts[this.m_primaryContext].m_routerContext);
                    if (this.m_contexts[this.m_secondaryContext] != null)
                    {
                        this.m_router.Shutdown(this.m_contexts[this.m_secondaryContext].m_routerContext);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error when shutting down the plugin:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            if (this.m_surfacedTimer != null)
            {
                this.m_surfacedTimer.Dispose();
                this.m_surfacedTimer = null;
            }
            this.ProgramEnd = null;
            this.SequenceChange = null;
            this.m_hardwareUpdate = null;
            if (this.m_program != null)
            {
                this.m_program.Dispose();
            }
            m_instanceList.Remove(this);
            GC.SuppressFinalize(this);
        }

        private void ExecutionStopThread()
        {
            if (this.m_secondaryEngine != null)
            {
                lock (this.m_secondaryEngine)
                {
                    if (this.m_secondaryEngine.IsRunning)
                    {
                        lock (this.m_runLock)
                        {
                            this.m_running = false;
                        }
                        this.m_secondaryEngine.Stop();
                    }
                }
            }
            else if (this.m_running && (this.m_router != null))
            {
                this.StopExecution();
                this.m_contexts[this.m_primaryContext].m_currentSequence = null;
                this.OnProgramEnd(true);
            }
            this.m_stopping = false;
        }

        ~Engine8()
        {
            this.Dispose();
        }

        private void FinalizeEngineContext(EngineContext context)
        {
            this.FinalizeEngineContext(context, true);
        }

        private void FinalizeEngineContext(EngineContext context, bool shutdownRouterContext)
        {
            if (context != null)
            {
                if (shutdownRouterContext && (context.m_routerContext != null))
                {
                    this.m_router.Shutdown(context.m_routerContext);
                    context.m_routerContext = null;
                }
                if (context.m_soundChannel != null)
                {
                    this.m_fmod.ReleaseSound(context.m_soundChannel);
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
            foreach (EventSequenceStub stub in this.m_program.EventSequences)
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
            foreach (MappedOutputPlugIn @in in this.m_contexts[this.m_primaryContext].m_routerContext.OutputPluginList)
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
            if ((context.m_timekeeper.IsRunning && this.m_eventTimer.Enabled) && !this.m_stopping)
            {
                for (int i = 0; i < context.m_currentSequence.Channels.Count; i++)
                {
                    context.m_routerContext.EngineBuffer[i] = context.m_data[i, index];
                }
                this.HardwareUpdate(context.m_routerContext.EngineBuffer, index);
            }
        }

        public void HardwareUpdate(byte[] values)
        {
            this.HardwareUpdate(values, -1);
        }

        public void HardwareUpdate(byte[] values, int eventIndex)
        {
            if (this.m_running && !this.m_stopping)
            {
                lock (this.m_runLock)
                {
                    if (this.m_running && !this.m_stopping)
                    {
                        int num;
                        EngineContext context = this.m_contexts[this.m_primaryContext];
                        byte[] engineBuffer = context.m_routerContext.EngineBuffer;
                        values.CopyTo(engineBuffer, 0);
                        this.m_router.BeginUpdate();
                        this.m_router.GetSequenceInputs(context.m_routerContext.ExecutableObject, engineBuffer, true, false);
                        bool flag = context.m_lastPeriod == null;
                        for (num = 0; (num < engineBuffer.Length) && !flag; num++)
                        {
                            flag |= engineBuffer[num] != context.m_lastPeriod[num];
                        }
                        if (!flag)
                        {
                            this.m_router.CancelUpdate();
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
                                Vixen.Channel channel = executableObject.Channels[num];
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
                                this.m_router.EndUpdate();
                            }
                            catch (Exception exception)
                            {
                                string stackTrace = exception.StackTrace;
                                this.StopExecution();
                                MessageBox.Show(exception.Message + "\n\nExecution has been stopped.", "Plugin error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        if ((!this.m_stopping && this.m_running) && (eventIndex != -1))
                        {
                            engineBuffer = new byte[engineBuffer.Length];
                            int num3 = context.m_currentSequence.Channels.Count;
                            byte[,] eventValues = context.m_currentSequence.EventValues;
                            if (this.m_router.GetSequenceInputs(context.m_routerContext.ExecutableObject, engineBuffer, false, true))
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
                    Host.Invoke(new InitEngineContextDelegate(this.InitEngineContext), new object[] { context, sequenceIndex });
                }
                else if ((sequenceIndex == -1) || (this.m_program.EventSequences.Count <= sequenceIndex))
                {
                    this.FinalizeEngineContext(context);
                }
                else
                {
                    EventSequence executableObject = this.m_program.EventSequences[sequenceIndex].Sequence;
                    if (context.m_routerContext == null)
                    {
                        context.m_routerContext = this.m_router.CreateContext(new byte[executableObject.ChannelCount], this.m_useSequencePluginData ? executableObject.PlugInData : this.m_program.SetupData, executableObject, this.m_surfacedTimer);
                    }
                    if (this.m_program.Mask.Length > sequenceIndex)
                    {
                        context.m_channelMask = this.m_program.Mask[sequenceIndex];
                    }
                    else if (context == this.m_contexts[this.m_primaryContext])
                    {
                        context.m_channelMask = this.m_contexts[this.m_secondaryContext].m_channelMask;
                    }
                    else
                    {
                        context.m_channelMask = this.m_contexts[this.m_primaryContext].m_channelMask;
                    }
                    context.m_sequenceIndex = sequenceIndex;
                    context.m_tickCount = 0;
                    context.m_lastIndex = -1;
                    context.m_sequenceTickLength = executableObject.Time;
                    context.m_fadeStartTickCount = ((this.m_program.CrossFadeLength == 0) || (this.m_program.EventSequences.Count == 1)) ? 0 : ((this.m_loop || (this.m_program.EventSequences.Count > (sequenceIndex + 1))) ? (executableObject.Time - (this.m_program.CrossFadeLength * 0x3e8)) : 0);
                    context.m_startOffset = 0;
                    if (executableObject.Audio != null)
                    {
                        context.m_soundChannel = this.m_fmod.LoadSound(Path.Combine(Paths.AudioPath, executableObject.Audio.FileName), context.m_soundChannel);
                    }
                    else
                    {
                        context.m_soundChannel = this.m_fmod.LoadSound(null);
                    }
                    if ((this.m_program.CrossFadeLength != 0) && (context.m_soundChannel != null))
                    {
                        if (this.m_loop || (sequenceIndex > 0))
                        {
                            context.m_soundChannel.SetEntryFade(this.m_program.CrossFadeLength);
                        }
                        if (this.m_loop || (this.m_program.EventSequences.Count > (sequenceIndex + 1)))
                        {
                            context.m_soundChannel.SetExitFade(this.m_program.CrossFadeLength);
                        }
                    }
                    context.m_currentSequence = executableObject;
                    context.m_maxEvent = context.m_currentSequence.TotalEventPeriods;
                    context.m_lastPeriod = new byte[context.m_currentSequence.Channels.Count];
                    for (int i = 0; i < context.m_lastPeriod.Length; i++)
                    {
                        context.m_lastPeriod[i] = (byte) i;
                    }
                    context.m_data = this.ReconfigureSourceData(executableObject);
                }
            }
        }

        public void Initialize(EventSequence sequence)
        {
            if (this.m_mode == EngineMode.Asynchronous)
            {
                this.InitializeForAsynchronous(sequence);
            }
            else
            {
                this.Initialize(new SequenceProgram(sequence));
            }
        }

        public void Initialize(IExecutable obj)
        {
            if (obj is SequenceProgram)
            {
                this.Initialize((SequenceProgram) obj);
            }
            else if (obj is EventSequence)
            {
                this.Initialize((EventSequence) obj);
            }
            else
            {
                if (!(obj is Profile))
                {
                    throw new Exception("Trying to initialize the engine with an unknown object type.\nType: " + obj.GetType().ToString());
                }
                this.Initialize((Profile) obj);
            }
        }

        public void Initialize(Profile profile)
        {
            if (this.m_mode == EngineMode.Synchronous)
            {
                throw new Exception("Only an asynchronous engine instance can be initialized with a profile.");
            }
            profile.Freeze();
            this.InitializeForAsynchronous(profile);
        }

        public void Initialize(SequenceProgram program)
        {
            if (this.m_mode == EngineMode.Asynchronous)
            {
                this.InitializeForAsynchronous(program);
            }
            else
            {
                if (program.EventSequences.Count == 0)
                {
                    throw new Exception("Cannot execute a program that has no sequences.");
                }
                this.m_program = program;
                this.m_useSequencePluginData = this.m_program.UseSequencePluginData;
                EventSequence sequence = this.m_program.EventSequences[0].Sequence;
                if (this.m_program.EventSequences.Count > 1)
                {
                    if (this.m_contexts[this.m_secondaryContext] == null)
                    {
                        this.m_contexts[this.m_secondaryContext] = new EngineContext();
                    }
                }
                else
                {
                    this.m_contexts[this.m_secondaryContext] = null;
                }
            }
        }

        private void InitializeForAsynchronous(IExecutable executableObject)
        {
            List<Vixen.Channel> channels = executableObject.Channels;
            if (channels.Count == 0)
            {
                throw new Exception("Trying to setup for asynchronous operation with no channels?");
            }
            if ((this.m_contexts[this.m_primaryContext].m_routerContext != null) && this.m_contexts[this.m_primaryContext].m_routerContext.Initialized)
            {
                this.m_router.Shutdown(this.m_contexts[this.m_primaryContext].m_routerContext);
            }
            this.m_contexts[this.m_primaryContext].m_routerContext = this.m_router.CreateContext(new byte[channels.Count], executableObject.PlugInData, executableObject, null);
            this.m_contexts[this.m_primaryContext].m_channelMask = executableObject.Mask[0];
            this.m_contexts[this.m_secondaryContext] = null;
            Host.Communication["CurrentObject"] = this.m_program;
            this.m_router.Startup(this.m_contexts[this.m_primaryContext].m_routerContext);
            this.m_running = true;
            Host.Communication["CurrentObject"] = null;
        }

        private bool LoadSecondaryEngine(string enginePath)
        {
            if (this.m_mode == EngineMode.Asynchronous)
            {
                return false;
            }
            if (enginePath == null)
            {
                this.ReleaseSecondaryEngine();
                return true;
            }
            IEngine engine = null;
            try
            {
                Assembly assembly = Assembly.LoadFile(enginePath);
                foreach (System.Type type in assembly.GetExportedTypes())
                {
                    foreach (System.Type type2 in type.GetInterfaces())
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
            this.m_secondaryEngine = engine;
            if (engine != null)
            {
                this.m_secondaryEngine.EngineError += new OnEngineError(this.SecondaryEngineError);
                this.m_secondaryEngine.EngineStopped += new EventHandler(this.SecondaryEngineStopped);
                return true;
            }
            return false;
        }

        private void LogAudio(EventSequence sequence)
        {
            if (this.m_loggingEnabled && (sequence.Audio != null))
            {
                Host.LogAudio("Sequence", sequence.Name, sequence.Audio.FileName, sequence.Audio.Duration);
            }
        }

        private void m_eventTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.TimerTick(this.m_contexts[this.m_primaryContext]);
            if (((this.m_contexts[this.m_secondaryContext] != null) && (this.m_contexts[this.m_secondaryContext].m_routerContext != null)) && this.m_contexts[this.m_secondaryContext].m_routerContext.Initialized)
            {
                this.TimerTick(this.m_contexts[this.m_secondaryContext]);
            }
        }

        protected virtual void OnProgramEnd(bool restartBackgroundObjects)
        {
            if (this.ProgramEnd != null)
            {
                this.m_host.DelegateNullMethod(new MethodInvoker(this.ProgramEnd.Invoke));
            }
            string key = this.CurrentObject.Key.ToString();
            bool flag = Host.Communication.ContainsKey(key);
            int count = Host.Communication.Count;
            Host.Communication.Remove("KeyInterceptor_" + key);
            Host.Communication.Remove("ExecutionContext_" + key);
            if (restartBackgroundObjects)
            {
                this.RestartBackgroundObjects();
            }
        }

        protected virtual void OnSequenceChange()
        {
            if (this.SequenceChange != null)
            {
                this.SequenceChange();
            }
        }

        public void Pause()
        {
            if (((this.m_mode != EngineMode.Asynchronous) && this.IsRunning) && !this.m_isPaused)
            {
                lock (this.m_runLock)
                {
                    if (this.m_secondaryEngine != null)
                    {
                        this.m_secondaryEngine.Pause();
                        this.m_isPaused = true;
                        this.m_contexts[this.m_primaryContext].m_timekeeper.Stop();
                        this.m_running = false;
                    }
                    else
                    {
                        this.m_eventTimer.Stop();
                        if (this.m_contexts[this.m_primaryContext].m_soundChannel != null)
                        {
                            this.m_contexts[this.m_primaryContext].m_soundChannel.Paused = true;
                        }
                        this.m_contexts[this.m_primaryContext].m_timekeeper.Stop();
                        if (this.m_contexts[this.m_secondaryContext] != null)
                        {
                            this.m_contexts[this.m_secondaryContext].m_timekeeper.Stop();
                            if (this.m_contexts[this.m_secondaryContext].m_soundChannel != null)
                            {
                                this.m_contexts[this.m_secondaryContext].m_soundChannel.Paused = true;
                            }
                        }
                        this.m_isPaused = true;
                        this.m_running = false;
                    }
                }
            }
        }

        public bool Play(int startMillisecond, int endMillisecond, bool logAudio)
        {
            if (this.m_mode == EngineMode.Asynchronous)
            {
                return false;
            }
            try
            {
                this.m_loggingEnabled = logAudio;
                if (this.m_eventTimer.Enabled)
                {
                    return false;
                }
                this.StopBackgroundObjects();
                this.CreateScriptEngine(this.m_program.EventSequences[0].Sequence);
                if (this.m_secondaryEngine != null)
                {
                    if (this.m_isPaused)
                    {
                        this.m_isPaused = false;
                    }
                    else
                    {
                        this.ResetScriptEngineContext();
                        this.OnSequenceChange();
                        Host.Communication["CurrentObject"] = this.m_program;
                        this.m_router.Startup(this.m_contexts[this.m_primaryContext].m_routerContext);
                        Host.Communication["CurrentObject"] = null;
                    }
                    this.StartTimekeepers();
                    return this.m_secondaryEngine.Play();
                }
                if (!this.m_isPaused)
                {
                    this.InitEngineContext(ref this.m_contexts[this.m_primaryContext], this.CalcContainingSequence(startMillisecond));
                    this.LogAudio(this.m_contexts[this.m_primaryContext].m_currentSequence);
                    this.m_contexts[this.m_primaryContext].m_startOffset = startMillisecond;
                    this.InitEngineContext(ref this.m_contexts[this.m_secondaryContext], this.DetermineSecondarySequenceIndex());
                    this.m_contexts[this.m_primaryContext].m_maxEvent = (endMillisecond == 0) ? this.m_contexts[this.m_primaryContext].m_currentSequence.TotalEventPeriods : (endMillisecond / this.m_contexts[this.m_primaryContext].m_currentSequence.EventPeriod);
                }
                int millisecondPosition = (this.m_contexts[this.m_primaryContext].m_currentSequence.Audio != null) ? startMillisecond : -1;
                this.PrepareAudio(this.m_contexts[this.m_primaryContext], millisecondPosition);
                if (this.m_isPaused)
                {
                    this.m_isPaused = false;
                }
                else
                {
                    this.OnSequenceChange();
                    Host.Communication["CurrentObject"] = this.m_program;
                    this.m_router.Startup(this.m_contexts[this.m_primaryContext].m_routerContext);
                    Host.Communication["CurrentObject"] = null;
                }
                this.StartAudio(this.m_contexts[this.m_primaryContext]);
                this.StartTimekeepers();
                this.m_eventTimer.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void PrepareAudio(EngineContext context, int millisecondPosition)
        {
            if (((this.m_mode != EngineMode.Asynchronous) && (context.m_soundChannel != null)) && (millisecondPosition != -1))
            {
                this.m_fmod.Play(context.m_soundChannel, true);
                context.m_soundChannel.Position = (uint) millisecondPosition;
                context.m_soundChannel.Frequency = this.m_audioSpeed;
            }
        }

        private string QueryContext(EngineContext context)
        {
            if (context == null)
            {
                return "   (null)\n";
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("   Sequence: " + ((context.m_currentSequence != null) ? context.m_currentSequence.Name : "(null)"));
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
                    builder.Append(this.QueryMappedPlugIn(@in));
                }
            }
            else
            {
                builder.AppendLine("   Router context: (null)");
            }
            return builder.ToString();
        }

        public string QueryInstance(int index)
        {
            StringBuilder builder = new StringBuilder();
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
                builder.Append(this.QueryContext(engine.m_contexts[engine.m_primaryContext]));
                builder.AppendLine("Secondary context: " + engine.m_secondaryContext);
                builder.Append(this.QueryContext(engine.m_contexts[engine.m_secondaryContext]));
                builder.AppendLine("Loop: " + engine.m_loop);
                builder.AppendLine("Paused: " + engine.m_isPaused);
                builder.AppendLine("Running: " + engine.m_running);
                builder.AppendLine("Use sequence plugin data: " + engine.m_useSequencePluginData);
                builder.AppendLine("Mode: " + engine.m_mode);
            }
            return builder.ToString();
        }

        private string QueryMappedPlugIn(MappedOutputPlugIn mappedPlugIn)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("         Name: " + mappedPlugIn.PlugIn.Name);
            builder.AppendLine("         From: " + mappedPlugIn.From);
            builder.AppendLine("         To: " + mappedPlugIn.To);
            builder.AppendLine("         Enabled: " + mappedPlugIn.Enabled);
            return builder.ToString();
        }

        private byte[,] ReconfigureSourceData(EventSequence sequence)
        {
            List<int> list = new List<int>();
            byte[,] buffer = new byte[sequence.ChannelCount, sequence.TotalEventPeriods];
            foreach (Vixen.Channel channel in sequence.Channels)
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
            if (this.m_secondaryEngine != null)
            {
                lock (this.m_secondaryEngine)
                {
                    this.m_secondaryEngine.Stop();
                    this.m_secondaryEngine.HardwareUpdate = null;
                    this.m_secondaryEngine.EngineError -= new OnEngineError(this.SecondaryEngineError);
                    this.m_secondaryEngine.EngineStopped -= new EventHandler(this.SecondaryEngineStopped);
                    this.m_secondaryEngine.Dispose();
                    this.m_secondaryEngine = null;
                    this.m_secondaryContext = 0;
                }
            }
        }

        private void ResetScriptEngineContext()
        {
            this.InitEngineContext(ref this.m_contexts[this.m_primaryContext], 0);
            this.LogAudio(this.m_contexts[this.m_primaryContext].m_currentSequence);
            if (this.m_contexts[this.m_primaryContext].m_soundChannel != null)
            {
                this.m_contexts[this.m_primaryContext].m_soundChannel.SetEntryFade(0);
            }
            this.m_secondaryEngine.Initialize(this.m_contexts[this.m_primaryContext].m_currentSequence);
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
                this.m_host.StartBackgroundObjects();
            }
        }

        private void SecondaryEngineError(string message, string stackTrace)
        {
            this.Stop();
            if (this.m_host.IsBackgroundExecutionEngineInstance(this))
            {
                this.m_host.StopBackgroundSequence();
            }
            MessageBox.Show(message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SecondaryEngineStopped(object sender, EventArgs e)
        {
            if (this.m_running && this.m_loop)
            {
                this.FinalizeEngineContext(this.m_contexts[this.m_primaryContext], false);
                this.FinalizeEngineContext(this.m_contexts[this.m_secondaryContext], false);
                this.ResetScriptEngineContext();
                this.StartTimekeepers();
                this.OnSequenceChange();
                this.m_secondaryEngine.Play();
            }
            else
            {
                bool flag = this.m_host.IsBackgroundExecutionEngineInstance(this);
                if (flag)
                {
                    this.m_host.StopBackgroundSequenceUI();
                }
                this.FinalizeEngineContext(this.m_contexts[this.m_primaryContext]);
                this.FinalizeEngineContext(this.m_contexts[this.m_secondaryContext]);
                this.OnProgramEnd(!flag);
            }
        }

        public void SetAudioDevice(int value)
        {
            this.m_fmod.DeviceIndex = value;
        }

        private void StartAudio(EngineContext context)
        {
            if (((this.m_mode != EngineMode.Asynchronous) && (context.m_soundChannel != null)) && context.m_soundChannel.Paused)
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
                        method = delegate {
                            this.PrepareAudio(context, context.m_startOffset);
                            this.StartAudio(context);
                        };
                    }
                    Host.Invoke(method, new object[0]);
                }
                else
                {
                    this.PrepareAudio(context, context.m_startOffset);
                    this.StartAudio(context);
                }
            }
        }

        private void StartTimekeepers()
        {
            lock (this.m_runLock)
            {
                this.m_running = true;
                this.m_contexts[this.m_primaryContext].m_timekeeper.Start();
                if ((this.m_contexts[this.m_primaryContext].m_fadeStartTickCount != 0) && (this.m_contexts[this.m_primaryContext].m_tickCount >= this.m_contexts[this.m_primaryContext].m_fadeStartTickCount))
                {
                    this.m_contexts[this.m_secondaryContext].m_timekeeper.Start();
                }
            }
        }

        public void Stop()
        {
            if (!this.m_stopping)
            {
                this.m_stopping = true;
                if (this.m_mode != EngineMode.Asynchronous)
                {
                    new Thread(new ThreadStart(this.ExecutionStopThread)).Start();
                }
                else
                {
                    this.FinalizeEngineContext(this.m_contexts[this.m_primaryContext], true);
                    lock (this.m_runLock)
                    {
                        this.m_running = false;
                    }
                    this.m_stopping = false;
                }
            }
        }

        private void StopBackgroundObjects()
        {
            if (!this.m_host.IsBackgroundExecutionEngineInstance(this))
            {
                this.m_host.StopBackgroundObjects();
            }
        }

        private void StopExecution()
        {
            this.StopExecution(true);
        }

        private void StopExecution(bool shutdownPlugins)
        {
            lock (this.m_runLock)
            {
                this.m_running = false;
            }
            if (this.m_eventTimer != null)
            {
                this.m_eventTimer.Stop();
            }
            this.m_fmod.Stop(this.m_contexts[this.m_primaryContext].m_soundChannel);
            if (this.m_contexts[this.m_secondaryContext] != null)
            {
                this.m_fmod.Stop(this.m_contexts[this.m_secondaryContext].m_soundChannel);
            }
            this.m_isPaused = false;
            this.FinalizeEngineContext(this.m_contexts[this.m_primaryContext], shutdownPlugins);
            this.FinalizeEngineContext(this.m_contexts[this.m_secondaryContext], shutdownPlugins);
        }

        private void TimerTick(EngineContext context)
        {
            if (this.m_eventTimer.Enabled && !this.m_stopping)
            {
                int num = 0;
                if ((context.m_soundChannel != null) && context.m_soundChannel.IsPlaying)
                {
                    context.m_tickCount = (int) context.m_soundChannel.Position;
                }
                else
                {
                    context.m_tickCount = context.m_startOffset + ((int) context.m_timekeeper.ElapsedMilliseconds);
                }
                num = context.m_tickCount / context.m_currentSequence.EventPeriod;
                if (((context.m_fadeStartTickCount != 0) && (context.m_tickCount >= context.m_fadeStartTickCount)) && !this.m_contexts[this.m_secondaryContext].m_timekeeper.IsRunning)
                {
                    this.m_eventTimer.Enabled = false;
                    Host.Communication["CurrentObject"] = this.m_program;
                    this.m_router.Startup(this.m_contexts[this.m_secondaryContext].m_routerContext);
                    Host.Communication["CurrentObject"] = null;
                    this.LogAudio(this.m_contexts[this.m_secondaryContext].m_currentSequence);
                    this.StartContextAudio(this.m_contexts[this.m_secondaryContext]);
                    if (this.m_contexts[this.m_secondaryContext].m_soundChannel != null)
                    {
                        this.m_contexts[this.m_secondaryContext].m_soundChannel.Volume = 0f;
                    }
                    this.m_contexts[this.m_secondaryContext].m_timekeeper.Start();
                    this.m_eventTimer.Enabled = true;
                }
                if ((context.m_tickCount >= context.m_sequenceTickLength) || (num >= context.m_maxEvent))
                {
                    this.m_fmod.Stop(context.m_soundChannel);
                    context.m_timekeeper.Stop();
                    context.m_timekeeper.Reset();
                    if ((this.m_contexts[this.m_secondaryContext] == null) || (this.m_contexts[this.m_secondaryContext].m_routerContext == null))
                    {
                        if (this.m_loop)
                        {
                            this.LogAudio(this.m_contexts[this.m_primaryContext].m_currentSequence);
                            this.StartContextAudio(this.m_contexts[this.m_primaryContext]);
                            context.m_timekeeper.Start();
                            this.OnSequenceChange();
                        }
                        else
                        {
                            Host.Invoke(new MethodInvoker(this.Stop), new object[0]);
                        }
                    }
                    else
                    {
                        if (this.m_contexts[this.m_secondaryContext].m_soundChannel != null)
                        {
                            this.m_contexts[this.m_secondaryContext].m_soundChannel.Volume = 1f;
                        }
                        if (this.m_contexts[this.m_secondaryContext] != null)
                        {
                            this.FinalizeEngineContext(this.m_contexts[this.m_primaryContext]);
                            this.m_primaryContext ^= 1;
                            this.m_secondaryContext ^= 1;
                            this.InitEngineContext(ref this.m_contexts[this.m_secondaryContext], this.DetermineSecondarySequenceIndex());
                        }
                        if (!this.m_contexts[this.m_primaryContext].m_timekeeper.IsRunning)
                        {
                            Host.Communication["CurrentObject"] = this.m_program;
                            this.m_router.Startup(this.m_contexts[this.m_primaryContext].m_routerContext);
                            Host.Communication["CurrentObject"] = null;
                            this.LogAudio(this.m_contexts[this.m_primaryContext].m_currentSequence);
                            this.StartContextAudio(this.m_contexts[this.m_primaryContext]);
                            this.m_contexts[this.m_primaryContext].m_timekeeper.Start();
                        }
                        this.OnSequenceChange();
                    }
                }
                else if (num != context.m_lastIndex)
                {
                    context.m_lastIndex = num;
                    this.FireEvent(context, context.m_lastIndex);
                }
            }
        }

        public bool UsesObject(object uniqueReference)
        {
            if (uniqueReference is EventSequence)
            {
                return this.FindEventSequence(uniqueReference);
            }
            if (uniqueReference is SequenceProgram)
            {
                return (uniqueReference == this.m_program);
            }
            return ((uniqueReference is IOutputPlugIn) && this.FindOutputPlugIn(uniqueReference));
        }

        public float AudioSpeed
        {
            get
            {
                return this.m_audioSpeed;
            }
            set
            {
                this.m_audioSpeed = value;
            }
        }

        public XmlDocument CommDoc
        {
            set
            {
                this.m_commDoc = value;
            }
        }

        public int Count
        {
            get
            {
                return m_instanceList.Count;
            }
        }

        public IExecutable CurrentObject
        {
            get
            {
                return this.m_program;
            }
        }

        public string ExecutingProgram
        {
            get
            {
                if ((this.m_mode != EngineMode.Asynchronous) && (this.IsRunning || this.IsPaused))
                {
                    return this.m_program.Name;
                }
                return string.Empty;
            }
        }

        public string ExecutingSequence
        {
            get
            {
                if (this.m_mode == EngineMode.Asynchronous)
                {
                    return string.Empty;
                }
                if (this.IsRunning || this.IsPaused)
                {
                    return this.m_contexts[this.m_primaryContext].m_currentSequence.Name;
                }
                return "None";
            }
        }

        public bool IsPaused
        {
            get
            {
                return this.m_isPaused;
            }
        }

        public bool IsRunning
        {
            get
            {
                if (this.m_secondaryEngine != null)
                {
                    return this.m_secondaryEngine.IsRunning;
                }
                return this.m_running;
            }
        }

        public string LoadedProgram
        {
            get
            {
                if (this.m_mode == EngineMode.Asynchronous)
                {
                    return string.Empty;
                }
                if (this.m_program == null)
                {
                    return string.Empty;
                }
                return this.m_program.Name;
            }
        }

        public int LoadedProgramLength
        {
            get
            {
                if (this.m_mode == EngineMode.Asynchronous)
                {
                    return 0;
                }
                if (this.m_program == null)
                {
                    return 0;
                }
                return this.m_program.Length;
            }
        }

        public string LoadedSequence
        {
            get
            {
                if (this.m_mode == EngineMode.Asynchronous)
                {
                    return string.Empty;
                }
                if (this.m_contexts[this.m_primaryContext].m_currentSequence == null)
                {
                    return string.Empty;
                }
                return this.m_contexts[this.m_primaryContext].m_currentSequence.Name;
            }
        }

        public int LoadedSequenceLength
        {
            get
            {
                if (this.m_mode == EngineMode.Asynchronous)
                {
                    return 0;
                }
                if (this.m_contexts[this.m_primaryContext].m_currentSequence == null)
                {
                    return 0;
                }
                return this.m_contexts[this.m_primaryContext].m_currentSequence.Length;
            }
        }

        public bool Loop
        {
            get
            {
                return this.m_loop;
            }
            set
            {
                this.m_loop = value;
            }
        }

        public EngineMode Mode
        {
            get
            {
                return this.m_mode;
            }
        }

        public int ObjectPosition
        {
            get
            {
                EngineContext context = this.m_contexts[this.m_primaryContext];
                int tickCount = context.m_tickCount;
                for (int i = 0; i < context.m_sequenceIndex; i++)
                {
                    tickCount += this.m_program.EventSequences[i].Length;
                }
                return tickCount;
            }
        }

        public int Position
        {
            get
            {
                return this.m_contexts[this.m_primaryContext].m_tickCount;
            }
        }

        internal enum EngineMode
        {
            Synchronous,
            Asynchronous
        }

        private delegate void InitEngineContextDelegate(ref EngineContext context, int sequenceIndex);

        public delegate void ProgramEndDelegate();

        public delegate void SequenceChangeDelegate();

        private delegate void ShutdownDelegate(RouterContext routerInstanceInfo);
    }
}

