namespace AppUpdate {
    using System;

    internal class MigrationCatalogItem {
        private string[] _flags;


        public MigrationCatalogItem() {}


        public MigrationCatalogItem(string migrationCatalogItemLine) {
            Parse(migrationCatalogItemLine);
        }


        public bool HasFlag(string flag) {
            foreach (var str in _flags) {
                if (flag == str) {
                    return true;
                }
            }
            return false;
        }


        public void Parse(string migrationCatalogItemLine) {
            var strArray = migrationCatalogItemLine.Split(new[] {'|'});
            SourcePath = strArray[0].Trim();
            DestPath = strArray[1].Trim();
            IsData = SourcePath.StartsWith("[Data]");
            if (IsData) {
                SourcePath = SourcePath.Substring(6);
            }
            var strArray2 = strArray[2].Trim().Split(new[] {','});
            _flags = new string[strArray2.Length];
            for (var i = 0; i < strArray2.Length; i++) {
                _flags[i] = strArray2[i].Trim();
            }
            try {
                CRC32 = Convert.ToUInt32(strArray[3].Trim());
            }
            catch {
                CRC32 = 0L;
            }
        }


        public long CRC32 { get; private set; }

        public string DestPath { get; private set; }

        public bool IsData { get; private set; }

        public string SourcePath { get; private set; }
    }
}
