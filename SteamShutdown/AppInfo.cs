using System;

namespace SteamShutdown
{
    public class AppInfo : IComparable<AppInfo>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int State { get; set; }

        /// <summary>
        /// Returns a value indicating whether the game is being downloaded. Includes games in queue for download.
        /// </summary>
        public bool IsDownloading
        {
            get { return CheckDownloading(State); }
        }

        /// <summary>
        /// Returns a value indicating whether the game is being downloaded. Includes games in queue for download.
        /// </summary>
        /// <param name="appState"></param>
        /// <returns></returns>
        public static bool CheckDownloading(int appState)
        {
            // 6: In queue for update
            // 1024: download running
            // 1026: In queue
            return appState == 6 || appState == 1026 || appState == 1042 || appState == 1062 || appState == 1030;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(AppInfo info)
        {
            return Name.CompareTo(info.Name);
        }
    }
}
