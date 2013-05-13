namespace VixenPlus
{
    public interface ITrigger
    {
        void ActivateTrigger(string interfaceTypeName, int index);
        int RegisterResponse(string interfaceTypeName, int index, string responseSequencePath);
        void ShowRegistrations();
        void UnregisterResponse(string interfaceTypeName, int index, int executionContextHandle);
    }
}