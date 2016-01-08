using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;

namespace MetroLog.Internal
{
    partial class LogManager : IWinRTLogManager
    {
       public async Task<IStorageFile> GetCompressedLogFile()
        {
            var stream = await GetCompressedLogs();

            if (stream != null)
            {
                // create a temp file
                var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(
                    $"Log - {DateTime.UtcNow.ToString("yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture)}.zip", CreationCollisionOption.ReplaceExisting);

                using (var ras = (await file.OpenAsync(FileAccessMode.ReadWrite)).AsStreamForWrite())
                {
                    await stream.CopyToAsync(ras);
                }

                stream.Dispose();

                return file;
            }

            return null;
        }

        public Task ShareLogFile(string title, string description)
        {
            var dtm = DataTransferManager.GetForCurrentView();
            
            var tcs = new TaskCompletionSource<object>();
            TypedEventHandler<DataTransferManager, DataRequestedEventArgs> handler = null;
            handler = async (sender, args) =>
                 {
                     args.Request.Data.Properties.Title = title;
                     args.Request.Data.Properties.Description = description;

                     var deferral = args.Request.GetDeferral();

                     try
                     {
                         var file = await GetCompressedLogFile();

                         args.Request.Data.SetStorageItems(new[] { file });
                         
                         tcs.SetResult(true);
                     }
                     catch (Exception ex)
                     {
                         tcs.SetException(ex);
                     }
                     finally
                     {
                         deferral.Complete();

                         dtm.DataRequested -= handler;
                     }
                 };

            dtm.DataRequested += handler;

            DataTransferManager.ShowShareUI();

//            return tcs.Task;

            return Task.FromResult(true);
        }

   
    }
}
