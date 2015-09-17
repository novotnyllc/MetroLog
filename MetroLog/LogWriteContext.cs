using CrossPlatformAdapter;


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
            environment = PlatformAdapter.Current.Resolve<ILoggingEnvironment>();
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
