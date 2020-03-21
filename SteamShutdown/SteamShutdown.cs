using SteamShutdown.Modes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SteamShutdown
{
    static class SteamShutdown
    {
        public static List<App> WatchedGames { get; set; } = new List<App>();

        public static bool WaitForAll { get; set; }

        public static Mode ActiveMode { get; set; } = new ShutdownMode();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!EnsureSingleInstance()) return;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var applicationContext = new CustomApplicationContext();
            Application.Run(applicationContext);
        }

        // IPC: https://gorillacoding.wordpress.com/2013/02/03/using-wcf-for-inter-process-communication/
        private static bool EnsureSingleInstance()
        {
            bool isAlreadyRunning = IsAlreadyRunning();

            if (isAlreadyRunning)
            {
                RPC.ShowAnInstanceIsRunning();
            }
            else
            {
                RPC.StartServer();
            }

            return !isAlreadyRunning;
        }

        private static bool IsAlreadyRunning()
        {
            // string appProcessName = Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath);
            string appProcessName = Process.GetCurrentProcess().ProcessName;
            Process[] RunningProcesses = Process.GetProcessesByName(appProcessName);
            return RunningProcesses.Length == 2;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SteamShutdown_Log.txt");
            File.AppendAllText(path, DateTime.Now + ": " + e.ExceptionObject.ToString() + Environment.NewLine);

            MessageBox.Show("Please send me the log file on GitHub or via E-Mail (Andreas.D.Korb@gmail.com)" + Environment.NewLine +
                "You find the log file on your Dekstop.", "An Error occured");
        }
    }
}
