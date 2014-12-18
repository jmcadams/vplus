using System;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen25SeqIO : ISeqIOHandler {

        public string DialogFilterList() {
            return "Vixen 2.5 format (*.vix)|*.vix";
        }


        public int VendorId() {
            return Vendor.Vixen25;
        }


        public int PreferredOrder() {
            return 2;
        }


        public long VGUID() {
            return 25L;
        }


        public bool IsNativeToVixenPlus() {
            return false;
        }


        public bool CanSave() {
            return true;
        }


        public void Save(EventSequence eventSequence) {
            MessageBox.Show("Vixen2.5");
        }


        public bool CanLoad() {
            return true;
        }


        public void Load() {
            throw new NotImplementedException();
        }
    }
}