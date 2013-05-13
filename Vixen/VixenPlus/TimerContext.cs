using System;

namespace VixenPlus
{
    internal class TimerContext : IDisposable
    {
        public delegate void OnExecutionChange(TimerContext context);

        public delegate void OnExecutionEnd(TimerContext context);

        private static IExecution _executionInterface;

        private readonly DateTime _endDateTime;
        private readonly int _executionContextHandle;
        private readonly Timer _timer;

        public TimerContext(Timer timer)
        {
            _timer = timer;
            _endDateTime =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, _timer.StartDateTime.Hour,
                             _timer.StartDateTime.Minute, 0).Add(_timer.TimerLength);
            if (_executionInterface == null)
            {
                _executionInterface = (IExecution) Interfaces.Available["IExecution"];
            }
            _executionContextHandle = _executionInterface.RequestContext(true, false, null);
            _executionInterface.SetSynchronousProgramChangeHandler(_executionContextHandle, ProgramChanged);
        }

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
        }

        public int ExecutionContextHandle
        {
            get { return _executionContextHandle; }
        }

        public IExecution ExecutionInterface
        {
            get { return _executionInterface; }
        }

        public bool Stopping { get; set; }

        public Timer Timer
        {
            get { return _timer; }
        }

        public void Dispose()
        {
            _executionInterface.ReleaseContext(_executionContextHandle);
            GC.SuppressFinalize(this);
        }

        public event OnExecutionChange ExecutionChange;

        public event OnExecutionEnd ExecutionEnd;

        ~TimerContext()
        {
            Dispose();
        }

        private void ProgramChanged(ProgramChange changeType)
        {
            switch (changeType)
            {
                case ProgramChange.SequenceChange:
                    if (ExecutionChange != null)
                    {
                        ExecutionChange(this);
                    }
                    break;

                case ProgramChange.End:
                    if (ExecutionEnd != null)
                    {
                        ExecutionEnd(this);
                    }
                    break;
            }
        }

        public override string ToString()
        {
            string str = _executionInterface.LoadedProgram(_executionContextHandle);
            string str2 = _executionInterface.LoadedSequence(_executionContextHandle);
            if (str.Length == 0)
            {
                return str2;
            }
            return string.Format("{0}: {1}", str, str2);
        }
    }
}