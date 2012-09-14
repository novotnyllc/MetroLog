using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MetroLog.Tests
{
    public class JsonTests
    {
        [Fact]
        public void TestLogEventInfoToJson()
        {
            var log = new LogEventInfo(LogLevel.Info, "foobar", "barfoo", null);
            var json = log.ToJson();

            // check...
            Assert.False(string.IsNullOrEmpty(json));
        }

        [Fact]
        public void TestLogEventInfoWithExceptionToJson()
        {
            Exception theEx = null;
            try
            {
                throw new InvalidOperationException("Testing.");
            }
            catch (Exception ex)
            {
                theEx = ex;
            }

            // create...
            var log = new LogEventInfo(LogLevel.Info, "foobar", "barfoo", theEx);
            var json = log.ToJson();

            // check...
            Assert.False(string.IsNullOrEmpty(json));
        }

        [Fact]
        public void TestLoggingEnvironmentToJson()
        {
            var environment = new LoggingEnvironment();
            var json = environment.ToJson();

            // check...
            Assert.False(string.IsNullOrEmpty(json));
        }
    }
}
