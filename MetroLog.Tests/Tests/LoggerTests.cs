using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class LoggerTests
    {
        private static Tuple<Logger, TestTarget> CreateTarget()
        {
            var testTarget = new TestTarget();
            var config = new LoggingConfiguration();
            config.AddTarget(LogLevel.Trace, LogLevel.Fatal, testTarget);

            return Tuple.Create(new Logger("Foobar", config), testTarget);
        }

        [TestMethod]
        public async Task TestTrace()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestLog()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.LogAsync(LogLevel.Trace, "Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }


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

        [TestMethod]
        public async Task TestInfo()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestWarn()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestError()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestFatal()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, world.");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestTraceWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestLogWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.LogAsync(LogLevel.Trace, "Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestDebugWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestInfoWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestWarnWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestErrorWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestFatalWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.IsNotNull(logger.Item2.LastWritten.Exception);
        }

        [TestMethod]
        public async Task TestTraceWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, {0}.", "**foo**");
            

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public async Task TestDebugWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public async Task TestInfoWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public async Task TestWarnWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public async Task TestErrorWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public async Task TestFatalWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, logger.Item2.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.IsNull(logger.Item2.LastWritten.Exception);
            Assert.AreNotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }
    }
}
