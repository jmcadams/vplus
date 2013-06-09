namespace NutcrackerEffects {
    interface INutcrackerEffect {
        string EffectName { get; }
        byte[] EffectData { get; }
        void Startup();
        void ShutDown();
    }
}
