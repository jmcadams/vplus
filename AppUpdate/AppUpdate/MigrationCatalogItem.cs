namespace AppUpdate
{
    using System;

    internal class MigrationCatalogItem
    {
        private long m_crc32;
        private string m_destPath;
        private string[] m_flags;
        private bool m_isData;
        private string m_sourcePath;

        public MigrationCatalogItem()
        {
        }

        public MigrationCatalogItem(string migrationCatalogItemLine)
        {
            this.Parse(migrationCatalogItemLine);
        }

        public bool HasFlag(string flag)
        {
            foreach (string str in this.m_flags)
            {
                if (flag == str)
                {
                    return true;
                }
            }
            return false;
        }

        public void Parse(string migrationCatalogItemLine)
        {
            string[] strArray = migrationCatalogItemLine.Split(new char[] { '|' });
            this.m_sourcePath = strArray[0].Trim();
            this.m_destPath = strArray[1].Trim();
            this.m_isData = this.m_sourcePath.StartsWith("[Data]");
            if (this.m_isData)
            {
                this.m_sourcePath = this.m_sourcePath.Substring(6);
            }
            string[] strArray2 = strArray[2].Trim().Split(new char[] { ',' });
            this.m_flags = new string[strArray2.Length];
            for (int i = 0; i < strArray2.Length; i++)
            {
                this.m_flags[i] = strArray2[i].Trim();
            }
            try
            {
                this.m_crc32 = Convert.ToUInt32(strArray[3].Trim());
            }
            catch
            {
                this.m_crc32 = 0L;
            }
        }

        public long CRC32
        {
            get
            {
                return this.m_crc32;
            }
        }

        public string DestPath
        {
            get
            {
                return this.m_destPath;
            }
        }

        public bool IsData
        {
            get
            {
                return this.m_isData;
            }
        }

        public string SourcePath
        {
            get
            {
                return this.m_sourcePath;
            }
        }
    }
}

