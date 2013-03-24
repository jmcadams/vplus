namespace RemoteClient
{
    using System;
    using System.IO;
    using VixenPlus;

    internal static class ErrorLog
    {
        private static string m_logFile = string.Empty;

        public static void Log(string message)
        {
            StreamWriter writer = OpenLogFile();
            writer.WriteLine("{0}: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), message);
            writer.Close();
        }

        private static StreamWriter OpenLogFile()
        {
            if (m_logFile == string.Empty)
            {
                m_logFile = Path.Combine(Paths.AddinPath, "RemoteClient.err");
            }
            return new StreamWriter(m_logFile, true);
        }
    }
}

