using MetroLog.Internal;

namespace MetroLog
{
    public class LogWriteContext
    {
        private static readonly ILoggingEnvironment environment;

        public LogWriteContext()
        {
        }

        static LogWriteContext()
        {
            environment = PlatformAdapter.Resolve<ILoggingEnvironment>();
        }

        public ILoggingEnvironment Environment
        {
            get
            {
                return environment;
            }
        }

        public bool IsFatalException { get; set; }
    }
}
