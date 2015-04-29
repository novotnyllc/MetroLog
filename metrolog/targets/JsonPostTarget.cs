using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MetroLog.Internal;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public class JsonPostTarget : BufferedTarget
    {
        public ILoggingEnvironment LoggingEnvironment { get; private set; }

        public Uri Uri { get; private set; }

        public event EventHandler<HttpClientEventArgs> BeforePost;

        public JsonPostTarget() : base(new NullLayout(), 1)
        {
            
        }

        public JsonPostTarget(int threshold, Uri uri, ILoggingEnvironment loggingEnvironment)
            : this(threshold, uri, loggingEnvironment, new NullLayout())
        {
        }

        public JsonPostTarget(int threshold, Uri uri, Layout layout)
            : this(threshold, uri, null, layout)
        {
            
        }

        public JsonPostTarget(int threshold, Uri uri, ILoggingEnvironment loggingEnvironment, Layout layout)
            : base(layout, threshold)
        {
            this.Uri = uri;

            if (loggingEnvironment != null)
            {
                this.LoggingEnvironment = loggingEnvironment;
            }
            else
            {
                this.LoggingEnvironment = PlatformAdapter.Resolve<ILoggingEnvironment>();
            }
        }

        protected override async Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush)
        {
            // create a json object...
            var wrapper = new JsonPostWrapper(this.LoggingEnvironment, toFlush);
            var json = wrapper.ToJson();

            // send...
            var client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType.MediaType = "text/json";

            // call...
            this.OnBeforePost(new HttpClientEventArgs(client));

            // send...
            await client.PostAsync(this.Uri, content);
        }

        protected virtual void OnBeforePost(HttpClientEventArgs args)
        {
            if (this.BeforePost != null)
                this.BeforePost(this, args);
        }
    }
}
