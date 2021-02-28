using VixenPlusCommon.Properties;

namespace VixenPlusCommon {
    public static class Vendor {
        public const int MinimumEventPeriod = 10;

        public static readonly string ProductDescription = Resources.ProductDescription;
        public const string ProductName = "Vixen+ {Beta}";
        public const string All = "*";
        public const string SeqFileIO = "SeqIOHelpers";
        
        public const string ModuleAuthoring = "Artisan";
        public const string ModuleManager = "Roadie";

        public const string AppExtension = ".dll";
        public const string DeletedExtension = ".x";
        public const string CsvExtension = ".csv";
        public const string GroupExtension = ".vpx";
        public const string MapperExtension = ".vmap";
        public const string NutcrakerModelExtension = ".nmx";
        public const string ProfileExtension = ".pro";
        public const string RoutineExtension = ".vir";
        public const string SequenceExtension = ".vix";
        public const string TemplateExtension = ".vpt";
        public const string UpdateFileExtension = ".7z";

        public const string SupportURL = "http://www.diychristmas.org/vb1/forumdisplay.php?19-VixenPlus";

        public const string DomainLS = "VixenplusDomain"; //Domain lookup string for preferences.
        public const string Protocol = "https://";
        public const string DistDir = "/alpha/";
        public const string UpdateFile = "version.php";
        public const string UpdateReleaseNote = "ReleaseNotes.txt";
        public const string DownloadFilePrefix = "Release";
        // ReSharper disable InconsistentNaming
        public const string UpdateSupport7zrProtected = "_7zr.exe";
        public const string UpdateSupport7zrReal = "7zr.exe";
        // ReSharper restore InconsistentNaming
        public const string UpdateSupportBatchProtected = "_update.bat";
        public const string UpdateSupportBatchReal = "update.bat";
    }
}