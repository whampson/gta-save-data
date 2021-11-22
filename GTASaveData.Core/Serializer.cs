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
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf,
            bool bigEndian = false)
        {
            Read(buf, FileFormat.Default, out T obj, bigEndian);
            return obj;
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static T Read<T>(byte[] buf, FileFormat fmt,
            bool bigEndian = false)
        {
            Read(buf, fmt, out T obj, bigEndian);
            return obj;
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <param name="obj">An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(byte[] buf, FileFormat fmt, out T obj,
            bool bigEndian = false)
        {
            using (DataBuffer stream = new DataBuffer(buf) { BigEndian = bigEndian })
            {
                return Read(stream, fmt, out obj);
            }
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="obj">The object to populate.</param>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, byte[] buf, FileFormat fmt,
            bool bigEndian = false) where T : ISaveDataObject
        {
            using (DataBuffer stream = new DataBuffer(buf) { BigEndian = bigEndian })
            {
                return Read(obj, stream, fmt);
            }
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="obj">The object to populate.</param>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(T obj, DataBuffer buf, FileFormat fmt) where T : ISaveDataObject
        {
            return obj.ReadData(buf, fmt);
        }

        /// <summary>
        /// Reads an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <param name="obj">An instance of <typeparamref name="T"/> containing data deserialized from the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Read<T>(DataBuffer buf, FileFormat fmt, out T obj)
        {
            return buf.GenericRead(fmt, out obj);
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>The serialized object.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj,
            bool bigEndian = false)
        {
            Write(obj, FileFormat.Default, out byte[] data, bigEndian);
            return data;
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is written.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <returns>The serialized object.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static byte[] Write<T>(T obj, FileFormat fmt,
            bool bigEndian = false)
        {
            Write(obj, fmt, out byte[] data, bigEndian);
            return data;
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="obj">The object to write.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is written.</param>
        /// <param name="data">The serialized object.</param>
        /// <param name="bigEndian">A value indicating whether to read data in big-endian byte order.</param>
        /// <param name="padding">The <see cref="PaddingScheme"/> to use for data alignment.</param>
        /// <param name="paddingBytes">The byte sequence to use for padding if the padding scheme is set to <see cref="PaddingScheme.Pattern"/>.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(T obj, FileFormat fmt, out byte[] data,
            bool bigEndian = false,
            PaddingScheme padding = PaddingScheme.Skip,
            byte[] paddingBytes = null)
        {
            using (DataBuffer stream = new DataBuffer() {
                BigEndian = bigEndian,
                PaddingType = padding,
                PaddingBytes = paddingBytes
            })
            {
                int bytesWritten = stream.GenericWrite(obj, fmt);
                data = stream.GetBuffer();
                return bytesWritten;
            }
        }

        /// <summary>
        /// Writes an object to a buffer.
        /// </summary>
        /// <typeparam name="T">The type to write.</typeparam>
        /// <param name="buf">The buffer to write into.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int Write<T>(DataBuffer buf, T obj, FileFormat fmt) where T : ISaveDataObject
        {
            return obj.WriteData(buf, fmt);
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>() where T : new()
        {
            return SizeOf(new T(), FileFormat.Default);
        }

        /// <summary>
        /// Gets the size of a type when serialized.
        /// </summary>
        /// <typeparam name="T">The type to get the size of.</typeparam>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is written, which may affect the size.</param>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(FileFormat fmt) where T : new()
        {
            return SizeOf(new T(), fmt);
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
            return SizeOf(obj, FileFormat.Default);
        }

        /// <summary>
        /// Gets the size of an object when serialized.
        /// </summary>
        /// <param name="obj">The object to get the size of.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is written, which may affect the size.</param>
        /// <exception cref="SerializationException">
        /// Thrown if the type is not serializable.
        /// </exception>
        public static int SizeOf<T>(T obj, FileFormat fmt)
        {
            return typeof(T).Implements(typeof(ISaveDataObject))
                ? ((ISaveDataObject) obj).GetSize(fmt)
                : Write(obj, fmt, out byte[] _);
        }
    }
}
