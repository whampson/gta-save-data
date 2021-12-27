using GTASaveData.Extensions;
using GTASaveData.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GTASaveData
{
    /// <summary>
    /// A random-access byte buffer with sequential read/write capability.
    /// </summary>
    /// <remarks>
    /// When a read or write operation is performed, the data pointer is incremented
    /// by the number of bytes operated on. The pointer may also be manually set forward
    /// or back arbitrarily within the bounds of the buffer.
    /// </remarks>
    public sealed class DataBuffer : IDisposable
    {
        private static readonly byte[] DefaultPaddingBytes = new byte[1] { 0 };

        private readonly MemoryStream m_buffer;
        private byte[] m_paddingBytes;
        private bool m_disposed;

        /// <summary>
        /// Gets or sets a value indicating whether to read and write data in big-endian byte order.
        /// </summary>
        /// <remarks>
        /// The endianness of serialized structs is determined by the host CPU. If you need to control
        /// the endianness of a structure, consider implementing <see cref="ISaveDataObject"/> and using
        /// <see cref="ReadObject{T}(SerializationParams)"/> and <see cref="Write(ISaveDataObject, SerializationParams)"/>
        /// to serialize your struct.
        /// </remarks>
        public bool BigEndian { get; set; }

        /// <summary>
        /// Gets or sets a saved position in the buffer.
        /// </summary>
        public int MarkedPosition { get; set; }

        /// <summary>
        /// Gets the current buffer position.
        /// </summary>
        public int Position => (int) m_buffer.Position;

        /// <summary>
        /// Gets the number of bytes spanning from <see cref="MarkedPosition"/> to <see cref="Position"/>.
        /// </summary>
        public int Offset => Position - MarkedPosition;

        /// <summary>
        /// Gets the length of the buffer in bytes.
        /// </summary>
        public int Length => (int) m_buffer.Length;

        /// <summary>
        /// Gets or sets the padding type.
        /// </summary>
        public PaddingScheme PaddingType { get; set; }

        /// <summary>
        /// Gets or sets the pattern used when the padding type is <see cref="PaddingScheme.Pattern"/>.
        /// </summary>
        public byte[] PaddingBytes
        { 
            get { return m_paddingBytes; }
            set { m_paddingBytes = value ?? DefaultPaddingBytes; }
        }

        /// <summary>
        /// Creates a new expandable <see cref="DataBuffer"/>.
        /// </summary>
        public DataBuffer()
            : this(new MemoryStream())
        { }

        /// <summary>
        /// Creates a new fixed-size <see cref="DataBuffer"/> with the specified data.
        /// </summary>
        /// <param name="data">The data to contain within the buffer.</param>
        public DataBuffer(byte[] data)
            : this(new MemoryStream(data))
        { }

        /// <summary>
        /// Creates a new fixed-size <see cref="DataBuffer"/>.
        /// </summary>
        /// <param name="size">The buffer size.</param>
        public DataBuffer(int size)
            : this(new byte[size])
        { }

        private DataBuffer(MemoryStream buffer)
        {
            m_buffer = buffer;
            PaddingBytes = DefaultPaddingBytes;
        }

        /// <summary>
        /// Reads a generic value.
        /// </summary>
        /// <typeparam name="T">The type of data to read.</typeparam>
        /// <param name="p">The <see cref="SerializationParams"/>, if applicable.</param>
        /// <param name="obj">
        /// An object of type <typeparamref name="T"/> extracted from the
        /// buffer.
        /// </param>
        /// <param name="length">
        /// The number of bytes/characters to read if <paramref name="obj"/> is a
        /// <see cref="byte"/> array, <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes read.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException">
        /// Thrown if <typeparamref name="T"/> is not one of the following:
        /// <see cref="byte"/>[],
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
        /// <see cref="ISaveDataObject"/>.
        /// </exception>
        internal int GenericRead<T>(SerializationParams p,
            out T obj,
            int length = 0,
            bool unicode = false)
        {
            p ??= new SerializationParams();

            Type t = typeof(T);
            if (t.IsEnum) t = Enum.GetUnderlyingType(t);

            object o = null;
            int mark = Position;

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
            else if (t.Implements(typeof(ISaveDataObject)))
            {
                var tp = new Type[] { typeof(SerializationParams) };
                var m = GetType().GetMethod(nameof(ReadObject), tp).MakeGenericMethod(t);
                try
                {
                    o = m.Invoke(this, new object[] { p });
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
            }
            else if (t.IsValueType)
            {
                var m = GetType().GetMethod(nameof(ReadStruct)).MakeGenericMethod(t);
                o = m.Invoke(this, null);
            }
            else throw SerializationNotSupported(typeof(T));

            obj = (T) o;
            return Position - mark;
        }

        /// <summary>
        /// Reads a block of bytes.
        /// </summary>
        /// <param name="buffer">The output buffer.</param>
        /// <param name="index">The index in the output buffer at which to begin storing bytes.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read, or -1 if the end of the stream has been reached.</returns>
        public int Read(byte[] buffer, int index, int count)
        {
            return m_buffer.Read(buffer, index, count);
        }

        /// <summary>
        /// Reads a byte array.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public byte[] ReadBytes(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            byte[] data = new byte[count];
            int bytesRead = Read(data, 0, count);
            if (bytesRead < count)
            {
                throw EndOfStream();
            }

            return data;
        }

        /// <summary>
        /// Reads an unsigned 8-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
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
        /// Reads a signed 8-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
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
        /// Reads a true/false value.
        /// </summary>
        /// <remarks>
        /// A stream of zeros is considered false.
        /// </remarks>
        /// <param name="byteCount">The number of bytes to read and treat as true/false.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public bool ReadBool(int byteCount = 1)
        {
            if (byteCount < 1) throw new ArgumentOutOfRangeException(nameof(byteCount));

            byte[] buffer = ReadBytes(byteCount);
            byte value = 0;

            foreach (byte b in buffer)
            {
                value |= b;
            }

            return value != 0;
        }

        /// <summary>
        /// Reads an 8- or 16-bit character.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read 16-bit characters.</param>
        /// <exception cref="EndOfStreamException"/>
        public char ReadChar(bool unicode = false)
        {
            return (unicode)
                ? (char) ReadUInt16()
                : (char) ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit IEEE-754 float value.
        /// </summary>
        /// <returns>A float value.</returns>
        /// <exception cref="EndOfStreamException"/>
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
        /// Reads a 64-bit IEEE-754 float value.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
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
        /// Reads a signed 16-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public short ReadInt16()
        {
            return (short) ReadUInt16();
        }

        /// <summary>
        /// Reads a signed 32-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public int ReadInt32()
        {
            return (int) ReadUInt32();
        }

        /// <summary>
        /// Reads a signed 64-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public long ReadInt64()
        {
            return (long) ReadUInt64();
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public ushort ReadUInt16()
        {
            byte[] data = ReadBytes(sizeof(ushort));
            return (BigEndian)
                ? (ushort) ((data[0] << 8) | data[1])
                : (ushort) ((data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public uint ReadUInt32()
        {
            byte[] data = ReadBytes(sizeof(uint));
            return (BigEndian)
                ? (uint) ((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3])
                : (uint) ((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer.
        /// </summary>
        /// <exception cref="EndOfStreamException"/>
        public ulong ReadUInt64()
        {
            ulong[] data = ReadBytes(sizeof(ulong)).Select(x => (ulong) x).ToArray();
            return (BigEndian)
                ? ((data[0] << 56) | (data[1] << 48) | (data[2] << 40) | (data[3] << 32) | (data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7])
                : ((data[7] << 56) | (data[6] << 48) | (data[5] << 40) | (data[4] << 32) | (data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
        }

        /// <summary>
        /// Reads a zero-terminated string.
        /// </summary>
        /// <param name="unicode">A value indicating whether to read 16-bit characters.</param>
        /// <exception cref="EndOfStreamException"/>
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
        /// Reads a fixed-length string.
        /// </summary>
        /// <remarks>
        /// The number of bytes necessary for <paramref name="length"/> characters
        /// will be read. If a zero is found, the resulting string will be truncated at that point.
        /// </remarks>
        /// <param name="unicode">A value indicating whether to read 16-bit characters.</param>
        /// <param name="length">The number of characters to read.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public string ReadString(int length, bool unicode = false)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0) return ReadString(unicode);

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
        /// Reads a structure.
        /// </summary>
        /// <remarks>
        /// The endianness of serialized structs is determined by the host CPU. If you need to control
        /// the endianness of a structure, consider implementing <see cref="ISaveDataObject"/> and using
        /// <see cref="ReadObject{T}(SerializationParams)"/> and <see cref="Write(ISaveDataObject, SerializationParams)"/>
        /// to serialize your struct.
        /// </remarks>
        /// <typeparam name="T">The type of struct to read.</typeparam>
        /// <exception cref="EndOfStreamException"/>
        public T ReadStruct<T>() where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var pObj = Marshal.AllocHGlobal(size);
            var data = ReadBytes(size);

            Marshal.Copy(data, 0, pObj, size);
            T obj = Marshal.PtrToStructure<T>(pObj);
            Marshal.FreeHGlobal(pObj);

            return obj;
        }

        /// <summary>
        /// Reads an <see cref="ISaveDataObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISaveDataObject"/> to read.</typeparam>
        /// <exception cref="EndOfStreamException"/>
        public T ReadObject<T>() where T : ISaveDataObject, new()
        {
            T obj = new T();
            obj.ReadData(this, new SerializationParams());

            return obj;
        }

        /// <summary>
        /// Reads an <see cref="ISaveDataObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISaveDataObject"/> to read.</typeparam>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is read.</param>
        /// <exception cref="EndOfStreamException"/>
        public T ReadObject<T>(SerializationParams p) where T : ISaveDataObject, new()
        {
            T obj = new T();
            obj.ReadData(this, p);

            return obj;
        }

        /// <summary>
        /// Reads an <see cref="ISaveDataObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISaveDataObject"/> to read.</typeparam>
        /// <param name="obj">the object to populate.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is read.</param>
        /// <exception cref="EndOfStreamException"/>
        public int ReadObject<T>(T obj, SerializationParams p) where T : ISaveDataObject
        {
            return obj.ReadData(this, p);
        }

        /// <summary>
        /// Reads an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="count">The number of elements to read.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to read per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public T[] ReadArray<T>(int count,
            int itemLength = 0,
            bool unicode = false)
        {
            return ReadArray<T>(count, new SerializationParams(), itemLength, unicode);
        }

        /// <summary>
        /// Reads an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="count">The number of elements to read.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is read per element.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to read per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public T[] ReadArray<T>(int count, SerializationParams p,
            int itemLength = 0,
            bool unicode = false)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                GenericRead(p, out T obj, itemLength, unicode);
                items.Add(obj);
            }

            return items.ToArray();
        }

        /// <summary>
        /// Writes a generic value.
        /// </summary>
        /// <typeparam name="T">The type of data to write.</typeparam>
        /// <param name="value">The <typeparamref name="T"/> value to write.</param>
        /// <param name="p">The <see cref="SerializationParams"/>, if applicable.</param>
        /// <param name="length">
        /// The number of bytes/characters to write if <paramref name="value"/> is a
        /// <see cref="byte"/>[], <see cref="bool"/> or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to write 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException">
        /// Thrown if <typeparamref name="T"/> is not one of the following:
        /// <see cref="byte"/>[],
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
        /// <see cref="ISaveDataObject"/>.
        /// </exception>
        internal int GenericWrite<T>(T value, SerializationParams p,
            int length = 0,
            bool unicode = false)
        {
            p ??= new SerializationParams();

            Type t = typeof(T);
            if (t.IsEnum) t = Enum.GetUnderlyingType(t);

            if (value == null) throw new ArgumentNullException(nameof(value));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            byte[] b = value as byte[];
            if (t == typeof(byte[])) return Write(b, 0, (length == 0) ? b.Length : length);
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
            else if (t.Implements(typeof(ISaveDataObject))) return Write((ISaveDataObject) value, p);
            else if (t.IsValueType) return Write(t, value);
            else throw SerializationNotSupported(typeof(T));
        }

        /// <summary>
        /// Writes a block of bytes.
        /// </summary>
        /// <param name="buffer">The input buffer containing data to write.</param>
        /// <param name="index">The starting index in the input buffer.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <returns>The actual number of bytes written, or -1 if the end of the stream has been reached.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(byte[] buffer, int index, int count)
        {
            try
            {
                m_buffer.Write(buffer, index, count);
                return count;
            }
            catch (NotSupportedException)   // "Memory stream is not expandable."
            {
                return -1;
            }
        }

        /// <summary>
        /// Writes a byte array.
        /// </summary>
        /// <param name="data">The bytes to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(byte[] data)
        {
            int bytesWritten = Write(data, 0, data.Length);
            if (bytesWritten < data.Length)
            {
                throw EndOfStream();
            }

            return bytesWritten;
        }

        /// <summary>
        /// Writes an unsigned 8-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(byte value)
        {
            try
            {
                m_buffer.WriteByte(value);
                return sizeof(byte);
            }
            catch (NotSupportedException)   // "Memory stream is not expandable."
            {
                throw EndOfStream();
            }
        }

        /// <summary>
        /// Writes a signed 8-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(sbyte value)
        {
            try
            {
                m_buffer.WriteByte((byte) value);
                return sizeof(sbyte);
            }
            catch (NotSupportedException)   // "Memory stream is not expandable."
            {
                throw EndOfStream();
            }
        }

        /// <summary>
        /// Writes a true/false value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(bool value)
        {
            return Write(value, 1);
        }

        /// <summary>
        /// Writes a true/false value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="byteCount">The number of bytes to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write(bool value, int byteCount)
        {
            if (byteCount < 1) throw new ArgumentOutOfRangeException(nameof(byteCount));

            byte[] data = new byte[byteCount];
            int index = (BigEndian) ? byteCount - 1 : 0;
            data[index] = (value) ? (byte) 1 : (byte) 0;

            return Write(data);
        }

        /// <summary>
        /// Writes an 8-bit character.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(char value)
        {
           return Write(value, false);
        }

        /// <summary>
        /// Writes an 8- or 16-bit character.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="unicode">A value indicating whether to write 16-bit characters.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(char value, bool unicode)
        {
            return (unicode)
                ? Write((ushort) value)
                : Write((byte) value);
        }

        /// <summary>
        /// Writes a 32-bit IEEE-754 float value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(float value)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return Write(data);
        }

        /// <summary>
        /// Writes a 64-bit IEEE-754 float value.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return Write(data);
        }

        /// <summary>
        /// Writes a signed 16-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(short value)
        {
            return Write((ushort) value);
        }

        /// <summary>
        /// Writes a signed 32-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(int value)
        {
            return Write((uint) value);
        }

        /// <summary>
        /// Writes a signed 64-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write(long value)
        {
            return Write((ulong) value);
        }

        /// <summary>
        /// Writes an unsigned 16-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
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

        /// <summary>
        /// Writes an unsigned 32-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
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

        /// <summary>
        /// Writes an unsigned 64-bit integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
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

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="value">
        /// The string value to write.
        /// </param>
        /// <param name="length">
        /// The number of characters to write. If this value exceeds the string
        /// length, the string will be truncated according to <paramref name="zeroTerminate"/>.
        /// If this value is less than the string length, zeros will be written
        /// until the length is reached. If this value is null, the entire string
        /// will be written.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to write 16-bit characters.
        /// </param>
        /// <param name="zeroTerminate">
        /// A value indicating whether to terminate the string with a zero.
        /// If this value is true and the string length is greater than <paramref name="length"/>,
        /// the string will be truncated and a zero will be written as the final character so as
        /// to not exceed <paramref name="length"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write(string value,
            int? length = null,
            bool unicode = false,
            bool zeroTerminate = true)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Encoding encoding = (unicode)
                ? (BigEndian) ? Encoding.BigEndianUnicode : Encoding.Unicode
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

        /// <summary>
        /// Writes a structure.
        /// </summary>
        /// <remarks>
        /// The endianness of serialized structs is determined by the host CPU. If you need to control
        /// the endianness of a structure, consider implementing <see cref="ISaveDataObject"/> and using
        /// <see cref="ReadObject{T}(SerializationParams)"/> and <see cref="Write(ISaveDataObject, SerializationParams)"/>
        /// to serialize your struct.
        /// </remarks>
        /// <typeparam name="T">The type of struct to write.</typeparam>
        /// <param name="value">The struct value to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="EndOfStreamException"/>
        public int Write<T>(T value) where T : struct
        {
            return Write(typeof(T), value);
        }

        private int Write(Type t, object obj)
        {
            var size = Marshal.SizeOf(t);
            var pObj = Marshal.AllocHGlobal(size);
            var data = new byte[size];

            Marshal.StructureToPtr(obj, pObj, true);
            Marshal.Copy(pObj, data, 0, size);
            Marshal.FreeHGlobal(pObj);

            return Write(data);
        }

        /// <summary>
        /// Writes an <see cref="ISaveDataObject"/>.
        /// </summary>
        /// <param name="value">The object to write.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write(ISaveDataObject value)
        {
            return Write(value, new SerializationParams());
        }

        /// <summary>
        /// Writes an <see cref="ISaveDataObject"/> using the specified data format.
        /// </summary>
        /// <param name="value">The object to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write(ISaveDataObject value, SerializationParams p)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.WriteData(this, p);
        }

        /// <summary>
        /// Writes an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="items">The elements to write.</param>
        /// <param name="count">The number of elements to write. If this value is null, the entire array will be written.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to write per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            return Write(items, new SerializationParams(), count, itemLength, unicode);
        }

        /// <summary>
        /// Writes an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="items">The elements to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is written per element.</param>
        /// <param name="count">The number of elements to write. If this value is null, the entire array will be written.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to write per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write<T>(T[] items,
            SerializationParams p,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            int capacity = items.Length;
            int bytesWritten = 0;

            for (int i = 0; i < (count ?? capacity); i++)
            {
                bytesWritten += (i < capacity)
                    ? GenericWrite(items.ElementAt(i), p, itemLength, unicode)
                    : GenericWrite(new T(), p, itemLength, unicode);
            }

            return bytesWritten;
        }

        /// <summary>
        /// Writes an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="items">The elements to write.</param>
        /// <param name="count">The number of elements to write. If this value is null, the entire array will be written.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to write per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write<T>(ObservableArray<T> items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            return Write(items, new SerializationParams(), count, itemLength, unicode);
        }

        /// <summary>
        /// Writes an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The array element type.</typeparam>
        /// <param name="items">The elements to write.</param>
        /// <param name="p">A <see cref="SerializationParams"/> object controlling how data is written per element.</param>
        /// <param name="count">The number of elements to write. If this value is null, the entire array will be written.</param>
        /// <param name="itemLength">
        /// The number of bytes/characters to write per element if <typeparamref name="T"/> is
        /// <see cref="byte"/>[], <see cref="bool"/>, or <see cref="string"/>.
        /// </param>
        /// <param name="unicode">
        /// A value indicating whether to read 16-bit characters if
        /// <typeparamref name="T"/> is a <see cref="char"/> or <see cref="string"/>.
        /// </param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="EndOfStreamException"/>
        public int Write<T>(ObservableArray<T> items,
            SerializationParams p,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            return Write(items.ToArray(), p, count, itemLength, unicode);
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
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
        public static int Align4(int addr)
        {
            if (addr < 0) throw new ArgumentOutOfRangeException(nameof(addr));

            const int WordSize = 4;
            return (addr + WordSize - 1) & ~(WordSize - 1);
        }

        /// <summary>
        /// Sets the cursor to the next address that is a multiple of 4.
        /// If the current address is a multiple of 4, the cursor is not moved.
        /// </summary>
        public void Align4()
        {
            Pad(Align4(Offset) - Offset);
        }

        /// <summary>
        /// Sets the cursor and mark to 0.
        /// </summary>
        public void Reset()
        {
            Seek(0);
            Mark();
        }

        /// <summary>
        /// Sets <see cref="MarkedPosition"/> to the current cursor position.
        /// </summary>
        /// <returns>The marked position.</returns>
        public int Mark()
        {
            return MarkedPosition = Position;
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
        public int Skip(int count)
        {
            if (m_buffer.Position + count > m_buffer.Length)
            {
                return Write(new byte[count]);
            }

            m_buffer.Position += count;
            return (int) m_buffer.Position;
        }

        /// <summary>
        /// Writes padding bytes to the buffer based on the current
        /// <see cref="PaddingType"/>.
        /// </summary>
        /// <param name="count">The number of bytes to write.</param>
        public int Pad(int count)
        {
            switch (PaddingType)
            {
                case PaddingScheme.Skip:
                {
                    return Skip(count);
                }

                case PaddingScheme.Zero:
                {
                    return Write(new byte[count]);
                }

                case PaddingScheme.Pattern:
                {
                    byte[] pad = new byte[count];
                    byte[] seq = PaddingBytes;
                    for (int i = 0; i < count; i++)
                    {
                        pad[i] = seq[i % seq.Length];
                    }
                    
                    return Write(pad);
                }

                case PaddingScheme.Random:
                {
                    byte[] pad = new byte[count];
                    Random rand = new Random();
                    rand.NextBytes(pad);
                    
                    return Write(pad);
                }
            }

            throw new InvalidOperationException(Strings.Error_InvalidOperation_Generic);
        }

        /// <summary>
        /// Gets a copy of the data in the buffer from the start up to the current cursor position.
        /// </summary>
        public byte[] GetBytes()
        {
            return GetBuffer().Take(Position).ToArray();
        }

        /// <summary>
        /// Gets a copy of the data in the buffer from the marked position up to the current cursor position.
        /// </summary>
        public byte[] GetBytesFromMark()
        {
            return GetBuffer().Skip(MarkedPosition).Take(Position).ToArray();
        }

        /// <summary>
        /// Gets a copy of all data in the buffer.
        /// </summary>
        public byte[] GetBuffer()
        {
            return m_buffer.ToArray();
        }

        private static NotSupportedException SerializationNotSupported(Type t)
        {
            return new NotSupportedException(string.Format(Strings.Error_Serialization_NotAllowed, t.Name));
        }

        private static EndOfStreamException EndOfStream()
        {
            return new EndOfStreamException(Strings.Error_EndOfStream);
        }

        private static EndOfStreamException EndOfStream(Exception innerException)
        {
            return new EndOfStreamException(Strings.Error_EndOfStream, innerException);
        }
    }

    /// <summary>
    /// Padding schemes are used to control the values written for data structure and buffer padding.
    /// </summary>
    public enum PaddingScheme
    {
        /// <summary>
        /// Skips over the number of bytes neccessary for padding, preserving the existing data.
        /// Behaves like a recycled C array.
        /// </summary>
        Skip,

        /// <summary>
        /// Zeros will be used as padding.
        /// </summary>
        Zero,

        /// <summary>
        /// A specific pattern will be used as padding.
        /// </summary>
        Pattern,

        /// <summary>
        /// Random bytes will be used as padding.
        /// </summary>
        Random,
    };
}
