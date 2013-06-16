using System;
using System.Drawing;

namespace VixenPlus {
    public interface INutcrackerEffect {
        event EventHandler OnControlChanged;
        string EffectName { get; }
        byte[] EffectData { get; }
        void Startup();
        Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender);
    }   
}   
