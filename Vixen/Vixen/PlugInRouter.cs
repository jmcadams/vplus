namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    internal class PlugInRouter
    {
        private byte[] m_data;
        private static PlugInRouter m_instance = null;
        private List<RouterContext> m_instances = new List<RouterContext>();
        private List<MappedOutputPlugIn> m_outputPlugins = new List<MappedOutputPlugIn>();
        private int m_refCount = 0;
        private Dictionary<IExecutable, List<InputPlugin>> m_sequenceInputPlugins = new Dictionary<IExecutable, List<InputPlugin>>();
        private MethodInvoker m_updateInvoker;
        private bool m_updateLock = false;

        private PlugInRouter()
        {
            this.m_updateInvoker = new MethodInvoker(this.EndUpdate);
        }

        public void BeginUpdate()
        {
            this.m_refCount++;
        }

        public void CancelUpdate()
        {
            if (this.m_refCount != 0)
            {
                this.m_refCount--;
            }
        }

        public RouterContext CreateContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject, ITickSource tickSource)
        {
            RouterContext item = new RouterContext(engineBuffer, pluginData, executableObject, tickSource);
            int newSize = 0;
            newSize = Math.Max((this.m_data == null) ? 0 : this.m_data.Length, item.EngineBuffer.Length);
            if (this.m_data == null)
            {
                this.m_data = new byte[newSize];
            }
            else if (this.m_data.Length < newSize)
            {
                Array.Resize<byte>(ref this.m_data, newSize);
            }
            foreach (MappedOutputPlugIn @in in item.OutputPluginList)
            {
                this.m_outputPlugins.Add(@in);
            }
            this.m_instances.Add(item);
            return item;
        }

        public void EndUpdate()
        {
            if (this.m_data != null)
            {
                this.m_updateLock = true;
                if (Host.InvokeRequired)
                {
                    Host.Invoke(this.m_updateInvoker, new object[0]);
                }
                else
                {
                    try
                    {
                        if (this.m_refCount == 0)
                        {
                            this.m_updateLock = false;
                        }
                        else if (--this.m_refCount == 0)
                        {
                            int index = 0;
                            while (index < this.m_data.Length)
                            {
                                this.m_data[index] = 0;
                                index++;
                            }
                            try
                            {
                                foreach (RouterContext context in this.m_instances)
                                {
                                    byte[] engineBuffer = context.EngineBuffer;
                                    for (index = 0; index < engineBuffer.Length; index++)
                                    {
                                        this.m_data[index] = Math.Max(this.m_data[index], engineBuffer[index]);
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                this.m_updateLock = false;
                                throw new Exception(string.Format("(Router - Update)\n{0}", exception.Message), exception);
                            }
                            bool flag = Host.GetDebugValue("EventAverages") != null;
                            Stopwatch stopwatch = null;
                            if (flag)
                            {
                                stopwatch = new Stopwatch();
                                Host.SetDebugValue("TotalEvents", (int.Parse(Host.GetDebugValue("TotalEvents")) + 1).ToString());
                            }
                            try
                            {
                                lock (this.m_outputPlugins)
                                {
                                    foreach (MappedOutputPlugIn @in in this.m_outputPlugins)
                                    {
                                        if (!((@in.From != 0) && @in.ContextInitialized))
                                        {
                                            continue;
                                        }
                                        Array.Copy(this.m_data, @in.From - 1, @in.Buffer, 0, (@in.To - @in.From) + 1);
                                        if (@in.PlugIn is IEventDrivenOutputPlugIn)
                                        {
                                            if (flag)
                                            {
                                                stopwatch.Reset();
                                                stopwatch.Start();
                                            }
                                            ((IEventDrivenOutputPlugIn) @in.PlugIn).Event(@in.Buffer);
                                            if (flag)
                                            {
                                                stopwatch.Stop();
                                                long userData = (long) @in.UserData;
                                                userData += stopwatch.ElapsedMilliseconds;
                                                @in.UserData = userData;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                this.m_updateLock = false;
                                throw new Exception(string.Format("(Router - plugins)\nAn output plugin caused the following exception: \n{0}", exception2.Message), exception2);
                            }
                            this.m_updateLock = false;
                        }
                    }
                    finally
                    {
                        this.m_updateLock = false;
                    }
                }
            }
        }

        public static PlugInRouter GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new PlugInRouter();
            }
            return m_instance;
        }

        public bool GetSequenceInputs(IExecutable executableObject, byte[] eventBuffer, bool forLiveUpdate, bool forRecord)
        {
            bool flag = false;
            foreach (InputPlugin plugin in this.m_sequenceInputPlugins[executableObject])
            {
                if (((forLiveUpdate == plugin.LiveUpdate) && forLiveUpdate) || ((forRecord == plugin.Record) && forRecord))
                {
                    Predicate<Channel> match = null;
                    ulong ulChannelId;
                    foreach (Input input in plugin.Inputs)
                    {
                        if (input.Enabled && input.GetChangedInternal())
                        {
                            flag = true;
                            byte valueInternal = input.GetValueInternal();
                            foreach (string str in plugin.MappingSets.CurrentMappingSet.GetOutputChannelIdList(input))
                            {
                                ulChannelId = ulong.Parse(str);
                                if (match == null)
                                {
                                    match = delegate (Channel c) {
                                        return c.ID == ulChannelId;
                                    };
                                }
                                int outputChannel = executableObject.Channels.Find(match).OutputChannel;
                                eventBuffer[outputChannel] = valueInternal;
                            }
                        }
                    }
                }
            }
            return flag;
        }

        public void Shutdown(RouterContext routerContext)
        {
            try
            {
                if ((routerContext != null) && routerContext.Initialized)
                {
                    try
                    {
                        routerContext.Initialized = false;
                        if (!this.m_instances.Contains(routerContext))
                        {
                            this.m_updateLock = false;
                        }
                        else
                        {
                            if (((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ClearAtEndOfSequence"))
                            {
                                this.BeginUpdate();
                                Array.Clear(routerContext.EngineBuffer, 0, routerContext.EngineBuffer.Length);
                                this.EndUpdate();
                            }
                            bool flag2 = Host.GetDebugValue("EventAverages") != null;
                            int num = 0;
                            int num2 = 0;
                            if (flag2)
                            {
                                num = int.Parse(Host.GetDebugValue("TotalEvents"));
                            }
                            lock (this.m_outputPlugins)
                            {
                                foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList)
                                {
                                    @in.PlugIn.Shutdown();
                                    if (flag2)
                                    {
                                        Host.SetDebugValue(string.Format("event_average_{0}", num2++), string.Format("{0}|{1}|{2}|{3}", new object[] { @in.PlugIn.Name, @in.From, @in.To, ((float) ((long) @in.UserData)) / ((float) num) }));
                                    }
                                    this.m_outputPlugins.Remove(@in);
                                }
                            }
                            if (this.m_sequenceInputPlugins.ContainsKey(routerContext.ExecutableObject))
                            {
                                lock (this.m_sequenceInputPlugins)
                                {
                                    foreach (InputPlugin plugin in this.m_sequenceInputPlugins[routerContext.ExecutableObject])
                                    {
                                        plugin.ShutdownInternal();
                                    }
                                    this.m_sequenceInputPlugins.Remove(routerContext.ExecutableObject);
                                }
                            }
                            lock (this.m_instances)
                            {
                                this.m_instances.Remove(routerContext);
                            }
                            if (this.m_instances.Count == 0)
                            {
                                this.m_data = null;
                            }
                        }
                    }
                    finally
                    {
                        this.m_updateLock = false;
                    }
                }
            }
            catch (Exception exception)
            {
                this.m_updateLock = false;
                MessageBox.Show(string.Format("(Router)\nAn output plugin caused the following exception: \n{0}\n\nExecution has been stopped.\n\n{1}", exception.Message, exception.StackTrace), "Plugin error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Startup(RouterContext routerContext)
        {
            int num = 0;
            string name = string.Empty;
            if (routerContext.Initialized)
            {
                this.Shutdown(routerContext);
            }
            try
            {
                List<InputPlugin> list;
                routerContext.Initialized = false;
                foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList)
                {
                    if (@in.PlugIn is IEventDrivenOutputPlugIn)
                    {
                        ((IEventDrivenOutputPlugIn) @in.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData, @in.SetupDataNode);
                    }
                    else
                    {
                        ((IEventlessOutputPlugIn) @in.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData, @in.SetupDataNode, routerContext.TickSource);
                    }
                }
                this.m_sequenceInputPlugins[routerContext.ExecutableObject] = list = new List<InputPlugin>();
                foreach (XmlNode node2 in routerContext.PluginData.GetAllPluginData(SetupData.PluginType.Input, true))
                {
                    InputPlugin item = (InputPlugin) InputPlugins.FindPlugin(node2.Attributes["name"].Value, true);
                    item.InitializeInternal(routerContext.PluginData, node2);
                    item.SetupDataToPlugin();
                    list.Add(item);
                }
                IExecutable executable = (IExecutable) Host.Communication["CurrentObject"];
                string str2 = string.Empty;
                if (executable != null)
                {
                    str2 = executable.Key.ToString();
                }
                try
                {
                    foreach (InputPlugin plugin2 in this.m_sequenceInputPlugins[routerContext.ExecutableObject])
                    {
                        name = plugin2.Name;
                        plugin2.StartupInternal();
                    }
                    foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList)
                    {
                        name = @in.PlugIn.Name;
                        XmlNode plugInData = routerContext.PluginData.GetPlugInData(num.ToString());
                        if (executable != null)
                        {
                            Host.Communication["SetupNode_" + str2] = plugInData;
                        }
                        @in.PlugIn.Startup();
                        if (Host.GetDebugValue("EventAverages") != null)
                        {
                            @in.UserData = 0L;
                            Host.SetDebugValue("TotalEvents", "0");
                        }
                        Host.Communication.Remove("SetupNode_" + str2);
                        num++;
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("(Router - Startup)\n{0}", exception.Message), exception);
                }
                routerContext.Initialized = true;
            }
            catch (Exception exception2)
            {
                MessageBox.Show(string.Format("{0}:\n\n{1}", name, exception2.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void WaitForLock()
        {
            while (this.m_updateLock)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
            this.m_updateLock = true;
        }
    }
}

