using System.Collections.Generic;

namespace MetroLog.Internal
{
    /// <summary>
    /// The CrashRecorder is comparable to the Flight Data Recorder of airplanes.
    /// It records all log events - no matter what severity they may have.
    /// In case of a fatal exception (unhandled application crash), the crash recordings are sent
    /// to the log targets which accept log level fatal.
    /// 
    /// Rationale: In case of an application crash it is often necessary to see what happened just before the application has crashed.
    /// What makes the CrashRecorder so powerful: There is no need to configure a debug target to see the in-depth detail of what 
    /// happens in your application. In case of an application crash you receive log entries of all severities if CrashRecorder is enabled.
    /// </summary>
    public interface ICrashRecorder
    {
        void Record(LogEventInfo logEventInfo);

        IEnumerable<LogEventInfo> GetRecords();

        bool IsEnabled { get; set; }
    }
}