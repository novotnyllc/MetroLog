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
            var target = new LogManager(new LoggingConfiguration());

            var logger = target.GetLogger("Foobar");
            Assert.NotNull(logger);
        }
    }
}
