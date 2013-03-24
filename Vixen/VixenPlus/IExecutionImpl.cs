using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	internal class ExecutionImpl : IExecution, IQueryable
	{
		private readonly string _errorLog;
		private readonly Host _host;
		private readonly Preference2 _preferences;
		private readonly Dictionary<int, ExecutionContext> _registeredContexts;

		public ExecutionImpl(Host host)
		{
			_host = host;
			_registeredContexts = new Dictionary<int, ExecutionContext>();
			_preferences = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences;
			_errorLog = Path.Combine(Paths.DataPath, "iexecution.err");
		}

		public int EngineStatus(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			int num = 0;
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			if (context.SynchronousEngineInstance.IsPaused)
			{
				num = 2;
			}
			else if (context.SynchronousEngineInstance.IsRunning)
			{
				num = 1;
			}
			return num;
		}

		public int EngineStatus(int contextHandle, out int position)
		{
			position = 0;
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			int num = 0;
			position = 0;
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			if (context.SynchronousEngineInstance.IsPaused)
			{
				num = 2;
				position = context.SynchronousEngineInstance.Position;
			}
			else if (context.SynchronousEngineInstance.IsRunning)
			{
				num = 1;
				position = context.SynchronousEngineInstance.Position;
			}
			return num;
		}

		public bool ExecuteChannelOff(int contextHandle, int channelIndex)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (context.AsynchronousEngineBuffer == null)
			{
				return false;
			}
			if ((channelIndex < -1) || (channelIndex >= context.AsynchronousEngineBuffer.Length))
			{
				return false;
			}
			if (!AsynchronousAccessSanityCheck(channelIndex, context))
			{
				return false;
			}
			if (channelIndex != -1)
			{
				context.AsynchronousEngineBuffer[channelIndex] = 0;
			}
			else
			{
				for (int i = 0; i < context.AsynchronousEngineBuffer.Length; i++)
				{
					context.AsynchronousEngineBuffer[i] = 0;
				}
			}
			try
			{
				context.AsynchronousEngineInstance.HardwareUpdate(context.AsynchronousEngineBuffer);
			}
			catch (Exception exception)
			{
				LogError("ExecuteChannelOff", exception);
			}
			return true;
		}

		public bool ExecuteChannelOn(int contextHandle, int channelIndex)
		{
			return ExecuteChannelOn(contextHandle, channelIndex, 100);
		}

		public bool ExecuteChannelOn(int contextHandle, int channelIndex, int percentLevel)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (context.AsynchronousEngineBuffer == null)
			{
				return false;
			}
			if ((channelIndex < -1) || (channelIndex >= context.AsynchronousEngineBuffer.Length))
			{
				return false;
			}
			if (!AsynchronousAccessSanityCheck(channelIndex, context))
			{
				return false;
			}
			if (channelIndex != -1)
			{
				context.AsynchronousEngineBuffer[channelIndex] = (byte) ((percentLevel*0xff)/100);
			}
			else
			{
				for (int i = 0; i < context.AsynchronousEngineBuffer.Length; i++)
				{
					context.AsynchronousEngineBuffer[i] = (byte) ((percentLevel*0xff)/100);
				}
			}
			try
			{
				context.AsynchronousEngineInstance.HardwareUpdate(context.AsynchronousEngineBuffer);
			}
			catch (Exception exception)
			{
				LogError("ExecuteChannelOn", exception);
			}
			return true;
		}

		public bool ExecuteChannelToggle(int contextHandle, int channelIndex)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (context.AsynchronousEngineBuffer == null)
			{
				return false;
			}
			if ((channelIndex < -1) || (channelIndex >= context.AsynchronousEngineBuffer.Length))
			{
				return false;
			}
			if (!AsynchronousAccessSanityCheck(channelIndex, context))
			{
				return false;
			}
			if (channelIndex != -1)
			{
				context.AsynchronousEngineBuffer[channelIndex] = (context.AsynchronousEngineBuffer[channelIndex] > 0)
					                                                 ? ((byte) 0)
					                                                 : ((byte) 0xff);
			}
			else
			{
				for (int i = 0; i < context.AsynchronousEngineBuffer.Length; i++)
				{
					context.AsynchronousEngineBuffer[i] = (context.AsynchronousEngineBuffer[i] > 0) ? ((byte) 0) : ((byte) 0xff);
				}
			}
			try
			{
				context.AsynchronousEngineInstance.HardwareUpdate(context.AsynchronousEngineBuffer);
			}
			catch (Exception exception)
			{
				LogError("ExecuteChannelToggle", exception);
			}
			return true;
		}

		public bool ExecutePause(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed))
			{
				return false;
			}
			if (context.SynchronousEngineComm != null)
			{
				Xml.SetAttribute(context.SynchronousEngineComm.DocumentElement, "Result", "response", string.Empty);
			}
			try
			{
				context.SynchronousEngineInstance.Pause();
			}
			catch (Exception exception)
			{
				LogError("ExecutePause", exception);
				return false;
			}
			return true;
		}

		public bool ExecutePlay(int contextHandle)
		{
			return ExecutePlay(contextHandle, _preferences.GetBoolean("LogAudioManual"));
		}

		public bool ExecutePlay(int contextHandle, bool logAudio)
		{
			return ExecutePlay(contextHandle, 0, 0, logAudio);
		}

		public bool ExecutePlay(int contextHandle, int millisecondStart, int millisecondCount)
		{
			return ExecutePlay(contextHandle, millisecondStart, millisecondCount, _preferences.GetBoolean("LogAudioManual"));
		}

		public bool ExecutePlay(int contextHandle, int startMillisecond, int endMillisecond, bool logAudio)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed))
			{
				return false;
			}
			if (context.SynchronousEngineInstance.IsRunning)
			{
				return false;
			}
			if (context.SynchronousEngineComm != null)
			{
				Xml.SetAttribute(context.SynchronousEngineComm.DocumentElement, "Result", "response", string.Empty);
			}
			if (!context.SynchronousEngineInstance.IsPaused)
			{
				try
				{
					context.SynchronousEngineInstance.Initialize(context.Object);
				}
				catch (Exception exception)
				{
					LogError("ExecutePlay", exception);
					return false;
				}
			}
			string str = context.SynchronousEngineInstance.CurrentObject.Key.ToString(CultureInfo.InvariantCulture);
			Host.Communication["KeyInterceptor_" + str] = context.KeyInterceptor;
			Host.Communication["ExecutionContext_" + str] = context;
			bool flag = context.SynchronousEngineInstance.Play(startMillisecond, endMillisecond, logAudio);
			foreach (Form form in context.OutputPlugInForms)
			{
				form.BringToFront();
			}
			return flag;
		}

		public bool ExecuteStop(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return false;
			}
			if (!(((context.SynchronousEngineInstance != null) && (context.Object != null)) && context.Object.CanBePlayed))
			{
				return false;
			}
			if (context.SynchronousEngineComm != null)
			{
				Xml.SetAttribute(context.SynchronousEngineComm.DocumentElement, "Result", "response", string.Empty);
			}
			try
			{
				context.SynchronousEngineInstance.Stop();
				if (_preferences.GetBoolean("SavePlugInDialogPositions"))
				{
					foreach (OutputPlugInUIBase base2 in context.OutputPlugInForms)
					{
						if (base2.WindowState == FormWindowState.Normal)
						{
							XmlNode nodeAlways = Xml.GetNodeAlways(Xml.GetNodeAlways(base2.DataNode, "DialogPositions"), base2.Name);
							Xml.SetAttribute(nodeAlways, "x", base2.Location.X.ToString(CultureInfo.InvariantCulture));
							Xml.SetAttribute(nodeAlways, "y", base2.Location.Y.ToString(CultureInfo.InvariantCulture));
						}
					}
				}
				context.OutputPlugInForms.Clear();
				GC.Collect();
			}
			catch (Exception exception)
			{
				LogError("ExecuteStop", exception);
				return false;
			}
			return true;
		}

		public int FindExecutionContextHandle(object uniqueReference)
		{
			if (uniqueReference is IOutputPlugIn)
			{
				return FindOutputPlugIn(uniqueReference);
			}
			if (uniqueReference is IExecutable)
			{
				return FindExecutableObject(uniqueReference);
			}
			if (uniqueReference is Engine8)
			{
				return FindEngine(uniqueReference);
			}
			if (uniqueReference is OutputPlugInUIBase)
			{
				return FindOutputPlugInUI(uniqueReference);
			}
			return 0;
		}

		public float GetAudioSpeed(int contextHandle)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null))
			{
				return context.SynchronousEngineInstance.AudioSpeed;
			}
			return 0f;
		}

		public int GetCurrentPosition(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			return context.SynchronousEngineInstance.Position;
		}

		public IExecutable GetObjectInContext(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return null;
			}
			return context.Object;
		}

		public int GetObjectPosition(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			return context.SynchronousEngineInstance.ObjectPosition;
		}

		public string LoadedProgram(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return string.Empty;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return string.Empty;
			}
			return context.SynchronousEngineInstance.LoadedProgram;
		}

		public string LoadedSequence(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return string.Empty;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return string.Empty;
			}
			return context.SynchronousEngineInstance.LoadedSequence;
		}

		public int ProgramLength(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			return context.SynchronousEngineInstance.LoadedProgramLength;
		}

		public void ReleaseContext(int contextHandle)
		{
			if (_registeredContexts.ContainsKey(contextHandle))
			{
				try
				{
					_registeredContexts[contextHandle].Dispose();
					_registeredContexts.Remove(contextHandle);
				}
				catch (Exception exception)
				{
					LogError("ReleaseContext", exception);
				}
			}
		}

		public int RequestContext(bool suppressAsynchronousContext, bool suppressSynchronousContext, Form keyInterceptor)
		{
			try
			{
				int num = ((int) DateTime.Now.ToBinary()) + _registeredContexts.Count;
				var context = new ExecutionContext
					{
						SuppressAsynchronousContext = suppressAsynchronousContext,
						SuppressSynchronousContext = suppressSynchronousContext
					};
				int integer = _preferences.GetInteger("SoundDevice");
				context.SynchronousEngineInstance = context.SuppressSynchronousContext ? null : new Engine8(_host, integer);
				context.AsynchronousEngineInstance = context.SuppressAsynchronousContext
					                                     ? null
					                                     : new Engine8(Engine8.EngineMode.Asynchronous, _host, integer);
				context.LocalRequestor = RequestorIsLocal(new StackTrace().GetFrame(1));
				context.Object = null;
				context.KeyInterceptor = keyInterceptor;
				if (!context.SuppressAsynchronousContext)
				{
					byte[][] mask;
					SetupData plugInData;
					string str = _preferences.GetString("AsynchronousData");
					string str2 = ((str != "Default") && (str != "Sync")) ? str : _preferences.GetString("DefaultProfile");
					Profile profile = null;
					if (str2.Length > 0)
					{
						try
						{
							profile = new Profile(Path.Combine(Paths.ProfilePath, str2 + ".pro"));
							profile.Freeze();
						}
						catch
						{
							LogError("RequestContext", "Error loading profile " + str2);
						}
					}
					if (profile == null)
					{
						if (str == "Default")
						{
							LogError("RequestContext",
							         "Preference set to use default profile for asynchronous execution, but no default profile exists.");
						}
						mask = null;
						plugInData = null;
					}
					else
					{
						mask = profile.Mask;
						plugInData = profile.PlugInData;
						context.Object = profile;
					}
					if (context.Object != null)
					{
						context.Object.Mask = mask;
						if (plugInData != null)
						{
							context.Object.PlugInData.ReplaceRoot(plugInData.RootNode);
						}
						AsyncInit(context);
					}
				}
				_registeredContexts[num] = context;
				return num;
			}
			catch (Exception exception)
			{
				LogError("RequestContext", exception);
				return 0;
			}
		}

		public int RequestContext(bool suppressAsynchronousContext, bool suppressSynchronousContext, Form keyInterceptor,
		                          ref XmlDocument syncEngineCommDoc)
		{
			int num = RequestContext(suppressAsynchronousContext, suppressSynchronousContext, keyInterceptor);
			if (num != 0)
			{
				ExecutionContext context = _registeredContexts[num];
				context.SynchronousEngineComm =
					syncEngineCommDoc = context.SynchronousEngineInstance.CommDoc = Xml.CreateXmlDocument("Engine");
			}
			return num;
		}

		public int SequenceLength(int contextHandle)
		{
			ExecutionContext context;
			if (!_registeredContexts.TryGetValue(contextHandle, out context))
			{
				return 0;
			}
			if (context.SynchronousEngineInstance == null)
			{
				return 0;
			}
			return context.SynchronousEngineInstance.LoadedSequenceLength;
		}

		public void SetAsynchronousContext(int contextHandle, IExecutable executableObject)
		{
			if (contextHandle != 0)
			{
				ExecutionContext context = _registeredContexts[contextHandle];
				if (!context.SuppressAsynchronousContext)
				{
					if (context.Object != null)
					{
						context.AsynchronousEngineInstance.Stop();
					}
					try
					{
						context.Object = executableObject;
						AsyncInit(context);
					}
					catch (Exception exception)
					{
						LogError("SetAsynchronousContext", exception);
					}
				}
			}
		}

		public void SetAsynchronousProgramChangeHandler(int contextHandle, ProgramChangeHandler programChangeHandler)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context))
			{
				context.AsynchronousProgramChangeHandler += programChangeHandler;
			}
		}

		public void SetAudioSpeed(int contextHandle, float rate)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null))
			{
				context.SynchronousEngineInstance.AudioSpeed = rate;
			}
		}

		public void SetChannelStates(int contextHandle, byte[] channelValues)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context))
			{
				if (context.SynchronousEngineInstance != null)
				{
					context.SynchronousEngineInstance.HardwareUpdate(channelValues);
				}
				if (context.AsynchronousEngineInstance != null)
				{
					context.AsynchronousEngineInstance.HardwareUpdate(channelValues);
				}
			}
		}

		public void SetLoopState(int contextHandle, bool state)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context) && (context.SynchronousEngineInstance != null))
			{
				context.SynchronousEngineInstance.IsLooping = state;
			}
		}

		public void SetSynchronousContext(int contextHandle, IExecutable executableObject)
		{
			if (contextHandle != 0)
			{
				ExecutionContext context = _registeredContexts[contextHandle];
				if (!context.SuppressSynchronousContext)
				{
					try
					{
						byte[][] mask = executableObject.Mask;
						SetupData plugInData = executableObject.PlugInData;
						if (!context.LocalRequestor && !executableObject.TreatAsLocal)
						{
							Profile profile;
							string str = _preferences.GetString("SynchronousData");
							if (str == "Default")
							{
								string str2 = _preferences.GetString("DefaultProfile");
								if (str2 == string.Empty)
								{
									LogError("SetSynchronousContext",
									         "Preference set to use default profile for synchronous execution, but no default profile set.");
									mask = null;
									plugInData = null;
								}
								else
								{
									profile = new Profile(Path.Combine(Paths.ProfilePath, str2 + ".pro"));
									profile.Freeze();
									mask = profile.Mask;
									plugInData = profile.PlugInData;
								}
							}
							else if (str != "Embedded")
							{
								profile = new Profile(Path.Combine(Paths.ProfilePath, str + ".pro"));
								profile.Freeze();
								mask = profile.Mask;
								plugInData = profile.PlugInData;
							}
						}
						context.Object = executableObject;
						context.Object.Mask = mask;
						if (plugInData != null)
						{
							context.Object.PlugInData.ReplaceRoot(plugInData.RootNode);
						}
						if (!(context.SuppressAsynchronousContext || _preferences.GetString("AsynchronousData") != "Sync"))
						{
							AsyncInit(context);
						}
						if (executableObject.AudioDeviceIndex != -1)
						{
							context.SynchronousEngineInstance.SetAudioDevice(executableObject.AudioDeviceIndex);
						}
						if (executableObject.AudioDeviceVolume != 0)
						{
							context.SynchronousEngineInstance.SetAudioDevice(executableObject.AudioDeviceIndex);
						}
					}
					catch (Exception exception)
					{
						LogError("SetSynchronousContext", exception);
					}
				}
			}
		}

		public void SetSynchronousProgramChangeHandler(int contextHandle, ProgramChangeHandler programChangeHandler)
		{
			ExecutionContext context;
			if (_registeredContexts.TryGetValue(contextHandle, out context))
			{
				context.SynchronousProgramChangeHandler += programChangeHandler;
			}
		}

		public string QueryInstance(int index)
		{
			var builder = new StringBuilder();
			if ((index >= 0) && (index < _registeredContexts.Count))
			{
				var array = new int[_registeredContexts.Count];
				_registeredContexts.Keys.CopyTo(array, 0);
				ExecutionContext context = _registeredContexts[array[index]];
				builder.AppendLine("Handle: " + array[index]);
				builder.AppendLine("Asynchronous suppressed: " + context.SuppressAsynchronousContext);
				builder.AppendLine("Synchronous suppressed: " + context.SuppressSynchronousContext);
				builder.AppendLine("Form count: " + context.OutputPlugInForms.Count);
				builder.AppendLine("Form captions:");
				foreach (Form form in context.OutputPlugInForms)
				{
					builder.AppendLine("   " + form.Text);
				}
				builder.AppendLine("Local requestor: " + context.LocalRequestor);
				builder.AppendLine("Key interceptor: " + ((context.KeyInterceptor != null) ? context.KeyInterceptor.Text : "(null)"));
				builder.AppendLine("Object: " + ((context.Object != null) ? context.Object.Name : "(null)"));
			}
			return builder.ToString();
		}

		public int Count
		{
			get { return _registeredContexts.Count; }
		}

		private bool AsynchronousAccessSanityCheck(int channelIndex, ExecutionContext context)
		{
			if (context.SuppressAsynchronousContext)
			{
				return false;
			}
			if (context.AsynchronousEngineInstance == null)
			{
				return false;
			}
			if (channelIndex >= context.Object.Channels.Count)
			{
				return false;
			}
			return true;
		}

		private void AsyncInit(ExecutionContext context)
		{
			context.AsynchronousEngineInstance.Initialize(context.Object);
			context.AsynchronousEngineBuffer = new byte[context.Object.Channels.Count];
		}

		private int FindEngine(object uniqueReference)
		{
			foreach (int num in _registeredContexts.Keys)
			{
				ExecutionContext context = _registeredContexts[num];
				if (context.AsynchronousEngineInstance == uniqueReference)
				{
					return num;
				}
				if (context.SynchronousEngineInstance == uniqueReference)
				{
					return num;
				}
			}
			return 0;
		}

		private int FindExecutableObject(object uniqueReference)
		{
			foreach (int num in _registeredContexts.Keys)
			{
				if (_registeredContexts[num].Object == uniqueReference)
				{
					return num;
				}
			}
			return 0;
		}

		private int FindOutputPlugIn(object uniqueReference)
		{
			foreach (int num in _registeredContexts.Keys)
			{
				ExecutionContext context = _registeredContexts[num];
				if ((context.AsynchronousEngineInstance != null) && context.AsynchronousEngineInstance.UsesObject(uniqueReference))
				{
					return num;
				}
				if ((context.SynchronousEngineInstance != null) && context.SynchronousEngineInstance.UsesObject(uniqueReference))
				{
					return num;
				}
			}
			return 0;
		}

		private int FindOutputPlugInUI(object uniqueReference)
		{
			foreach (int num in _registeredContexts.Keys)
			{
				foreach (Form form in _registeredContexts[num].OutputPlugInForms)
				{
					if (form == uniqueReference)
					{
						return num;
					}
				}
			}
			return 0;
		}

		private void LogError(string errorSource, Exception e)
		{
			using (var writer = new StreamWriter(_errorLog, true))
			{
				try
				{
					writer.WriteLine("\n{0} {1}: {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), errorSource, e.Message);
					writer.WriteLine(e.StackTrace);
				}
				finally
				{
					writer.Close();
				}
			}
		}

		private void LogError(string errorSource, string message)
		{
			using (var writer = new StreamWriter(_errorLog, true))
			{
				try
				{
					writer.WriteLine("\n{0} {1}: {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), errorSource, message);
				}
				finally
				{
					writer.Close();
				}
			}
		}

		private bool RequestorIsLocal(StackFrame requestorFrame)
		{
			Assembly assembly = requestorFrame.GetMethod().Module.Assembly;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			using (Process process = Process.GetCurrentProcess())
			{
				if (requestorFrame.GetMethod().Module.Name == process.MainModule.ModuleName)
				{
					return true;
				}
			}
			foreach (Type type in assembly.GetExportedTypes())
			{
				if (type.BaseType != null)
				{
					flag |= type.BaseType.Name == "UIBase";
				}
				foreach (Type type2 in type.GetInterfaces())
				{
					flag3 |= type2.Name == "IVixenMDI";
					flag2 |= type2.Name == "IAddIn";
				}
			}
			return ((flag && flag3) && !flag2);
		}
	}
}