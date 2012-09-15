using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using WebTargetSample.Model;

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
            string json = null;
            using (var reader = new StreamReader(context.Request.InputStream))
                json = reader.ReadToEnd();

            // deserialize...
            var wrapper = JsonConvert.DeserializeObject<JsonPostWrapper>(json);

            //Console.WriteLine(info);
            Debug.WriteLine(wrapper);
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