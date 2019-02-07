using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SteamShutdown
{
    public static partial class Steam
    {
        public delegate void AppInfoChangedEventHandler(object sender, AppInfoChangedEventArgs e);
        public static event AppInfoChangedEventHandler AppInfoChanged;

        public delegate void AppInfoDeletedEventHandler(object sender, AppInfoEventArgs e);
        public static event AppInfoDeletedEventHandler AppInfoDeleted;



        static void OnAppInfoChanged(object sender, AppInfoChangedEventArgs e)
        {
            AppInfoChanged?.Invoke(sender, e);
        }

        static void OnAppInfoDeleted(object sender, AppInfoEventArgs e)
        {
            AppInfoDeleted?.Invoke(sender, e);
        }



        private static void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            int id = IdFromAcfFilename(e.FullPath);

            App info = Apps.FirstOrDefault(x => x.ID == id);
            if (info == null) return;

            var eventArgs = new AppInfoEventArgs(info);
            OnAppInfoDeleted(info, eventArgs);
        }

        private static void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            string json = null;
            try
            {
                // This is necessary because sometimes the file is still accessed by steam, so let's wait for 10 ms and try again.
                // Maximum 5 times
                int counter = 1;
                do
                {
                    try
                    {
                        json = AcfToJson(File.ReadAllLines(e.FullPath));
                        break;
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
                while (counter++ <= 5);
            }
            catch
            {
                return;
            }

            // Shouldn't happen, but might occur if Steam holds the acf file too long
            if (json == null) return;

            dynamic newJson = JsonConvert.DeserializeObject(json);
            int newID = JsonToAppInfo(newJson).ID;

            // Search for changed app, if null it's a new app
            App info = Apps.FirstOrDefault(x => x.ID == newID);
            AppInfoChangedEventArgs eventArgs;

            if (info != null) // Download state changed
            {
                eventArgs = new AppInfoChangedEventArgs(info, info.State);
                // Only update existing AppInfo
                info.State = int.Parse(newJson.StateFlags.ToString());
            }
            else // New download started
            {
                // Add new AppInfo
                info = JsonToAppInfo(newJson);
                Apps.Add(info);
                eventArgs = new AppInfoChangedEventArgs(info, -1);
            }

            OnAppInfoChanged(info, eventArgs);
        }
    }



    public class AppInfoChangedEventArgs : AppInfoEventArgs
    {
        public int PreviousState { get; private set; }

        public AppInfoChangedEventArgs(App appInfo, int oldState) : base(appInfo)
        {
            PreviousState = oldState;
        }
    }

    public class AppInfoEventArgs : EventArgs
    {
        public App AppInfo { get; private set; }

        public AppInfoEventArgs(App info)
        {
            AppInfo = info;
        }
    }
}
