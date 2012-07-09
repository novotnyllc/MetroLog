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
        [TestInitialize]
        public void Initialize()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, TestTarget.Current);
        }

        [TestMethod]
        public void TestTrace()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Trace("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestDebug()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Debug("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestInfo()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Info("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestWarn()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Warn("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestError()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Error("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestFatal()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Fatal("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestTraceWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Trace("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestDebugWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Debug("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestInfoWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Info("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestWarnWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Warn("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestErrorWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Error("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestFatalWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Fatal("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestTraceWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Trace("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestDebugWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Debug("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestInfoWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Info("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestWarnWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Warn("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestErrorWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Error("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestFatalWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var logger = LogManager.GetLogger("Foobar");
            logger.Fatal("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }
    }
}
