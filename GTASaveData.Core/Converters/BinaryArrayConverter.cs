using GTASaveData.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GTASaveData.Converters
{
    public class ByteArrayConverter : JsonConverter<Array<byte>>
    {
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
                throw new JsonSerializationException(string.Format(Strings.Error_JsonSerialization_BinaryUnexpectedToken, reader.TokenType));
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
                    case JsonToken.Comment:
                        // skip
                        break;
                    default:
                        throw new JsonSerializationException(string.Format(Strings.Error_JsonSerialization_BinaryUnexpectedToken, reader.TokenType));
                }
            }

            throw new JsonSerializationException(Strings.Error_JsonSerialization_BinaryUnexpectedEnd);
        }

        public override void WriteJson(JsonWriter writer, Array<byte> value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToArray());
            }
        }
    }
}
