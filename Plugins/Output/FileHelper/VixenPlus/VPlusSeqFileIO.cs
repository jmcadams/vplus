using System;
using System.Collections.Generic;

using FileHelper.Annotations;

namespace FileHelper.VixenPlus {
    [UsedImplicitly]
    public class VPlusSeqFileIO : ISeqFileIO {

        public List<string> SupportedFileExt() {
            return new List<string> {".vpx"};
        }


        public void Save(Delegate seqSaveDelegate) {
            throw new NotImplementedException();
        }


        public void Load(Delegate seqLoadDelegate) {
            throw new NotImplementedException();
        }
    }
}