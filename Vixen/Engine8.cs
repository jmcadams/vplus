using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

using FMOD;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus {
    internal sealed class Engine8 : IDisposable, IQueryable {
        public delegate void ProgramEndDelegate();

        public delegate void SequenceChangeDelegate();

        private static readonly List<Engine8> InstanceList = new List<Engine8>();
        private readonly object _runLock;
        private EngineContext _engineContext;
        private System.Timers.Timer _eventTimer;
        private fmod _fmod;
        private Host _host;
        private bool _isLoggingEnabled;
        private bool _isStopping;
        private PlugInRouter _plugInRouter;
        private bool _useSequencePluginData;


        internal Engine8(Host host, int audioDeviceIndex) {
            IsPaused = false;
            IsLooping = false;
            IsRunning = false;
            _runLock = new object();
            _useSequencePluginData = false;
            AudioSpeed = 1f;
            _isLoggingEnabled = false;
            _isStopping = false;
            ConstructUsing(EngineMode.Synchronous, host, audioDeviceIndex);
        }


        internal Engine8(EngineMode mode, Host host, int audioDeviceIndex) {
            IsPaused = false;
            IsLooping = false;
            IsRunning = false;
            _runLock = new object();
            _useSequencePluginData = false;
            AudioSpeed = 1f;
            _isLoggingEnabled = false;
            _isStopping = false;
            ConstructUsing(mode, host, audioDeviceIndex);
        }


        public float AudioSpeed { get; set; }

        public SequenceProgram CurrentObject { get; private set; }

        public bool IsPaused { get; private set; }

        public bool IsRunning { get; private set; }

        public bool IsLooping { private get; set; }

        private EngineMode Mode { get; set; }

        public int Position {
            get { return _engineContext.TickCount; }
        }


        public void Dispose() {
            if (Mode == EngineMode.Synchronous) {
                if (_eventTimer != null) {
                    _eventTimer.Stop();
                    _eventTimer.Elapsed -= EventTimerElapsed;
                    _eventTimer.Dispose();
                    _eventTimer = null;
                }
                _fmod.Stop(_engineContext.SoundChannel);
                _fmod.Shutdown();
            }
            if (_plugInRouter != null) {
                try {
                    _plugInRouter.Shutdown(_engineContext.RouterContext);
                }
                catch (Exception exception) {
                    MessageBox.Show(Resources.engineShutDownError + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            ProgramEnd = null;
            SequenceChange = null;
            if (CurrentObject != null) {
                CurrentObject.Dispose();
            }
            InstanceList.Remove(this);
            GC.SuppressFinalize(this);
        }


        public event ProgramEndDelegate ProgramEnd;

        public event SequenceChangeDelegate SequenceChange;

        private int CalcContainingSequence(int milliseconds) {
            var mills = 0;
            for (var i = 0; i < CurrentObject.EventSequences.Count; i++) {
                mills += CurrentObject.EventSequences[i].Length;
                if (milliseconds <= mills) {
                    return i;
                }
            }
            return -1;
        }

        private void ConstructUsing(EngineMode mode, Host host, int audioDeviceIndex) {
            Mode = mode;
            _host = host;
            _plugInRouter = Host.Router;
            if (mode == EngineMode.Synchronous) {
                _eventTimer = new System.Timers.Timer(1.0);
                _eventTimer.Elapsed += EventTimerElapsed;
                _fmod = fmod.GetInstance(audioDeviceIndex);
            }
            else {
                _eventTimer = null;
                _fmod = null;
            }
            _engineContext = new EngineContext();
            InstanceList.Add(this);
        }


        private void ExecutionStopThread() {
            if ((IsRunning || IsPaused) && _plugInRouter != null) {
                StopExecution();
                _engineContext.CurrentSequence = null;
                OnProgramEnd();
            }
            _isStopping = false;
        }


        ~Engine8() {
            Dispose();
        }


        private void FinalizeEngineContext(EngineContext context, bool shutdownRouterContext = true) {
            if (context == null) {
                return;
            }

            if (shutdownRouterContext && (context.RouterContext != null)) {
                _plugInRouter.Shutdown(context.RouterContext);
                context.RouterContext = null;
            }
            if (context.SoundChannel != null) {
                _fmod.ReleaseSound(context.SoundChannel);
                context.SoundChannel = null;
            }
            if (!context.Timekeeper.IsRunning) {
                return;
            }
            context.Timekeeper.Stop();
            context.Timekeeper.Reset();
        }


        private void FireEvent(EngineContext context, int index) {
            if ((!context.Timekeeper.IsRunning || !_eventTimer.Enabled) || _isStopping) {
                return;
            }

            for (var i = 0; i < context.CurrentSequence.FullChannels.Count; i++) {
                context.RouterContext.EngineBuffer[i] = context.Data[i, index];
            }
            HardwareUpdate(context.RouterContext.EngineBuffer);
        }


        public void HardwareUpdate(byte[] values) {
            if (!IsRunning || _isStopping) {
                return;
            }

            lock (_runLock) {
                if (!IsRunning || _isStopping) {
                    return;
                }

                int num;
                var context = _engineContext;
                var engineBuffer = context.RouterContext.EngineBuffer;
                values.CopyTo(engineBuffer, 0);
                _plugInRouter.BeginUpdate();
                var flag = context.LastPeriod == null;
                for (num = 0; (num < engineBuffer.Length) && !flag; num++) {
                    flag |= engineBuffer[num] != context.LastPeriod[num];
                }
                if (!flag) {
                    _plugInRouter.CancelUpdate();
                }
                else {
                    if ((context.LastPeriod != null) && (engineBuffer.Length == context.LastPeriod.Length)) {
                        engineBuffer.CopyTo(context.LastPeriod, 0);
                    }
                    var executableObject = context.RouterContext.ExecutableObject;
                    var count = executableObject.Channels.Count;
                    for (num = 0; num < count; num++) {
                        var channel = executableObject.Channels[num];
                        if (channel.DimmingCurve != null) {
                            engineBuffer[num] = channel.DimmingCurve[engineBuffer[num]];
                        }
                    }
                    for (num = 0; num < context.ChannelMask.Length; num++) {
                        engineBuffer[num] = (byte) (engineBuffer[num] & context.ChannelMask[num]);
                    }
                    try {
                        _plugInRouter.EndUpdate();
                    }
                    catch (Exception) {
                        StopExecution();
                    }
                }
            }
        }


        private void InitEngineContext(ref EngineContext context, int sequenceIndex) {
            if (context == null) {
                return;
            }

            context.Timekeeper.Stop();
            if (Host.InvokeRequired) {
                Host.Invoke(new InitEngineContextDelegate(InitEngineContext), new object[] {context, sequenceIndex});
            }
            else if ((sequenceIndex == -1) || (CurrentObject.EventSequences.Count <= sequenceIndex)) {
                FinalizeEngineContext(context);
            }
            else {
                var executableObject = CurrentObject.EventSequences[sequenceIndex].Sequence;
                if (context.RouterContext == null) {
                    context.RouterContext = _plugInRouter.CreateContext(new byte[executableObject.FullChannelCount],
                        _useSequencePluginData ? executableObject.PlugInData : CurrentObject.SetupData,
                        executableObject);
                }
                if (CurrentObject.Mask.Length > sequenceIndex) {
                    context.ChannelMask = CurrentObject.Mask[sequenceIndex];
                }
                context.TickCount = 0;
                context.LastIndex = -1;
                context.SequenceTickLength = executableObject.Time;
                context.StartOffset = 0;
                context.SoundChannel = executableObject.Audio != null ? _fmod.LoadSound(Path.Combine(Paths.AudioPath, executableObject.Audio.FileName), context.SoundChannel) : _fmod.LoadSound(null);
                context.CurrentSequence = executableObject;
                context.MaxEvent = context.CurrentSequence.TotalEventPeriods;
                context.LastPeriod = new byte[context.CurrentSequence.FullChannels.Count];
                for (var i = 0; i < context.LastPeriod.Length; i++) {
                    context.LastPeriod[i] = (byte) i;
                }
                context.Data = ReconfigureSourceData(executableObject);
            }
        }


        private void Initialize(EventSequence sequence) {
            switch (Mode) {
                case EngineMode.Asynchronous:
                    InitializeForAsynchronous(sequence);
                    break;
                default:
                    Initialize(new SequenceProgram(sequence));
                    break;
            }
        }


        public void Initialize(IExecutable obj) {
            var sequenceProgram = obj as SequenceProgram;
            if (sequenceProgram != null) {
                Initialize(sequenceProgram);
            }
            else {
                var eventSequence = obj as EventSequence;
                if (eventSequence != null) {
                    Initialize(eventSequence);
                }
                else {
                    var profile = obj as Profile;
                    if (profile == null) {
                        throw new Exception("Trying to initialize the engine with an unknown object type.\nType: " + obj.GetType());
                    }
                    Initialize((Profile) obj);
                }
            }
        }


        private void Initialize(Profile profile) {
            if (Mode == EngineMode.Synchronous) {
                throw new Exception("Only an asynchronous engine instance can be initialized with a profile.");
            }
            profile.Freeze();
            InitializeForAsynchronous(profile);
        }


        private void Initialize(SequenceProgram program) {
            if (Mode == EngineMode.Asynchronous) {
                InitializeForAsynchronous(program);
            }
            else {
                if (program.EventSequences.Count == 0) {
                    throw new Exception("Cannot execute a program that has no sequences.");
                }
                CurrentObject = program;
                _useSequencePluginData = CurrentObject.UseSequencePluginData;
            }
        }


        private void InitializeForAsynchronous(IExecutable executableObject) {
            var channels = executableObject.FullChannels;
            if (channels.Count == 0) {
                throw new Exception("Trying to setup for asynchronous operation with no channels?");
            }
            if ((_engineContext.RouterContext != null) && _engineContext.RouterContext.Initialized) {
                _plugInRouter.Shutdown(_engineContext.RouterContext);
            }
            _engineContext.RouterContext = _plugInRouter.CreateContext(new byte[channels.Count], executableObject.PlugInData,
                executableObject);
            _engineContext.ChannelMask = executableObject.Mask[0];
            Host.Communication["CurrentObject"] = CurrentObject;
            _plugInRouter.Startup(_engineContext.RouterContext);
            IsRunning = true;
            Host.Communication["CurrentObject"] = null;
        }


        private void LogAudio(EventSequence sequence) {
            if (_isLoggingEnabled && (sequence.Audio != null)) {
                Host.LogAudio("Sequence", sequence.Name, sequence.Audio.FileName, sequence.Audio.Duration);
            }
        }


        private void EventTimerElapsed(object sender, ElapsedEventArgs e) {
            TimerTick(_engineContext);
        }


        private void OnProgramEnd() {
            if (ProgramEnd != null) {
                _host.DelegateNullMethod(ProgramEnd.Invoke);
            }
            var key = CurrentObject.Key.ToString(CultureInfo.InvariantCulture);
            Host.Communication.Remove("KeyInterceptor_" + key);
            Host.Communication.Remove("ExecutionContext_" + key);
        }


        private void OnSequenceChange() {
            if (SequenceChange != null) {
                SequenceChange();
            }
        }


        public void Pause() {
            if (((Mode == EngineMode.Asynchronous) || !IsRunning) || IsPaused) {
                return;
            }
            _eventTimer.Stop();
            if (_engineContext.SoundChannel != null) {
                _engineContext.SoundChannel.Paused = true;
            }
            _engineContext.Timekeeper.Stop();
            IsPaused = true;
            IsRunning = false;
        }


        public bool Play(int startMillisecond, int endMillisecond, bool logAudio) {
            if (Mode == EngineMode.Asynchronous) {
                return false;
            }
            try {
                _isLoggingEnabled = logAudio;
                if (_eventTimer.Enabled) {
                    return false;
                }
                if (!IsPaused) {
                    InitEngineContext(ref _engineContext, CalcContainingSequence(startMillisecond));
                    LogAudio(_engineContext.CurrentSequence);
                    _engineContext.StartOffset = startMillisecond;
                    _engineContext.MaxEvent = (endMillisecond == 0)
                        ? _engineContext.CurrentSequence.TotalEventPeriods
                        : (endMillisecond / _engineContext.CurrentSequence.EventPeriod);
                }
                var millisecondPosition = (_engineContext.CurrentSequence.Audio != null) ? startMillisecond : -1;
                PrepareAudio(_engineContext, millisecondPosition);
                if (IsPaused) {
                    IsPaused = false;
                }
                else {
                    OnSequenceChange();
                    Host.Communication["CurrentObject"] = CurrentObject;
                    _plugInRouter.Startup(_engineContext.RouterContext);
                    Host.Communication["CurrentObject"] = null;
                }
                StartAudio(_engineContext);
                StartTimekeepers();
                _eventTimer.Start();
                return true;
            }
            catch {
                return false;
            }
        }


        private void PrepareAudio(EngineContext context, int millisecondPosition) {
            if (((Mode == EngineMode.Asynchronous) || (context.SoundChannel == null)) || (millisecondPosition == -1)) {
                return;
            }
            _fmod.Play(context.SoundChannel, true);
            context.SoundChannel.Position = (uint) millisecondPosition;
            context.SoundChannel.Frequency = AudioSpeed;
        }

        private static byte[,] ReconfigureSourceData(EventSequence sequence) {
            var buffer = new byte[sequence.FullChannelCount,sequence.TotalEventPeriods];
            var list = sequence.FullChannels.Select(channel => channel.OutputChannel).ToList();
            for (var i = 0; i < sequence.FullChannelCount; i++) {
                var row = list[i];
                for (var column = 0; column < sequence.TotalEventPeriods; column++) {
                    buffer[row, column] = sequence.EventValues[i, column];
                }
            }
            return buffer;
        }


        public void SetAudioDevice(int value) {
            _fmod.DeviceIndex = value;
        }


        private void StartAudio(EngineContext context) {
            if (((Mode != EngineMode.Asynchronous) && (context.SoundChannel != null)) && context.SoundChannel.Paused) {
                context.SoundChannel.Paused = false;
            }
        }


        private void StartContextAudio(EngineContext context) {
            if (context.CurrentSequence.Audio == null) {
                return;
            }
            if (Host.InvokeRequired) {
                MethodInvoker method = delegate {
                    PrepareAudio(context, context.StartOffset);
                    StartAudio(context);
                };
                Host.Invoke(method, new object[0]);
            }
            else {
                PrepareAudio(context, context.StartOffset);
                StartAudio(context);
            }
        }


        private void StartTimekeepers() {
            lock (_runLock) {
                IsRunning = true;
                _engineContext.Timekeeper.Start();
            }
        }


        public void Stop() {
            if (_isStopping) {
                return;
            }
            _isStopping = true;
            if (Mode != EngineMode.Asynchronous) {
                new Thread(ExecutionStopThread).Start();
            }
            else {
                FinalizeEngineContext(_engineContext);
                lock (_runLock) {
                    IsRunning = false;
                }
                _isStopping = false;
            }
        }


        private void StopExecution(bool shutdownPlugins = true) {
            lock (_runLock) {
                IsRunning = false;
            }
            if (_eventTimer != null) {
                _eventTimer.Stop();
            }
            _fmod.Stop(_engineContext.SoundChannel);
            IsPaused = false;
            FinalizeEngineContext(_engineContext, shutdownPlugins);
        }


        private void TimerTick(EngineContext context) {
            if (!_eventTimer.Enabled || _isStopping) {
                return;
            }
            context.TickCount = (context.SoundChannel != null) && context.SoundChannel.IsPlaying
                ? (int) context.SoundChannel.Position : context.StartOffset + ((int) context.Timekeeper.ElapsedMilliseconds);
            var num = context.TickCount / context.CurrentSequence.EventPeriod;
            if ((context.TickCount >= context.SequenceTickLength) || (num >= context.MaxEvent)) {
                _fmod.Stop(context.SoundChannel);
                context.Timekeeper.Stop();
                context.Timekeeper.Reset();
                if (IsLooping) {
                    LogAudio(_engineContext.CurrentSequence);
                    StartContextAudio(_engineContext);
                    context.Timekeeper.Start();
                    OnSequenceChange();
                }
                else {
                    Host.Invoke(new MethodInvoker(Stop), new object[0]);
                }
            }
            else if (num != context.LastIndex) {
                context.LastIndex = num;
                FireEvent(context, context.LastIndex);
            }
        }


        internal enum EngineMode {
            Synchronous,
            Asynchronous
        }

        private delegate void InitEngineContextDelegate(ref EngineContext context, int sequenceIndex);
    }
}