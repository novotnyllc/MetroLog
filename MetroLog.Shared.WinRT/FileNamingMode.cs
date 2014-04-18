using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public enum FileNamingMode
    {
        SingleFile = 0,
        FilePerDate = 1,
        FilePerSession = 2,
        FilePerDateAndSession = 3
    }
}
