using System;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen25SeqIO : ISeqIOHandler {

        public string DialogFilterList() {
            return "Vixen 2.5 format (*.vix)|*.vix";
        }


        public int Version() {
            return Vendor.Vixen25;
        }


        public int PreferredOrder() {
            return 2;
        }


        public bool IsNativeToVixenPlus() {
            return false;
        }


        public void Save(Delegate seqDataDelegate) {
            throw new NotImplementedException();
        }


        public void Load(Delegate seqDataDelegate) {
            throw new NotImplementedException();
        }
    }
}