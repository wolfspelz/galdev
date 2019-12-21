using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace galdevtool.Test
{
    [TestClass]
    public class AppConfigTest
    {
        [TestMethod]
        public void ParseCommandline()
        {
            var c = new AppConfig();
            c.ParseCommandline(new[] { "TestInt=43" });
            Assert.AreEqual(43, c.TestInt);
        }

        [TestMethod]
        public void implements_IConfigCallback_Get()
        {
            var c = new AppConfig();
            Assert.AreEqual(42, c.Get(nameof(AppConfig.TestInt)));
        }
    }
}
