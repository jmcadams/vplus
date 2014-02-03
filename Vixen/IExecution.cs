using System.Windows.Forms;

namespace VixenPlus {
    public interface IExecution
    {
        int EngineStatus(int contextHandle);
        int EngineStatus(int contextHandle, out int sequencePosition);
        void ExecutePause(int contextHandle);
        bool ExecutePlay(int contextHandle, bool logAudio);
        bool ExecutePlay(int contextHandle, int millisecondStart, int millisecondCount);
        bool ExecutePlay(int contextHandle, int millisecondStart, int millisecondCount, bool logAudio);
        void ExecuteStop(int contextHandle);
        int FindExecutionContextHandle(object uniqueReference);
        float GetAudioSpeed(int contextHandle);
        int GetObjectPosition(int contextHandle);
        string LoadedProgram(int contextHandle);
        string LoadedSequence(int contextHandle);
        void ReleaseContext(int contextHandle);
        int RequestContext(bool suppressAsynchronousContext, bool suppressSynchronousContext, Form keyInterceptor);
        void SetAsynchronousContext(int contextHandle, IExecutable executableObject);
        void SetAudioSpeed(int contextHandle, float rate);
        void SetChannelStates(int contextHandle, byte[] channelValues);
        void SetLoopState(int contextHandle, bool state);
        void SetSynchronousContext(int contextHandle, IExecutable executableObject);
        void SetSynchronousProgramChangeHandler(int contextHandle, ProgramChangeHandler programChangeHandler);
    }
}