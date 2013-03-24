using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace VixenPlus
{
	internal class ExecutionContext : IDisposable
	{
		public byte[] AsynchronousEngineBuffer;
		public Form KeyInterceptor;
		public bool LocalRequestor;
		public IExecutable Object;
		public List<Form> OutputPlugInForms;
		public bool SuppressAsynchronousContext;
		public bool SuppressSynchronousContext;
		public XmlDocument SynchronousEngineComm;
		private Async m_asynchronous;
		private Engine8 m_asynchronousEngineInstance;
		private IEngine2 m_synchronous;
		private Engine8 m_synchronousEngineInstance;

		public ExecutionContext()
		{
			m_synchronous = null;
			m_asynchronous = null;
			OutputPlugInForms = new List<Form>();
			KeyInterceptor = null;
		}

		public ExecutionContext(bool useSynchronous, bool useAsynchronous)
		{
			m_synchronous = null;
			m_asynchronous = null;
			OutputPlugInForms = new List<Form>();
			KeyInterceptor = null;
			if (useSynchronous)
			{
				m_synchronous = Engines.GetInstance();
			}
			if (useAsynchronous)
			{
				m_asynchronous = new Async();
			}
		}

		public Engine8 AsynchronousEngineInstance
		{
			get { return m_asynchronousEngineInstance; }
			set
			{
				m_asynchronousEngineInstance = value;
				if (value != null)
				{
					m_asynchronousEngineInstance.ProgramEnd += AsynchronousEngineProgramEndHandler;
					m_asynchronousEngineInstance.SequenceChange += AsynchronousEngineProgramChangeHandler;
				}
			}
		}

		public Engine8 SynchronousEngineInstance
		{
			get { return m_synchronousEngineInstance; }
			set
			{
				m_synchronousEngineInstance = value;
				if (value != null)
				{
					m_synchronousEngineInstance.ProgramEnd += SynchronousEngineProgramEndHandler;
					m_synchronousEngineInstance.SequenceChange += SynchronousEngineProgramChangeHandler;
				}
			}
		}

		public void Dispose()
		{
			ReleaseAsynchronousEngine();
			ReleaseSynchronousEngine();
			Object = null;
			GC.SuppressFinalize(this);
		}

		public event ProgramChangeHandler AsynchronousProgramChangeHandler;

		public event ProgramChangeHandler SynchronousProgramChangeHandler;

		private void AsynchronousEngineProgramChangeHandler()
		{
			if (AsynchronousProgramChangeHandler != null)
			{
				AsynchronousProgramChangeHandler(ProgramChange.SequenceChange);
			}
		}

		private void AsynchronousEngineProgramEndHandler()
		{
			if (AsynchronousProgramChangeHandler != null)
			{
				AsynchronousProgramChangeHandler(ProgramChange.End);
			}
		}

		~ExecutionContext()
		{
			Dispose();
		}

		private void ReleaseAsynchronousEngine()
		{
			if (AsynchronousEngineInstance != null)
			{
				AsynchronousEngineInstance.Stop();
				AsynchronousEngineInstance.Dispose();
				AsynchronousEngineInstance = null;
			}
		}

		private void ReleaseSynchronousEngine()
		{
			if (SynchronousEngineInstance != null)
			{
				SynchronousEngineInstance.Stop();
				SynchronousEngineInstance.Dispose();
				SynchronousEngineInstance = null;
			}
		}

		private void SynchronousEngineProgramChangeHandler()
		{
			if (SynchronousProgramChangeHandler != null)
			{
				SynchronousProgramChangeHandler(ProgramChange.SequenceChange);
			}
		}

		private void SynchronousEngineProgramEndHandler()
		{
			if (SynchronousProgramChangeHandler != null)
			{
				SynchronousProgramChangeHandler(ProgramChange.End);
			}
		}
	}
}