using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog.Tests
{
    class TestTarget : SyncTarget
    {
        internal int NumWritten { get; private set; }
        internal LogEventInfo LastWritten { get; private set; }
        
		public TestTarget()
            : base(new SingleLineLayout())
		{
		}
			

        protected override void Write(LogWriteContext context, LogEventInfo entry)
        {
            NumWritten++;
            LastWritten = entry;
        }
    }
}
