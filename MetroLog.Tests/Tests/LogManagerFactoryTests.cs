using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Xunit;

namespace MetroLog.Tests.Tests
{
    public class LogManagerFactoryTests
    {
        [Fact]
        public void InitalizeCallsSetDefault()
        {
            bool wasCalled = false;
           // using (ShimsContext.Create())
            //{
             //   ShimLogManagerFactoryBase.SetDefaultLogManagerILogManager = lm => wasCalled = true;

                //LogManagerFactory.Initialize();
            //}

            Assert.True(wasCalled);
        }
    }
}
