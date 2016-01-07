using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    public class WebClientWrapper : IWebClientWrapper
    {
        public bool HasInternetConnection
        {
            get
            {
                // TODO: Implement
                return true;
            }
        }

        public void UploadString(Uri uri, IDictionary<HttpRequestHeader, string> headers, string message)
        {
            var webClient = new WebClient();
            webClient.Encoding = this.Encoding;

            foreach (var header in headers)
            {
                webClient.Headers[header.Key] = header.Value;
            }

            webClient.UploadStringAsync(uri, message);
        }

        public async Task UploadStringAsync(Uri uri, Dictionary<HttpRequestHeader, string> headers, string message)
        {
            var webClient = new WebClient();
            webClient.Encoding = this.Encoding;

            foreach (var header in headers)
            {
                webClient.Headers[header.Key] = header.Value;
            }

            var tcs = new TaskCompletionSource<string>();

            webClient.UploadStringCompleted += (sender, args) =>
               {
                   if (args.Error == null)
                   {
                       tcs.SetResult(args.Result);
                   }
                   else
                   {
                       tcs.SetException(args.Error);
                   }
               };

            webClient.UploadStringAsync(uri, message);

            await tcs.Task;
        }

        public Encoding Encoding { get; set; }
    }
}