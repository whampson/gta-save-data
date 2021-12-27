using GTASaveData.Extensions;
using GTASaveData.Interfaces;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="DataBuffer"/> wrapper class for easy data serialization.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf)
        {
            Read(buf, new SerializationParams(), out T obj);
            return obj;
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is read.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf, SerializationParams p)
        {
            Read(buf, p, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is read.</param>
        /// <param name="obj">An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(byte[] buf, SerializationParams p, out T obj)
        {
            using DataBuffer stream = CreateDataBufferStream(buf, p);
            return Read(stream, p, out obj);
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="obj">The object to populate.</param>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, byte[] buf, SerializationParams p)
            where T : ISaveDataObject
        {
            using DataBuffer stream = CreateDataBufferStream(buf, p);
            return Read(obj, stream, p);
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="obj">The object to populate.</param>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, DataBuffer buf, SerializationParams p)
            where T : ISaveDataObject
        {
            return obj.ReadData(buf, p);
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is read.</param>
        /// <param name="obj">An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(DataBuffer buf, SerializationParams p, out T obj)
        {
            return buf.GenericRead(p, out obj);
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <returns>The serialized object.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj)
        {
            Write(obj, new SerializationParams(), out byte[] data);
            return data;
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written.</param>
        /// <returns>The serialized object.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj, SerializationParams p)
        {
            Write(obj, p, out byte[] data);
            return data;
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written.</param>
        /// <param name="data">The serialized object.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(T obj, SerializationParams p, out byte[] data)
        {
            using DataBuffer stream = CreateDataBufferStream(p);
            int bytesWritten = stream.GenericWrite(obj, p);
            data = stream.GetBuffer();
            return bytesWritten;
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="buf">The buffer to write into.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(DataBuffer buf, T obj, SerializationParams p)
            where T : ISaveDataObject
        {
            return obj.WriteData(buf, p);
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>()
            where T : new()
        {
            return SizeOf(new T(), new SerializationParams());
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written, which may affect the size.</param>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(SerializationParams p)
            where T : new()
        {
            return SizeOf(new T(), p);
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <param name="obj">The object to get the size of.</param>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(T obj)
        {
            return SizeOf(obj, new SerializationParams());
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <param name="obj">The object to get the size of.</param>
        /// <param name="p">A <see cref="SerializationParams"/> descriptor controlling how data is written, which may affect the size.</param>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(T obj, SerializationParams p)
        {
            return typeof(T).Implements(typeof(ISaveDataObject))
                ? ((ISaveDataObject) obj).GetSize(p)
                : Write(obj, p, out byte[] _);
        }

        private static DataBuffer CreateDataBufferStream(SerializationParams p)
        {
            return new DataBuffer()
            {
                BigEndian = p.BigEndian,
                PaddingType = p.PaddingType,
                PaddingBytes = p.PaddingBytes
            };
        }

        private static DataBuffer CreateDataBufferStream(byte[] buf, SerializationParams p)
        {
            return new DataBuffer(buf)
            {
                BigEndian = p.BigEndian,
                PaddingType = p.PaddingType,
                PaddingBytes = p.PaddingBytes
            };
        }
    }
}
