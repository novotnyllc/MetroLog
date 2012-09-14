using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroLog.Internal;
using Xunit;

namespace MetroLog.Tests
{
    public class LogManagerTests
    {
        [Fact]
        public void TestGetLogger()
        {
            var manager = new LogManager(new LoggingEnvironment(), new LoggingConfiguration());

            var logger = manager.GetLogger("Foobar");
            Assert.NotNull(logger);
        }

        [Fact]
        public void TestGetLoggerByType()
        {
            var manager = new LogManager(new LoggingEnvironment(), new LoggingConfiguration());

            var logger = manager.GetLogger<LogManagerTests>();
            Assert.NotNull(logger);
        }

        [Fact]
        public void TestLoggingEnvironment()
        {
            var manager = new LogManager(new LoggingEnvironment(), new LoggingConfiguration());
            Assert.NotNull(manager.LoggingEnvironment);
        }

        [Fact]
        public void TestLoggerCreated()
        {
            bool called = false;
            var manager = new LogManager(new LoggingEnvironment(), new LoggingConfiguration());
            manager.LoggerCreated += (sender, args) =>
            {
                called = true;
            };

            // call...
            var logger = manager.GetLogger<LogManagerTests>();

            // check...
            Assert.True(called);
        }
    }
}
