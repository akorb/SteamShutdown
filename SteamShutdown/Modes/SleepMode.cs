using System.Windows.Forms;

namespace SteamShutdown.Modes
{
    public class SleepMode : Mode
    {
        public override string Name { get; protected set; } = "Sleep";

        public override void Execute()
        {
            Application.SetSuspendState(PowerState.Hibernate, false, false);
        }
    }
}
