using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus {
    internal class PlugInRouter {
        private static PlugInRouter _instance;
        private readonly List<RouterContext> _instances = new List<RouterContext>();
        private readonly MethodInvoker _methodInvoker;
        private readonly List<MappedOutputPlugIn> _outputPlugins = new List<MappedOutputPlugIn>();

        private byte[] _data;
        private int _refCount;

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


        public RouterContext CreateContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject) {
            var item = new RouterContext(engineBuffer, pluginData, executableObject);
            var newSize = Math.Max((_data == null) ? 0 : _data.Length, item.EngineBuffer.Length);
            if (_data == null) {
                _data = new byte[newSize];
            }
            else if (_data.Length < newSize) {
                Array.Resize(ref _data, newSize);
            }
            foreach (var outputPlugIn in item.OutputPluginList) {
                _outputPlugins.Add(outputPlugIn);
            }
            _instances.Add(item);
            return item;
        }


        public void EndUpdate() {
            if (_data == null) {
                return;
            }
            if (Host.InvokeRequired) {
                Host.Invoke(_methodInvoker, new object[0]);
            }
            else {
                if (_refCount == 0) {
                }
                else if (--_refCount == 0) {
                    var index = 0;
                    while (index < _data.Length) {
                        _data[index] = 0;
                        index++;
                    }
                    try {
                        foreach (var engineBuffer in _instances.Select(context => context.EngineBuffer)) {
                            for (index = 0; index < engineBuffer.Length; index++) {
                                _data[index] = Math.Max(_data[index], engineBuffer[index]);
                            }
                        }
                    }
                    catch (Exception exception) {
                        throw new Exception(string.Format("(Router - Update)\n{0}", exception.Message), exception);
                    }
                    try {
                        lock (_outputPlugins) {
                            foreach (var outputPlugIn in _outputPlugins.Where(outputPlugIn => (outputPlugIn.From != 0) && outputPlugIn.ContextInitialized)) {
                                Array.Copy(_data, outputPlugIn.From - 1, outputPlugIn.Buffer, 0, (outputPlugIn.To - outputPlugIn.From) + 1);
                                var eventDrivenOutputPlugIn = outputPlugIn.PlugIn as IEventDrivenOutputPlugIn;
                                if (eventDrivenOutputPlugIn == null) {
                                    continue;
                                }
                                eventDrivenOutputPlugIn.Event(outputPlugIn.Buffer);
                            }
                        }
                    }
                    catch (Exception exception2) {
                        throw new Exception(string.Format(Resources.RouterOutputError, exception2.Message), exception2);
                    }
                }
            }
        }


        public static PlugInRouter GetInstance() {
            return _instance ?? (_instance = new PlugInRouter());
        }


        public void Shutdown(RouterContext routerContext) {
            try {
                if ((routerContext == null) || !routerContext.Initialized) {
                    return;
                }
                routerContext.Initialized = false;
                if (!_instances.Contains(routerContext)) {
                    return;
                }
                var clearAtEnd = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ClearAtEndOfSequence");
                if (clearAtEnd) {
                    BeginUpdate();
                    Array.Clear(routerContext.EngineBuffer, 0, routerContext.EngineBuffer.Length);
                    EndUpdate();
                }
                lock (_outputPlugins) {
                    foreach (var outputPlugIn in routerContext.OutputPluginList) {
                        outputPlugIn.PlugIn.Shutdown();
                        _outputPlugins.Remove(outputPlugIn);
                    }
                }
                lock (_instances) {
                    _instances.Remove(routerContext);
                }
                if (_instances.Count == 0) {
                    _data = null;
                }
            }
            catch (Exception exception) {
                MessageBox.Show(string.Format(Resources.RouterError, exception.Message, exception.StackTrace), Resources.PluginError,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public void Startup(RouterContext routerContext) {
            var num = 0;
            var name = string.Empty;
            if (routerContext.Initialized) {
                Shutdown(routerContext);
            }
            try {
                routerContext.Initialized = false;
                foreach (var outputPlugIn in routerContext.OutputPluginList) {
                    var eventDrivenOutputPlugIn = outputPlugIn.PlugIn as IEventDrivenOutputPlugIn;
                    if (eventDrivenOutputPlugIn != null) {
                        eventDrivenOutputPlugIn.Initialize(routerContext.ExecutableObject, routerContext.PluginData, outputPlugIn.SetupDataNode);
                    }
                    //else {
                    //    ((IEventlessOutputPlugIn) outputPlugIn.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData,
                    //        outputPlugIn.SetupDataNode, routerContext.TickSource);
                    //}
                }
                var executable = (IExecutable) Host.Communication["CurrentObject"];
                var str2 = string.Empty;
                if (executable != null) {
                    str2 = executable.Key.ToString(CultureInfo.InvariantCulture);
                }
                try {
                    foreach (var outputPlugIn in routerContext.OutputPluginList) {
                        name = outputPlugIn.PlugIn.Name;
                        var plugInData = routerContext.PluginData.GetPlugInData(num.ToString(CultureInfo.InvariantCulture));
                        if (executable != null) {
                            Host.Communication["SetupNode_" + str2] = plugInData;
                        }
                        outputPlugIn.PlugIn.Startup();
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