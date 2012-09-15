using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebTargetSample.Model;

namespace WebTargetSample.Json
{
    public class LoggingEnvironmentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(LoggingEnvironment).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var env = new LoggingEnvironment();
            
            // walk...
            string prop = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                    prop = Convert.ToString(reader.Value);
                else if (reader.TokenType == JsonToken.EndObject)
                    break;

                // set...
                env.Values[prop] = reader.Value;
            }

            // return...
            return env;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}