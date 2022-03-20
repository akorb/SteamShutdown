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
            Assert.AreEqual(ai.ID, 213670);
            Assert.AreEqual(ai.Name, "South Park™: The Stick of Truth™");
            Assert.AreEqual(ai.State, 4);

            ai = Steam.FileToAppInfo("AcfFiles\\corrupted.acf");
            Assert.AreEqual(ai, null);

            ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_286080.acf");
            Assert.AreEqual(ai.ID, 286080);
            Assert.AreEqual(ai.Name, "Thinking with Time Machine");
            Assert.AreEqual(ai.State, 4);

            ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_403640.acf");
            Assert.AreEqual(ai.ID, 403640);
            Assert.AreEqual(ai.Name, "Dishonored 2");
            Assert.AreEqual(ai.State, 4);
        }
    }
}
