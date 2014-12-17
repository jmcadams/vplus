using System;
using System.Collections.Generic;

namespace FileHelper {
    public interface ISeqFileIO {

        List<string> SupportedFileExt();
        void Save(Delegate seqSaveDelegate);
        void Load(Delegate seqLoadDelegate);

    }
}