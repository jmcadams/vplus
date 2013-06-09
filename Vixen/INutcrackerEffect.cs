using System;

public interface INutcrackerEffect
{
    string Name { get; }
    byte[] EffectData { get; }
    void Startup();
    void ShutDown();
}
