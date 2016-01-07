using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !REF_ASSM 
using System.Net.Http;
#endif
namespace MetroLog.Targets
{
    public class HttpClientEventArgs : EventArgs
    {
#if !REF_ASSM 
        public HttpClient Client { get; private set; }

        public HttpClientEventArgs(HttpClient client)
        {
        }
#endif
    }
}
