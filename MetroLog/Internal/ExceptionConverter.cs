using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Internal
{
    public class ExceptionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var ex = (Exception)value;

                // ok...
                writer.WriteStartObject();
                writer.WritePropertyName("Type");
                writer.WriteValue(ex.GetType().AssemblyQualifiedName);
                writer.WritePropertyName("Data");
                if (ex.Data != null && ex.Data.Count > 0)
                {
                    writer.WriteStartObject();
                    foreach (var key in ex.Data.Keys)
                    {
                        writer.WritePropertyName(key.ToString());
                        writer.WriteValue(ex.Data[key]);
                    }
                    writer.WriteEndObject();
                }
                else
                    writer.WriteValue((string)null);
                writer.WritePropertyName("AsString");
                writer.WriteValue(ex.ToString());
                writer.WritePropertyName("Hresult");
                writer.WriteValue(ex.HResult.ToString("x8"));
                writer.WriteEndObject();
            }
            else
                writer.WriteRawValue(null);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
