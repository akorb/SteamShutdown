using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteamShutdown.Tests
{
    [TestClass]
    public class AcfParsingTests
    {
        [TestMethod]
        public void AppInfoTests()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_213670.acf");
            Assert.AreEqual(213670, ai.ID);
            Assert.AreEqual("South Park™: The Stick of Truth™", ai.Name);
            Assert.AreEqual(4, ai.State);

            ai = Steam.FileToAppInfo("AcfFiles\\corrupted.acf");
            Assert.AreEqual(null, ai);

            ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_286080.acf");
            Assert.AreEqual(286080, ai.ID);
            Assert.AreEqual("Thinking with Time Machine", ai.Name);
            Assert.AreEqual(4, ai.State);

            ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_403640.acf");
            Assert.AreEqual(403640, ai.ID);
            Assert.AreEqual("Dishonored 2", ai.Name);
            Assert.AreEqual(4, ai.State);
        }
    }
}
