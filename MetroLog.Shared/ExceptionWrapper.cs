using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog.Internal;

namespace MetroLog
{
    public class ExceptionWrapper
    {
        public string TypeName { get; set; }
        public string AsString { get; set; }
        public int Hresult { get; set; }

        public ExceptionWrapper()
        {
        }

        internal ExceptionWrapper(Exception ex)
        {
            TypeName = ex.GetType().AssemblyQualifiedName;
            AsString = ex.ToString();
            Hresult = ex.HResult;
        }

        public string ToJson()
        {
            return SimpleJson.SerializeObject(this);
        }
    }
}
