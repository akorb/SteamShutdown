namespace SteamShutdown
{
    public class AppInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int State { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
