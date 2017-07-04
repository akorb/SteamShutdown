using System.Collections.Generic;

namespace SteamShutdown
{
    public class StateMachine
    {
        public static List<AppInfo> WatchedGames { get; set; } = new List<AppInfo>();

        public static bool WaitForAll { get; set; }
    }
}
