namespace VixenPlus {
    public interface IFileIOHandler {
        string DialogFilterList();
        string Name();
        string FileExtension();
        int PreferredOrder();
        bool IsNativeToVixenPlus();
        bool CanSave();
        void SaveSequence(EventSequence eventSequence);
        void SaveProfile(Profile profile);
        bool CanOpen();
        EventSequence OpenSequence(string fileName);
        Profile OpenProfile(string fileName);
        void LoadEmbeddedData(string fileName, EventSequence es);
        bool SupportsProfiles { get; }
    }
}