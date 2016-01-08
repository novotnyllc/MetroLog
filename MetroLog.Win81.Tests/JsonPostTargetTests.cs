using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Targets;
using Xunit;

namespace MetroLog.Win81.Tests
{
    public class JsonPostTargetTests
    {
        [Fact]
        public void SerializeException()
        {
            var eventInfo = new LogEventInfo(LogLevel.Error, "some logger", "test message", new Exception("error message"));
            var target = new JsonPostWrapper(new LoggingEnvironment(), new[] { eventInfo });

            var json = target.ToJson();

            Assert.NotNull(json);
        }
    }
}
