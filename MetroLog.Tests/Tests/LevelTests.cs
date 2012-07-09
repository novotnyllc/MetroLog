using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MetroLog.Tests
{
    [TestClass]
    public class LevelTests
    {
        [TestMethod]
        public void TestIsTraceEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Debug, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsTrue(logger.IsDebugEnabled);
            Assert.IsTrue(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestTraceIgnored()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Debug, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // run...
            TestTarget.Current.Reset();
            logger.Trace("Foobar");

            // check...
            Assert.AreEqual(0, TestTarget.Current.NumWritten);
        }

        [TestMethod]
        public void TestIsDebugEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsTrue(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestDebugIgnored()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // run...
            TestTarget.Current.Reset();
            logger.Debug("Foobar");

            // check...
            Assert.AreEqual(0, TestTarget.Current.NumWritten);
        }

        [TestMethod]
        public void TestIsInfoEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Warn, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestInfoIgnored()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Warn, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // run...
            TestTarget.Current.Reset();
            logger.Info("Foobar");

            // check...
            Assert.AreEqual(0, TestTarget.Current.NumWritten);
        }

        [TestMethod]
        public void TestIsWarnEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestWarnIgnored()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // run...
            TestTarget.Current.Reset();
            logger.Warn("Foobar");

            // check...
            Assert.AreEqual(0, TestTarget.Current.NumWritten);
        }

        [TestMethod]
        public void TestIsErrorEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestErrorIgnored()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, TestTarget.Current);

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // run...
            TestTarget.Current.Reset();
            logger.Error("Foobar");

            // check...
            Assert.AreEqual(0, TestTarget.Current.NumWritten);
        }

        [TestMethod]
        public void TestIsFatalEnabled()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();

            // get a logger...
            var logger = LogManager.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsFatalEnabled);
        }

        [TestMethod]
        public void TestIsTraceEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Debug, LogLevel.Fatal, TestTarget.Current);

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsTrue(loggable.IsDebugEnabled());
            Assert.IsTrue(loggable.IsInfoEnabled());
            Assert.IsTrue(loggable.IsWarnEnabled());
            Assert.IsTrue(loggable.IsErrorEnabled());
            Assert.IsTrue(loggable.IsFatalEnabled());
        }

        [TestMethod]
        public void TestIsDebugEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, TestTarget.Current);

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsFalse(loggable.IsDebugEnabled());
            Assert.IsTrue(loggable.IsInfoEnabled());
            Assert.IsTrue(loggable.IsWarnEnabled());
            Assert.IsTrue(loggable.IsErrorEnabled());
            Assert.IsTrue(loggable.IsFatalEnabled());
        }

        [TestMethod]
        public void TestIsInfoEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Warn, LogLevel.Fatal, TestTarget.Current);

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsFalse(loggable.IsDebugEnabled());
            Assert.IsFalse(loggable.IsInfoEnabled());
            Assert.IsTrue(loggable.IsWarnEnabled());
            Assert.IsTrue(loggable.IsErrorEnabled());
            Assert.IsTrue(loggable.IsFatalEnabled());
        }

        [TestMethod]
        public void TestIsWarnEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, TestTarget.Current);

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsFalse(loggable.IsDebugEnabled());
            Assert.IsFalse(loggable.IsInfoEnabled());
            Assert.IsFalse(loggable.IsWarnEnabled());
            Assert.IsTrue(loggable.IsErrorEnabled());
            Assert.IsTrue(loggable.IsFatalEnabled());
        }

        [TestMethod]
        public void TestIsErrorEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();
            LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, TestTarget.Current);

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsFalse(loggable.IsDebugEnabled());
            Assert.IsFalse(loggable.IsInfoEnabled());
            Assert.IsFalse(loggable.IsWarnEnabled());
            Assert.IsFalse(loggable.IsErrorEnabled());
            Assert.IsTrue(loggable.IsFatalEnabled());
        }

        [TestMethod]
        public void TestIsFatalEnabledForLoggable()
        {
            LogManager.Reset();
            LogManager.DefaultConfiguration.ClearTargets();

            // get a loggable...
            var loggable = new TestLoggable();

            // check...
            Assert.IsFalse(loggable.IsTraceEnabled());
            Assert.IsFalse(loggable.IsDebugEnabled());
            Assert.IsFalse(loggable.IsInfoEnabled());
            Assert.IsFalse(loggable.IsWarnEnabled());
            Assert.IsFalse(loggable.IsErrorEnabled());
            Assert.IsFalse(loggable.IsFatalEnabled());
        }
    }
}
