using System;
using System.Diagnostics;
using System.Drawing;
using MetroLog;
using MetroLog.Targets;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iOSClassicSample
{
    public partial class iOSClassicSampleViewController : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public iOSClassicSampleViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

          
                // Initialize MetroLog using the defaults
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
                ILogManager logManager = LogManagerFactory.DefaultLogManager;

                // Inject the ILogManager manually
                SomeMagicClass c = new SomeMagicClass(logManager);
                c.DoMagic();
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }


    class SomeMagicClass
    {
        readonly ILogger _log;
        public SomeMagicClass(ILogManager logManager)
        {
            _log = logManager.GetLogger<SomeMagicClass>();
        }

        public void DoMagic()
        {
            // Log something interesting
            _log.Info("Information - We are about to do magic!");
            _log.Warn("Warning!");
            _log.Trace("Trace some data.");
            _log.Error("Something bad happened at {0}", DateTime.Now);
            _log.Fatal("Danger Will Robinson!");

            try
            {
                throw new InvalidOperationException("Test");
            }
            catch (Exception e)
            {
                _log.Error("Bad thing!", e);
            }
        }
    }

}
