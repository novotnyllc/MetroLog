using MetroLog;
using MetroLog.Packaging;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Win8Sample
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LogSamplePage : Win8Sample.Common.LayoutAwarePage
    {
        private ILogger Log { get; set; }

        private SQLiteTarget SQLiteTarget { get; set; }

        private bool DoFileStreaming { get; set; }
        private bool DoJsonPost { get; set; }
        private bool DoSqlite { get; set; }

        private IStorageFile FileToShare { get; set; }

        public LogSamplePage()
        {
            this.InitializeComponent();

            // you can either initialize the logger like this:
            //      private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LogSamplePage>();
            // ...or with the extension method...
            this.Log = this.GetLogger();

            // you could even add Log as a property your LayoutAwarePage class...

            // setup...
            this.SetupSharing();
        }

        private void SetupSharing()
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }

        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var data = args.Request.Data;
            data.Properties.Title = "Diagnostic Information";
            data.Properties.Description = "Email to <your support email address here>";
            data.SetStorageItems(new List<IStorageItem>() { this.FileToShare });
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // set...
            this.labelPath.Text = "Error log files are written to: " + ApplicationData.Current.LocalFolder.Path;
        }

        private void HandleTrace(object sender, RoutedEventArgs e)
        {
            this.Log.Trace("This is a trace message.");
        }

        private void HandleDebug(object sender, RoutedEventArgs e)
        {
            this.Log.Debug("This is a debug message.");
        }

        private void HandleInfo(object sender, RoutedEventArgs e)
        {
            this.Log.Info("This is an info message.");
        }

        private void HandleWarn(object sender, RoutedEventArgs e)
        {
            this.Log.Warn("This is a warning message.");
        }

        private void HandleError(object sender, RoutedEventArgs e)
        {
            try
            {
                // fake an error...
                this.DoMagic();
            }
            catch (Exception ex)
            {
                this.Log.Error("This is an error message.", ex);
            }
        }

        private void DoMagic()
        {
            throw new NotImplementedException();
        }

        private void HandleFatal(object sender, RoutedEventArgs e)
        {
            // the idea here is to invoke the global error handler...
            throw new InvalidOperationException("Bang.");
        }

        private void HandleRegisterStreamingTarget(object sender, RoutedEventArgs e)
        {
            this.DoFileStreaming = true;
            var settings = CreateNewSettings();

            this.Log = LogManagerFactory.CreateLogManager(settings).GetLogger<LogSamplePage>();

            // set...
            this.buttonFileStreaming.IsEnabled = false;
        }

        private void HandleRegisterJsonPostTarget(object sender, RoutedEventArgs e)
        {
            this.DoJsonPost = true;
            var settings = CreateNewSettings();
            

            this.Log = LogManagerFactory.CreateLogManager(settings).GetLogger<LogSamplePage>();

            // set...
            this.buttonJsonPost.IsEnabled = false;
        }

        private void HandleRegisterSQLiteTarget(object sender, RoutedEventArgs e)
        {
            this.DoSqlite = true;
            var settings = CreateNewSettings();

            // reset...
            this.Log = LogManagerFactory.CreateLogManager(settings).GetLogger<LogSamplePage>();

            // set...
            this.buttonSQLite.IsEnabled = false;
            this.buttonReadSQLite.IsEnabled = true;
        }

        private LoggingConfiguration CreateNewSettings()
        {
            var settings = LogManagerFactory.CreateLibraryDefaultSettings();
            if (this.DoFileStreaming)
                settings.AddTarget(LogLevel.Trace, LogLevel.Fatal, new FileStreamingTarget());
            if (this.DoJsonPost)
                settings.AddTarget(LogLevel.Trace, LogLevel.Fatal, new JsonPostTarget(5, new Uri("http://localhost/metrologweb/ReceiveLogEntries.ashx")));
            if (this.DoSqlite)
            {
                this.SQLiteTarget = new SQLiteTarget();
                settings.AddTarget(LogLevel.Trace, LogLevel.Fatal, this.SQLiteTarget);
            }

            // return...
            return settings;
        }

        private async void HandleReadSQLiteValues(object sender, RoutedEventArgs e)
        {
            // read some values back...
            var query = new LogReadQuery();
            query.SetLevels(LogLevel.Trace, LogLevel.Fatal);
            var file = await this.SQLiteTarget.PackageToTempFileAsync(query);

            // replace...
            if (this.FileToShare != null)
                await this.FileToShare.DeleteAsync();
            this.FileToShare = file;

            // share...
            DataTransferManager.ShowShareUI();
        }
    }
}
