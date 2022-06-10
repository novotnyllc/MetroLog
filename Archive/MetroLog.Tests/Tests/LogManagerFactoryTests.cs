using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MetroLog.Tests.Tests
{
    public class LogManagerBaseFactoryTests
    {
       // [Fact]
        public void InitalizeCallsSetDefault()
        {
            bool wasCalled = false;
           // using (ShimsContext.Create())
            //{
             //   ShimLogManagerBaseFactoryBase.SetDefaultLogManagerBaseILogManager = lm => wasCalled = true;

                //LogManagerBaseFactory.Initialize();
            //}

            Assert.True(wasCalled);
        }
    }
}
