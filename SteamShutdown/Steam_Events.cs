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
            SteamShutdown.Log("Fsw_Deleted: " + e.FullPath + " ChangeType: " + e.ChangeType);

            /*
             * Unfortunately, even for changing files, Steam creates a tmp file, deletes the old file and then renames the tmp file to its actual name.
             * Therefore, we cannot distinguish anymore if a file was actually deleted or just "changed".
             * Luckily, this functionality is hardly needed so I just disable it.
             * 
           
            int id = IdFromAcfFilename(e.FullPath);

            App info = Apps.FirstOrDefault(x => x.ID == id);
            if (info == null)
            {
                SteamShutdown.Log("Fsw_Deleted: info == null");
                return;
            }

            var eventArgs = new AppInfoEventArgs(info);
            OnAppInfoDeleted(info, eventArgs);*/
        }

        private static void UpdateAppInfo(string filename)
        {
            SteamShutdown.Log("UpdateAppInfo: " + filename);
            string json = null;
            try
            {
                // This is necessary because sometimes the file is still accessed by steam, so let's wait for 50 ms and try again.
                // Maximum 5 times
                int counter = 1;
                do
                {
                    try
                    {
                        SteamShutdown.Log($"Attempt #{counter} to read: {filename}");
                        json = AcfToJson(File.ReadAllLines(filename));
                        break;
                    }
                    catch (FileNotFoundException)
                    {
                        // It's possible that this file got deleted in the meanwhile.
                        return;
                    }
                    catch (IOException)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
                while (counter++ <= 5);
            }
            catch (Exception ex)
            {
                SteamShutdown.Log($"Fsw_Changed: {ex}");
                return;
            }

            // Shouldn't happen, but might occur if Steam holds the acf file too long
            if (json == null)
            {
                SteamShutdown.Log($"Fsw_Changed: json == null");
                return;
            }

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
                SteamShutdown.Log("Download state changed: " + info.Name + " " + info.State + " IsDownloading: " + info.IsDownloading);
            }
            else // New download started
            {
                // Add new AppInfo
                info = JsonToAppInfo(newJson);
                Apps.Add(info);
                eventArgs = new AppInfoChangedEventArgs(info, -1);
                SteamShutdown.Log("New download started: " + info.Name + " " + info.State + " IsDownloading: " + info.IsDownloading);
            }

            OnAppInfoChanged(info, eventArgs);
        }

        private static void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            SteamShutdown.Log("UpdateAppInfo: " + e.FullPath + " Changetype: " + e.ChangeType);

            UpdateAppInfo(e.FullPath);
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
