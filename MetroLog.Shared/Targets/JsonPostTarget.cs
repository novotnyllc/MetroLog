using MetroLog.Internal;
using MetroLog.Layouts;
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
            Url = url;
        }

        protected override async Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush)
        {
#if REF_ASSM
            await Task.Delay(0);
            throw new InvalidOperationException("Cannot use ref assm at runtime");
#else
            // create a json object...

            var env = new LoggingEnvironment();
            var wrapper = new JsonPostWrapper(env, toFlush);
            var json = wrapper.ToJson();

            // send...
            var client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "text/json";

            // call...
            OnBeforePost(new HttpClientEventArgs(client));

            // send...
            await client.PostAsync(Url, content);
#endif
        }

        protected virtual void OnBeforePost(HttpClientEventArgs args)
        {
            BeforePost?.Invoke(this, args);
        }
    }
}
