using System;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class VixenPlusSeqIO : ISeqIOHandler {

        public string DialogFilterList() {
            return "Vixen Plus format (*.vix)|*.vix";
        }


        public int Version() {
            return Vendor.VixenPlus;
        }


        public int PreferredOrder() {
            return 0;
        }


        public bool IsNativeToVixenPlus() {
            return true;
        }


        public void Save(Delegate seqDataDelegate) {
            throw new NotImplementedException();
        }


        public void Load(Delegate seqDataDelegate) {
            throw new NotImplementedException();
        }
    }
}