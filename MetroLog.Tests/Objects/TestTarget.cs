using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace MetroLog.Tests
{
    internal class TestTarget : Target
    {
        internal int NumWritten { get; private set; }
        internal LogEventInfo LastWritten { get; private set; }

		private static TestTarget _current = new TestTarget();
				
		private TestTarget()
            : base(new SingleLineLayout())
		{
		}
						
		internal static TestTarget Current
		{
			get
			{
				if(_current == null)
					throw new ObjectDisposedException("TestTarget");
				return _current;
			}
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
