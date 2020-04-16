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
            Read(buf, DataFormat.Default, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads a value from a byte array using the specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf, DataFormat fmt)
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
        public static int Read<T>(byte[] buf, DataFormat fmt, out T obj)
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
        public static int Read<T>(StreamBuffer buf, DataFormat fmt, out T obj)
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
        public static int Read<T>(T obj, byte[] buf, DataFormat fmt) where T : ISerializable
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
        public static int Read<T>(T obj, StreamBuffer buf, DataFormat fmt) where T : ISerializable
        {
            return obj.ReadObjectData(buf, fmt);
        }

        /// <summary>
        /// Converts a value into a byte array.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj)
        {
            Write(obj, DataFormat.Default, out byte[] data);
            return data;
        }

        /// <summary>
        /// Converts a value into a byte array using the specified data format.
        /// </summary>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj, DataFormat fmt)
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
        public static int Write<T>(T obj, DataFormat fmt, out byte[] data)
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
        public static int Write<T>(StreamBuffer buf, T obj, DataFormat fmt) where T : ISerializable
        {
            return obj.WriteObjectData(buf, fmt);
        }
    }
}
