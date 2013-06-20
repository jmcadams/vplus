using System;
using System.Drawing;
using System.Xml;

namespace VixenPlus {
    public interface INutcrackerEffect {
        event EventHandler OnControlChanged;
        string EffectName { get; }
        string Notes { get; }
        bool UsesPalette { get; }
        bool UsesSpeed { get; }
        XmlElement Settings { get; set; }
        Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender);
    }   
}   
