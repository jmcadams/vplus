using System.Xml;

namespace VixenPlus {
    public interface INutcrackerModel {
        string EffectName { get; }
        string Notes { get; }
        XmlElement Settings { get; set; }
        NutcrackerNodes[,] InitializeNodes { get; }
        bool SetDirection { set; }
    }
}