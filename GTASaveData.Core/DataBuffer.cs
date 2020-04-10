using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GTASaveData
{
    public sealed class DataBuffer : IDisposable
    {
        private readonly MemoryStream m_buffer;
        private bool m_disposed;

        public bool BigEndian { get; set; }
        public int Mark { get; set; }
        public int Position => (int) m_buffer.Position;
        public int Offset => Position - Mark;
        public int Length => (int) m_buffer.Length;
        public int Capacity => m_buffer.Capacity;

        public DataBuffer()
            : this(new MemoryStream())
        { }

        public DataBuffer(byte[] data)
            : this(new MemoryStream(data))
        { }

        private DataBuffer(MemoryStream buffer)
        {
            m_buffer = buffer;
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
            int b = m_buffer.ReadByte();
            if (b == -1)
            {
                throw EndOfStream();
            }

            return (sbyte) b;
        }

        public byte ReadByte()
        {
            int b = m_buffer.ReadByte();
            if (b == -1)
            {
                throw EndOfStream();
            }

            return (byte) b;
        }

        public byte[] ReadBytes(int count)
        {
            byte[] data = new byte[count];
            Read(data, 0, count);

            return data;
        }

        public int Read(byte[] buffer, int index, int count)
        {
            return m_buffer.Read(buffer, index, count);
        }

        public char ReadChar(bool unicode = false)
        {
            return (unicode)
                ? (char) ReadUInt16()   // "Unicode", aka UCS-2/UTF-16
                : (char) ReadByte();
        }

        public float ReadFloat()
        {
            byte[] data = ReadBytes(sizeof(float));
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return BitConverter.ToSingle(data, 0);
        }

        public double ReadDouble()
        {
            byte[] data = ReadBytes(sizeof(double));
            if (BigEndian)
            {
                data = data.Reverse().ToArray();
            }

            return BitConverter.ToDouble(data, 0);
        }

        public short ReadInt16()
        {
            return (short) ReadUInt16();
        }

        public int ReadInt32()
        {
            return (int) ReadUInt32();
        }

        public long ReadInt64()
        {
            return (long) ReadUInt64();
        }

        public ushort ReadUInt16()
        {
            byte[] data = ReadBytes(sizeof(ushort));
            return (BigEndian)
                ? (ushort) ((data[0] << 8) | data[1])
                : (ushort) ((data[1] << 8) | data[0]);
        }

        public uint ReadUInt32()
        {
            byte[] data = ReadBytes(sizeof(uint));
            return (BigEndian)
                ? (uint) ((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3])
                : (uint) ((data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
        }

        public ulong ReadUInt64()
        {
            ulong[] data = ReadBytes(sizeof(ulong)).Select(x => (ulong) x).ToArray();
            return (BigEndian)
                ? ((data[0] << 56) | (data[1] << 48) | (data[2] << 40) | (data[3] << 32) | (data[4] << 24) | (data[5] << 16) | (data[6] << 8) | data[7])
                : ((data[7] << 56) | (data[6] << 48) | (data[5] << 40) | (data[4] << 32) | (data[3] << 24) | (data[2] << 16) | (data[1] << 8) | data[0]);
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

        public T Read<T>() where T : ISaveDataObject, new()
        {
            T obj = new T();
            obj.ReadObjectData(this);

            return obj;
        }

        public T Read<T>(SaveFileFormat format) where T : ISaveDataObject, new()
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
                o = ReadFloat();
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
                var m = GetType().GetMethod(nameof(Read), new Type[] { typeof(SaveFileFormat) }).MakeGenericMethod(t);
                o = m.Invoke(this, new object[] { format });
            }
            else
            {
                throw SerializationNotSupported(typeof(T));
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

            byte[] data = new byte[byteCount];
            int index = (BigEndian) ? byteCount - 1 : 0;
            data[index] = (value) ? (byte) 1 : (byte) 0;

            return Write(data);
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

        public int Write(byte[] data)
        {
            return Write(data, 0, data.Length);
        }

        public int Write(byte[] data, int index, int count)
        {
            m_buffer.Write(data, index, count);
            return count;
        }

        public int Write(char value, bool unicode = false)
        {
            if (char.IsSurrogate(value))
            {
                throw new ArgumentException(Strings.Error_Argument_NoSurrogateChars, nameof(value));
            }

            return (unicode)
                ? Write((ushort) value)     // "Unicode", aka UCS-2/UTF-16
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

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

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

            throw SerializationNotSupported(typeof(T));
        }
        #endregion

        #region Helper Functions
        public void Dispose()
        {
            if (!m_disposed)
            {
                m_buffer.Dispose();
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

        private static SerializationException SerializationNotSupported(Type t)
        {
            return new SerializationException(Strings.Error_InvalidOperation_Serialization, t.Name);
        }

        private static EndOfStreamException EndOfStream()
        {
            return new EndOfStreamException(Strings.Error_IO_EndOfStream);
        }
        #endregion
    }
}
