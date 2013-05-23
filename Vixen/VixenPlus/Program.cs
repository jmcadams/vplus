using System;
using System.IO;
using System.Windows.Forms;

using Properties;

namespace VixenPlus {
    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VixenPlusForm(args));
            }
            catch (Exception exception) {
                // ReSharper disable AssignNullToNotNullAttribute
                var log = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "crash.log");
                // ReSharper restore AssignNullToNotNullAttribute
                using (var crash = new StreamWriter(log, true)) {
                    crash.WriteLine(DateTime.Now);
                    crash.WriteLine(exception.Message);
                    crash.WriteLine(exception.StackTrace);
                }
                MessageBox.Show(string.Format(Vendor.ProductName + Resources.CriticalErrorOccurred, log, exception.Message, exception.StackTrace),
                                Resources.ErrorLogCreated, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
