namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    internal class ExecutionContext : IDisposable
    {
        public byte[] AsynchronousEngineBuffer;
        public Form KeyInterceptor;
        public bool LocalRequestor;
        private Async m_asynchronous;
        private Engine8 m_asynchronousEngineInstance;
        private IEngine2 m_synchronous;
        private Engine8 m_synchronousEngineInstance;
        public IExecutable Object;
        public List<Form> OutputPlugInForms;
        public bool SuppressAsynchronousContext;
        public bool SuppressSynchronousContext;
        public XmlDocument SynchronousEngineComm;

        public event ProgramChangeHandler AsynchronousProgramChangeHandler;

        public event ProgramChangeHandler SynchronousProgramChangeHandler;

        public ExecutionContext()
        {
            this.m_synchronous = null;
            this.m_asynchronous = null;
            this.OutputPlugInForms = new List<Form>();
            this.KeyInterceptor = null;
        }

        public ExecutionContext(bool useSynchronous, bool useAsynchronous)
        {
            this.m_synchronous = null;
            this.m_asynchronous = null;
            this.OutputPlugInForms = new List<Form>();
            this.KeyInterceptor = null;
            if (useSynchronous)
            {
                this.m_synchronous = Engines.GetInstance();
            }
            if (useAsynchronous)
            {
                this.m_asynchronous = new Async();
            }
        }

        private void AsynchronousEngineProgramChangeHandler()
        {
            if (this.AsynchronousProgramChangeHandler != null)
            {
                this.AsynchronousProgramChangeHandler(ProgramChange.SequenceChange);
            }
        }

        private void AsynchronousEngineProgramEndHandler()
        {
            if (this.AsynchronousProgramChangeHandler != null)
            {
                this.AsynchronousProgramChangeHandler(ProgramChange.End);
            }
        }

        public void Dispose()
        {
            this.ReleaseAsynchronousEngine();
            this.ReleaseSynchronousEngine();
            this.Object = null;
            GC.SuppressFinalize(this);
        }

        ~ExecutionContext()
        {
            this.Dispose();
        }

        private void ReleaseAsynchronousEngine()
        {
            if (this.AsynchronousEngineInstance != null)
            {
                this.AsynchronousEngineInstance.Stop();
                this.AsynchronousEngineInstance.Dispose();
                this.AsynchronousEngineInstance = null;
            }
        }

        private void ReleaseSynchronousEngine()
        {
            if (this.SynchronousEngineInstance != null)
            {
                this.SynchronousEngineInstance.Stop();
                this.SynchronousEngineInstance.Dispose();
                this.SynchronousEngineInstance = null;
            }
        }

        private void SynchronousEngineProgramChangeHandler()
        {
            if (this.SynchronousProgramChangeHandler != null)
            {
                this.SynchronousProgramChangeHandler(ProgramChange.SequenceChange);
            }
        }

        private void SynchronousEngineProgramEndHandler()
        {
            if (this.SynchronousProgramChangeHandler != null)
            {
                this.SynchronousProgramChangeHandler(ProgramChange.End);
            }
        }

        public Engine8 AsynchronousEngineInstance
        {
            get
            {
                return this.m_asynchronousEngineInstance;
            }
            set
            {
                this.m_asynchronousEngineInstance = value;
                if (value != null)
                {
                    this.m_asynchronousEngineInstance.ProgramEnd += new Engine8.ProgramEndDelegate(this.AsynchronousEngineProgramEndHandler);
                    this.m_asynchronousEngineInstance.SequenceChange += new Engine8.SequenceChangeDelegate(this.AsynchronousEngineProgramChangeHandler);
                }
            }
        }

        public Engine8 SynchronousEngineInstance
        {
            get
            {
                return this.m_synchronousEngineInstance;
            }
            set
            {
                this.m_synchronousEngineInstance = value;
                if (value != null)
                {
                    this.m_synchronousEngineInstance.ProgramEnd += new Engine8.ProgramEndDelegate(this.SynchronousEngineProgramEndHandler);
                    this.m_synchronousEngineInstance.SequenceChange += new Engine8.SequenceChangeDelegate(this.SynchronousEngineProgramChangeHandler);
                }
            }
        }
    }
}

