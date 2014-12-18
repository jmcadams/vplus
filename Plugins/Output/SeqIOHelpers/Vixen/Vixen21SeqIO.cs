using System;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen21SeqIO : ISeqIOHandler {

        public string DialogFilterList() {
            return "Vixen 2.1 format (*.vix)|*.vix";
        }


        public int VendorId() {
            return Vendor.Vixen21;
        }


        public int PreferredOrder() {
            return 1;
        }


        public long VGUID() {
            return 21L;
        }


        public bool IsNativeToVixenPlus() {
            return false;
        }


        public bool CanSave() {
            return true;
        }


        public void Save(EventSequence eventSequence) {
            MessageBox.Show("Vixen2.1");
        }


        public bool CanLoad() {
            return true;
        }


        public void Load() {
            throw new NotImplementedException();
        }
    }
}
