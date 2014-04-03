using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VixenPlus {
    internal class ExecutionContext : IDisposable
    {
        public Form KeyInterceptor;
        public bool LocalRequestor;
        public IExecutable Object;
        public readonly List<Form> OutputPlugInForms;
        public bool SuppressAsynchronousContext;
        public bool SuppressSynchronousContext;
        private Engine8 _asynchronousEngineInstance;
        private Engine8 _synchronousEngineInstance;

        public ExecutionContext()
        {
            OutputPlugInForms = new List<Form>();
            KeyInterceptor = null;
        }

        public Engine8 AsynchronousEngineInstance
        {
            get { return _asynchronousEngineInstance; }
            set
            {
                _asynchronousEngineInstance = value;
                if (value == null) {
                    return;
                }
                _asynchronousEngineInstance.ProgramEnd += AsynchronousEngineProgramEndHandler;
                _asynchronousEngineInstance.SequenceChange += AsynchronousEngineProgramChangeHandler;
            }
        }

// ReSharper disable ConvertToAutoProperty
        public Engine8 SynchronousEngineInstance
// ReSharper restore ConvertToAutoProperty
        {
            get { return _synchronousEngineInstance; }
            set
            {
                _synchronousEngineInstance = value;
                //if (value == null) {
                //    return;
                //}
                //_synchronousEngineInstance.ProgramEnd += SynchronousEngineProgramEndHandler;
                //_synchronousEngineInstance.SequenceChange += SynchronousEngineProgramChangeHandler;
            }
        }

        public void Dispose()
        {
            ReleaseAsynchronousEngine();
            ReleaseSynchronousEngine();
            Object = null;
            GC.SuppressFinalize(this);
        }


        //public event ProgramChangeHandler SynchronousProgramChangeHandler;

        private void AsynchronousEngineProgramChangeHandler()
        {
        }

        private void AsynchronousEngineProgramEndHandler()
        {
        }

        ~ExecutionContext()
        {
            Dispose();
        }

        private void ReleaseAsynchronousEngine()
        {
            if (AsynchronousEngineInstance == null) {
                return;
            }
            AsynchronousEngineInstance.Stop();
            AsynchronousEngineInstance.Dispose();
            AsynchronousEngineInstance = null;
        }

        private void ReleaseSynchronousEngine()
        {
            if (SynchronousEngineInstance == null) {
                return;
            }
            SynchronousEngineInstance.Stop();
            SynchronousEngineInstance.Dispose();
            SynchronousEngineInstance = null;
        }

        //private void SynchronousEngineProgramChangeHandler()
        //{
        //    if (SynchronousProgramChangeHandler != null)
        //    {
        //        SynchronousProgramChangeHandler(ProgramChange.SequenceChange);
        //    }
        //}

        //private void SynchronousEngineProgramEndHandler()
        //{
        //    if (SynchronousProgramChangeHandler != null)
        //    {
        //        SynchronousProgramChangeHandler(ProgramChange.End);
        //    }
        //}
    }
}