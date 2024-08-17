using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteamShutdown.Tests
{
    [TestClass]
    public class AcfParsingTests
    {
        [TestMethod]
        public void Valid1()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_213670.acf");
            Assert.AreEqual(213670, ai.ID);
            Assert.AreEqual("South Park™: The Stick of Truth™", ai.Name);
            Assert.AreEqual(4, ai.State);
        }

        [TestMethod]
        public void Valid2()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_286080.acf");
            Assert.AreEqual(286080, ai.ID);
            Assert.AreEqual("Thinking with Time Machine", ai.Name);
            Assert.AreEqual(4, ai.State);
        }

        [TestMethod]
        public void Valid3()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\appmanifest_403640.acf");
            Assert.AreEqual(403640, ai.ID);
            Assert.AreEqual("Dishonored 2", ai.Name);
            Assert.AreEqual(4, ai.State);
        }

        [TestMethod]
        public void Incomplete()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\incomplete.acf");
            Assert.IsNull(ai);
        }

        [TestMethod]
        public void CorruptNulls()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\nulls.acf");
            Assert.IsNull(ai);
        }

        [TestMethod]
        public void CorruptEmpty()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\empty.acf");
            Assert.IsNull(ai);
        }

        [TestMethod]
        public void CorruptJsonLike()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\json-like.acf");
            Assert.IsNull(ai);
        }

        [TestMethod]
        public void CorruptRandomBinary()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\random-binary.acf");
            Assert.IsNull(ai);
        }

        [TestMethod]
        public void CorruptCsv()
        {
            App ai = Steam.FileToAppInfo("AcfFiles\\csv.acf");
            Assert.IsNull(ai);
        }
    }
}
