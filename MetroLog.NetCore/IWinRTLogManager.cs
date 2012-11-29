using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MetroLog
{
    public interface IWinRTLogManager : ILogManager
    {
        Task<IStorageFile> GetCompressedLogFile();
        Task ShareLogFile(string title, string description);

    }
}
