using System.Drawing;
using System.Xml;

public interface INutcrackerModel {
    string EffectName { get; }
    string Notes { get; }
    XmlElement Settings { get; set; }
    NutcrackerNodes[,] InitializeNodes(Rectangle rect);
    bool IsLtoR { get;  set; }
}