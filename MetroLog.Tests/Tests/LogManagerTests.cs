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
            var logger = LogManager.GetLogger("Foobar");
            Assert.IsNotNull(logger);
        }
    }
}
