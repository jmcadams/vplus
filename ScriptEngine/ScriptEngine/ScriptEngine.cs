namespace ScriptEngine
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml;
    using VixenPlus;

    public class ScriptEngine : IEngine, IDisposable
    {
        private XmlDocument m_commDoc = null;
        private ICompile m_compiler = null;
        private IUser m_executionInstance = null;
        private Thread m_executionThread = null;
        private HardwareUpdateDelegate m_hardwareUpdate;
        private bool m_isRunning = false;
        private EventSequence m_sequence = null;

        public event OnEngineError EngineError;

        public event EventHandler EngineStopped;

        public ScriptEngine()
        {
            this.m_compiler = Compiler.CreateCompiler();
        }

        public void Dispose()
        {
            this.Stop();
            GC.SuppressFinalize(this);
        }

        private void DoEngineError(string message)
        {
            if (this.EngineError != null)
            {
                this.EngineError(message, string.Empty);
            }
        }

        private void ExecutionThread()
        {
            try
            {
                this.m_isRunning = true;
                this.m_executionInstance.ScriptStarting();
                this.m_executionInstance.HardwareUpdate = this.m_hardwareUpdate;
                this.m_executionInstance.Sequence = this.m_sequence;
                this.m_executionInstance.GetType().InvokeMember("Start", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, this.m_executionInstance, null);
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                {
                    this.DoEngineError(string.Format("{0}\n{1}\n\n{2}", exception.Message, exception.InnerException.Message, exception.InnerException.StackTrace));
                }
                else
                {
                    this.DoEngineError(string.Format("{0}\n\n{1}", exception.Message, exception.StackTrace));
                }
            }
            finally
            {
                this.m_isRunning = false;
                lock (this.m_executionInstance)
                {
                    this.m_executionInstance.ScriptEnded();
                    this.m_executionInstance.Dispose();
                    this.m_executionInstance = null;
                }
                if (this.EngineStopped != null)
                {
                    this.EngineStopped(this, null);
                }
            }
        }

        ~ScriptEngine()
        {
            this.Dispose();
        }

        public void Initialize(EventSequence sequence)
        {
            this.m_sequence = sequence;
            if (!this.m_compiler.Compile(sequence))
            {
                if (this.m_commDoc != null)
                {
                    XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(Xml.SetAttribute(this.m_commDoc.DocumentElement, "Result", "response", "ERROR"), "Errors");
                    foreach (CompileError error in this.m_compiler.Errors)
                    {
                        XmlNode node = Xml.SetNewValue(emptyNodeAlways, "Error", error.ErrorMessage);
                        Xml.SetAttribute(node, "line", error.Line.ToString());
                        Xml.SetAttribute(node, "filename", error.FileName);
                    }
                }
                throw new CompileException();
            }
            if (this.m_commDoc != null)
            {
                Xml.SetAttribute(this.m_commDoc.DocumentElement, "Result", "response", "OK");
            }
        }

        public void Pause()
        {
        }

        public bool Play()
        {
            try
            {
                Type type = this.m_compiler.CompiledAssembly.GetTypes()[0];
                this.m_executionInstance = (IUser) this.m_compiler.CompiledAssembly.CreateInstance(type.FullName);
                if (this.m_executionInstance == null)
                {
                    throw new Exception("Unable to create an instance of the sequence assembly.");
                }
                this.m_executionThread = new Thread(new ThreadStart(this.ExecutionThread));
                this.m_executionThread.IsBackground = true;
                this.m_executionThread.Start();
                return true;
            }
            catch (Exception exception)
            {
                this.DoEngineError(exception.Message);
                return false;
            }
        }

        public void Stop()
        {
            if (((this.m_executionThread != null) && this.m_executionThread.IsAlive) && this.m_isRunning)
            {
                if (this.m_executionInstance != null)
                {
                    lock (this.m_executionInstance)
                    {
                        this.m_executionInstance.ForceStop();
                        while (this.m_executionInstance.Running)
                        {
                            Thread.Sleep(10);
                        }
                    }
                }
                if (this.m_hardwareUpdate != null)
                {
                    lock (this.m_hardwareUpdate)
                    {
                    }
                }
                this.m_executionThread.Abort();
            }
        }

        public XmlDocument CommDoc
        {
            set
            {
                this.m_commDoc = value;
            }
        }

        public HardwareUpdateDelegate HardwareUpdate
        {
            set
            {
                this.m_hardwareUpdate = value;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.m_isRunning;
            }
        }
    }
}

