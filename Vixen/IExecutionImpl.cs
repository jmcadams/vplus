using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus {
    internal class ExecutionImpl : IExecution {
        private readonly string _errorLog;
        private readonly Host _host;
        private readonly Preference2 _preferences;
        private readonly Dictionary<int, ExecutionContext> _registeredContexts;


        public ExecutionImpl(Host host) {
            _host = host;
            _registeredContexts = new Dictionary<int, ExecutionContext>();
            _preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
            _errorLog = Path.Combine(Paths.DataPath, "iexecution.err");
        }


        public int EngineStatus(int contextHandle) {
            var status = Utils.ExecutionStopped;
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return status;
            }
            if (context.SynchronousEngineInstance == null) {
                return status;
            }
            if (context.SynchronousEngineInstance.IsPaused) {
                status = Utils.ExecutionPaused;
            }
            else if (context.SynchronousEngineInstance.IsRunning) {
                status = Utils.ExecutionRunning;
            }
            return status;
        }


        public int EngineStatus(int contextHandle, out int position) {
            var status = Utils.ExecutionStopped;
            position = 0;
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return status;
            }

            if (context.SynchronousEngineInstance == null) {
                return status;
            }
            if (context.SynchronousEngineInstance.IsPaused) {
                status = Utils.ExecutionPaused;
                position = context.SynchronousEngineInstance.Position;
            }
            else if (context.SynchronousEngineInstance.IsRunning) {
                status = Utils.ExecutionRunning;
                position = context.SynchronousEngineInstance.Position;
            }
            return status;
        }


        public void ExecutePause(int contextHandle) {
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return;
            }
            if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed)) {
                return;
            }
            try {
                context.SynchronousEngineInstance.Pause();
            }
            catch (Exception exception) {
                LogError("ExecutePause", exception);
            }
        }


        public bool ExecutePlay(int contextHandle, int millisecondStart, int millisecondCount) {
            return ExecutePlay(contextHandle, millisecondStart, millisecondCount, _preferences.GetBoolean("LogAudioManual"));
        }


        private bool ExecutePlay(int contextHandle, int startMillisecond, int endMillisecond, bool logAudio) {
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return false;
            }
            if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed)) {
                return false;
            }
            if (context.SynchronousEngineInstance.IsRunning) {
                return false;
            }
            if (!context.SynchronousEngineInstance.IsPaused) {
                try {
                    context.SynchronousEngineInstance.Initialize(context.Object);
                }
                catch (Exception exception) {
                    LogError("ExecutePlay", exception);
                    return false;
                }
            }
            var str = context.SynchronousEngineInstance.CurrentObject.Key.ToString(CultureInfo.InvariantCulture);
            Host.Communication["KeyInterceptor_" + str] = context.KeyInterceptor;
            Host.Communication["ExecutionContext_" + str] = context;
            var flag = context.SynchronousEngineInstance.Play(startMillisecond, endMillisecond, logAudio);
            foreach (var form in context.OutputPlugInForms) {
                form.BringToFront();
            }
            return flag;
        }


        public void ExecuteStop(int contextHandle) {
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return;
            }
            if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed)) {
                return;
            }
            try {
                context.SynchronousEngineInstance.Stop();
                if (_preferences.GetBoolean("SavePlugInDialogPositions")) {
                    foreach (var form in context.OutputPlugInForms) {
                        var base2 = (OutputPlugInUIBase) form;
                        if (base2.WindowState != FormWindowState.Normal) {
                            continue;
                        }
                        var nodeAlways = Xml.GetNodeAlways(Xml.GetNodeAlways(base2.DataNode, "DialogPositions"), base2.Name);
                        Xml.SetAttribute(nodeAlways, "x", base2.Location.X.ToString(CultureInfo.InvariantCulture));
                        Xml.SetAttribute(nodeAlways, "y", base2.Location.Y.ToString(CultureInfo.InvariantCulture));
                    }
                }
                context.OutputPlugInForms.Clear();
                GC.Collect();
            }
            catch (Exception exception) {
                LogError("ExecuteStop", exception);
            }
        }


        public float GetAudioSpeed(int contextHandle) {
            ExecutionContext context;
            if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null)) {
                return context.SynchronousEngineInstance.AudioSpeed;
            }
            return 0f;
        }


        public void ReleaseContext(int contextHandle) {
            if (!_registeredContexts.ContainsKey(contextHandle)) {
                return;
            }

            try {
                _registeredContexts[contextHandle].Dispose();
                _registeredContexts.Remove(contextHandle);
            }
            catch (Exception exception) {
                LogError("ReleaseContext", exception);
            }
        }


        public int RequestContext(bool suppressAsynchronousContext, bool suppressSynchronousContext, Form keyInterceptor) {
            try {
                var num = ((int) DateTime.Now.ToBinary()) + _registeredContexts.Count;
                var context = new ExecutionContext
                {SuppressAsynchronousContext = suppressAsynchronousContext, SuppressSynchronousContext = suppressSynchronousContext};
                var integer = _preferences.GetInteger("SoundDevice");
                context.SynchronousEngineInstance = context.SuppressSynchronousContext ? null : new Engine8(_host, integer);
                context.AsynchronousEngineInstance = context.SuppressAsynchronousContext
                    ? null : new Engine8(Engine8.EngineMode.Asynchronous, _host, integer);
                context.LocalRequestor = RequestorIsLocal(new StackTrace().GetFrame(1));
                context.Object = null;
                context.KeyInterceptor = keyInterceptor;
                if (!context.SuppressAsynchronousContext) {
                    byte[][] mask;
                    SetupData plugInData;
                    var str = _preferences.GetString("AsynchronousData");
                    var str2 = ((str != "Default") && (str != "Sync")) ? str : _preferences.GetString("DefaultProfile");
                    Profile profile = null;
                    if (str2.Length > 0) {
                        try {
                            profile = new Profile(Path.Combine(Paths.ProfilePath, str2 + ".pro"));
                            profile.Freeze();
                        }
                        catch {
                            LogError("RequestContext", "Error loading profile " + str2);
                        }
                    }
                    if (profile == null) {
                        if (str == "Default") {
                            LogError("RequestContext",
                                "Preference set to use default profile for asynchronous execution, but no default profile exists.");
                        }
                        mask = null;
                        plugInData = null;
                    }
                    else {
                        mask = profile.Mask;
                        plugInData = profile.PlugInData;
                        context.Object = profile;
                    }
                    if (context.Object != null) {
                        context.Object.Mask = mask;
                        if (plugInData != null) {
                            context.Object.PlugInData.ReplaceRoot(plugInData.RootNode);
                        }
                        AsyncInit(context);
                    }
                }
                _registeredContexts[num] = context;
                return num;
            }
            catch (Exception exception) {
                LogError("RequestContext", exception);
                return 0;
            }
        }

        public void SetAsynchronousContext(int contextHandle, IExecutable executableObject) {
            if (contextHandle == 0) {
                return;
            }
            var context = _registeredContexts[contextHandle];
            if (context.SuppressAsynchronousContext) {
                return;
            }
            if (context.Object != null) {
                context.AsynchronousEngineInstance.Stop();
            }
            try {
                context.Object = executableObject;
                AsyncInit(context);
            }
            catch (Exception exception) {
                LogError("SetAsynchronousContext", exception);
            }
        }

        public void SetAudioSpeed(int contextHandle, float rate) {
            ExecutionContext context;
            if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null)) {
                context.SynchronousEngineInstance.AudioSpeed = rate;
            }
        }


        public void SetChannelStates(int contextHandle, byte[] channelValues) {
            ExecutionContext context;
            if (!_registeredContexts.TryGetValue(contextHandle, out context)) {
                return;
            }

            if (context.SynchronousEngineInstance != null) {
                context.SynchronousEngineInstance.HardwareUpdate(channelValues);
            }
            if (context.AsynchronousEngineInstance != null) {
                context.AsynchronousEngineInstance.HardwareUpdate(channelValues);
            }
        }


        public void SetLoopState(int contextHandle, bool state) {
            ExecutionContext context;
            if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null)) {
                context.SynchronousEngineInstance.IsLooping = state;
            }
        }


        public void SetSynchronousContext(int contextHandle, IExecutable executableObject) {
            if (contextHandle == 0) {
                return;
            }

            var context = _registeredContexts[contextHandle];
            if (context.SuppressSynchronousContext) {
                return;
            }

            try {
                var mask = executableObject.Mask;
                var plugInData = executableObject.PlugInData;
                if (!context.LocalRequestor && !executableObject.TreatAsLocal) {
                    Profile profile;
                    var str = _preferences.GetString("SynchronousData");
                    if (str == "Default") {
                        var str2 = _preferences.GetString("DefaultProfile");
                        if (str2 == string.Empty) {
                            LogError("SetSynchronousContext",
                                "Preference set to use default profile for synchronous execution, but no default profile set.");
                            mask = null;
                            plugInData = null;
                        }
                        else {
                            profile = new Profile(Path.Combine(Paths.ProfilePath, str2 + ".pro"));
                            profile.Freeze();
                            mask = profile.Mask;
                            plugInData = profile.PlugInData;
                        }
                    }
                    else if (str != "Embedded") {
                        profile = new Profile(Path.Combine(Paths.ProfilePath, str + ".pro"));
                        profile.Freeze();
                        mask = profile.Mask;
                        plugInData = profile.PlugInData;
                    }
                }
                context.Object = executableObject;
                context.Object.Mask = mask;
                if (plugInData != null) {
                    context.Object.PlugInData.ReplaceRoot(plugInData.RootNode);
                }
                if (!(context.SuppressAsynchronousContext || _preferences.GetString("AsynchronousData") != "Sync")) {
                    AsyncInit(context);
                }
                if (executableObject.AudioDeviceIndex != -1) {
                    context.SynchronousEngineInstance.SetAudioDevice(executableObject.AudioDeviceIndex);
                }
                if (executableObject.AudioDeviceVolume != 0) {
                    context.SynchronousEngineInstance.SetAudioDevice(executableObject.AudioDeviceIndex);
                }
            }
            catch (Exception exception) {
                LogError("SetSynchronousContext", exception);
            }
        }


        // Async context means it is not synced with any music (e.g. channel testing)
        private static void AsyncInit(ExecutionContext context) {
            context.AsynchronousEngineInstance.Initialize(context.Object);
        }


        private void LogError(string errorSource, Exception e) {
            using (var writer = new StreamWriter(_errorLog, true)) {
                try {
                    writer.WriteLine("\n{0} {1}: {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), errorSource, e.Message);
                    writer.WriteLine(e.StackTrace);
                }
                finally {
                    writer.Close();
                }
            }
        }


        private void LogError(string errorSource, string message) {
            using (var writer = new StreamWriter(_errorLog, true)) {
                try {
                    writer.WriteLine("\n{0} {1}: {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), errorSource, message);
                }
                finally {
                    writer.Close();
                }
            }
        }


        private static bool RequestorIsLocal(StackFrame requestorFrame) {
            var assembly = requestorFrame.GetMethod().Module.Assembly;

            using (var process = Process.GetCurrentProcess()) {
                if (requestorFrame.GetMethod().Module.Name == process.MainModule.ModuleName) {
                    return true;
                }
            }
            var uiBaseFlag = false;
            var iAddInFlag = false;
            var iVixenMdiFlag = false;
            foreach (var type in assembly.GetExportedTypes()) {
                if (type.BaseType != null) {
                    uiBaseFlag |= type.BaseType.Name == "UIBase";
                }
                foreach (var type2 in type.GetInterfaces()) {
                    iVixenMdiFlag |= type2.Name == "IVixenMDI";
                    iAddInFlag |= type2.Name == "IAddIn";
                }
            }
            return ((uiBaseFlag && iVixenMdiFlag) && !iAddInFlag);
        }
    }
}