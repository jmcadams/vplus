namespace NutcrackerEffects.Models {
    public interface INutcrackerModel {
        string EffectName { get; }
        string Notes { get; }
        bool IsLtoR { set; }
    }
}