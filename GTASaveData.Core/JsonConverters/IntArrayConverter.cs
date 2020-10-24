using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GTASaveData.JsonConverters
{
    /// <summary>
    /// Converts a <see cref="int"/> <see cref="Array{T}"/> to
    /// a base64 string if the array is sufficiently large.
    /// </summary>
    public class IntArrayConverter : JsonConverter<Array<int>>
    {
        public const int DefaultThreshold = 25;

        static IntArrayConverter()
        {
            Threshold = DefaultThreshold;
        }

        /// <summary>
        /// The maximum array length at which bytes will be encoded as a regular JSON array.
        /// After the threshold is surpassed, the byte array will be encoded as a base64 string.
        /// </summary>
        public static int Threshold { get; set; }

        public override Array<int> ReadJson(JsonReader reader, Type objectType,
            Array<int> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            int[] data;
            if (reader.TokenType == JsonToken.StartArray)
            {
                data = ReadIntArrayFromJson(reader);
            }
            else if (reader.TokenType == JsonToken.String)
            {
                data = ReadIntArray(Convert.FromBase64String(reader.Value.ToString())).ToArray();
            }
            else
            {
                throw UnexpectedToken(reader.TokenType);
            }

            return data;
        }

        private int[] ReadIntArrayFromJson(JsonReader reader)
        {
            List<int> byteList = new List<int>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                        byteList.Add(Convert.ToInt32(reader.Value));
                        break;
                    case JsonToken.EndArray:
                        return byteList.ToArray();
                    default:
                        throw UnexpectedToken(reader.TokenType);
                }
            }

            throw EndOfStream();
        }

        private Array<int> ReadIntArray(byte[] value)
        {
            using (DataBuffer buf = new DataBuffer(value))
            {
                int count = value.Length / sizeof(int);
                return buf.ReadArray<int>(count);
            }
        }

        private byte[] WriteIntArray(Array<int> value)
        {
            using (DataBuffer buf = new DataBuffer(value.Count * sizeof(int)))
            {
                buf.Write(value);
                return buf.GetBytes();
            }
        }

        public override void WriteJson(JsonWriter writer, Array<int> value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            
            if (value.Count < Threshold)
            {
                // Regular JSON array
                serializer.Serialize(writer, value);
            }
            else
            {
                // Base64 string
                writer.WriteValue(WriteIntArray(value));
            }
        }

        private JsonSerializationException EndOfStream()
        {
            return new JsonSerializationException(Strings.Error_JsonSerialization_EndOfStream);
        }

        private JsonSerializationException UnexpectedToken(JsonToken t)
        {
            string msg = string.Format(Strings.Error_JsonSerialization_UnexpectedToken, t);
            return new JsonSerializationException(msg);
        }
    }
}
