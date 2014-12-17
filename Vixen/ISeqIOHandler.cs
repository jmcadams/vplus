using System;

namespace VixenPlus {
    public interface ISeqIOHandler {

        string DialogFilterList();
        int Version();
        int PreferredOrder();
        bool IsNativeToVixenPlus();
        void Save(Delegate seqDataDelegate);
        void Load(Delegate seqDataDelegate);
    }
}