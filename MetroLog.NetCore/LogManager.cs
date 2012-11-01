using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using Windows.Storage;

namespace MetroLog
{
    class LogManager : LogManagerBase, IWinRTLogManager
    {
        public LogManager(LoggingConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IStorageFile> GetCompressedLogFile()
        {
            var stream = await GetCompressedLogs();

            if (stream != null)
            {
                // create a temp file
                var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(
                    string.Format("Log - {0}.zip", DateTime.UtcNow.ToString("yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture)), CreationCollisionOption.ReplaceExisting);

                using (var ras = (await file.OpenAsync(FileAccessMode.ReadWrite)).AsStreamForWrite())
                {
                    await stream.CopyToAsync(ras);
                }

                stream.Dispose();

                return file;
            }

            return null;
        }
    }
}
