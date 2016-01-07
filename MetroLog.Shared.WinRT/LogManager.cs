using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;

using MetroLog.Internal;

namespace MetroLog
{
    public class LogManager : LogManagerBase, IWinRTLogManager
    {
        public LogManager(LoggingConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IStorageFile> GetCompressedLogFile()
        {
            var stream = await this.GetCompressedLogs();

            if (stream != null)
            {
                // create a temp file
                var file =
                    await
                    ApplicationData.Current.TemporaryFolder.CreateFileAsync(
                        string.Format("Log - {0}.zip", DateTime.UtcNow.ToString("yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture)),
                        CreationCollisionOption.ReplaceExisting);

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
                        var file = await this.GetCompressedLogFile();

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
            //dtm.DataRequested += dtm_DataRequested;
            DataTransferManager.ShowShareUI();

            //            return tcs.Task;

            return Task.FromResult(true);
        }

        private void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = "Foobar";
            args.Request.Data.SetText("Yay!");
        }
    }
}