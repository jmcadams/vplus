using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

using Properties;

namespace VixenPlus {
    internal class PlugInRouter {
        private static PlugInRouter _instance;
        private readonly List<RouterContext> _instances = new List<RouterContext>();
        private readonly MethodInvoker _methodInvoker;
        private readonly List<MappedOutputPlugIn> _outputPlugins = new List<MappedOutputPlugIn>();

        private readonly Dictionary<IExecutable, List<InputPlugin>> _sequenceInputPlugins = new Dictionary<IExecutable, List<InputPlugin>>();

        private byte[] _data;
        private int _refCount;
        //private bool _updateLock;

        private PlugInRouter() {
            _methodInvoker = EndUpdate;
        }


        public void BeginUpdate() {
            _refCount++;
        }


        public void CancelUpdate() {
            if (_refCount != 0) {
                _refCount--;
            }
        }


        public RouterContext CreateContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject, ITickSource tickSource) {
            var item = new RouterContext(engineBuffer, pluginData, executableObject, tickSource);
            int newSize = Math.Max((_data == null) ? 0 : _data.Length, item.EngineBuffer.Length);
            if (_data == null) {
                _data = new byte[newSize];
            }
            else if (_data.Length < newSize) {
                Array.Resize(ref _data, newSize);
            }
            foreach (MappedOutputPlugIn @in in item.OutputPluginList) {
                _outputPlugins.Add(@in);
            }
            _instances.Add(item);
            return item;
        }


        public void EndUpdate() {
            if (_data == null) {
                return;
            }
            //_updateLock = true;
            if (Host.InvokeRequired) {
                Host.Invoke(_methodInvoker, new object[0]);
            }
            else {
                //try
                //{
                if (_refCount == 0) {
                    //_updateLock = false;
                }
                else if (--_refCount == 0) {
                    int index = 0;
                    while (index < _data.Length) {
                        _data[index] = 0;
                        index++;
                    }
                    try {
                        foreach (RouterContext context in _instances) {
                            byte[] engineBuffer = context.EngineBuffer;
                            for (index = 0; index < engineBuffer.Length; index++) {
                                _data[index] = Math.Max(_data[index], engineBuffer[index]);
                            }
                        }
                    }
                    catch (Exception exception) {
                        //_updateLock = false;
                        throw new Exception(string.Format("(Router - Update)\n{0}", exception.Message), exception);
                    }
                    bool flag = Host.GetDebugValue("EventAverages") != null;
                    Stopwatch stopwatch = null;
                    if (flag) {
                        stopwatch = new Stopwatch();
                        Host.SetDebugValue("TotalEvents",
                                           (int.Parse(Host.GetDebugValue("TotalEvents")) + 1).ToString(CultureInfo.InvariantCulture));
                    }
                    try {
                        lock (_outputPlugins) {
                            foreach (MappedOutputPlugIn @in in _outputPlugins) {
                                if (!((@in.From != 0) && @in.ContextInitialized)) {
                                    continue;
                                }
                                Array.Copy(_data, @in.From - 1, @in.Buffer, 0, (@in.To - @in.From) + 1);
                                var eventDrivenOutputPlugIn = @in.PlugIn as IEventDrivenOutputPlugIn;
                                if (eventDrivenOutputPlugIn == null) {
                                    continue;
                                }
                                if (flag) {
                                    stopwatch.Reset();
                                    stopwatch.Start();
                                }
                                eventDrivenOutputPlugIn.Event(@in.Buffer);
                                if (!flag) {
                                    continue;
                                }
                                stopwatch.Stop();
                                var userData = (long) @in.UserData;
                                userData += stopwatch.ElapsedMilliseconds;
                                @in.UserData = userData;
                            }
                        }
                    }
                    catch (Exception exception2) {
                        //_updateLock = false;
                        throw new Exception(string.Format(Resources.RouterOutputError, exception2.Message), exception2);
                    }
                    //_updateLock = false;
                }
                //}
                //finally
                //{
                //    //_updateLock = false;
                //}
            }
        }


        public static PlugInRouter GetInstance() {
            return _instance ?? (_instance = new PlugInRouter());
        }


        public bool GetSequenceInputs(IExecutable executableObject, byte[] eventBuffer, bool forLiveUpdate, bool forRecord) {
            bool flag = false;
            foreach (InputPlugin plugin in _sequenceInputPlugins[executableObject]) {
                if (((forLiveUpdate != plugin.LiveUpdate) || !forLiveUpdate) && ((forRecord != plugin.Record) || !forRecord)) {
                    continue;
                }
                Predicate<Channel> match = null;
                foreach (Input input in plugin.Inputs) {
                    if (!input.Enabled || !input.GetChangedInternal()) {
                        continue;
                    }
                    flag = true;
                    byte valueInternal = input.GetValueInternal();
                    foreach (string str in plugin.MappingSets.CurrentMappingSet.GetOutputChannelIdList(input)) {
                        ulong ulChannelId = ulong.Parse(str);
                        if (match == null) {
                            ulong id = ulChannelId;
                            match = c => c.Id == id;
                        }
                        int outputChannel = executableObject.Channels.Find(match).OutputChannel;
                        eventBuffer[outputChannel] = valueInternal;
                    }
                }
            }
            return flag;
        }


        public void Shutdown(RouterContext routerContext) {
            try {
                if ((routerContext == null) || !routerContext.Initialized) {
                    return;
                }
                //try
                //{
                routerContext.Initialized = false;
                if (!_instances.Contains(routerContext)) {
                    return;
                }
                if (((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ClearAtEndOfSequence")) {
                    BeginUpdate();
                    Array.Clear(routerContext.EngineBuffer, 0, routerContext.EngineBuffer.Length);
                    EndUpdate();
                }
                bool flag2 = Host.GetDebugValue("EventAverages") != null;
                int num = 0;
                int num2 = 0;
                if (flag2) {
                    num = int.Parse(Host.GetDebugValue("TotalEvents"));
                }
                lock (_outputPlugins) {
                    foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList) {
                        @in.PlugIn.Shutdown();
                        if (flag2) {
                            Host.SetDebugValue(string.Format("event_average_{0}", num2++),
                                               string.Format("{0}|{1}|{2}|{3}",
                                                             new object[]
                                                             {@in.PlugIn.Name, @in.From, @in.To, (((long) @in.UserData)) / ((float) num)}));
                        }
                        _outputPlugins.Remove(@in);
                    }
                }
                if (_sequenceInputPlugins.ContainsKey(routerContext.ExecutableObject)) {
                    lock (_sequenceInputPlugins) {
                        foreach (InputPlugin plugin in _sequenceInputPlugins[routerContext.ExecutableObject]) {
                            plugin.ShutdownInternal();
                        }
                        //TODO This causes a bug is another window is open that uses this xObject since the xObject gets removed.
                        _sequenceInputPlugins.Remove(routerContext.ExecutableObject);
                    }
                }
                lock (_instances) {
                    _instances.Remove(routerContext);
                }
                if (_instances.Count == 0) {
                    _data = null;
                }
                //}
                //finally
                //{
                //    //_updateLock = false;
                //}
            }
            catch (Exception exception) {
                //_updateLock = false;
                MessageBox.Show(string.Format(Resources.RouterError, exception.Message, exception.StackTrace), Resources.PluginError,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public void Startup(RouterContext routerContext) {
            int num = 0;
            string name = string.Empty;
            if (routerContext.Initialized) {
                Shutdown(routerContext);
            }
            try {
                List<InputPlugin> list;
                routerContext.Initialized = false;
                foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList) {
                    var eventDrivenOutputPlugIn = @in.PlugIn as IEventDrivenOutputPlugIn;
                    if (eventDrivenOutputPlugIn != null) {
                        eventDrivenOutputPlugIn.Initialize(routerContext.ExecutableObject, routerContext.PluginData, @in.SetupDataNode);
                    }
                    else {
                        ((IEventlessOutputPlugIn) @in.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData, @in.SetupDataNode,
                                                                         routerContext.TickSource);
                    }
                }
                _sequenceInputPlugins[routerContext.ExecutableObject] = list = new List<InputPlugin>();
                foreach (XmlNode node2 in routerContext.PluginData.GetAllPluginData(SetupData.PluginType.Input, true)) {
                    if (node2.Attributes == null) {
                        continue;
                    }
                    var item = (InputPlugin) InputPlugins.FindPlugin(node2.Attributes["name"].Value, true);
                    item.InitializeInternal(routerContext.PluginData, node2);
                    item.SetupDataToPlugin();
                    list.Add(item);
                }
                var executable = (IExecutable) Host.Communication["CurrentObject"];
                string str2 = string.Empty;
                if (executable != null) {
                    str2 = executable.Key.ToString(CultureInfo.InvariantCulture);
                }
                try {
                    foreach (InputPlugin plugin2 in _sequenceInputPlugins[routerContext.ExecutableObject]) {
                        name = plugin2.Name;
                        plugin2.StartupInternal();
                    }
                    foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList) {
                        name = @in.PlugIn.Name;
                        XmlNode plugInData = routerContext.PluginData.GetPlugInData(num.ToString(CultureInfo.InvariantCulture));
                        if (executable != null) {
                            Host.Communication["SetupNode_" + str2] = plugInData;
                        }
                        @in.PlugIn.Startup();
                        if (Host.GetDebugValue("EventAverages") != null) {
                            @in.UserData = 0L;
                            Host.SetDebugValue("TotalEvents", "0");
                        }
                        Host.Communication.Remove("SetupNode_" + str2);
                        num++;
                    }
                }
                catch (Exception exception) {
                    throw new Exception(string.Format(Resources.RouterStartupError, exception.Message), exception);
                }
                routerContext.Initialized = true;
            }
            catch (Exception exception2) {
                MessageBox.Show(string.Format("{0}:\n\n{1}", name, exception2.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}
