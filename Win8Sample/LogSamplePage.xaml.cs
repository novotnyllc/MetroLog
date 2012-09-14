using MetroLog;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Win8Sample.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<LogSamplePage>();

        public LogSamplePage()
        {
            this.InitializeComponent();
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
            this.Log.Debug("This is a trace message.");
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
            throw new NotImplementedException("Bang.");
        }

        private void HandleRegisterStreamingTarget(object sender, RoutedEventArgs e)
        {
            var settings = LogManagerFactory.CreateLibraryDefaultSettings();
            settings.AddTarget(LogLevel.Debug, LogLevel.Fatal, new FileStreamingTarget());

            

            this.Log = LogManagerFactory.CreateLogManager(settings).GetLogger<LogSamplePage>();

            // set...
            this.buttonFileStreaming.IsEnabled = false;
        }

        private void HandleRegisterJsonPostTarget(object sender, RoutedEventArgs e)
        {
            var settings = LogManagerFactory.CreateLibraryDefaultSettings();
            settings.AddTarget(LogLevel.Debug, LogLevel.Fatal,
                new JsonPostTarget(5, new Uri("http://localhost/metrologweb/receivelogentries.ashx")));


            this.Log = LogManagerFactory.CreateLogManager(settings).GetLogger<LogSamplePage>();

            // set...
            this.buttonJsonPost.IsEnabled = false;
        }
    }
}
