using System;
using System.Drawing;

namespace VixenPlus {
    public interface INutcrackerEffect {
        event EventHandler OnControlChanged;
        string EffectName { get; }
        Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender);
    }   
}   
