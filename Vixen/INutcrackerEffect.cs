using System;
using System.Collections.Generic;
using System.Drawing;

public interface INutcrackerEffect {
    event EventHandler OnControlChanged;
    string EffectName { get; }
    string Notes { get; }
    bool UsesPalette { get; }
    bool UsesSpeed { get; }
    List<string> Settings { get; set; }
    Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender);
}