using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GTASaveData.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Serializer : IDisposable
    {
        /// <summary>
        /// Aligns an address to the next multiple of the specified word size.
        /// </summary>
        /// <param name="address">The address to align.</param>
        /// <param name="wordSize">The word size to align to. Note: must be a power of 2.</param>
        /// <returns>The aligned address.</returns>
        public static long Align(long address, int wordSize = 4)
        {
            if (wordSize < 1 || ((wordSize - 1) & wordSize) != 0)
            {
                throw new ArgumentException("Word size must be a power of 2.", nameof(wordSize));
            }

            wordSize--;
            return (address + wordSize) & ~wordSize;
        }

        /// <summary>
        /// Creates an object from the specifed data buffer.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="buffer">The data to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during deserialization.</exception>
        public static T Deserialize<T>(byte[] buffer)
        {
            return Deserialize<T>(buffer, SystemType.Unspecified, 0, false);
        }

        /// <summary>
        /// Creates an object from the specifed data buffer.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="buffer">The data to deserialize.</param>
        /// <param name="system">The system this data is formatted for.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during deserialization.</exception>
        public static T Deserialize<T>(byte[] buffer, SystemType system)
        {
            return Deserialize<T>(buffer, system, 0, false);
        }

        /// <summary>
        /// Creates an object from the specifed data buffer.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="buffer">The data to deserialize.</param>
        /// <param name="system">The system this data is formatted for.</param>
        /// <param name="length">The length of the data to be deserialized. Note: only pertains to some types.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during deserialization.</exception>
        public static T Deserialize<T>(byte[] buffer, SystemType system, int length)
        {
            return Deserialize<T>(buffer, system, length, false);
        }

        /// <summary>
        /// Creates an object from the specifed data buffer.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="buffer">The data to deserialize.</param>
        /// <param name="system">The system this data is formatted for.</param>
        /// <param name="length">The length of the data to be deserialized. Note: only pertains to some types.</param>
        /// <param name="unicode">Indicates whether to decode characters as Unicode.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during deserialization.</exception>
        public static T Deserialize<T>(byte[] buffer, SystemType system, int length, bool unicode)
        {
            using (Serializer s = new Serializer(new MemoryStream(buffer)))
            {
                return s.GenericRead<T>(system, length, unicode);
            }
        }

        /// <summary>
        /// Encodes this object as a sequence of bytes.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized object as a byte array.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during serialization.</exception>
        public static byte[] Serialize<T>(T obj)
        {
            return Serialize(obj, SystemType.Unspecified, 0, false);
        }

        /// <summary>
        /// Encodes this object as a sequence of bytes.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="system">The system this data should be formatted for.</param>
        /// <returns>The serialized object as a byte array.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during serialization.</exception>
        public static byte[] Serialize<T>(T obj, SystemType system)
        {
            return Serialize(obj, system, 0, false);
        }

        /// <summary>
        /// Encodes this object as a sequence of bytes.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="system">The system this data should be formatted for.</param>
        /// <param name="length">The length of the data to be serialized. Note: only pertains to some types.</param>
        /// <returns>The serialized object as a byte array.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during serialization.</exception>
        public static byte[] Serialize<T>(T obj, SystemType system, int length)
        {
            return Serialize(obj, system, length, false);
        }

        /// <summary>
        /// Encodes an object as a sequence of bytes.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="system">The system this data should be formatted for.</param>
        /// <param name="length">The length of the data to be serialized. Note: only pertains to some types.</param>
        /// <param name="unicode">Indicates whether to encode characters as Unicode.</param>
        /// <returns>The serialized object as a byte array.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during serialization.</exception>
        public static byte[] Serialize<T>(T obj, SystemType system, int length, bool unicode)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer s = new Serializer(m))
                {
                    s.GenericWrite(obj, system, length, unicode);
                }

                return m.ToArray();
            }
        }

        private readonly BinaryReader m_reader;
        private readonly BinaryWriter m_writer;
        private bool m_disposed;

        /// <summary>
        /// Creates a new <see cref="GTAObject"/> using the specified
        /// stream as the serialization endpoint.
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
                throw new ArgumentException("The base stream must be both readable and writable.", nameof(baseStream));
            }
            
            m_reader = new BinaryReader(baseStream, Encoding.ASCII, true);
            m_writer = new BinaryWriter(baseStream, Encoding.ASCII, true);
            m_disposed = false;
        }

        /// <summary>
        /// Gets the underlying serialization <see cref="Stream"/>.
        /// </summary>
        public Stream BaseStream
        {
            get { return m_reader.BaseStream; }
        }

        /// <summary>
        /// Aligns the current position in the serialization stream to a multiple
        /// of the specified word size.
        /// </summary>
        /// <param name="wordSize">The word size to align to. Note: must be a power of 2.</param>
        public void Align(int wordSize = 4)
        {
            long pos = BaseStream.Position;
            long num = Align(pos, wordSize) - pos;

            BaseStream.Write(new byte[num], 0, (int) num);
        }

        /// <summary>
        /// Reads an n-byte Boolean value.
        /// False is represented by all bits being 0.
        /// </summary>
        /// <param name="byteCount">The number of bytes to treat as a single Boolean.</param>
        /// <returns>A bool.</returns>
        public bool ReadBool(int byteCount = 1)
        {
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
            // BinaryWriter::ReadChar() relies on the encoding specified in the constructor
            // to determine how many bytes to write for a character. Since GTA saves sometimes
            // have a mixture of Unicode and ASCII strings, and we can't change the encoding
            // once the writer is created, we're going to bypass the built-in encoding processing
            // altogether. "Unicode" in this sense means UTF-16, or 16-bit characters.

            return (unicode)
                ? (char) ReadUInt16()
                : (char) ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit single-precision floating-point number.
        /// </summary>
        /// <returns>A float.</returns>
        public float ReadSingle()
        {
            return m_reader.ReadSingle();
        }

        /// <summary>
        /// Reads a 64-bit double-precision floating-point number.
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
        /// Reads a string of the specified length. If a zero character is found, the
        /// string will be truncated at that point. If the specified length is
        /// nonzero, the stream position will advance until the length is reached.
        /// </summary>
        /// <param name="length">The number of characters to read. Specify 0 to read until the first zero character.</param>
        /// <param name="unicode">A value indicating whether to read unicode characters.</param>
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

        /// <summary>
        /// Reads an object of the specified type. Object creation occurs by invoking that object's
        /// deserialization constructor.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="systemType">The system that the data is formatted for.</param>
        /// <returns>An object.</returns>
        /// <exception cref="SerializationException">Thrown if something goes wrong during deserialization.</exception>
        public T ReadObject<T>(SystemType systemType = SystemType.Unspecified)
        {
            ConstructorInfo ctor0 = typeof(T).GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                null,
                new[] { typeof(Serializer), typeof(SystemType) },
                null
            );
            ConstructorInfo ctor1 = typeof(T).GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                null,
                new[] { typeof(Serializer) },
                null
            );

            if (ctor0 != null)
            {
                return (T) ctor0.Invoke(new object[] { this, systemType });
            }
            else if (ctor1 != null)
            {
                return (T) ctor1.Invoke(new object[] { this });
            }

            throw new SerializationException(
                   string.Format("The type '{0}' does not implement a de-serialization constructor.", typeof(T).Name));

            
        }

        /// <summary>
        /// Reads an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="count">The number of items to read.</param>
        /// <param name="system">The system that the data is formatted for.</param>
        /// <param name="itemLength">The length of each item. Note: only applies to bool and string types.</param>
        /// <param name="unicode">A value indicating whether to read unicode characters.</param>
        /// <returns>An array of the specified type.</returns>
        public T[] ReadArray<T>(int count, SystemType system = SystemType.Unspecified, int itemLength = 0, bool unicode = false)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                items.Add(GenericRead<T>(system, itemLength, unicode));
            }

            return items.ToArray();
        }

        public void Write(bool value, int byteCount = 1)
        {
            byte[] buffer = new byte[byteCount];
            buffer[0] = (value) ? (byte) 1 : (byte) 0;      // little endian

            Write(buffer);
        }

        public void Write(byte value)
        {
            m_writer.Write(value);
        }

        public void Write(sbyte value)
        {
            m_writer.Write(value);
        }

        public void Write(byte[] buffer)
        {
            m_writer.Write(buffer);
        }

        public void Write(char value, bool unicode = false)
        {
            // BinaryWriter::Write(char) relies on the encoding specified in the constructor
            // to determine how many bytes to write for a character. Since GTA saves sometimes
            // have a mixture of Unicode and ASCII strings, and we can't change the encoding
            // once the writer is created, we're going to bypass the encoding altogether.
            // "Unicode" in this sense means UTF-16, or 16-bit characters.

            if (char.IsSurrogate(value))
            {
                throw new ArgumentException("Surrogate code units are not supported.", nameof(value));
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

        public void Write(float value)
        {
            m_writer.Write(value);
        }

        public void Write(double value)
        {
            m_writer.Write(value);
        }

        public void Write(short value)
        {
            m_writer.Write(value);
        }

        public void Write(int value)
        {
            m_writer.Write(value);
        }

        public void Write(long value)
        {
            m_writer.Write(value);
        }

        public void Write(ushort value)
        {
            m_writer.Write(value);
        }

        public void Write(uint value)
        {
            m_writer.Write(value);
        }

        public void Write(ulong value)
        {
            m_writer.Write(value);
        }

        public void Write(string value, int? length = null, bool unicode = false, bool zeroTerminate = true)
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
                    ? value.Substring(0, actualLength - 1).PadRight(actualLength, '\0')
                    : value.Substring(0, actualLength);
            }
            else
            {
                value = value.PadRight(actualLength, '\0');
            }

            Write(encoding.GetBytes(value));
        }

        public void WriteObject<T>(T value, SystemType systemType = SystemType.Unspecified)
        {
            const string SerializationMethodName = "Serialize";

            MethodInfo method0 = typeof(T).GetMethod(
                SerializationMethodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(Serializer), typeof(SystemType) },
                null
            );
            MethodInfo method1 = typeof(T).GetMethod(
                SerializationMethodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(Serializer) },
                null
            );

            if (method0 != null)
            {
                method0.Invoke(value, new object[] { this, systemType });
            }
            else if (method1 != null)
            {
                method1.Invoke(value, new object[] { this });
            }
            else
            {
                throw new SerializationException(
                    string.Format("The type '{0}' does not implement a serialization function.", typeof(T).Name));
            }
        }

        public void WriteArray<T>(ObservableCollection<T> items, int? count = null, SystemType systemType = SystemType.Unspecified, int? itemLength = null, bool unicode = false)
            where T : new()
        {
            int capacity = items.Count;
            for (int i = 0; i < (count ?? capacity); i++)
            {
                if (i < capacity)
                {
                    GenericWrite(items.ElementAt(i), systemType, itemLength ?? 0, unicode);
                }
                else
                {
                    GenericWrite(new T(), systemType, itemLength ?? 0, unicode);
                }
            }
        }

        private T GenericRead<T>(SystemType systemType, int length, bool unicode)
        {
            Type t = typeof(T);
            object ret;

            if (t == typeof(bool))
            {
                ret = ReadBool(length);
            }
            else if (t == typeof(byte))
            {
                ret = ReadByte();
            }
            else if (t == typeof(sbyte))
            {
                ret = ReadSByte();
            }
            else if (t == typeof(char))
            {
                ret = ReadChar();
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
                ret = (length == 0)
                    ? ReadString(unicode)
                    : ReadString(length, unicode);
            }
            else
            {
                ret = ReadObject<T>(systemType);
            }

            return (T) ret;
        }

        private void GenericWrite<T>(T value, SystemType systemType, int length, bool unicode)
        {
            Type t = typeof(T);

            if (t == typeof(bool))
            {
                Write(Convert.ToBoolean(value), length);
            }
            else if (t == typeof(byte))
            {
                Write(Convert.ToByte(value));
            }
            else if (t == typeof(sbyte))
            {
                Write(Convert.ToSByte(value));
            }
            else if (t == typeof(char))
            {
                Write(Convert.ToChar(value));
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
            else
            {
                WriteObject(value, systemType);
            }
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
