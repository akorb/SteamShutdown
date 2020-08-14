using Newtonsoft.Json;
using SteamShutdown.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace SteamShutdown
{
    static class SteamShutdown
    {
        public static List<App> WatchedGames { get; set; } = new List<App>();

        public static bool WaitForAll { get; set; }

        public static Actions.Action ActiveMode { get; set; } = new Shutdown();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (!EnsureSingleInstance()) return;
            if (IsUpdateAvailable())
            {
                DialogResult dr = MessageBox.Show(
                    "Do you want to update now?",
                    "New version available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Process.Start("https://github.com/akorb/SteamShutdown/releases/latest");
                }
            }

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

        private static bool IsUpdateAvailable()
        {
            try
            {
                // Usually HttpClient should stay alive over the whole application lifetime
                // but since we know we'll never require network access again after this function,
                // dispose it.
                using (HttpClient client = new HttpClient())
                {
                    Uri ur = new Uri("https://api.github.com/repos/akorb/steamshutdown/releases/latest");
                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                    // User-Agent is necessary for the GitHub api
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:79.0) Gecko/20100101 Firefox/79.0");

                    // Blocking call. Program will wait here until a response is received or a timeout occurs.
                    HttpResponseMessage response = client.GetAsync(ur).Result;
                    if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        // Parse the response body.
                        string text = response.Content.ReadAsStringAsync().Result;
                        dynamic json = JsonConvert.DeserializeObject(text);
                        string tag = json.tag_name;
                        // Use trim (char based) instead of substring (index based) to allow to leave the 'v' in later tag names
                        Version serverVersion = new Version(tag.TrimStart('v'));
                        Version currentVersion = new Version(Application.ProductVersion);
                        return serverVersion > currentVersion;
                    }
                }
            }
            catch
            {
                // Runs into catch for example if there is no internet connection available.
                // No further handling necessary since checking for an update is just a bonus.
            }
            return false;
        }
    }
}
