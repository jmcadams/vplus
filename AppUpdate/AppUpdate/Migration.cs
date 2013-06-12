using System.Linq;

namespace AppUpdate
{
    using System;

    internal class Migration
    {
        public Migration(string migrationLine)
        {
            var strArray = migrationLine.Split(new[] { '|' });
            KeyFileVersion = new Version(strArray[0].Trim());
            VersionCatalogFile = strArray[1].Trim();
            Flags = strArray[2].Trim().Split(new[] { ',' });
        }

        public bool HasFlag(string flag) {
            //foreach (var str in Flags)
            //{
            //    if (flag == str)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return Flags.Any(str => flag == str);
        }


        public string[] Flags { get; private set; }

        public Version KeyFileVersion { get; private set; }

        public string VersionCatalogFile { get; private set; }
    }
}

