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
        private Tuple<ILogManager, TestTarget> CreateWithLevel(LogLevel min, LogLevel max)
        {
            var testTarget = new TestTarget();

            var config = new LoggingConfiguration();
            config.AddTarget(min, max, testTarget);

            return Tuple.Create<ILogManager, TestTarget>(new LogManager(config), testTarget);
        }

        [TestMethod]
        public void TestIsTraceEnabled()
        {
            
            var target = CreateWithLevel(LogLevel.Debug, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsTrue(logger.IsDebugEnabled);
            Assert.IsTrue(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public async Task TestTraceIgnored()
        {
            var target = CreateWithLevel(LogLevel.Debug, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.TraceAsync("Foobar");

            // check...
            Assert.AreEqual(0, target.Item2.NumWritten);
        }

        [TestMethod]
        public void TestIsDebugEnabled()
        {
            var target = CreateWithLevel(LogLevel.Info, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsTrue(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public async Task TestDebugIgnored()
        {
            var target = CreateWithLevel(LogLevel.Info, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.DebugAsync("Foobar");

            // check...
            Assert.AreEqual(0, target.Item2.NumWritten);
        }

        [TestMethod]
        public void TestIsInfoEnabled()
        {
            var target = CreateWithLevel(LogLevel.Warn, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsTrue(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public async Task TestInfoIgnored()
        {
            var target = CreateWithLevel(LogLevel.Warn, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.InfoAsync("Foobar");

            // check...
            Assert.AreEqual(0, target.Item2.NumWritten);
        }

        [TestMethod]
        public void TestIsWarnEnabled()
        {
            var target = CreateWithLevel(LogLevel.Error, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsWarnEnabled);
            Assert.IsTrue(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public async Task TestWarnIgnored()
        {
            var target = CreateWithLevel(LogLevel.Error, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.WarnAsync("Foobar");
            
            // check...
            Assert.AreEqual(0, target.Item2.NumWritten);
        }

        [TestMethod]
        public void TestIsErrorEnabled()
        {
            var target = CreateWithLevel(LogLevel.Fatal, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsTrue(logger.IsFatalEnabled);
        }

        [TestMethod]
        public async Task TestErrorIgnored()
        {
            var target = CreateWithLevel(LogLevel.Fatal, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.ErrorAsync("Foobar");

            // check...
            Assert.AreEqual(0, target.Item2.NumWritten);
        }

        [TestMethod]
        public void TestIsFatalEnabled()
        {
            var target = new LogManager(new LoggingConfiguration());

            // get a logger...
            var logger = target.GetLogger("foo");

            // check...
            Assert.IsFalse(logger.IsTraceEnabled);
            Assert.IsFalse(logger.IsDebugEnabled);
            Assert.IsFalse(logger.IsInfoEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsErrorEnabled);
            Assert.IsFalse(logger.IsFatalEnabled);
        }

        //[TestMethod]
        //public void TestIsTraceEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Debug, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsTrue(loggable.IsDebugEnabled());
        //    Assert.IsTrue(loggable.IsInfoEnabled());
        //    Assert.IsTrue(loggable.IsWarnEnabled());
        //    Assert.IsTrue(loggable.IsErrorEnabled());
        //    Assert.IsTrue(loggable.IsFatalEnabled());
        //}

        //[TestMethod]
        //public void TestIsDebugEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsFalse(loggable.IsDebugEnabled());
        //    Assert.IsTrue(loggable.IsInfoEnabled());
        //    Assert.IsTrue(loggable.IsWarnEnabled());
        //    Assert.IsTrue(loggable.IsErrorEnabled());
        //    Assert.IsTrue(loggable.IsFatalEnabled());
        //}

        //[TestMethod]
        //public void TestIsInfoEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Warn, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsFalse(loggable.IsDebugEnabled());
        //    Assert.IsFalse(loggable.IsInfoEnabled());
        //    Assert.IsTrue(loggable.IsWarnEnabled());
        //    Assert.IsTrue(loggable.IsErrorEnabled());
        //    Assert.IsTrue(loggable.IsFatalEnabled());
        //}

        //[TestMethod]
        //public void TestIsWarnEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsFalse(loggable.IsDebugEnabled());
        //    Assert.IsFalse(loggable.IsInfoEnabled());
        //    Assert.IsFalse(loggable.IsWarnEnabled());
        //    Assert.IsTrue(loggable.IsErrorEnabled());
        //    Assert.IsTrue(loggable.IsFatalEnabled());
        //}

        //[TestMethod]
        //public void TestIsErrorEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, TestTarget.Current);



        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsFalse(loggable.IsDebugEnabled());
        //    Assert.IsFalse(loggable.IsInfoEnabled());
        //    Assert.IsFalse(loggable.IsWarnEnabled());
        //    Assert.IsFalse(loggable.IsErrorEnabled());
        //    Assert.IsTrue(loggable.IsFatalEnabled());
        //}

        //[TestMethod]
        //public void TestIsFatalEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.IsFalse(loggable.IsTraceEnabled());
        //    Assert.IsFalse(loggable.IsDebugEnabled());
        //    Assert.IsFalse(loggable.IsInfoEnabled());
        //    Assert.IsFalse(loggable.IsWarnEnabled());
        //    Assert.IsFalse(loggable.IsErrorEnabled());
        //    Assert.IsFalse(loggable.IsFatalEnabled());
        //}
    }
}
