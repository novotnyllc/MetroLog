using System;
using System.Net.Http;

using MetroLog.Internal;

namespace MetroLog.Targets
{
    public class HttpClientEventArgs : EventArgs
    {
        public HttpClient Client { get; private set; }

        public HttpClientEventArgs(IWebClientWrapper client)
        {
        }
    }
}
