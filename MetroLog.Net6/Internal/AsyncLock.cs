using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MetroLog.Internal;

internal class AsyncLock
{
    private readonly Task<Releaser> _releaser;
    private readonly SemaphoreSlim _semaphore;

    public AsyncLock()
    {
        _semaphore = new SemaphoreSlim(1);
        _releaser = Task.FromResult(new Releaser(this));
    }

#if DEBUG
    public Task<Releaser> LockAsync(
        [CallerMemberName] string callingMethod = null,
        [CallerFilePath] string path = null,
        [CallerLineNumber] int line = 0)
    {
        Debug.WriteLine("AsyncLock.LockAsync called by: " + callingMethod + " in file: " + path + " : " + line);
#else
        public Task<Releaser> LockAsync()
        {
#endif
        var wait = _semaphore.WaitAsync();

        return wait.IsCompleted
            ? _releaser
            : wait.ContinueWith(
                (_, state) => new Releaser((AsyncLock)state!),
                this,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
    }

    public readonly struct Releaser : IDisposable
    {
        private readonly AsyncLock _toRelease;

        internal Releaser(AsyncLock toRelease)
        {
            _toRelease = toRelease;
        }

        public void Dispose()
        {
            _toRelease?._semaphore.Release();
        }
    }
}