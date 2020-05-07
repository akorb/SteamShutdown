using System.Diagnostics;

namespace SteamShutdown.Actions
{
    public class Shutdown : Action
    {
        public override string Name { get; protected set; } = "Shutdown";

        public override void Execute()
        {
            Process.Start("shutdown", "/s /t 0");
        }
    }
}
