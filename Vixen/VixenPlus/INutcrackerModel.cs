using System.Xml;

namespace VixenPlus {
    internal interface INutcrackerModel {
        string EffectName { get; }
        string Notes { get; }
        XmlElement Settings { get; set; }
        NutcrackerNodes[,] InitializeNodes { get; }
    }
}