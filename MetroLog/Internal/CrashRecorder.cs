using System;
using System.Collections.Generic;

namespace MetroLog.Internal
{
    internal class CrashRecorder : ICrashRecorder
    {
        private readonly object lockObject = new object();
        private readonly FixedSizedQueue<LogEventInfo> queue;
        private bool isEnabled;

        public CrashRecorder(int size)
        {
            this.queue = new FixedSizedQueue<LogEventInfo>(size);
        }

        public void Record(LogEventInfo logEventInfo)
        {
            lock (this.lockObject)
            {
                if (this.IsEnabled)
                {
                    this.queue.Enqueue(logEventInfo);
                }
                else
                {
                    throw new InvalidOperationException("CrashRecorder is not enabled. Please set IsEnabled=true to record log events.");
                }
            }
        }

        public IEnumerable<LogEventInfo> GetRecords()
        {
            return this.queue;
        }

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                lock (this.lockObject)
                {
                    this.isEnabled = value;
                    if (this.isEnabled == false)
                    {
                        this.queue.Clear();
                    }
                }
            }
        }
    }
}