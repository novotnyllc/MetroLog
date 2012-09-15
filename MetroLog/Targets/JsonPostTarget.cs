using MetroLog.Internal;
using MetroLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    public class JsonPostTarget : BufferedTarget
    {
        public Uri Url { get; private set; }

        public event EventHandler<HttpClientEventArgs> BeforePost;

        public JsonPostTarget(int threshold, Uri uri)
            : this(new NullLayout(), threshold, uri)
        {
        }

        public JsonPostTarget(Layout layout, int threshold, Uri url)
            : base(layout, threshold)
        {
            this.Url = url;
        }

        protected override async Task DoFlushAsync(IEnumerable<LogEventInfo> toFlush)
        {
            // create a json object...

            var env = PlatformAdapter.Resolve<ILoggingEnvironment>();
            var wrapper = new JsonPostWrapper(env, toFlush);
            var json = wrapper.ToJson();

            // send...
            var client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "text/json";

            // call...
            this.OnBeforePost(new HttpClientEventArgs(client));

            // send...
            await client.PostAsync(this.Url, content);
        }

        protected virtual void OnBeforePost(HttpClientEventArgs args)
        {
            if (this.BeforePost != null)
                this.BeforePost(this, args);
        }
    }
}
