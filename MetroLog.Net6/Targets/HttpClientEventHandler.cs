namespace MetroLog.Targets;

public class HttpClientEventArgs : EventArgs
{
    public HttpClientEventArgs(HttpClient client)
    {
        Client = client;
    }

    public HttpClient Client { get; }
}