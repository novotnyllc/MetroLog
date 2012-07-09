using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Tests
{
    public class TestLoggable : ILoggable
    {
        public void DoMagic()
        {
            this.Info("In this case, Info is an extension method...");

            var buf = "like this";
            this.Warn("You can also use formatting, {0}", buf);
        }
    }
}
