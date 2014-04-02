namespace NutcrackerEffectsControl.Models {
    public interface INutcrackerModel {
        string EffectName { get; }
        string Notes { get; }
        bool IsLtoR { set; }
    }
}