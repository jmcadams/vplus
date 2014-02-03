namespace VixenPlus {
    public interface INutcrackerModel {
        string EffectName { get; }
        string Notes { get; }
        bool IsLtoR { set; }
    }
}