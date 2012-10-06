extern alias netfx;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            
            // create...
            var log = new LogEventInfo(LogLevel.Info, "foobar", "barfoo", new InvalidOperationException("Testing."));
            var json = log.ToJson();

            // check...
            Assert.False(string.IsNullOrEmpty(json));
        }

        [Fact]
        public void TestLogEventInfoFromJson()
        {
            var json = @"{""SequenceID"":1,""Level"":""Info"",""Logger"":""foobar"",""Message"":""barfoo"",""TimeStamp"":""2012-09-17T06:50:05.1511288+00:00"",""ExceptionWrapper"":null}";
            
            // load...
            var log = FromJson(json);

            // check...
            Assert.Equal(1, log.SequenceID);
            Assert.Equal(LogLevel.Info, log.Level);
            Assert.Equal("foobar", log.Logger);
            Assert.Equal("barfoo", log.Message);
            Assert.Equal(17, log.TimeStamp.Day);
            Assert.Null(log.Exception);
        }

        [Fact]
        public void TestLogEventInfoFromJsonWithException()
        {
            var json = @"{""SequenceID"":1,""Level"":""Info"",""Logger"":""foobar"",""Message"":""barfoo"",""TimeStamp"":""2012-09-17T07:01:55.062637+00:00"",""ExceptionWrapper"":{""TypeName"":""System.InvalidOperationException, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"",""Data"":null,""AsString"":""System.InvalidOperationException: Testing.\r\n   at MetroLog.Tests.JsonTests.TestLogEventInfoWithExceptionToJson() in c:\\Code\\MetroLog\\MetroLog\\MetroLog.Tests\\Tests\\JsonTests.cs:line 28"",""Hresult"":""80131509""}}";

            // load...
            var log = FromJson(json);

            // check...
            Assert.Equal(1, log.SequenceID);
            Assert.Equal(LogLevel.Info, log.Level);
            Assert.Equal("foobar", log.Logger);
            Assert.Equal("barfoo", log.Message);
            Assert.Equal(17, log.TimeStamp.Day);

            // check...
            Assert.Equal(typeof(InvalidOperationException).AssemblyQualifiedName, log.ExceptionWrapper.TypeName);
            Assert.False(string.IsNullOrEmpty(log.ExceptionWrapper.AsString));
            Assert.Equal(80131509, log.ExceptionWrapper.Hresult);
        }

        [Fact]
        public void TestLoggingEnvironmentToJson()
        {
            var environment = new netfx::MetroLog.LoggingEnvironment();
            var json = environment.ToJson();

            // check...
            Assert.False(string.IsNullOrEmpty(json));
        }

        public static LogEventInfo FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LogEventInfo>(json);
        }
    }
}
