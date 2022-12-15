using MetroLog.Internal;
using MetroLog.Layouts;
using MetroLog.Operators;

namespace MetroLog.Targets;

public class MemoryTarget : SyncTarget, ILogLister
{
    private readonly int _maxLines;
    private readonly Queue<string> _queue;

    private readonly AsyncLock _lock = new();

    public MemoryTarget(int maxLines = 1024)
        : this(maxLines, new SingleLineLayout())
    {
    }

    public MemoryTarget(int maxLines, Layout layout)
        : base(layout)
    {
        _maxLines = maxLines;
        _queue = new Queue<string>(maxLines);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, _queue);
    }

    public async Task<List<string>> GetLogList()
    {
        using (await _lock.LockAsync().ConfigureAwait(false))
        {
            return await Task.Run(
                () => new List<string>(_queue)).ConfigureAwait(false);
        }
    }

    protected override async void Write(LogWriteContext context, LogEventInfo entry)
    {
        using (await _lock.LockAsync().ConfigureAwait(false))
        {
            if (_queue.Count == _maxLines)
            {
                _queue.Dequeue();
            }

            _queue.Enqueue(Layout.GetFormattedString(context, entry));
        }
    }
}