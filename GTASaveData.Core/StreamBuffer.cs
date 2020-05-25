using GTASaveData.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GTASaveData
{
    /// <summary>
    /// A byte buffer that can be read from and written to like a data stream.
    /// </summary>
    public sealed class StreamBuffer : IDisposable
    {
        private readonly MemoryStream m_buffer;
        private bool m_disposed;

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether to read and write data in big-endian byte order.
        /// </summary>
        public bool BigEndian { get; set; }

        /// <summary>
        /// Gets or sets a saved position in the buffer.
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// Gets the current buffer position.
        /// </summary>
        public int Cursor => (int) m_buffer.Position;

        /// <summary>
        /// Gets the number of bytes spanning from <see cref="Mark"/> to <see cref="Cursor"/>.
        /// </summary>
        public int Offset => Cursor - Mark;

        /// <summary>
        /// Gets the length of the buffer in bytes.
        /// </summary>
        public int Length => (int) m_buffer.Length;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new expandable <see cref="StreamBuffer"/>.
        /// </summary>
        public StreamBuffer()
            : this(new MemoryStream())
        { }

        /// <summary>
        /// Creates a new fixed-size <see cref="StreamBuffer"/> with the specified data.
        /// </summary>
        /// <param name="data">The data to contain within the buffer.</param>
        public StreamBuffer(byte[] data)
            : this(new MemoryStream(data))
        { }

        /// <summary>
        /// Creates a new fixed-size <see cref="StreamBuffer"/>.
        /// </summary>
        /// <param name="size">The buffer size.</param>
        public StreamBuffer(int size)
            : this(new byte[size])
        { }

        private StreamBuffer(MemoryStream buffer)
        {
            m_buffer = buffer;
        }
        #endregion

        #region Read Functions
        /// <summary>
        /// Reads a value from the buffer.
        /// </summary>
        /// <typeparam name="T">The type of data to read.</typeparam>
        /// <param name="format">The data format, if applicable.</param>
        /// <param name="obj">
        /// An object of type <typeparamref name="T"/> extracted from the
        /// buffer.
        /// </param>
        /// <param name="length">
        /// The number of bytes to read if <paramref name="obj"/> is a
        /// <see cref="byte"/> array, <see cref="bool"/>, or <see cref="string"/> value.
        /// Ignored otherwise.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read UTF-16 characters if
        /// <typeparamref name="T"/> is a <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if <typeparamref name="T"/> is not one of the following:
        /// <see cref="byte"/> array,
        /// <see cref="byte"/>,
        /// <see cref="sbyte"/>,
        /// <see cref="bool"/>,
        /// <see cref="char"/>,
        /// <see cref="double"/>,
        /// <see cref="float"/>,
        /// <see cref="int"/>,
        /// <see cref="long"/>,
        /// <see cref="short"/>,
        /// <see cref="uint"/>,
        /// <see cref="ulong"/>,
        /// <see cref="ushort"/>,
        /// <see cref="string"/>,
        /// <see cref="ISerializable"/>.
        /// </exception>
        internal int GenericRead<T>(SaveDataFormat format,
            out T obj,
            int length = 0,
            bool unicode = false)
        {
            Type t = typeof(T);
            if (t.IsEnum) t = Enum.GetUnderlyingType(t);

            object o = null;
            int mark = Cursor;

            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            if (t == typeof(byte[])) o = ReadBytes(length);
            else if (t == typeof(byte)) o = ReadByte();
            else if (t == typeof(sbyte)) o = ReadSByte();
            else if (t == typeof(bool)) o = ReadBool((length == 0) ? 1 : length);
            else if (t == typeof(char)) o = ReadChar(unicode);
            else if (t == typeof(double)) o = ReadDouble();
            else if (t == typeof(float)) o = ReadFloat();
            else if (t == typeof(int)) o = ReadInt32();
            else if (t == typeof(uint)) o = ReadUInt32();
            else if (t == typeof(long)) o = ReadInt64();
            else if (t == typeof(ulong)) o = ReadUInt64();
            else if (t == typeof(short)) o = ReadInt16();
            else if (t == typeof(ushort)) o = ReadUInt16();
            else if (t == typeof(string)) o = (length == 0) ? ReadString(unicode) : ReadString(length, unicode);
            else if (t.Implements(typeof(ISerializable)))
            {
                var p = new Type[] { typeof(SaveDataFormat) };
                var m = GetType().GetMethod(nameof(Read), p).MakeGenericMethod(t);
                o = m.Invoke(this, new object[] { format });
            }
            else throw SerializationNotSupported(typeof(T));

            obj = (T) o;
            return Cursor - mark;
        }

        /// <summary>
        /// Reads a block of bytes from the buffer.
        /// </summary>
        /// <param name="buffer">The target buffer to which the block of bytes will be written.</param>
        /// <param name="index">The index in the target buffer at which to begin writing data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>The number of bytes read.</returns>
        public int Read(byte[] buffer, int index, int count)
        {
            return m_buffer.Read(buffer, index, count);
        }

        /// <summary>
        /// Reads a byte array from the buffer.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array with <paramref name="count"/> bytes.</returns>
        public byte[] ReadBytes(int count)
        {
            byte[] data = new byte[count];
            Read(data, 0, count);

            return data;
        }

        /// <summary>
        /// Reads an unsigned 8-bit integer from the buffer.
        /// </summary>
        /// <returns>An byte value.</returns>
        public byte ReadByte()
        {
            int b = m_buffer.ReadByte();
            if (b == -1)
            {
                throw EndOfStream();
            }

            return (byte) b;
        }

        /// <summary>
        /// Reads a signed 8-bit integer from the buffer.
        /// </summary>
        /// <returns>An sbyte value.</returns>
        public sbyte ReadSByte()
        {
            int b = m_buffer.ReadByte();
            if (b == -1)
            {
                throw EndOfStream();
            }

            return (sbyte) b;
        }



        /// <summary>
        /// Reads a true/false value from the buffer.
        /// False is represented with zero, true is represented with nonzero.
        /// </summary>
        /// <param name="byteCount">The number of bytes to treat as true/false.</param>
        /// <returns>A bool value.</returns>
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
        /// Read an 8-bit or 16-bit character from the buffer.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read UTF-16 characters.</param>
        /// <returns>A char value.</returns>
        public char ReadChar(bool unicode = false)
        {
            return (unicode)
                ? (char) ReadUInt16()
                : (char) ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit float (IEEE 754) from the buffer.
        /// </summary>
        /// <returns>A float value.</returns>
        public float ReadFloat()
        {
            byte[] data = ReadBytes(sizeof(float));
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return BitConverter.ToSingle(data, 0);
        }

        /// <summary>
        /// Reads a 64-bit double (IEEE 754) from the buffer.
        /// </summary>
        /// <returns>A float value.</returns>
        public double ReadDouble()
        {
            byte[] data = ReadBytes(sizeof(double));
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return BitConverter.ToDouble(data, 0);
        }

        /// <summary>
        /// Reads a signed 16-bit integer from the buffer.
        /// </summary>
        /// <returns>A short value.</returns>
        public short ReadInt16()
        {
            return (short) ReadUInt16();
        }

        /// <summary>
        /// Reads a signed 32-bit integer from the buffer.
        /// </summary>
        /// <returns>An int value.</returns>
        public int ReadInt32()
        {
            return (int) ReadUInt32();
        }

        /// <summary>
        /// Reads a signed 64-bit integer from the buffer.
        /// </summary>
        /// <returns>A long value.</returns>
        public long ReadInt64()
        {
            return (long) ReadUInt64();
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the buffer.
        /// </summary>
        /// <returns>A short value.</returns>
        public ushort ReadUInt16()
        {
            byte[] data = ReadBytes(sizeof(ushort));
            return (BigEndian)
                ? (ushort) ((data[0] << 8) | data[1])
                : (ushort) ((data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the buffer.
        /// </summary>
        /// <returns>An int value.</returns>
        public uint ReadUInt32()
        {
            byte[] data = ReadBytes(sizeof(uint));
            return (BigEndian)
                ? (uint) ((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3])
                : (uint) ((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the buffer.
        /// </summary>
        /// <returns>A long value.</returns>
        public ulong ReadUInt64()
        {
            ulong[] data = ReadBytes(sizeof(ulong)).Select(x => (ulong) x).ToArray();
            return (BigEndian)
                ? ((data[0] << 56) | (data[1] << 48) | (data[2] << 40) | (data[3] << 32) | (data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7])
                : ((data[7] << 56) | (data[6] << 48) | (data[5] << 40) | (data[4] << 32) | (data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads a zero-terminated string from the buffer.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read UTF-16 characters.</param>
        /// <returns>A string value.</returns>
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
        /// Reads a fixed-length string from the buffer.
        /// The number of bytes necessary to read <paramref name="length"/> characters
        /// will be read from the buffer. If a zero-character is found, the string will
        /// be truncated at that point.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read UTF-16 characters.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <returns>A string value.</returns>
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

        /// <summary>
        /// Reads an object from the buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISerializable"/> to read.</typeparam>
        /// <returns>An <see cref="ISerializable"/> object.</returns>
        public T Read<T>() where T : ISerializable, new()
        {
            T obj = new T();
            obj.ReadData(this, SaveDataFormat.Default);

            return obj;
        }

        /// <summary>
        /// Reads an object from the buffer using the specified data format.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISerializable"/> to read.</typeparam>
        /// <param name="format">The data format.</param>
        /// <returns>An <see cref="ISerializable"/> object.</returns>
        public T Read<T>(SaveDataFormat format) where T : ISerializable, new()
        {
            T obj = new T();
            obj.ReadData(this, format);

            return obj;
        }

        /// <summary>
        /// Reads an array from the buffer.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="count">The number of elements to read.</param>
        /// <param name="itemLength">
        /// The number of bytes to read per element if <typeparamref name="T"/> is
        /// <see cref="byte"/> array, <see cref="bool"/>, or <see cref="string"/>.
        /// Ignored otherwise.
        /// </param>
        /// <param name="unicode">A value indicating whether to read UTF-16 characters.</param>
        /// <returns>An array of type <typeparamref name="T"/>.</returns>
        public T[] Read<T>(int count,
            int itemLength = 0,
            bool unicode = false)
        {
            return Read<T>(count, SaveDataFormat.Default, itemLength, unicode);
        }

        /// <summary>
        /// Reads an array from the buffer using the specified data format for
        /// each element.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="count">The number of elements to read.</param>
        /// <param name="format">The element data format, if applicable.</param>
        /// <param name="itemLength">
        /// The number of bytes to read per element if <typeparamref name="T"/> is
        /// <see cref="byte"/> array, <see cref="bool"/>, or <see cref="string"/>.
        /// Ignored otherwise.
        /// </param>
        /// <param name="unicode">A value indicating whether to read UTF-16 characters.</param>
        /// <returns>An array of type <typeparamref name="T"/>.</returns>
        public T[] Read<T>(int count, SaveDataFormat format,
            int itemLength = 0,
            bool unicode = false)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                GenericRead(format, out T obj, itemLength, unicode);
                items.Add(obj);
            }

            return items.ToArray();
        }
        #endregion

        #region Write Functions
        /// <summary>
        /// Writes a value from the buffer.
        /// </summary>
        /// <typeparam name="T">The type of data to write.</typeparam>
        /// <param name="value">The <typeparamref name="T"/> value to write.</param>
        /// <param name="format">The data format, if applicable.</param>
        /// <param name="length">
        /// The number of bytes to write if <paramref name="value"/> is a
        /// <see cref="byte"/> array, <see cref="bool"/>, or <see cref="string"/> value.
        /// Ignored otherwise.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to write UTF-16 characters if
        /// <typeparamref name="T"/> is a <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="SerializationException">
        /// Thrown if <typeparamref name="T"/> is not one of the following:
        /// <see cref="byte"/> array,
        /// <see cref="byte"/>,
        /// <see cref="sbyte"/>,
        /// <see cref="bool"/>,
        /// <see cref="char"/>,
        /// <see cref="double"/>,
        /// <see cref="float"/>,
        /// <see cref="int"/>,
        /// <see cref="long"/>,
        /// <see cref="short"/>,
        /// <see cref="uint"/>,
        /// <see cref="ulong"/>,
        /// <see cref="ushort"/>,
        /// <see cref="string"/>,
        /// <see cref="ISerializable"/>.
        /// </exception>
        internal int GenericWrite<T>(T value, SaveDataFormat format,
            int length = 0,
            bool unicode = false)
        {
            Type t = typeof(T);
            if (t.IsEnum) t = Enum.GetUnderlyingType(t);

            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            if (t == typeof(byte[])) return Write((byte[]) (object) value);
            else if (t == typeof(byte)) return Write(Convert.ToByte(value));
            else if (t == typeof(sbyte)) return Write(Convert.ToSByte(value));
            else if (t == typeof(bool)) return Write(Convert.ToBoolean(value), (length == 0) ? 1 : length);
            else if (t == typeof(char)) return Write(Convert.ToChar(value), unicode);
            else if (t == typeof(double)) return Write(Convert.ToDouble(value));
            else if (t == typeof(float)) return Write(Convert.ToSingle(value));
            else if (t == typeof(int)) return Write(Convert.ToInt32(value));
            else if (t == typeof(uint)) return Write(Convert.ToUInt32(value));
            else if (t == typeof(long)) return Write(Convert.ToInt64(value));
            else if (t == typeof(ulong)) return Write(Convert.ToUInt64(value));
            else if (t == typeof(short)) return Write(Convert.ToInt16(value));
            else if (t == typeof(ushort)) return Write(Convert.ToUInt16(value));
            else if (t == typeof(string)) return Write(Convert.ToString(value), length, unicode);
            else if (t.Implements(typeof(ISerializable))) return Write((ISerializable) value, format);
            else throw SerializationNotSupported(typeof(T));
        }

        public int Write(byte[] data, int index, int count)
        {
            try
            {
                m_buffer.Write(data, index, count);
                return count;
            }
            catch (NotSupportedException)
            {
                throw new SerializationException(Strings.Error_NotExpandable);
            }
        }

        public int Write(byte[] data)
        {
            return Write(data, 0, data.Length);
        }

        public int Write(byte value)
        {
            m_buffer.WriteByte(value);
            return sizeof(byte);
        }

        public int Write(sbyte value)
        {
            m_buffer.WriteByte((byte) value);
            return sizeof(sbyte);
        }

        public int Write(bool value, int byteCount = 1)
        {
            if (byteCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            }

            byte[] data = new byte[byteCount];
            int index = (BigEndian) ? byteCount - 1 : 0;
            data[index] = (value) ? (byte) 1 : (byte) 0;

            return Write(data);
        }

        public int Write(char value, bool unicode = false)
        {
            return (unicode)
                ? Write((ushort) value)
                : Write((byte) value);
        }

        public int Write(float value)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return Write(data);
        }

        public int Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return Write(data);
        }

        public int Write(short value)
        {
            return Write((ushort) value);
        }

        public int Write(int value)
        {
            return Write((uint) value);
        }

        public int Write(long value)
        {
            return Write((ulong) value);
        }

        public int Write(ushort value)
        {
            byte[] data = new byte[sizeof(ushort)];
            if (BigEndian)
            {
                data[0] = (byte) ((value & 0xFF00) >> 8);
                data[1] = (byte) ((value & 0x00FF));
            }
            else
            {
                data[0] = (byte) ((value & 0x00FF));
                data[1] = (byte) ((value & 0xFF00) >> 8);
            }

            return Write(data);
        }

        public int Write(uint value)
        {
            byte[] data = new byte[sizeof(uint)];
            if (BigEndian)
            {
                data[0] = (byte) ((value & 0xFF000000) >> 24);
                data[1] = (byte) ((value & 0x00FF0000) >> 16);
                data[2] = (byte) ((value & 0x0000FF00) >> 8);
                data[3] = (byte) ((value & 0x000000FF));
            }
            else
            {
                data[0] = (byte) ((value & 0x000000FF));
                data[1] = (byte) ((value & 0x0000FF00) >> 8);
                data[2] = (byte) ((value & 0x00FF0000) >> 16);
                data[3] = (byte) ((value & 0xFF000000) >> 24);

            }

            return Write(data);
        }

        public int Write(ulong value)
        {
            byte[] data = new byte[sizeof(ulong)];
            if (BigEndian)
            {
                data[0] = (byte) ((value & 0xFF00000000000000) >> 56);
                data[1] = (byte) ((value & 0x00FF000000000000) >> 48);
                data[2] = (byte) ((value & 0x0000FF0000000000) >> 40);
                data[3] = (byte) ((value & 0x000000FF00000000) >> 32);
                data[4] = (byte) ((value & 0x00000000FF000000) >> 24);
                data[5] = (byte) ((value & 0x0000000000FF0000) >> 16);
                data[6] = (byte) ((value & 0x000000000000FF00) >> 8);
                data[7] = (byte) ((value & 0x00000000000000FF));
            }
            else
            {
                data[0] = (byte) ((value & 0x00000000000000FF));
                data[1] = (byte) ((value & 0x000000000000FF00) >> 8);
                data[2] = (byte) ((value & 0x0000000000FF0000) >> 16);
                data[3] = (byte) ((value & 0x00000000FF000000) >> 24);
                data[4] = (byte) ((value & 0x000000FF00000000) >> 32);
                data[5] = (byte) ((value & 0x0000FF0000000000) >> 40);
                data[6] = (byte) ((value & 0x00FF000000000000) >> 48);
                data[7] = (byte) ((value & 0xFF00000000000000) >> 56);
            }

            return Write(data);
        }

        public int Write(string value,
            int? length = null,
            bool unicode = false,
            bool zeroTerminate = true)
        {  
            Encoding encoding = (unicode)
                ? Encoding.Unicode
                : Encoding.ASCII;

            // Use whole string if 'length' parameter not specified
            int actualLength = (length == 0)
                ? (value.Length)
                : (length ?? value.Length);


            // Add extra byte for null terminator if 'length' not specified
            if ((length == null || length == 0) && zeroTerminate)
            {
                actualLength += 1;
            }

            // If the length is unspecified, the null character will be added to the end of the
            // string regardless. Otherwise, the string will be truncated if necessary so as to
            // not exceed the specified length when the null character is written.
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

            int bytesWritten = Write(encoding.GetBytes(value));
            Skip(numPadding);

            return bytesWritten + numPadding;
        }

        public int Write<T>(T value) where T : ISerializable
        {
            return Write(value, SaveDataFormat.Default);
        }

        public int Write<T>(T value, SaveDataFormat format) where T : ISerializable
        {
            if (value == null) throw new ArgumentNullException("The value cannot be null.", nameof(value));
            return value.WriteData(this, format);
        }

        public int Write<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            return Write(items, SaveDataFormat.Default, count, itemLength, unicode);
        }

        public int Write<T>(T[] items,
            SaveDataFormat format,
            int? count = null,
            int itemLength = 0, // Note: only applies to "bool" and "string types
            bool unicode = false)
            where T : new()
        {
            int capacity = items.Length;
            int bytesWritten = 0;

            for (int i = 0; i < (count ?? capacity); i++)
            {
                bytesWritten += (i < capacity)
                    ? GenericWrite(items.ElementAt(i), format, itemLength, unicode)
                    : GenericWrite(new T(), format, itemLength, unicode);
            }

            return bytesWritten;
        }
        #endregion

        #region Other Functions
        public void Dispose()
        {
            if (!m_disposed)
            {
                m_buffer.Dispose();
                m_disposed = true;
            }
        }

        /// <summary>
        /// Rounds an address up to the next multiple of 4.
        /// </summary>
        /// <param name="addr">The address to align.</param>
        /// <returns>The aligned address.</returns>
        public static int Align4Bytes(int addr)
        {
            const int WordSize = 4;
            return (addr + WordSize - 1) & ~(WordSize - 1);
        }

        /// <summary>
        /// Sets the cursor to the next multiple of 4.
        /// </summary>
        public void Align4Bytes()
        {
            Skip(Align4Bytes(Cursor) - Cursor);
        }

        /// <summary>
        /// Sets the cursor and mark to 0.
        /// </summary>
        public void Reset()
        {
            Seek(0);
            MarkCurrentPosition();
        }

        /// <summary>
        /// Sets <see cref="Mark"/> to the current cursor position.
        /// </summary>
        /// <returns>The marked position.</returns>
        public int MarkCurrentPosition()
        {
            return Mark = Cursor;
        }

        /// <summary>
        /// Sets the cursor to the specified offset in the buffer.
        /// </summary>
        /// <param name="pos">The new position in the buffer.</param>
        public void Seek(int pos)
        {
            m_buffer.Position = pos;
        }

        /// <summary>
        /// Advances the cursor ahead or behind the specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip</param>
        public void Skip(int count)
        {
            if (m_buffer.Position + count > m_buffer.Length)
            {
                Write(new byte[count]);
            }
            else
            {
                m_buffer.Position += count;
            }
        }

        /// <summary>
        /// Gets all data up to the current cursor position.
        /// </summary>
        /// <returns>The data up to the current position.</returns>
        public byte[] GetBytes()
        {
            return GetAllBytes().Take(Cursor).ToArray();
        }

        /// <summary>
        /// Gets all data in the buffer.
        /// </summary>
        /// <returns>The data in the buffer.</returns>
        public byte[] GetAllBytes()
        {
            return m_buffer.ToArray();
        }
        #endregion

        #region Helpers
        private static SerializationException SerializationNotSupported(Type t)
        {
            return new SerializationException(Strings.Error_SerializationNotAllowed, t.Name);
        }

        private static EndOfStreamException EndOfStream()
        {
            return new EndOfStreamException(Strings.Error_EndOfStream);
        }
        #endregion
    }
}
