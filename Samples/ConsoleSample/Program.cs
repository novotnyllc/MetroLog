using System;
using System.Diagnostics;
using MetroLog;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            SomeMagicClass c = null;
            try
            {
                // Initialize MetroLog using the defaults
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, 
                    new JsonPostTarget(5, new Uri("http://metrolog.thomasgalliker.ch/index.php"), new NullLayout()));
                ILogManager logManager = LogManagerFactory.DefaultLogManager;

                // Inject the ILogManager manually
                c = new SomeMagicClass(logManager);
                c.DoMagic();
            }
            finally
            {
                // If we have a debugger, stop so you can see the output
                if (Debugger.IsAttached)
                    while (true)
                    {
                        Console.ReadKey();
                        if (c != null)
                        {
                            c.DoMagic();
                        }
                    }
            }
        }
    }


    class SomeMagicClass
    {
        private readonly ILogger _log;
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
            catch(Exception e)
            {
                _log.Error("Bad thing!", e);
            }
        }
    }

}
