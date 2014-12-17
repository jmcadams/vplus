using System;
using System.Collections.Generic;

using FileHelper.Annotations;

namespace FileHelper.Vixen2x {
    [UsedImplicitly]
    public class Vixen2XSeqFileIO : ISeqFileIO {

        public List<string> SupportedFileExt() {
            return new List<string> { ".vix" };
        }


        public void Save(Delegate seqSaveDelegate) {
            throw new NotImplementedException();
        }


        public void Load(Delegate seqLoadDelegate) {
            throw new NotImplementedException();
        }
    }
}