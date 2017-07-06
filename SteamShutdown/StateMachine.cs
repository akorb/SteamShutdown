using SteamShutdown.Modes;
using System.Collections.Generic;

namespace SteamShutdown
{
    public static class StateMachine
    {
        public static List<AppInfo> WatchedGames { get; set; } = new List<AppInfo>();

        public static bool WaitForAll { get; set; }

        public static Mode ActiveMode { get; set; } = new ShutdownMode();
    }
}
