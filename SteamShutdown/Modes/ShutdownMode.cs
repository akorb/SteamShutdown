using System.Diagnostics;

namespace SteamShutdown.Modes
{
    public class ShutdownMode : Mode
    {
        public override string Name { get; protected set; } = "Shutdown";

        public override void Execute()
        {
            Process.Start("shutdown", "/s /t 0");
        }
    }
}
