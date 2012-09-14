using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTargetSample
{
    /// <summary>
    /// Summary description for ReceiveLogEntries
    /// </summary>
    public class ReceiveLogEntries : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // get the json out...

            // deserialize...

            // dump to the event log...

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}