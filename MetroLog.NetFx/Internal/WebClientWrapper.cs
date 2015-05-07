using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    public class WebClientWrapper : IWebClientWrapper
    {
        public WebClientWrapper()
        {
        }

        public void UploadString(Uri uri, IDictionary<HttpRequestHeader, string> headers, string message)
        {
            using (var client = new WebClient())
            {
                foreach (var header in headers)
                {
                    client.Headers.Add(header.Key, header.Value);
                }

                client.Encoding = this.Encoding;
                client.UploadString(uri, message);
            }
        }

        public async Task UploadStringAsync(Uri uri, Dictionary<HttpRequestHeader, string> headers, string message)
        {
            using (var client = new WebClient())
            {
                foreach (var header in headers)
                {
                    client.Headers.Add(header.Key, header.Value);
                }

                client.Encoding = this.Encoding;
                await client.UploadStringTaskAsync(uri, message);
            }
        }

        public Encoding Encoding { get; set; }
    }
}