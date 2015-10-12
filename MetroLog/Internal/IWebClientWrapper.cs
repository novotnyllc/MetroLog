
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    public interface IWebClientWrapper
    {
        bool HasInternetConnection { get; }

        void UploadString(Uri uri, IDictionary<HttpRequestHeader, string> headers, string message);

        Task UploadStringAsync(Uri uri, Dictionary<HttpRequestHeader, string> headers, string message);

        Encoding Encoding { get; set; }
    }
}
