using GTASaveData.Extensions;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="StreamBuffer"/> wrapper for serializing and deserializing data.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Gets or sets whether to read and write data in big-endian byte order
        /// when a <see cref="StreamBuffer"/> instance is not provided.
        /// </summary>
        public static bool BigEndian { get; set; }

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
            using (StreamBuffer workBuf = new StreamBuffer(buf) { BigEndian = BigEndian })
            {
                return Read(workBuf, fmt, out obj);
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
        public static int Read<T>(T obj, byte[] buf, FileFormat fmt) where T : ISerializable
        {
            using (StreamBuffer workBuf = new StreamBuffer(buf) { BigEndian = BigEndian })
            {
                return Read(obj, workBuf, fmt);
            }
        }

        /// <summary>
        /// Reads the object's data from the current position in the buffer
        /// using the specified data format and returns the number of bytes read.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, StreamBuffer buf, FileFormat fmt) where T : ISerializable
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
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
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
            using (StreamBuffer workBuf = new StreamBuffer() { BigEndian = BigEndian })
            {
                int bytesWritten = workBuf.GenericWrite(obj, fmt);
                data = workBuf.GetAllBytes();
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
        public static int Write<T>(StreamBuffer buf, T obj, FileFormat fmt) where T : ISerializable
        {
            return obj.WriteData(buf, fmt);
        }

        /// <summary>
        /// Gets the serialized size of the specified type.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>() where T : new()
        {
            return SizeOf(new T(), FileFormat.Default);
        }

        /// <summary>
        /// Gets the serialized size of the specified type using the
        /// specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(FileFormat fmt) where T : new()
        {
            return SizeOf(new T(), fmt);
        }

        /// <summary>
        /// Gets the serialized size of an object.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(T obj)
        {
            return SizeOf(obj, FileFormat.Default);
        }

        /// <summary>
        /// Gets the serialized size of an object using the
        /// specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(T obj, FileFormat fmt)
        {
            // Get size from GetSize() function if T is ISerializable
            if (typeof(T).Implements(typeof(ISerializable)))
            {
                return ((ISerializable) obj).GetSize(fmt);
            }

            // Otherwise get size by serializing the data
            return Write(obj, fmt, out byte[] _);
        }
    }
}
