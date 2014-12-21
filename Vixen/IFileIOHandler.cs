namespace VixenPlus {
    public interface IFileIOHandler {

        string DialogFilterList();
        string FileExtension();
        int PreferredOrder();
        bool IsNativeToVixenPlus();
        bool CanSave();
        void SaveSequence(EventSequence eventSequence);
        void SaveProfile(Profile profile);
        bool CanOpen();
        EventSequence OpenSequence(string filename);
        Profile OpenProfile(string filename);
    }
}