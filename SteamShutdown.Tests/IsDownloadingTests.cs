using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SteamShutdown.App;

namespace SteamShutdown.Tests
{
    [TestClass]
    public class IsDownloadingTests
    {
        [TestMethod]
        public void TestDownloadingStates()
        {
            int[] downloadingStates =
            {
                6,
                1026,
                1030,
                1042,
                1044,
                1062,
            };

            foreach (int state in downloadingStates)
            {
                Assert.IsTrue(CheckDownloading(state), $"State {state} was erroneously detected as not downloading.");
            }
        }

        [TestMethod]
        public void TestNotDownloadingStates()
        {
            int[] downloadingStates =
            {
                4,
                70,
            };

            foreach (int state in downloadingStates)
            {
                Assert.IsFalse(CheckDownloading(state), $"State {state} was erroneously detected as downloading.");
            }
        }
    }
}
