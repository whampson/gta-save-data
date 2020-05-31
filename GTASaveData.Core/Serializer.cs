using GTASaveData.Extensions;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="StreamBuffer"/> wrapper for serializing and deserializing data.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Gets or sets whether to read and write data using big-endian byte order
        /// when a <see cref="StreamBuffer"/> instance is not provided.
        /// </summary>
        public static bool BigEndian { get; set; }

        /// <summary>
        /// Gets or sets the padding mode to use when a <see cref="StreamBuffer"/>
        /// instance is not provided.
        /// </summary>
        public static PaddingType PaddingType { get; set; }

        /// <summary>
        /// Gets or sets the pattern used for padding when a <see cref="StreamBuffer"/>
        /// instance is not provided and the <see cref="PaddingType"/> is set to
        /// <see cref="PaddingType.Pattern"/>.
        /// </summary>
        public static byte[] PaddingBytes { get; set; }

        /// <summary>
        /// Reads a value from a byte array.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf)
        {
            Read(buf, FileFormat.Default, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads a value from a byte array using the specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf, FileFormat fmt)
        {
            Read(buf, fmt, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads a value from a byte array using the specified data format
        /// and returns the number of bytes read.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(byte[] buf, FileFormat fmt, out T obj)
        {
            using (StreamBuffer stream = CreateStreamBuffer(buf))
            {
                return Read(stream, fmt, out obj);
            }
        }

        /// <summary>
        /// Reads a value from the current position in the buffer using the
        /// specified data format and returns the number of bytes read.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(StreamBuffer buf, FileFormat fmt, out T obj)
        {
            return buf.GenericRead(fmt, out obj);
        }

        /// <summary>
        /// Reads the object's data from a byte array using the specified
        /// data format and returns the number of bytes read.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, byte[] buf, FileFormat fmt) where T : ISaveDataObject
        {
            using (StreamBuffer stream = CreateStreamBuffer(buf))
            {
                return Read(obj, stream, fmt);
            }
        }

        /// <summary>
        /// Reads the object's data from the current position in the buffer
        /// using the specified data format and returns the number of bytes read.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, StreamBuffer buf, FileFormat fmt) where T : ISaveDataObject
        {
            return obj.ReadData(buf, fmt);
        }

        /// <summary>
        /// Converts a value into a byte array.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj)
        {
            Write(obj, FileFormat.Default, out byte[] data);
            return data;
        }

        /// <summary>
        /// Converts a value into a byte array using the specified data format.
        /// </summary>
        /// <exception cref="SerializationException"/>
        public static byte[] Write<T>(T obj, FileFormat fmt)
        {
            Write(obj, fmt, out byte[] data);
            return data;
        }

        /// <summary>
        /// Converts a value into a byte array using the specified data format
        /// and returns the number of bytes written.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(T obj, FileFormat fmt, out byte[] data)
        {
            using (StreamBuffer stream = CreateStreamBuffer())
            {
                int bytesWritten = stream.GenericWrite(obj, fmt);
                data = stream.GetBufferBytes();
                return bytesWritten;
            }
        }

        /// <summary>
        /// Writes an object to the current postion in the buffer using the
        /// specified data format and returns the number of bytes written.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(StreamBuffer buf, T obj, FileFormat fmt) where T : ISaveDataObject
        {
            return obj.WriteData(buf, fmt);
        }

        /// <summary>
        /// Gets the serialized size of the specified type.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOfType<T>() where T : new()
        {
            return SizeOfObject(new T(), FileFormat.Default);
        }

        /// <summary>
        /// Gets the serialized size of the specified type using the
        /// specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOfType<T>(FileFormat fmt) where T : new()
        {
            return SizeOfObject(new T(), fmt);
        }

        /// <summary>
        /// Gets the serialized size of an object.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOfObject<T>(T obj)
        {
            return SizeOfObject(obj, FileFormat.Default);
        }

        /// <summary>
        /// Gets the serialized size of an object using the
        /// specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOfObject<T>(T obj, FileFormat fmt)
        {
            // Get size from GetSize() function if T is ISerializable
            if (typeof(T).Implements(typeof(ISaveDataObject)))
            {
                return ((ISaveDataObject) obj).GetSize(fmt);
            }

            // Otherwise get size by serializing the data
            return Write(obj, fmt, out byte[] _);
        }

        private static StreamBuffer CreateStreamBuffer()
        {
            return new StreamBuffer()
            {
                BigEndian = BigEndian,
                PaddingType = PaddingType,
                PaddingBytes = PaddingBytes
            };
        }

        private static StreamBuffer CreateStreamBuffer(byte[] buf)
        {
            return new StreamBuffer(buf)
            {
                BigEndian = BigEndian,
                PaddingType = PaddingType,
                PaddingBytes = PaddingBytes
            };
        }
    }
}
