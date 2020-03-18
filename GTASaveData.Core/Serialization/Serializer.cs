using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Handles the reading and writing of the binary data stored within Grand Theft Auto save data files.
    /// </summary>
    public sealed class Serializer : IDisposable
    {
        public static T Deserialize<T>(byte[] buffer)
        {
            return Deserialize<T>(buffer, FileFormat.None);
        }

        /// <summary>
        /// Creates an object from the data stored in a byte array.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="buffer">The byte array containing serialized object data.</param>
        /// <param name="format">The format of the data, if applicable</param>
        /// <returns>An object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="SerializationException">Thrown if the type does not support serialization.</exception>
        public static T Deserialize<T>(byte[] buffer, FileFormat format)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            using (Serializer s = new Serializer(new MemoryStream(buffer)))
            {
                return s.GenericRead<T>(format, 0, false);
            }
        }

        public static byte[] Serialize<T>(T obj,
            PaddingMode padding = PaddingMode.Zeros,
            byte[] paddingSequence = null)
        {
            return Serialize(obj, FileFormat.None, padding, paddingSequence);
        }

        /// <summary>
        /// Creates a byte array from data in the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="format">The serialization format, if applicable.</param>
        /// <param name="padding">The <see cref="Serialization.PaddingMode"/> to use for alignment.</param>
        /// <param name="paddingSequence">The sequence of bytes to use when the padding mode is <see cref="PaddingMode.Sequence"/>.</param>
        /// <returns>A byte array containing the serialized object data.</returns>
        /// <exception cref="SerializationException">Thrown if the type does not support serialization.</exception>
        public static byte[] Serialize<T>(T obj, FileFormat format,
            PaddingMode padding = PaddingMode.Zeros,
            byte[] paddingSequence = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer s = new Serializer(m, padding, paddingSequence))
                {
                    s.GenericWrite(obj, format, 0, false);
                }

                return m.ToArray();
            }
        }

        /// <summary>
        /// Aligns an address to the next multiple of the specified word size.
        /// </summary>
        /// <param name="address">The address to align.</param>
        /// <param name="wordSize">The word size to align to. Note: must be a power of 2.</param>
        /// <returns>The aligned address.</returns>
        public static long GetAlignedAddress(long address, int wordSize = 4)
        {
            if (wordSize < 1 || ((wordSize - 1) & wordSize) != 0)
            {
                throw new ArgumentException(Strings.Error_Argument_PowerOf2, nameof(wordSize));
            }

            wordSize--;
            return (address + wordSize) & ~wordSize;
        }

        private readonly BinaryReader m_reader;
        private readonly BinaryWriter m_writer;
        private bool m_disposed;
        private PaddingMode m_paddingMode;
        private byte[] m_paddingSequence;
        private long m_mark;

        /// <summary>
        /// Creates a new <see cref="Serializer"/> using the specified stream as the serialization endpoint.
        /// </summary>
        /// <param name="baseStream">A readable and writable data stream.</param>
        public Serializer(Stream baseStream)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException(nameof(baseStream));
            }

            if (!(baseStream.CanRead && baseStream.CanWrite))
            {
                throw new ArgumentException(Strings.Error_Argument_StreamMustBeRW, nameof(baseStream));
            }

            m_mark = baseStream.Position;
            m_reader = new BinaryReader(baseStream, Encoding.ASCII, true);
            m_writer = new BinaryWriter(baseStream, Encoding.ASCII, true);
            m_disposed = false;
        }

        /// <summary>
        /// Creates a new <see cref="Serializer"/> using the specified stream as the serialization endpoint.
        /// </summary>
        /// <param name="baseStream">A readable and writable data stream.</param>
        /// <param name="paddingMode">The <see cref="Serialization.PaddingMode"/> to use for alignment.</param>
        /// <param name="paddingSequence">The sequence of bytes to use if the padding mode is set to <see cref="PaddingMode.Sequence"/>.</param>
        public Serializer(Stream baseStream, PaddingMode paddingMode, byte[] paddingSequence = null)
            : this(baseStream)
        {
            m_paddingMode = paddingMode;
            PaddingSequence = paddingSequence;      // use setter to handle null case
        }

        /// <summary>
        /// Gets the underlying serialization <see cref="Stream"/>.
        /// </summary>
        public Stream BaseStream
        {
            get { return m_reader.BaseStream; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Serialization.PaddingMode"/> to use during serialization.
        /// </summary>
        public PaddingMode PaddingMode
        {
            get { return m_paddingMode; }
            set { m_paddingMode = value; }
        }

        /// <summary>
        /// Gets or sets the padding sequence to use if <see cref="PaddingMode.Sequence"/> is selected.
        /// </summary>
        public byte[] PaddingSequence
        {
            get { return m_paddingSequence; }
            set
            {
                if (value == null)
                {
                    value = new byte[1] { 0 };
                }
                m_paddingSequence = value;
            }
        }

        /// <summary>
        /// Aligns the current position in the serialization stream to a multiple of the specified word size.
        /// </summary>
        /// <param name="wordSize">The word size to align to. Note: must be a power of 2.</param>
        public void Align(int wordSize = 4)
        {
            long pos = BaseStream.Position;
            int num = (int) (GetAlignedAddress(pos, wordSize) - pos);

            WritePadding(num);
        }

        public void Mark()
        {
            m_mark = BaseStream.Position;
        }

        public long Marked()
        {
            return m_mark;
        }

        public long Position()
        {
            return BaseStream.Position;
        }

        /// <summary>
        /// Skips ahead or beind the specified number of bytes.
        /// </summary>
        /// <param name="amount">The number of bytes by which to skip ahead or back.</param>
        public void Skip(int amount)
        {
            BaseStream.Position += amount;
        }

        /// <summary>
        /// Reads an n-byte Boolean value.
        /// 'False' is represented by all bits being 0.
        /// </summary>
        /// <param name="byteCount">The number of bytes to treat as a single Boolean.</param>
        /// <returns>A bool.</returns>
        public bool ReadBool(int byteCount = 1)
        {
            if (byteCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            }

            byte[] buffer = ReadBytes(byteCount);
            byte value = 0;

            foreach (byte b in buffer)
            {
                value |= b;
            }

            return value != 0;
        }

        /// <summary>
        /// Reads an 8-bit integer.
        /// </summary>
        /// <returns>An sbyte.</returns>
        public sbyte ReadSByte()
        {
            return m_reader.ReadSByte();
        }

        /// <summary>
        /// Reads an 8-bit unsigned integer.
        /// </summary>
        /// <returns>A byte.</returns>
        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        /// <summary>
        /// Reads the specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array.</returns>
        public byte[] ReadBytes(int count)
        {
            return m_reader.ReadBytes(count);
        }

        /// <summary>
        /// Reads an ASCII or Unicode character. Note: Surrogate characters are not supported.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read a unicode character.</param>
        /// <returns>A char.</returns>
        public char ReadChar(bool unicode = false)
        {
            // BinaryWriter#ReadChar() relies on the encoding specified in the constructor
            // to determine how many bytes to write for a character. Since GTA saves sometimes
            // have a mixture of Unicode and ASCII strings, and we can't change the encoding
            // once the writer is created, we're going to bypass the built-in encoding processing
            // altogether. "Unicode" in this sense means UTF-16, i.e. 16-bit characters.

            return (unicode)
                ? (char) ReadUInt16()
                : (char) ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit single precision floating-point number.
        /// </summary>
        /// <returns>A float.</returns>
        public float ReadSingle()
        {
            return m_reader.ReadSingle();
        }

        /// <summary>
        /// Reads a 64-bit double precision floating-point number.
        /// </summary>
        /// <returns>A double.</returns>
        public double ReadDouble()
        {
            return m_reader.ReadDouble();
        }

        /// <summary>
        /// Reads a 16-bit integer.
        /// </summary>
        /// <returns>A short.</returns>
        public short ReadInt16()
        {
            return m_reader.ReadInt16();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <returns>A ushort.</returns>
        public ushort ReadUInt16()
        {
            return m_reader.ReadUInt16();
        }

        /// <summary>
        /// Reads a 32-bit integer.
        /// </summary>
        /// <returns>An int.</returns>
        public int ReadInt32()
        {
            return m_reader.ReadInt32();
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>A uint.</returns>
        public uint ReadUInt32()
        {
            return m_reader.ReadUInt32();
        }

        /// <summary>
        /// Reads a 64-bit integer.
        /// </summary>
        /// <returns>A long.</returns>
        public long ReadInt64()
        {
            return m_reader.ReadInt64();
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <returns>A ulong.</returns>
        public ulong ReadUInt64()
        {
            return m_reader.ReadUInt64();
        }

        /// <summary>
        /// Reads a zero-terminated string.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read unicode characters.</param>
        /// <returns>A string.</returns>
        public string ReadString(bool unicode = false)
        {
            string s = "";

            char c;
            while ((c = ReadChar(unicode)) != '\0')
            {
                s += c;
            }

            return s;
        }

        /// <summary>
        /// Reads a string of the specified length.
        /// </summary>
        /// <remarks>
        /// If a null character is found, the returned string will be terminated at
        /// that point. If the specified length is nonzero, the stream position will
        /// advance until the length is reached, even if a null character is seen.
        /// </remarks>
        /// <param name="length">The number of characters to read. Specify 0 to read until the first null character.</param>
        /// <param name="unicode">A value indicating whether to read Unicode characters.</param>
        /// <returns>A string.</returns>
        public string ReadString(int length, bool unicode = false)
        {
            if (length == 0)
            {
                return ReadString(unicode);
            }

            string s = "";
            bool foundNull = false;

            for (int i = 0; i < length; i++)
            {
                char c = ReadChar(unicode);
                if (c == '\0')
                {
                    foundNull = true;
                }

                if (!foundNull)
                {
                    s += c;
                }
            }

            return s;
        }

        public T ReadObject<T>() where T : ISerializable, new()
        {
            T obj = new T();
            obj.ReadObjectData(this);

            return obj;
        }

        /// <summary>
        /// Reads an object.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="format">The data format.</param>
        /// <returns>An object.</returns>
        /// <exception cref="SerializationException">Thrown if an error occurs during deserialization.</exception>
        public T ReadObject<T>(FileFormat format) where T : ISerializable, new()
        {
            T obj = new T();
            obj.ReadObjectData(this, format);

            return obj;
        }

        public T[] ReadArray<T>(int count,
            int itemLength = 0,
            bool unicode = false)
        {
            return ReadArray<T>(count, FileFormat.None, itemLength, unicode);
        }

        /// <summary>
        /// Reads an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="count">The number of items to read.</param>
        /// <param name="format">The data format, if applicable.</param>
        /// <param name="itemLength">The length of each item. Note: only applies to <see cref="bool"/> and <see cref="string"/> types.</param>
        /// <param name="unicode">A value indicating whether to read unicode characters.</param>
        /// <returns>An array of the specified type.</returns>
        /// <exception cref="SerializationException">Thrown if the type does not support serialization.</exception>
        public T[] ReadArray<T>(int count,
            FileFormat format,
            int itemLength = 0,
            bool unicode = false)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                items.Add(GenericRead<T>(format, itemLength, unicode));
            }

            return items.ToArray();
        }

        /// <summary>
        /// Writes an n-byte Boolean value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="byteCount">The number of bytes to write.</param>
        public void Write(bool value, int byteCount = 1)
        {
            if (byteCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            }

            byte[] buffer = new byte[byteCount];
            buffer[0] = (value) ? (byte) 1 : (byte) 0;      // little endian

            Write(buffer);
        }

        /// <summary>
        /// Writes an 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(byte value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes an 8-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(sbyte value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a byte array.
        /// </summary>
        /// <param name="buffer">The byte array to write.</param>
        public int Write(byte[] buffer)
        {
            m_writer.Write(buffer);

            return buffer.Length;
        }

        /// <summary>
        /// Writes an ASCII or Unicode character. Note: Surrogate characters are not supported.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="unicode">A value indicating whether to read a unicode character.</param>
        public void Write(char value, bool unicode = false)
        {
            // BinaryWriter#Write(char) relies on the encoding specified in the constructor
            // to determine how many bytes to write for a character. Since GTA saves sometimes
            // have a mixture of Unicode and ASCII strings, and we can't change the encoding
            // once the writer is created, we're going to bypass the encoding altogether.
            // "Unicode" in this sense means UTF-16, i.e. 16-bit characters.

            if (char.IsSurrogate(value))
            {
                throw new ArgumentException(Strings.Error_Argument_SurrogateChars, nameof(value));
            }

            if (unicode)
            {
                Write((ushort) value);
            }
            else
            {
                Write((byte) value);
            }
        }

        /// <summary>
        /// Writes a 32-bit single precision floating-point value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(float value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit double precision floating-point value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(double value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(short value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(int value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 64-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(long value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(ushort value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(uint value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Write an 64-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(ulong value)
        {
            m_writer.Write(value);
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <param name="length">
        /// The number of characters to write. The string will be truncated if this value is less than
        /// the string's length. The string will be zero-padded on the right side if this value exceeds
        /// the string's length. If this value is null, the string will be written in its entirety and
        /// a null teminator will be appended.
        /// </param>
        /// <param name="unicode">A value indicating whether to write Unicode characters.</param>
        /// <param name="zeroTerminate">
        /// A value indicating whether to terminate the string with a null character (C-style).
        /// If the length is unspecified, the null character will be added to the end of the
        /// string regardless. Otherwise, the string will be truncated if necessary so as to
        /// not exceed the specified length when the null character is written.
        /// </param>
        public void Write(string value,
            int? length = null,
            bool unicode = false,
            bool zeroTerminate = true)
        {
            Encoding encoding = (unicode)
                ? Encoding.Unicode
                : Encoding.ASCII;

            // Use whole string if 'length' parameter is unspecified (null or 0)
            int actualLength = (length == 0)
                ? (value.Length)
                : (length ?? value.Length);

            // Add extra byte for null terminator if 'length' is unspecified
            if ((length == null || length == 0) && zeroTerminate)
            {
                actualLength += 1;
            }

            if (value.Length >= actualLength)
            {
                value = (zeroTerminate)
                    ? value.Substring(0, actualLength - 1) + '\0'
                    : value.Substring(0, actualLength);
            }
            else
            {
                value += '\0';
            }

            int numPadding = (actualLength - value.Length);
            if (unicode)
            {
                numPadding *= 2;
            }

            Write(encoding.GetBytes(value));
            WritePadding(numPadding);
        }

        public void Write<T>(T value) where T : ISerializable
        {
            Write(value, FileFormat.None);
        }

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <remarks>
        /// The object type must implement the <see cref="ISerializable"/> interface
        /// and specify its serialization.
        /// </remarks>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="value">The object to write</param>
        /// <param name="format">The data format.</param>
        /// <exception cref="SerializationException">Thrown if an error occurs during serialization.</exception>
        public void Write<T>(T value, FileFormat format) where T : ISerializable
        {
            value.WriteObjectData(this, format);
        }

        public void Write<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            Write<T>(items, FileFormat.None, count, itemLength, unicode);
        }

        /// <summary>
        /// Writes a collection as a contiguous sequence of bytes.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="items">The items to write.</param>
        /// <param name="count">
        /// The number of items to write. Use null to write all elements. If the count is larger than
        /// the collection length, default values will be written until 'count' elements are written.
        /// </param>
        /// <param name="format">The data format.</param>
        /// <param name="itemLength">
        /// The length of each item. Note: only applies to <see cref="bool"/> and <see cref="string"/> types.
        /// </param>
        /// <param name="unicode">A value indicating whether to write unicode characters.</param>
        /// <exception cref="SerializationException">Thrown if the type does not support serialization.</exception>
        public void Write<T>(T[] items,
            FileFormat format,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            int capacity = items.Length;
            for (int i = 0; i < (count ?? capacity); i++)
            {
                if (i < capacity)
                {
                    GenericWrite(items.ElementAt(i), format, itemLength, unicode);
                }
                else
                {
                    GenericWrite(new T(), format, itemLength, unicode);
                }
            }
        }

        /// <summary>
        /// Writes the specified number of padding bytes.
        /// The exact bytes written depends on the current <see cref="PaddingMode"/>.
        /// </summary>
        /// <param name="length">The number of bytes to write.</param>
        public void WritePadding(int length)
        {
            switch (m_paddingMode)
            {
                case PaddingMode.Zeros:
                {
                    Write(Enumerable.Repeat<byte>(0, length).ToArray());
                    break;
                }

                case PaddingMode.Random:
                {
                    byte[] pad = new byte[length];
                    Random rand = new Random();

                    rand.NextBytes(pad);
                    Write(pad);
                    break;
                }

                case PaddingMode.Sequence:
                {
                    byte[] pad = new byte[length];
                    byte[] seq = m_paddingSequence;

                    for (int i = 0; i < length; i++)
                    {
                        pad[i] = seq[i % seq.Length];
                    }
                    Write(pad);
                    break;
                }
            }
        }

        private T GenericRead<T>(FileFormat format, int length, bool unicode)
        {
            object ret;

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Type t = typeof(T);
            if (t == typeof(bool))
            {
                ret = ReadBool((length == 0) ? 1 : length);
            }
            else if (t == typeof(byte))
            {
                ret = ReadByte();
            }
            else if (t == typeof(sbyte))
            {
                ret = ReadSByte();
            }
            else if (t == typeof(byte[]))
            {
                ret = ReadBytes(length);
            }
            else if (t == typeof(char))
            {
                ret = ReadChar(unicode);
            }
            else if (t == typeof(double))
            {
                ret = ReadDouble();
            }
            else if (t == typeof(float))
            {
                ret = ReadSingle();
            }
            else if (t == typeof(int))
            {
                ret = ReadInt32();
            }
            else if (t == typeof(uint))
            {
                ret = ReadUInt32();
            }
            else if (t == typeof(long))
            {
                ret = ReadInt64();
            }
            else if (t == typeof(ulong))
            {
                ret = ReadUInt64();
            }
            else if (t == typeof(short))
            {
                ret = ReadInt16();
            }
            else if (t == typeof(ushort))
            {
                ret = ReadUInt16();
            }
            else if (t == typeof(string))
            {
                ret = (length == 0) ? ReadString(unicode) : ReadString(length, unicode);
            }
            else if (t.GetInterface(nameof(ISerializable)) != null)
            {
                MethodInfo readChunk = GetType().GetMethod(nameof(ReadObject)).MakeGenericMethod(t);
                ret = readChunk.Invoke(this, new object[] { format });
            }
            else
            {
                throw SerializationNotSupported();
            }

            return (T) ret;
        }

        private void GenericWrite<T>(T value, FileFormat format, int length, bool unicode)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Type t = typeof(T);
            if (t == typeof(bool))
            {
                Write(Convert.ToBoolean(value), (length == 0) ? 1 : length);
            }
            else if (t == typeof(byte))
            {
                Write(Convert.ToByte(value));
            }
            else if (t == typeof(sbyte))
            {
                Write(Convert.ToSByte(value));
            }
            else if (t == typeof(byte[]))
            {
                Write((byte[]) (object) value);
            }
            else if (t == typeof(char))
            {
                Write(Convert.ToChar(value), unicode);
            }
            else if (t == typeof(double))
            {
                Write(Convert.ToDouble(value));
            }
            else if (t == typeof(float))
            {
                Write(Convert.ToSingle(value));
            }
            else if (t == typeof(int))
            {
                Write(Convert.ToInt32(value));
            }
            else if (t == typeof(uint))
            {
                Write(Convert.ToUInt32(value));
            }
            else if (t == typeof(long))
            {
                Write(Convert.ToInt64(value));
            }
            else if (t == typeof(ulong))
            {
                Write(Convert.ToUInt64(value));
            }
            else if (t == typeof(short))
            {
                Write(Convert.ToInt16(value));
            }
            else if (t == typeof(ushort))
            {
                Write(Convert.ToUInt16(value));
            }
            else if (t == typeof(string))
            {
                Write(Convert.ToString(value), length, unicode);
            }
            else if (t.GetInterface(nameof(ISerializable)) != null)
            {
                Write((ISerializable) value, format);
            }
            else
            {
                throw SerializationNotSupported();
            }
        }

        private SerializationException SerializationNotSupported()
        {
            return new SerializationException(Strings.Error_NotSupported_Serialization);
        }

        #region IDisposable
        public void Dispose()
        {
            if (!m_disposed)
            {
                m_writer.Dispose();
                m_reader.Dispose();
                m_disposed = true;
            }
        }
        #endregion
    }
}
