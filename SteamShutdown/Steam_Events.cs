using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SteamShutdown
{
    public static partial class Steam
    {
        public delegate void AppInfoChangedEventHandler(AppInfoChangedEventArgs e);
        public static event AppInfoChangedEventHandler AppInfoChanged;

        public delegate void AppInfoDeletedEventHandler(AppInfoEventArgs e);
        public static event AppInfoDeletedEventHandler AppInfoDeleted;



        static void OnAppInfoChanged(AppInfoChangedEventArgs e)
        {
            AppInfoChanged?.Invoke(e);
        }

        static void OnAppInfoDeleted(AppInfoEventArgs e)
        {
            AppInfoDeleted?.Invoke(e);
        }



        private static void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            int id = IdFromAcfFilename(e.FullPath);

            AppInfo info = Apps.FirstOrDefault(x => x.ID == id);
            if (info == null) return;

            var eventArgs = new AppInfoEventArgs(info);
            OnAppInfoDeleted(eventArgs);
        }

        private static void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            string json;
            try
            {
                json = AcfToJson(File.ReadAllLines(e.FullPath).ToList());
            }
            catch
            {
                return;
            }

            dynamic newJson = JsonConvert.DeserializeObject(json);

            AppInfo info = Apps.FirstOrDefault(x => x.ID == int.Parse(newJson.appID.ToString()));
            AppInfoChangedEventArgs eventArgs;

            if (info != null)
            {
                eventArgs = new AppInfoChangedEventArgs(info, info.State);
                // Only update existing AppInfo
                info.State = int.Parse(newJson.StateFlags.ToString());
            }
            else
            {
                // Add new AppInfo
                info = JsonToAppInfo(newJson);
                Apps.Add(info);
                eventArgs = new AppInfoChangedEventArgs(info, -1);
            }

            OnAppInfoChanged(eventArgs);
        }
    }



    public class AppInfoChangedEventArgs : AppInfoEventArgs
    {
        public int PreviousState { get; private set; }

        public AppInfoChangedEventArgs(AppInfo appInfo, int oldState) : base(appInfo)
        {
            PreviousState = oldState;
        }
    }

    public class AppInfoEventArgs : EventArgs
    {
        public AppInfo AppInfo { get; private set; }

        public AppInfoEventArgs(AppInfo info)
        {
            AppInfo = info;
        }
    }
}
