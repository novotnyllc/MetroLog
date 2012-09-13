using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class HttpClientEventArgs : EventArgs
    {
        public HttpClient Client { get; private set; }

        public HttpClientEventArgs(HttpClient client)
        {
        }
    }
}
