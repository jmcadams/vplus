using System.Xml;

namespace VixenPlus {
    public interface ISeqIOHandler {

        string DialogFilterList();
        string FileExtension();
        int VendorId();
        int PreferredOrder();
        long VGUID();
        bool IsNativeToVixenPlus();
        bool CanSave();
        void Save(EventSequence eventSequence);
        bool CanLoad();
        void Load(XmlNode contextNode, EventSequence eventSequence);
    }
}