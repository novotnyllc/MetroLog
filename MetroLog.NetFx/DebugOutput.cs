using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    internal sealed class DebugOutput : IDebugOutput
    {
        public void WriteLine(string message)
        {
            // TODO: Determine correct P/Invoke signature
        }

        internal static class SafeImports
        {
            [DllImport("user32.dll")]
            public static extern int OutputWin32(string str);
        }
    }
}
