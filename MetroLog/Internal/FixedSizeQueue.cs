using System.Collections.Generic;

namespace MetroLog.Internal
{
    public class FixedSizedQueue<T> : Queue<T>
        // TODO: Maybe a concurrent queue would be better here? https://github.com/xunit/xunit/blob/master/src/xunit.execution.wp8/Concurrent/ConcurrentQueue.cs
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            this.Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);

            lock (this.syncObject)
            {
                while (this.Count > this.Size)
                {
                    this.Dequeue();
                }
            }
        }
    }
}