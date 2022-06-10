using MetroLog.Layouts;

namespace MetroLog.Targets;

public class JsonPostTarget : BufferedTarget
{
    public JsonPostTarget(int threshold, Uri uri)
        : this(new NullLayout(), threshold, uri)
    {
    }

    public JsonPostTarget(Layout layout, int threshold, Uri url)
        : base(layout, threshold)
    {
        Url = url;
    }

    public Uri Url { get; }

    public event EventHandler<HttpClientEventArgs>? BeforePost;

    protected override async Task DoFlushAsync(LogWriteContext context, IEnumerable<LogEventInfo> toFlush)
    {
        // create a json object...

        var env = new LoggingEnvironment();
        var wrapper = new JsonPostWrapper(env, toFlush);
        var json = wrapper.ToJson();

        // send...
        var client = new HttpClient();
        var content = new StringContent(json);
        content.Headers.ContentType!.MediaType = "text/json";

        // call...
        OnBeforePost(new HttpClientEventArgs(client));

        // send...
        await client.PostAsync(Url, content);
    }

    protected virtual void OnBeforePost(HttpClientEventArgs args)
    {
        BeforePost?.Invoke(this, args);
    }
}