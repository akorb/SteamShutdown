using System.Windows.Forms;

namespace SteamShutdown.Modes
{
    public class HibernationMode : Mode
    {
        public override string Name { get; protected set; } = "Hibernate";

        public override void Execute()
        {
            Application.SetSuspendState(PowerState.Hibernate, false, false);
        }
    }
}
