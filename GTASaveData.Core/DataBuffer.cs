using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GTASaveData
{
    public sealed class DataBuffer : IDisposable
    {
        private readonly MemoryStream m_buffer;
        private readonly BinaryReader m_reader;
        private readonly BinaryWriter m_writer;
        private bool m_disposed;

        public int Mark { get; set; }

        public int Position
        {
            get { return (int) m_buffer.Position; }
        }

        public int Offset
        {
            get { return Position - Mark; }
        }

        public int Length
        {
            get { return (int) m_buffer.Length; }
        }

        public int Capacity
        {
            get { return m_buffer.Capacity; }
        }

        public DataBuffer()
            : this(new MemoryStream())
        { }

        public DataBuffer(byte[] data)
            : this(new MemoryStream(data))
        { }

        private DataBuffer(MemoryStream buffer)
        {
            m_buffer = buffer;
            m_reader = new BinaryReader(m_buffer, Encoding.ASCII, true);
            m_writer = new BinaryWriter(m_buffer, Encoding.ASCII, true);
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_writer.Dispose();
                m_reader.Dispose();
                m_disposed = true;
            }
        }

        public static int Align4Bytes(int addr)
        {
            const int WordSize = 4;

            return (addr + WordSize - 1) & ~(WordSize - 1);
        }

        public void Align4Bytes()
        {
            Skip(Align4Bytes(Position) - Position);
        }

        public void Reset()
        {
            Seek(0);
            MarkPosition();
        }

        public int MarkPosition()
        {
            return Mark = Position;
        }

        public void Seek(int pos)
        {
            m_buffer.Position = pos;
        }

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

        public byte[] GetBytes()
        {
            return m_buffer.ToArray();
        }

        public byte[] GetBytesUpToCursor()
        {
            return GetBytes().Take(Position).ToArray();
        }

        #region Read Functions
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

        public sbyte ReadSByte()
        {
            return m_reader.ReadSByte();
        }

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return m_reader.ReadBytes(count);
        }

        public int Read(byte[] buffer, int index, int count)
        {
            return m_reader.Read(buffer, index, count);
        }

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

        public float ReadSingle()
        {
            return m_reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return m_reader.ReadDouble();
        }

        public short ReadInt16()
        {
            return m_reader.ReadInt16();
        }

        public ushort ReadUInt16()
        {
            return m_reader.ReadUInt16();
        }

        public int ReadInt32()
        {
            return m_reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return m_reader.ReadUInt32();
        }

        public long ReadInt64()
        {
            return m_reader.ReadInt64();
        }

        public ulong ReadUInt64()
        {
            return m_reader.ReadUInt64();
        }

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

        public T ReadObject<T>() where T : ISaveDataObject, new()
        {
            T obj = new T();
            obj.ReadObjectData(this);

            return obj;
        }

        public T ReadObject<T>(SaveFileFormat format) where T : ISaveDataObject, new()
        {
            T obj = new T();
            obj.ReadObjectData(this, format);

            return obj;
        }

        public T[] ReadArray<T>(int count,
            int itemLength = 0,
            bool unicode = false)
        {
            return ReadArray<T>(count, SaveFileFormat.Default, itemLength, unicode);
        }

        public T[] ReadArray<T>(int count,
            SaveFileFormat format,
            int itemLength = 0,     // Note: only applies to bool string types.
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

        internal int GenericRead<T>(SaveFileFormat format,
            out T obj,
            int length = 0,
            bool unicode = false)
        {
            Type t = typeof(T);
            object o = null;
            int mark = Position;

            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            if (t == typeof(bool))
            {
                o = ReadBool((length == 0) ? 1 : length);
            }
            else if (t == typeof(byte))
            {
                o = ReadByte();
            }
            else if (t == typeof(sbyte))
            {
                o = ReadSByte();
            }
            else if (t == typeof(byte[]))
            {
                o = ReadBytes(length);
            }
            else if (t == typeof(char))
            {
                o = ReadChar(unicode);
            }
            else if (t == typeof(double))
            {
                o = ReadDouble();
            }
            else if (t == typeof(float))
            {
                o = ReadSingle();
            }
            else if (t == typeof(int))
            {
                o = ReadInt32();
            }
            else if (t == typeof(uint))
            {
                o = ReadUInt32();
            }
            else if (t == typeof(long))
            {
                o = ReadInt64();
            }
            else if (t == typeof(ulong))
            {
                o = ReadUInt64();
            }
            else if (t == typeof(short))
            {
                o = ReadInt16();
            }
            else if (t == typeof(ushort))
            {
                o = ReadUInt16();
            }
            else if (t == typeof(string))
            {
                o = (length == 0) ? ReadString(unicode) : ReadString(length, unicode);
            }
            else if (t.GetInterface(nameof(ISaveDataObject)) != null)
            {
                var m = GetType().GetMethod(nameof(ReadObject), new Type[] { typeof(SaveFileFormat) }).MakeGenericMethod(t);
                o = m.Invoke(this, new object[] { format });
            }
            else
            {
                throw SerializationNotSupportedException(typeof(T));
            }

            obj = (T) o;
            return Position - mark;
        }
        #endregion

        #region Write Functions
        public int Write(bool value, int byteCount = 1)
        {
            if (byteCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            }

            byte[] buffer = new byte[byteCount];
            buffer[0] = (value) ? (byte) 1 : (byte) 0;      // little endian

            return Write(buffer);
        }

        public int Write(byte value)
        {
            m_writer.Write(value);

            return sizeof(byte);
        }

        public int Write(sbyte value)
        {
            m_writer.Write(value);

            return sizeof(sbyte);
        }

        public int Write(byte[] buffer)
        {
            m_writer.Write(buffer);

            return buffer.Length;
        }

        public int Write(byte[] buffer, int index, int count)
        {
            m_writer.Write(buffer, index, count);

            return count;
        }

        public int Write(char value, bool unicode = false)
        {
            // BinaryWriter#Write(char) relies on the encoding specified in the constructor
            // to determine how many bytes to write for a character. Since GTA saves sometimes
            // have a mixture of Unicode and ASCII strings, and we can't change the encoding
            // once the writer is created, we're going to bypass the encoding altogether.
            // "Unicode" in this sense means UTF-16, i.e. 16-bit characters.

            if (char.IsSurrogate(value))
            {
                throw new ArgumentException(Strings.Error_Argument_NoSurrogateChars, nameof(value));
            }

            return (unicode)
                ? Write((ushort) value)
                : Write((byte) value);
        }

        public int Write(float value)
        {
            m_writer.Write(value);

            return sizeof(float);
        }

        public int Write(double value)
        {
            m_writer.Write(value);

            return sizeof(double);
        }

        public int Write(short value)
        {
            m_writer.Write(value);

            return sizeof(short);
        }

        public int Write(int value)
        {
            m_writer.Write(value);

            return sizeof(int);
        }

        public int Write(long value)
        {
            m_writer.Write(value);

            return sizeof(long);
        }

        public int Write(ushort value)
        {
            m_writer.Write(value);

            return sizeof(ushort);
        }

        public int Write(uint value)
        {
            m_writer.Write(value);

            return sizeof(uint);
        }

        public int Write(ulong value)
        {
            m_writer.Write(value);

            return sizeof(ulong);
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

        public int Write<T>(T value) where T : ISaveDataObject
        {
            return Write(value, SaveFileFormat.Default);
        }

        public int Write<T>(T value, SaveFileFormat format) where T : ISaveDataObject
        {
            return value.WriteObjectData(this, format);
        }

        public int Write<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            return Write(items, SaveFileFormat.Default, count, itemLength, unicode);
        }

        public int Write<T>(T[] items,
            SaveFileFormat format,
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

        internal int GenericWrite<T>(T value, SaveFileFormat format,
            int length = 0,
            bool unicode = false)
        {
            Type t = typeof(T);

            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            if (t == typeof(bool))
            {
                return Write(Convert.ToBoolean(value), (length == 0) ? 1 : length);
            }
            else if (t == typeof(byte))
            {
                return Write(Convert.ToByte(value));
            }
            else if (t == typeof(sbyte))
            {
                return Write(Convert.ToSByte(value));
            }
            else if (t == typeof(byte[]))
            {
                return Write((byte[]) (object) value);
            }
            else if (t == typeof(char))
            {
                return Write(Convert.ToChar(value), unicode);
            }
            else if (t == typeof(double))
            {
                return Write(Convert.ToDouble(value));
            }
            else if (t == typeof(float))
            {
                return Write(Convert.ToSingle(value));
            }
            else if (t == typeof(int))
            {
                return Write(Convert.ToInt32(value));
            }
            else if (t == typeof(uint))
            {
                return Write(Convert.ToUInt32(value));
            }
            else if (t == typeof(long))
            {
                return Write(Convert.ToInt64(value));
            }
            else if (t == typeof(ulong))
            {
                return Write(Convert.ToUInt64(value));
            }
            else if (t == typeof(short))
            {
                return Write(Convert.ToInt16(value));
            }
            else if (t == typeof(ushort))
            {
                return Write(Convert.ToUInt16(value));
            }
            else if (t == typeof(string))
            {
                return Write(Convert.ToString(value), length, unicode);
            }
            else if (t.GetInterface(nameof(ISaveDataObject)) != null)
            {
                return Write((ISaveDataObject) value, format);
            }

            throw SerializationNotSupportedException(typeof(T));
        }
        #endregion

        private static SerializationException SerializationNotSupportedException(Type t)
        {
            return new SerializationException(Strings.Error_InvalidOperation_Serialization, t.Name);
        }
    }
}
