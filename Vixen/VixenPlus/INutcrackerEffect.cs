using System;
using System.Drawing;

namespace VixenPlus {
    public interface INutcrackerEffect {
        event EventHandler OnControlChanged;
        string EffectName { get; }
        string Notes { get; }
        bool UsesPalette { get; }
        bool UsesSpeed { get; }
        Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender);
    }   
}   
