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
            return (IsBitSet(appState, 1) || IsBitSet(appState, 10)) && !IsBitSet(appState, 9);

            /* Counting from zero and starting from the right
             * Bit 1 indicates if a download is running
             * Bit 2 indicates if a game is installed
             * Bit 9 indicates if the download has been stopped by the user. The download will not happen, so don't wait for it.
             * Bit 10 (or maybe Bit 5) indicates if a DLC is downloaded for a game
             * 
             * All known stateFlags while a download is running so far:
             * 00000000110
             * 10000000010
             * 10000010010
             * 10000100110
             * 10000000110
             * 10000010100 Bit 1 not set, but Bit 5 and Bit 10. Happens if downloading a DLC for an already downloaded game.
             *             Because for a very short time after starting the download for this DLC the stateFlags becomes 20 = 00000010100
             *             I think Bit 5 indicates if "something" is happening with a DLC and Bit 10 indicates if it is downloading.
             */
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
