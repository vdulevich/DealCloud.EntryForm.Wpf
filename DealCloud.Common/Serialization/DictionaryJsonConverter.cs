using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DealCloud.Common.Extensions;

namespace DealCloud.Common.Serialization
{
    public class DictionaryJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
            {
                throw new Exception($"This converter is not for {objectType}.");
            }
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            IDictionary result = (IDictionary)Activator.CreateInstance(objectType);
            Type dictionaryType = objectType.IsA(typeof(IDictionary<,>));
            var keyType = dictionaryType.GetGenericArguments()[0];
            var valueType = dictionaryType.GetGenericArguments()[1];

            JObject jObject = JObject.Load(reader);
            using (JsonReader jObjectReader = reader.CopyReaderForObject(jObject))
            {
                serializer.Populate(jObjectReader, result);
                var keys = jObject.GetValue("keys", StringComparison.OrdinalIgnoreCase);
                var values = jObject.GetValue("values", StringComparison.OrdinalIgnoreCase);
                for (int i = 0; i < keys.Count(); i++)
                {
                    var key = keys[i].ToObject(keyType);
                    var value = (values[i] is JObject && !string.IsNullOrEmpty(values[i]["$type"]?.Value<string>())) ?
                        values[i].ToObject(Type.GetType(values[i]["$type"].ToString())):
                        values[i].ToObject(valueType);

                    result.Add(key, value);
                }
            }
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsA(typeof(IDictionary<,>)) != null;
        }
    }
}
