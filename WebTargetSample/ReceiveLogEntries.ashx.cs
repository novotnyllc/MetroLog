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