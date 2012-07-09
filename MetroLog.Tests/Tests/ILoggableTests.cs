using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class ILoggableTests
    {
        [TestInitialize]
        public void Initialize()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, TestTarget.Current);
        }

        [TestMethod]
        public void TestTraceForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Trace("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestDebugForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Debug("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestInfoForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Info("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestWarnForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Warn("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestErrorForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Error("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestFatalForLoggable()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Fatal("Hello, world.");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestTraceForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Trace("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestDebugForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Debug("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestInfoForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Info("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestWarnForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Warn("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestErrorForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Error("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestFatalForLoggableWithException()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Fatal("Hello, world", new InvalidOperationException("Foobar"));

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNotNull(TestTarget.Current.LastWritten.Exception);
        }

        [TestMethod]
        public void TestTraceForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Trace("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Trace, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestDebugForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Debug("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Debug, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestInfoForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Info("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Info, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestWarnForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Warn("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Warn, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestErrorForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Error("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Error, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }

        [TestMethod]
        public void TestFatalForLoggableWithFormat()
        {
            TestTarget.Current.Reset();

            // run...
            var loggable = new TestLoggable();
            loggable.Fatal("Hello, {0}.", "**foo**");

            // check...
            Assert.AreEqual(1, TestTarget.Current.NumWritten);
            Assert.AreEqual(LogLevel.Fatal, TestTarget.Current.LastWritten.Level);
            Assert.IsNull(TestTarget.Current.LastWritten.Exception);
            Assert.AreNotEqual(-1, TestTarget.Current.LastWritten.Message.IndexOf("**foo**"));
        }
    }
}
