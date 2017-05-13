using System;
using System.IO;
using System.Windows.Forms;

namespace SteamShutdown
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SteamShutdown_Log.txt");
            File.WriteAllText(path, e.ExceptionObject.ToString());

            MessageBox.Show("Please send me the log file on GitHub or via E-Mail (Andreas.D.Korb@gmail.com)" + Environment.NewLine + path, "An Error occured");
        }
    }
}
