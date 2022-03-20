using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SteamShutdown.App;

namespace SteamShutdown.Tests
{
    [TestClass]
    public class IsDownloadingTests
    {
        [TestMethod]
        public void Downloading_6()
        {
            Assert.IsTrue(CheckDownloading(6));
        }

        [TestMethod]
        public void Downloading_1026()
        {
            Assert.IsTrue(CheckDownloading(1026));
        }

        [TestMethod]
        public void Downloading_1030()
        {
            Assert.IsTrue(CheckDownloading(1030));
        }

        [TestMethod]
        public void Downloading_1042()
        {
            Assert.IsTrue(CheckDownloading(1042));
        }

        [TestMethod]
        public void Downloading_1044()
        {
            Assert.IsTrue(CheckDownloading(1044));
        }

        [TestMethod]
        public void Downloading_1062()
        {
            Assert.IsTrue(CheckDownloading(1062));
        }


        [TestMethod]
        public void NotDownloading_4()
        {
            Assert.IsFalse(CheckDownloading(4));
        }

        [TestMethod]
        public void NotDownloading_70()
        {
            Assert.IsFalse(CheckDownloading(70));
        }
    }
}
