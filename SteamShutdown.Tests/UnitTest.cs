using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SteamShutdown.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void AppInfoTests()
        {
            AppInfo ai = Steam.FileToAppInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\TestFiles\appmanifest_213670.acf");
            Assert.AreEqual(ai.ID, 213670);
            Assert.AreEqual(ai.Name, "South Park™: The Stick of Truth™");

            ai = Steam.FileToAppInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\TestFiles\appmanifest_252490.acf");
            Assert.AreEqual(ai, null);

            ai = Steam.FileToAppInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\TestFiles\appmanifest_286080.acf");
            Assert.AreEqual(ai.ID, 286080);
            Assert.AreEqual(ai.Name, "Thinking with Time Machine");

            ai = Steam.FileToAppInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\TestFiles\appmanifest_403640.acf");
            Assert.AreEqual(ai.ID, 403640);
            Assert.AreEqual(ai.Name, "Dishonored 2");
        }
    }
}
