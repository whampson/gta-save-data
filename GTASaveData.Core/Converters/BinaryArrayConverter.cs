using GTASaveData.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GTASaveData.Converters
{
    /// <summary>
    /// Converts an <see cref="Array{T}"/> (where the generic type argument is <see cref="byte"/>)
    /// to a base64 string if the array is sufficiently large.
    /// </summary>
    public class ByteArrayConverter : JsonConverter<Array<byte>>
    {
        public const int DefaultThreshold = 32;

        /// <summary>
        /// The maximum array length at which bytes will be encoded as a JSON array.
        /// After the threshold is surpassed, the byte array will be encoded as a base64 string.
        /// </summary>
        public static int Threshold { get; set; }

        public override Array<byte> ReadJson(JsonReader reader, Type objectType, Array<byte> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            byte[] data;
            if (reader.TokenType == JsonToken.StartArray)
            {
                data = ReadByteArray(reader);
            }
            else if (reader.TokenType == JsonToken.String)
            {
                data = Convert.FromBase64String(reader.Value.ToString());
            }
            else
            {
                throw new JsonSerializationException(string.Format(Strings.Error_JsonBinaryUnexpectedToken, reader.TokenType));
            }

            return data;
        }

        private byte[] ReadByteArray(JsonReader reader)
        {
            List<byte> byteList = new List<byte>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                        byteList.Add(Convert.ToByte(reader.Value));
                        break;
                    case JsonToken.EndArray:
                        return byteList.ToArray();
                    default:
                        throw new JsonSerializationException(string.Format(Strings.Error_JsonBinaryUnexpectedToken, reader.TokenType));
                }
            }

            throw new JsonSerializationException(Strings.Error_JsonBinaryUnexpectedEnd);
        }

        public override void WriteJson(JsonWriter writer, Array<byte> value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                if (value.Count < Threshold)
                {
                    // Regular ol' array
                    serializer.Serialize(writer, value);
                }
                else
                {
                    // Base64 string
                    writer.WriteValue(value.ToArray());
                }
            }
        }
    }
}
