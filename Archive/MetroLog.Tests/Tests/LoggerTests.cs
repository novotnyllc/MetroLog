using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using Xunit;

namespace MetroLog.Tests
{
    public class LoggerTests
    {
        static Tuple<Logger, TestTarget> CreateTarget()
        {
            var testTarget = new TestTarget();
            var config = new LoggingConfiguration();
            config.AddTarget(LogLevel.Trace, LogLevel.Fatal, testTarget);
            
            return Tuple.Create(new Logger("Foobar", config), testTarget);
        }

        [Fact]
        public async Task TestTrace()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestLog()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.LogAsync(LogLevel.Trace, "Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }


        [Fact]
        public async Task TestDebug()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestInfo()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestWarn()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestError()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestFatal()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, world.");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestTraceWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestLogWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.LogAsync(LogLevel.Trace, "Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestDebugWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestInfoWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestWarnWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestErrorWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestFatalWithException()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, world.", new InvalidOperationException("Foobar"));

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.NotNull(logger.Item2.LastWritten.Exception);
        }

        [Fact]
        public async Task TestTraceWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.TraceAsync("Hello, {0}.", "**foo**");
            

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Trace, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [Fact]
        public async Task TestDebugWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.DebugAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Debug, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [Fact]
        public async Task TestInfoWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.InfoAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Info, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [Fact]
        public async Task TestWarnWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.WarnAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Warn, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [Fact]
        public async Task TestErrorWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.ErrorAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Error, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }

        [Fact]
        public async Task TestFatalWithFormat()
        {
            // run...
            var logger = CreateTarget();
            await logger.Item1.FatalAsync("Hello, {0}.", "**foo**");

            // check...
            Assert.Equal(1, logger.Item2.NumWritten);
            Assert.Equal(LogLevel.Fatal, logger.Item2.LastWritten.Level);
            Assert.Null(logger.Item2.LastWritten.Exception);
            Assert.NotEqual(-1, logger.Item2.LastWritten.Message.IndexOf("**foo**"));
        }
    }
}
