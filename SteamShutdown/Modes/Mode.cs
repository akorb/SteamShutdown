namespace SteamShutdown.Modes
{
    public abstract class Mode
    {
        public abstract string Name { get; protected set; }

        public abstract void Execute();

        public override string ToString()
        {
            return Name;
        }

        public static Mode[] GetAllModes => new Mode[] { new ShutdownMode(), new HibernationMode(), new SleepMode() };
    }
}
