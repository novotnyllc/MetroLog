using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Targets
{
    class LogEventInfoItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var item = (LogEventInfoItem)value;

            writer.WriteStartObject();
            writer.WritePropertyName("ItemId");
            writer.WriteValue(item.ItemId);
            writer.WritePropertyName("SequenceId");
            writer.WriteValue(item.SequenceId);
            writer.WritePropertyName("TimeStamp");
            writer.WriteValue(new DateTimeOffset(item.DateTimeUtc));
            writer.WritePropertyName("Level");
            writer.WriteValue(item.Level.ToString());
            writer.WritePropertyName("Logger");
            writer.WriteValue(item.Logger);
            writer.WritePropertyName("Message");
            writer.WriteValue(item.Message);

            // exception?
            writer.WritePropertyName("ExceptionWrapper");
            var wrapper = item.GetExceptionWrapper();
            if (wrapper != null)
                writer.WriteRawValue(wrapper.ToJson());
            else
                writer.WriteValue((object)null);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
