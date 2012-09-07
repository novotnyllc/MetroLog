using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog.Tests
{
    internal class TestTarget : SyncTarget
    {
        internal int NumWritten { get; private set; }
        internal LogEventInfo LastWritten { get; private set; }
        
		public TestTarget()
            : base(new SingleLineLayout())
		{
		}
			

        protected override void Write(LogEventInfo entry)
        {
            this.NumWritten++;
            this.LastWritten = entry;
        }

        internal void Reset()
        {
            this.NumWritten = 0;
            this.LastWritten = null;
        }
    }
}
