using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using Xunit;

namespace MetroLog.Tests
{
    public class LevelTests
    {
        Tuple<ILogManager, TestTarget> CreateWithLevel(LogLevel min, LogLevel max)
        {
            var testTarget = new TestTarget();

            var config = new LoggingConfiguration();
            config.AddTarget(min, max, testTarget);

            return Tuple.Create<ILogManager, TestTarget>(new LogManager(config), testTarget);
        }

        [Fact]
        public void TestIsTraceEnabled()
        {
            
            var target = CreateWithLevel(LogLevel.Debug, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.True(logger.IsDebugEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsWarnEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
        }

        [Fact]
        public async Task TestTraceIgnored()
        {
            var target = CreateWithLevel(LogLevel.Debug, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.TraceAsync("Foobar");

            // check...
            Assert.Equal(0, target.Item2.NumWritten);
        }

        [Fact]
        public void TestIsDebugEnabled()
        {
            var target = CreateWithLevel(LogLevel.Info, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.False(logger.IsDebugEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsWarnEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
        }

        [Fact]
        public async Task TestDebugIgnored()
        {
            var target = CreateWithLevel(LogLevel.Info, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.DebugAsync("Foobar");

            // check...
            Assert.Equal(0, target.Item2.NumWritten);
        }

        [Fact]
        public void TestIsInfoEnabled()
        {
            var target = CreateWithLevel(LogLevel.Warn, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.False(logger.IsDebugEnabled);
            Assert.False(logger.IsInfoEnabled);
            Assert.True(logger.IsWarnEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
        }

        [Fact]
        public async Task TestInfoIgnored()
        {
            var target = CreateWithLevel(LogLevel.Warn, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.InfoAsync("Foobar");

            // check...
            Assert.Equal(0, target.Item2.NumWritten);
        }

        [Fact]
        public void TestIsWarnEnabled()
        {
            var target = CreateWithLevel(LogLevel.Error, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.False(logger.IsDebugEnabled);
            Assert.False(logger.IsInfoEnabled);
            Assert.False(logger.IsWarnEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
        }

        [Fact]
        public async Task TestWarnIgnored()
        {
            var target = CreateWithLevel(LogLevel.Error, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.WarnAsync("Foobar");
            
            // check...
            Assert.Equal(0, target.Item2.NumWritten);
        }

        [Fact]
        public void TestIsErrorEnabled()
        {
            var target = CreateWithLevel(LogLevel.Fatal, LogLevel.Fatal);

            // get a logger...
            var logger = target.Item1.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.False(logger.IsDebugEnabled);
            Assert.False(logger.IsInfoEnabled);
            Assert.False(logger.IsErrorEnabled);
            Assert.False(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
        }

        [Fact]
        public async Task TestErrorIgnored()
        {
            var target = CreateWithLevel(LogLevel.Fatal, LogLevel.Fatal);

            // get a logger...
            var logger = (Logger)target.Item1.GetLogger("foo");

            // run...
            await logger.ErrorAsync("Foobar");

            // check...
            Assert.Equal(0, target.Item2.NumWritten);
        }

        [Fact]
        public void TestIsFatalEnabled()
        {
            var target = new LogManager(new LoggingConfiguration());

            // get a logger...
            var logger = target.GetLogger("foo");

            // check...
            Assert.False(logger.IsTraceEnabled);
            Assert.False(logger.IsDebugEnabled);
            Assert.False(logger.IsInfoEnabled);
            Assert.False(logger.IsErrorEnabled);
            Assert.False(logger.IsErrorEnabled);
            Assert.False(logger.IsFatalEnabled);
        }

        //[Fact]
        //public void TestIsTraceEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Debug, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.True(loggable.IsDebugEnabled());
        //    Assert.True(loggable.IsInfoEnabled());
        //    Assert.True(loggable.IsWarnEnabled());
        //    Assert.True(loggable.IsErrorEnabled());
        //    Assert.True(loggable.IsFatalEnabled());
        //}

        //[Fact]
        //public void TestIsDebugEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.False(loggable.IsDebugEnabled());
        //    Assert.True(loggable.IsInfoEnabled());
        //    Assert.True(loggable.IsWarnEnabled());
        //    Assert.True(loggable.IsErrorEnabled());
        //    Assert.True(loggable.IsFatalEnabled());
        //}

        //[Fact]
        //public void TestIsInfoEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Warn, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.False(loggable.IsDebugEnabled());
        //    Assert.False(loggable.IsInfoEnabled());
        //    Assert.True(loggable.IsWarnEnabled());
        //    Assert.True(loggable.IsErrorEnabled());
        //    Assert.True(loggable.IsFatalEnabled());
        //}

        //[Fact]
        //public void TestIsWarnEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, TestTarget.Current);

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.False(loggable.IsDebugEnabled());
        //    Assert.False(loggable.IsInfoEnabled());
        //    Assert.False(loggable.IsWarnEnabled());
        //    Assert.True(loggable.IsErrorEnabled());
        //    Assert.True(loggable.IsFatalEnabled());
        //}

        //[Fact]
        //public void TestIsErrorEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();
        //    LogManager.DefaultConfiguration.AddTarget(LogLevel.Fatal, TestTarget.Current);



        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.False(loggable.IsDebugEnabled());
        //    Assert.False(loggable.IsInfoEnabled());
        //    Assert.False(loggable.IsWarnEnabled());
        //    Assert.False(loggable.IsErrorEnabled());
        //    Assert.True(loggable.IsFatalEnabled());
        //}

        //[Fact]
        //public void TestIsFatalEnabledForLoggable()
        //{
        //    LogManager.Reset();
        //    LogManager.DefaultConfiguration.ClearTargets();

        //    // get a loggable...
        //    var loggable = new TestLoggable();

        //    // check...
        //    Assert.False(loggable.IsTraceEnabled());
        //    Assert.False(loggable.IsDebugEnabled());
        //    Assert.False(loggable.IsInfoEnabled());
        //    Assert.False(loggable.IsWarnEnabled());
        //    Assert.False(loggable.IsErrorEnabled());
        //    Assert.False(loggable.IsFatalEnabled());
        //}
    }
}
