using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SteamShutdown.Steam;

namespace AppInfoTests
{
    [TestClass]
    public class VdfParsingTests
    {
        [TestMethod]
        public void OldFormat()
        {
            string[] libraryPaths = InstallationPathsFromVdf("VdfFiles\\old_format.vdf");
            Assert.AreEqual(2, libraryPaths.Length);
            Assert.AreEqual("E:\\Gry\\Steam\\SteamApps", libraryPaths[0]);
            Assert.AreEqual("F:\\Gry\\Steam\\SteamApps", libraryPaths[1]);
        }

        [TestMethod]
        public void NewFormatWithoutMounted()
        {
            string[] libraryPaths = InstallationPathsFromVdf("VdfFiles\\new_format_without_mounted.vdf");
            Assert.AreEqual(2, libraryPaths.Length);
            Assert.AreEqual("D:\\Games\\SteamApps", libraryPaths[0]);
            Assert.AreEqual("F:\\SteamLibrary\\SteamApps", libraryPaths[1]);
        }

        [TestMethod]
        public void NewFormatWithMounted()
        {
            string[] libraryPaths = InstallationPathsFromVdf("VdfFiles\\new_format_with_mounted.vdf");
            Assert.AreEqual(1, libraryPaths.Length);
            Assert.AreEqual("E:\\SteamLibrary\\SteamApps", libraryPaths[0]);
        }
    }
}
