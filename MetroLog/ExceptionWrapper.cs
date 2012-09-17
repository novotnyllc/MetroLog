using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.TypeName = ex.GetType().AssemblyQualifiedName;
            this.AsString = ex.ToString();
            this.Hresult = ex.HResult;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
