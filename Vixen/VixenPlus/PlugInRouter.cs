using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Vixen
{
	internal class PlugInRouter
	{
		private static PlugInRouter m_instance;
		private readonly List<RouterContext> m_instances = new List<RouterContext>();
		private readonly List<MappedOutputPlugIn> m_outputPlugins = new List<MappedOutputPlugIn>();

		private readonly Dictionary<IExecutable, List<InputPlugin>> m_sequenceInputPlugins =
			new Dictionary<IExecutable, List<InputPlugin>>();

		private readonly MethodInvoker m_updateInvoker;
		private byte[] m_data;
		private int m_refCount;
		private bool m_updateLock;

		private PlugInRouter()
		{
			m_updateInvoker = EndUpdate;
		}

		public void BeginUpdate()
		{
			m_refCount++;
		}

		public void CancelUpdate()
		{
			if (m_refCount != 0)
			{
				m_refCount--;
			}
		}

		public RouterContext CreateContext(byte[] engineBuffer, SetupData pluginData, IExecutable executableObject,
		                                   ITickSource tickSource)
		{
			var item = new RouterContext(engineBuffer, pluginData, executableObject, tickSource);
			int newSize = 0;
			newSize = Math.Max((m_data == null) ? 0 : m_data.Length, item.EngineBuffer.Length);
			if (m_data == null)
			{
				m_data = new byte[newSize];
			}
			else if (m_data.Length < newSize)
			{
				Array.Resize(ref m_data, newSize);
			}
			foreach (MappedOutputPlugIn @in in item.OutputPluginList)
			{
				m_outputPlugins.Add(@in);
			}
			m_instances.Add(item);
			return item;
		}

		public void EndUpdate()
		{
			if (m_data != null)
			{
				m_updateLock = true;
				if (Host.InvokeRequired)
				{
					Host.Invoke(m_updateInvoker, new object[0]);
				}
				else
				{
					try
					{
						if (m_refCount == 0)
						{
							m_updateLock = false;
						}
						else if (--m_refCount == 0)
						{
							int index = 0;
							while (index < m_data.Length)
							{
								m_data[index] = 0;
								index++;
							}
							try
							{
								foreach (RouterContext context in m_instances)
								{
									byte[] engineBuffer = context.EngineBuffer;
									for (index = 0; index < engineBuffer.Length; index++)
									{
										m_data[index] = Math.Max(m_data[index], engineBuffer[index]);
									}
								}
							}
							catch (Exception exception)
							{
								m_updateLock = false;
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
								lock (m_outputPlugins)
								{
									foreach (MappedOutputPlugIn @in in m_outputPlugins)
									{
										if (!((@in.From != 0) && @in.ContextInitialized))
										{
											continue;
										}
										Array.Copy(m_data, @in.From - 1, @in.Buffer, 0, (@in.To - @in.From) + 1);
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
												var userData = (long) @in.UserData;
												userData += stopwatch.ElapsedMilliseconds;
												@in.UserData = userData;
											}
										}
									}
								}
							}
							catch (Exception exception2)
							{
								m_updateLock = false;
								throw new Exception(
									string.Format("(Router - plugins)\nAn output plugin caused the following exception: \n{0}", exception2.Message),
									exception2);
							}
							m_updateLock = false;
						}
					}
					finally
					{
						m_updateLock = false;
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
			foreach (InputPlugin plugin in m_sequenceInputPlugins[executableObject])
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
									match = delegate(Channel c) { return c.ID == ulChannelId; };
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
						if (!m_instances.Contains(routerContext))
						{
							m_updateLock = false;
						}
						else
						{
							if (((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ClearAtEndOfSequence"))
							{
								BeginUpdate();
								Array.Clear(routerContext.EngineBuffer, 0, routerContext.EngineBuffer.Length);
								EndUpdate();
							}
							bool flag2 = Host.GetDebugValue("EventAverages") != null;
							int num = 0;
							int num2 = 0;
							if (flag2)
							{
								num = int.Parse(Host.GetDebugValue("TotalEvents"));
							}
							lock (m_outputPlugins)
							{
								foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList)
								{
									@in.PlugIn.Shutdown();
									if (flag2)
									{
										Host.SetDebugValue(string.Format("event_average_{0}", num2++),
										                   string.Format("{0}|{1}|{2}|{3}",
										                                 new object[]
											                                 {@in.PlugIn.Name, @in.From, @in.To, (((long) @in.UserData))/((float) num)}));
									}
									m_outputPlugins.Remove(@in);
								}
							}
							if (m_sequenceInputPlugins.ContainsKey(routerContext.ExecutableObject))
							{
								lock (m_sequenceInputPlugins)
								{
									foreach (InputPlugin plugin in m_sequenceInputPlugins[routerContext.ExecutableObject])
									{
										plugin.ShutdownInternal();
									}
									m_sequenceInputPlugins.Remove(routerContext.ExecutableObject);
								}
							}
							lock (m_instances)
							{
								m_instances.Remove(routerContext);
							}
							if (m_instances.Count == 0)
							{
								m_data = null;
							}
						}
					}
					finally
					{
						m_updateLock = false;
					}
				}
			}
			catch (Exception exception)
			{
				m_updateLock = false;
				MessageBox.Show(
					string.Format(
						"(Router)\nAn output plugin caused the following exception: \n{0}\n\nExecution has been stopped.\n\n{1}",
						exception.Message, exception.StackTrace), "Plugin error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		public void Startup(RouterContext routerContext)
		{
			int num = 0;
			string name = string.Empty;
			if (routerContext.Initialized)
			{
				Shutdown(routerContext);
			}
			try
			{
				List<InputPlugin> list;
				routerContext.Initialized = false;
				foreach (MappedOutputPlugIn @in in routerContext.OutputPluginList)
				{
					if (@in.PlugIn is IEventDrivenOutputPlugIn)
					{
						((IEventDrivenOutputPlugIn) @in.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData,
						                                                   @in.SetupDataNode);
					}
					else
					{
						((IEventlessOutputPlugIn) @in.PlugIn).Initialize(routerContext.ExecutableObject, routerContext.PluginData,
						                                                 @in.SetupDataNode, routerContext.TickSource);
					}
				}
				m_sequenceInputPlugins[routerContext.ExecutableObject] = list = new List<InputPlugin>();
				foreach (XmlNode node2 in routerContext.PluginData.GetAllPluginData(SetupData.PluginType.Input, true))
				{
					var item = (InputPlugin) InputPlugins.FindPlugin(node2.Attributes["name"].Value, true);
					item.InitializeInternal(routerContext.PluginData, node2);
					item.SetupDataToPlugin();
					list.Add(item);
				}
				var executable = (IExecutable) Host.Communication["CurrentObject"];
				string str2 = string.Empty;
				if (executable != null)
				{
					str2 = executable.Key.ToString();
				}
				try
				{
					foreach (InputPlugin plugin2 in m_sequenceInputPlugins[routerContext.ExecutableObject])
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
				MessageBox.Show(string.Format("{0}:\n\n{1}", name, exception2.Message), Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
		}

		private void WaitForLock()
		{
			while (m_updateLock)
			{
				Application.DoEvents();
				Thread.Sleep(10);
			}
			m_updateLock = true;
		}
	}
}