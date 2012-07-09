using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class BrokenTargetTests
    {
        [TestInitialize]
        public void Initialize()
        {
            LogManager.Reset();

            // add a broken target, then the normal target...
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new BrokenTarget());
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, TestTarget.Current);
        }

        [TestMethod]
        public void TestBrokenTarget()
        {
            TestTarget.Current.Reset();

            // this should ignore errors in the broken target and flip down to the working target...
            var logger = LogManager.GetLogger("Foobar");
            logger.Trace("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }
    }
}
