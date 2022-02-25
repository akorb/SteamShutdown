using System.Windows.Forms;

namespace SteamShutdown.Actions
{
    public class Sleep : Action
    {
        public override string Name { get; protected set; } = "Sleep";

        public override void Execute()
        {
            base.Execute();
            Application.SetSuspendState(PowerState.Suspend, false, false);
        }
    }
}
