using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public interface ISuspendNotify
    {
        void Suspending(LogWriteContext context);
    }
}
