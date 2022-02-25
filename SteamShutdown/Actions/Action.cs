namespace SteamShutdown.Actions
{
    public abstract class Action
    {
        public abstract string Name { get; protected set; }

        public virtual void Execute()
        {
            SteamShutdown.Log("Execute action: " + Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public static Action[] GetAllActions => new Action[] { new Shutdown(), new Hibernation(), new Sleep() };
    }
}
