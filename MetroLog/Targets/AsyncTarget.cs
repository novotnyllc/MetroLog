using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;

namespace MetroLog.Targets
{
    public abstract class AsyncTarget : Target
    {
        protected AsyncTarget(Layout layout)
            : base(layout)
        {
        }

        protected internal override sealed void Write(LogEventInfo entry)
        {
            // send in a cloned copy so that we're a little safer from this
            // changing underneath us...
            var cloned = entry.Clone();
            var task = Task.Factory.StartNew(() => WriteAsync(cloned));

            // set..
            entry.SetTask(task);
            return;
        }

        protected internal abstract void WriteAsync(LogEventInfo entry);
    }
}
