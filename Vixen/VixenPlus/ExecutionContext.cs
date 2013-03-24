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
		private Engine8 _asynchronousEngineInstance;
		private Engine8 _synchronousEngineInstance;

		public ExecutionContext()
		{
			OutputPlugInForms = new List<Form>();
			KeyInterceptor = null;
		}

		public ExecutionContext(bool useSynchronous, bool useAsynchronous)
		{
			OutputPlugInForms = new List<Form>();
			KeyInterceptor = null;
			if (useSynchronous)
			{
				Engines.GetInstance();
			}
			if (useAsynchronous)
			{
				new Async();
			}
		}

		public Engine8 AsynchronousEngineInstance
		{
			get { return _asynchronousEngineInstance; }
			set
			{
				_asynchronousEngineInstance = value;
				if (value != null)
				{
					_asynchronousEngineInstance.ProgramEnd += AsynchronousEngineProgramEndHandler;
					_asynchronousEngineInstance.SequenceChange += AsynchronousEngineProgramChangeHandler;
				}
			}
		}

		public Engine8 SynchronousEngineInstance
		{
			get { return _synchronousEngineInstance; }
			set
			{
				_synchronousEngineInstance = value;
				if (value != null)
				{
					_synchronousEngineInstance.ProgramEnd += SynchronousEngineProgramEndHandler;
					_synchronousEngineInstance.SequenceChange += SynchronousEngineProgramChangeHandler;
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