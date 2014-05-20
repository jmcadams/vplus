using System;
using System.Threading;
using System.Windows.Forms;

using VixenPlusCommon;

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
            catch (Exception ex) {
                ex.ProcessException(true);
                Application.Exit();
            }
        }


        private static void ApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            ex.ProcessException(e.IsTerminating);
        }


        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs ex) {
            ex.Exception.ProcessException(true);
        }
    }
}