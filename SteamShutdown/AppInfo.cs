namespace SteamShutdown
{
    public class AppInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int State { get; set; }

        public bool IsDownloading
        {
            get { return CheckDownloading(State); }
        }

        public static bool CheckDownloading(int appState)
        {
            return appState == 1026 || appState == 1042 || appState == 1062 || appState == 1030;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
