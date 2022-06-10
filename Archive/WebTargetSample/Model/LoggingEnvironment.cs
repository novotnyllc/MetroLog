using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTargetSample.Model
{
    public class LoggingEnvironment
    {
        public Dictionary<string, object> Values { get; private set; }

        public LoggingEnvironment()
        {
            this.Values = new Dictionary<string, object>();
        }
    }
}