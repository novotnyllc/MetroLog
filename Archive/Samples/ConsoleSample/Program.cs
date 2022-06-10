using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using System.Diagnostics;
using MetroLog.Targets;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize MetroLog using the defaults
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());
                ILogManager logManager = LogManagerFactory.DefaultLogManager;

                // Inject the ILogManager manually
                SomeMagicClass c = new SomeMagicClass(logManager);
                c.DoMagic();
            }
            finally
            {
                // If we have a debugger, stop so you can see the output
                if (Debugger.IsAttached)
                    Console.ReadKey();
            }
        }
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
            catch(Exception e)
            {
                _log.Error("Bad thing!", e);
            }
        }
    }

}
