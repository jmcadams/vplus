namespace AppUpdate
{
    using System;

    internal class Migration
    {
        private string[] m_flags;
        private Version m_keyFileVersion;
        private string m_versionCatalogFile;

        public Migration(string migrationLine)
        {
            string[] strArray = migrationLine.Split(new char[] { '|' });
            this.m_keyFileVersion = new Version(strArray[0].Trim());
            this.m_versionCatalogFile = strArray[1].Trim();
            this.m_flags = strArray[2].Trim().Split(new char[] { ',' });
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

        public string[] Flags
        {
            get
            {
                return this.m_flags;
            }
        }

        public Version KeyFileVersion
        {
            get
            {
                return this.m_keyFileVersion;
            }
        }

        public string VersionCatalogFile
        {
            get
            {
                return this.m_versionCatalogFile;
            }
        }
    }
}

