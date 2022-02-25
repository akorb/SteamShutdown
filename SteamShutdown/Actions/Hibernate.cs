using System.Windows.Forms;

namespace SteamShutdown.Actions
{
    public class Hibernation : Action
    {
        public override string Name { get; protected set; } = "Hibernate";

        public override void Execute()
        {
            base.Execute();
            Application.SetSuspendState(PowerState.Hibernate, false, false);
        }
    }
}
