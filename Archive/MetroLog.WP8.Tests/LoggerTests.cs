using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MetroLog.Tests;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;



namespace MetroLog.WP8.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public async Task TestDebug()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        static Tuple<ILoggerAsync, TestTarget> CreateTarget()
        {
            var testTarget = new TestTarget();
            var config = new LoggingConfiguration();
            config.AddTarget(LogLevel.Trace, LogLevel.Fatal, testTarget);

            var type = Type.GetType("MetroLog.Internal.Logger, MetroLog");

            var ctor = type.GetConstructors().First();

            var created = ctor.Invoke(new object[]{"Foobar", config});
            return Tuple.Create((ILoggerAsync)created, testTarget);
        }
    }
}
