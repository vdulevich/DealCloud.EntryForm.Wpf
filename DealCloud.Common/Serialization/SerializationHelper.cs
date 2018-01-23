using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DealCloud.Common.Serialization
{
    public static class SerializationHelper
    {
        /// <summary>
        ///     Serializes .NEt object using .NET contract serialize
        /// </summary>
        public static string SerializeNetObjectToString(object instance)
        {
            var serializer = new NetDataContractSerializer();

            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, instance);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var result = XDocument.Load(memoryStream).ToString();

                Debug.WriteLine(result);

                return result;
            }
        }

        /// <summary>
        ///     Deserializes .NET object from xml
        /// </summary>
        public static T DeserializeNetObjectFromString<T>(string objectString) where T : class
        {
            var serializer = new NetDataContractSerializer();

            using (var reader = XmlReader.Create(new StringReader(objectString)))
            {
                return serializer.ReadObject(reader) as T;
            }
        }

        public static string SerializeDataContract<T>(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                SerializeDataContract<T>(memoryStream, instance);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var result = XDocument.Load(memoryStream).ToString();
                return result;
            }
        }

        public static void SerializeDataContract<T>(Stream stream, object instance)
        {
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, instance);
        }

        private static JsonSerializerSettings _newtonsoftSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public static readonly JsonSerializerSettings JsonSettingsCamelCaseTypeAuto = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static readonly JsonSerializerSettings JsonSettingsCamelCase = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        /// <summary>
        /// serialize object using Newtonsoft.JSON
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string SerializeNewtonsoftJson(object instance, bool useAutoTypeHandling = false)
        {
            return useAutoTypeHandling ? JsonConvert.SerializeObject(instance, _newtonsoftSettings) : JsonConvert.SerializeObject(instance);
        }

        /// <summary>
        /// Deserializes object from json using Newtonsoft.JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeNewtonsoftJson<T>(string json, bool useAutoTypeHandling = false)
        {
            return useAutoTypeHandling ? JsonConvert.DeserializeObject<T>(json, _newtonsoftSettings) : JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerializeDataContractToJson<T>(object instance)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);

                memoryStream.Position = 0;

                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T DeserializeDataContract<T>(string xmlString)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var reader = XmlReader.Create(new StringReader(xmlString)))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static T DeserializeDataContractFromJson<T>(string jsonString)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)serializer.ReadObject(memoryStream);
            }
        }

        public static T DeserializeXml<T>(string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString)) return default(T);

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xmlString))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string SerializeXml<T>(T instance)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                serializer.Serialize(writer, instance);
            }

            return sb.ToString();
        }

        public static string SerializeXml<T>(T instance, XmlSerializerNamespaces namespaces)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                serializer.Serialize(writer, instance, namespaces);
            }

            return sb.ToString();
        }

        public static byte[] SerializeBinary(object instance)
        {
            byte[] byteArray;

            using (Stream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(memoryStream, instance);

                memoryStream.Seek(0, SeekOrigin.Begin);

                byteArray = new byte[memoryStream.Length];

                memoryStream.Read(byteArray, 0, (int)memoryStream.Length);
            }

            return byteArray;
        }

        public static object DeserializeBinary(byte[] bytes)
        {
            object result;

            using (Stream memoryStream = new MemoryStream(bytes.Length))
            {
                IFormatter formatter = new BinaryFormatter();

                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                result = formatter.Deserialize(memoryStream);
            }

            return result;
        }

        public static string SerializeBase64(object instance)
        {
            return Convert.ToBase64String(SerializeBinary(instance));
        }

        public static object DeserializeBase64(string instance)
        {
            return DeserializeBinary(Convert.FromBase64String(instance));
        }

        public static void SerializeSoap(object instance, string filePath)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(filePath,
                    FileMode.Create, FileAccess.Write);
                IFormatter formatter = new SoapFormatter();
                formatter.Serialize(fileStream, instance);
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
        }

        public static object DeserializeSoap(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            FileStream fileStream = null;
            object result = null;

            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                IFormatter formatter = new SoapFormatter();

                result = formatter.Deserialize(fileStream);
            }
            finally
            {
                fileStream?.Close();
            }

            return result;
        }

        /// <summary>
        ///     Deserialize either from <string>Value</string> or strValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeserializeFromXmlValue(string value)
        {
            try
            {
                var val = DeserializeXml<StringValue>(value);

                return val?.Value;
            }
            catch (Exception)
            {
                return value;
            }
        }

        [XmlRoot("value")]
        public class StringValue
        {
            [XmlText]
            public string Value { get; set; }
        }
    }
}