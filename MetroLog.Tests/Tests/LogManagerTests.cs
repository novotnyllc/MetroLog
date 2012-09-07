using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class LogManagerTests
    {
        [TestMethod]
        public void TestGetLogger()
        {
            var target = new LogManager();

            var logger = target.GetLogger("Foobar");
            Assert.IsNotNull(logger);
        }
    }
}
