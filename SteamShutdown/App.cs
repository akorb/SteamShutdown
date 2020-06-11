namespace SteamShutdown
{
    public class App
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int State { get; set; }

        /// <summary>
        /// Returns a value indicating whether the game is being downloaded.
        /// </summary>
        public bool IsDownloading => CheckDownloading(State);

        /// <summary>
        /// Returns a value indicating whether the game is being downloaded.
        /// </summary>
        public static bool CheckDownloading(int appState)
        {
            // The second bit defines if anything for the app needs to be downloaded
            // Doesn't matter if queued, download running and so on
            return IsBitSet(appState, 1);
        }

        public override string ToString()
        {
            return Name;
        }

        private static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
    }
}
