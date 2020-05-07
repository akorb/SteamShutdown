namespace SteamShutdown.Actions
{
    public abstract class Action
    {
        public abstract string Name { get; protected set; }

        public abstract void Execute();

        public override string ToString()
        {
            return Name;
        }

        public static Action[] GetAllActions => new Action[] { new Shutdown(), new Hibernation(), new Sleep() };
    }
}
