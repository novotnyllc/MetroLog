#define SHOW_NESTED_INNEREXCEPTIONS

using System;
using System.Collections;
using MetroLog.Internal;

namespace MetroLog
{
    public class ExceptionWrapper
    {
        public ExceptionWrapper()
        {
        }

        internal ExceptionWrapper(Exception ex)
        {
            this.TypeName = ex.GetType().AssemblyQualifiedName;
            this.Message = ex.Message;
            this.Source = ex.Source;
            this.StackTrace = ex.StackTrace;
            this.Hresult = ex.HResult;
            this.Data = ex.Data;

#if SHOW_NESTED_INNEREXCEPTIONS
            if (ex.InnerException != null)
            {
                this.InnerException = new ExceptionWrapper(ex.InnerException);
            }
#else
            this.AsString = ex.ToString();
#endif
        }

        public string TypeName { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public int Hresult { get; set; }

        public IDictionary Data { get; set; }

#if SHOW_NESTED_INNEREXCEPTIONS
        public ExceptionWrapper InnerException { get; set; }
#else
        public string AsString { get; set; }
#endif

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
