using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using CommonControls;

using CommonUtils;

using VixenPlus.Properties;

namespace VixenPlus {
    internal static class Program {

        [STAThread]
        private static void Main(string[] args) {
            Application.ThreadException += ApplicationOnThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += ApplicationUnhandledException;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try {
                Application.Run(new VixenPlusForm(args));
            }
            catch (Exception e) {
                ProcessException(e, true);
                Application.Exit();
            }
        }



        private static void ApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            ProcessException(ex, e.IsTerminating);
        }


        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e) {
            ProcessException(e.Exception, true);
        }


        private static void ProcessException(Exception ex, bool isTerminating) {
            LogException(ex.Message, ex.StackTrace, isTerminating);
            ShowException(ex, isTerminating);
        }



        private static void LogException(string message, string stack, bool isTerminating) {
            string.Format(Resources.FormattedVersion, Assembly.GetExecutingAssembly().GetName().Version).CrashLog();
            DateTime.Now.ToString(CultureInfo.InvariantCulture).CrashLog();
            string.Format("Is Terminating? {0}", isTerminating).CrashLog();
            message.CrashLog();
            stack.CrashLog();
        }


        private static void ShowException(Exception exception, bool isTerminating) {
            var msgFormat = isTerminating ? Resources.CriticalErrorOccurred : Resources.SoftErrorOccured;
            var msg = string.Format(msgFormat, Utils.LogFileName, exception.Message, exception.StackTrace, Vendor.ProductName);
            var btns = isTerminating ? MessageBoxButtons.OK : MessageBoxButtons.YesNo;
            var icon = isTerminating ? MessageBoxIcon.Error : MessageBoxIcon.Question;

            var res = MessageBox.Show(msg, Resources.ErrorLogCreated, btns, icon);
            if (res == DialogResult.No || res == DialogResult.OK) {
                Application.Exit();
            }
        }
    }
}