using Bogus;
using GTASaveData.Types;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSerialization : TestBase
    {
        [Fact]
        public void AlignedAddress()
        {
            Assert.Equal(0, DataBuffer.Align4(0));
            Assert.Equal(4, DataBuffer.Align4(4));
            Assert.Equal(8, DataBuffer.Align4(5));
            Assert.Equal(8, DataBuffer.Align4(6));
            Assert.Equal(8, DataBuffer.Align4(7));
        }

        [Fact]
        public void AlignedData()
        {
            Faker f = new Faker();

            bool b0 = f.Random.Bool();
            int i0 = f.Random.Int();
            float f0 = f.Random.Float();
            byte[] data;
            int i1;
            bool b1;
            float f1;

            using (DataBuffer wb = new DataBuffer())
            {
                wb.Write(b0);
                wb.Align4();
                wb.Write(i0);
                wb.Write(f0);
                data = wb.GetBuffer();
            }

            using (DataBuffer wb = new DataBuffer(data))
            {
                b1 = wb.ReadBool();
                wb.Align4();
                i1 = wb.ReadInt32();
                f1 = wb.ReadFloat();
            }

            Assert.Equal(b0, b1);
            Assert.Equal(i0, i1);
            Assert.Equal(f0, f1);
            Assert.Equal(12, data.Length);
        }

        [Theory]
        [InlineData(PaddingScheme.Zero, null)]
        [InlineData(PaddingScheme.Random, null)]
        [InlineData(PaddingScheme.Pattern, new byte[] { 0 })]
        [InlineData(PaddingScheme.Pattern, new byte[] { 0xCA, 0xFE, 0xBA, 0xBE })]
        public void Padding(PaddingScheme mode, byte[] seq)
        {
            byte[] data;
            using (DataBuffer wb = new DataBuffer() { PaddingType = mode, PaddingBytes = seq })
            {
                wb.Pad(100);
                data = wb.GetBuffer();
                Assert.True(data.Length > 0);
            }

            switch (mode)
            {
                case PaddingScheme.Zero:
                {
                    Assert.Equal(0, data.Sum(x => x));
                    break;
                }
                case PaddingScheme.Random:
                {
                    Assert.NotEqual(0, data.Sum(x => x));
                    break;
                }
                case PaddingScheme.Pattern:
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        Assert.Equal(seq[i % seq.Length], data[i]);
                    }
                    break;
                }
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void Bool(bool value, bool bigEndian)
        {
            bool x0 = value;
            byte[] data = Serializer.Write(x0, bigEndian);
            bool x1 = Serializer.Read<bool>(data);

            Assert.Equal(x0, x1);
            Assert.Single(data);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void BoolMultiByte(bool value, bool bigEndian)
        {
            Faker f = new Faker();
            int numBytes = f.Random.Int(2, 8);

            bool x0 = value;
            byte[] data;
            bool x1;

            using (DataBuffer wb = new DataBuffer() { BigEndian = bigEndian })
            {
                wb.Write(x0, numBytes);
                data = wb.GetBuffer();
            }

            using (DataBuffer wb = new DataBuffer(data) { BigEndian = bigEndian })
            {
                x1 = wb.ReadBool(numBytes);
            }

            Assert.Equal(x0, x1);
            Assert.Equal(numBytes, data.Length);
        }

        [Fact]
        public void Byte()
        {
            Faker f = new Faker();

            byte x0 = f.Random.Byte();
            byte[] data = Serializer.Write(x0);
            byte x1 = Serializer.Read<byte>(data);

            Assert.Equal(x0, x1);
            Assert.Single(data);
        }

        [Fact]
        public void SByte()
        {
            Faker f = new Faker();

            sbyte x0 = f.Random.SByte();
            byte[] data = Serializer.Write(x0);
            sbyte x1 = Serializer.Read<sbyte>(data);

            Assert.Equal(x0, x1);
            Assert.Single(data);
        }

        [Fact]
        public void ByteArray()
        {
            Faker f = new Faker();
            int numBytes = f.Random.Int(10, 100);
            
            byte[] x0 = f.Random.Bytes(numBytes);
            byte[] data = Serializer.Write(x0);
            byte[] x1;

            using (DataBuffer wb = new DataBuffer(data))
            {
                x1 = wb.ReadBytes(data.Length);
            }

            Assert.Equal(x0, x1);
            Assert.Equal(x0, data);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Char(bool bigEndian)
        {
            Faker f = new Faker();

            char x0 = f.Random.Char('\u0000', '\u00FF');
            byte[] data = Serializer.Write(x0, bigEndian);
            char x1 = Serializer.Read<char>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Single(data);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CharUnicode(bool bigEndian)
        {
            Faker f = new Faker();

            char x0 = f.Random.Char('\u0000', '\uD7FF');
            byte[] data;
            char x1;

            using (DataBuffer wb = new DataBuffer() { BigEndian = bigEndian })
            {
                wb.Write(x0, true);
                data = wb.GetBuffer();
            }

            using (DataBuffer wb = new DataBuffer(data) { BigEndian = bigEndian })
            {
                x1 = wb.ReadChar(true);
            }

            Assert.Equal(x0, x1);
            Assert.Equal(2, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Double(bool bigEndian)
        {
            Faker f = new Faker();

            double x0 = f.Random.Double();
            byte[] data = Serializer.Write(x0, bigEndian);
            double x1 = Serializer.Read<double>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Float(bool bigEndian)
        {
            Faker f = new Faker();

            float x0 = f.Random.Float();
            byte[] data = Serializer.Write(x0, bigEndian);
            float x1 = Serializer.Read<float>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int16(bool bigEndian)
        {
            Faker f = new Faker();

            short x0 = f.Random.Short();
            byte[] data = Serializer.Write(x0, bigEndian);
            short x1 = Serializer.Read<short>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(2, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt16(bool bigEndian)
        {
            Faker f = new Faker();

            ushort x0 = f.Random.UShort();
            byte[] data = Serializer.Write(x0, bigEndian);
            ushort x1 = Serializer.Read<ushort>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(2, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int32(bool bigEndian)
        {
            Faker f = new Faker();

            int x0 = f.Random.Int();
            byte[] data = Serializer.Write(x0, bigEndian);
            int x1 = Serializer.Read<int>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt32(bool bigEndian)
        {
            Faker f = new Faker();

            uint x0 = f.Random.UInt();
            byte[] data = Serializer.Write(x0, bigEndian);
            uint x1 = Serializer.Read<uint>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int64(bool bigEndian)
        {
            Faker f = new Faker();

            long x0 = f.Random.Long();
            byte[] data = Serializer.Write(x0, bigEndian);
            long x1 = Serializer.Read<long>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt64(bool bigEndian)
        {
            Faker f = new Faker();

            ulong x0 = f.Random.ULong();
            byte[] data = Serializer.Write(x0, bigEndian);
            ulong x1 = Serializer.Read<ulong>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Fact]
        public void AsciiString()
        {
            Faker f = new Faker();
            string s0 = Generator.AsciiString(f, f.Random.Int(1, 100));
            byte[] data = StringToBytes(s0);
            string s1 = BytesToString(data);

            Assert.Equal(s0, s1);
            Assert.Equal(s0.Length + 1, data.Length);
        }

        [Theory]
        [InlineData(8, 7, 7)]       // Shorter than buffer length
        [InlineData(8, 8, 7)]       // Equal to buffer length
        [InlineData(8, 9, 7)]       // Larger than buffer length
        public void AsciiStringFixedLength(int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.AsciiString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength);
            string s1 = BytesToString(data, bufferLength);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Theory]
        [InlineData(8, 7, 7)]       // Shorter than buffer length
        [InlineData(8, 8, 8)]       // Equal to buffer length
        [InlineData(8, 9, 8)]       // Larger than buffer length
        public void AsciiStringFixedLengthNoZero(int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.AsciiString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, zeroTerminate: false);
            string s1 = BytesToString(data, bufferLength);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UnicodeString(bool bigEndian)
        {
            Faker f = new Faker();
            string s0 = Generator.UnicodeString(f, f.Random.Int(1, 100));
            byte[] data = StringToBytes(s0, unicode: true, bigEndian: bigEndian);
            string s1 = BytesToString(data, unicode: true, bigEndian: bigEndian);

            Assert.Equal(s0, s1);
            Assert.Equal(s0.Length + 1, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
        }

        [Theory]
        [InlineData(false, 24, 23, 23)]        // Shorter than buffer length
        [InlineData(false, 24, 24, 23)]        // Equal to buffer length
        [InlineData(false, 24, 25, 23)]        // Larger than buffer length
        [InlineData(true, 24, 23, 23)]
        [InlineData(true, 24, 24, 23)]
        [InlineData(true, 24, 25, 23)]
        public void UnicodeStringFixedLength(bool bigEndian, int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.UnicodeString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, unicode: true, bigEndian: bigEndian);
            string s1 = BytesToString(data, bufferLength, unicode: true, bigEndian: bigEndian);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Theory]
        [InlineData(false, 24, 23, 23)]        // Shorter than buffer length
        [InlineData(false, 24, 24, 24)]        // Equal to buffer length
        [InlineData(false, 24, 25, 24)]        // Larger than buffer length
        [InlineData(true, 24, 23, 23)]
        [InlineData(true, 24, 24, 24)]
        [InlineData(true, 24, 25, 24)]
        public void UnicodeStringFixedLengthNoZero(bool bigEndian, int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.UnicodeString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, true, bigEndian: bigEndian, zeroTerminate: false);
            string s1 = BytesToString(data, bufferLength, true, bigEndian: bigEndian);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValueArray(bool bigEndian)
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            int[] x0 = Generator.Array(count, g => f.Random.Int());
            byte[] data = ArrayToBytes(x0, bigEndian: bigEndian);
            int[] x1 = BytesToArray<int>(data, count, bigEndian: bigEndian);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(4 * count, data.Length);
        }

        [Theory]
        [InlineData(false, 8, 7, 8)]       // Shorter than buffer count
        [InlineData(false, 8, 8, 8)]       // Equal to buffer count
        [InlineData(false, 8, 9, 8)]       // Larger than buffer count
        [InlineData(true, 8, 7, 8)]
        [InlineData(true, 8, 8, 8)]
        [InlineData(true, 8, 9, 8)]
        public void ValueArrayFixedLength(bool bigEndian, int bufferCount, int initialCount, int expectedCount)
        {
            Faker f = new Faker();

            int[] x0 = Generator.Array(initialCount, g => f.Random.Int());
            byte[] data = ArrayToBytes(x0, bufferCount, bigEndian: bigEndian);
            int[] x1 = BytesToArray<int>(data, bufferCount, bigEndian: bigEndian);

            Assert.Equal(initialCount, x0.Length);
            Assert.Equal(expectedCount, x1.Length);
            Assert.Equal(x0.Take(Math.Min(bufferCount, initialCount)), x1.Take(Math.Min(bufferCount, initialCount)));
            Assert.Equal(4 * bufferCount, data.Length);
        }

        [Fact]
        public void Struct()
        {
            SystemTime x0 = new SystemTime(Generator.Date(new Faker()));
            byte[] data = Serializer.Write(x0);
            SystemTime x1 = Serializer.Read<SystemTime>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(Serializer.SizeOf<SystemTime>(), data.Length);
        }

        [Fact]
        public void StructArray()
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            SystemTime[] x0 = Generator.Array(count, g => new SystemTime(Generator.Date(f)));
            byte[] data = ArrayToBytes(x0);
            SystemTime[] x1 = BytesToArray<SystemTime>(data, count);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(Serializer.SizeOf<SystemTime>() * count, data.Length);
        }

        [Theory]
        [InlineData(8, 7, 8)]       // Shorter than buffer count
        [InlineData(8, 8, 8)]       // Equal to buffer count
        [InlineData(8, 9, 8)]       // Larger than buffer count
        public void StructArrayFixedLength(int bufferCount, int initialCount, int expectedCount)
        {
            Faker f = new Faker();

            SystemTime[] x0 = Generator.Array(initialCount, g => new SystemTime(Generator.Date(f)));
            byte[] data = ArrayToBytes(x0, bufferCount);
            SystemTime[] x1 = BytesToArray<SystemTime>(data, bufferCount);

            Assert.Equal(initialCount, x0.Length);
            Assert.Equal(expectedCount, x1.Length);
            Assert.Equal(x0.Take(Math.Min(bufferCount, initialCount)), x1.Take(Math.Min(bufferCount, initialCount)));
            Assert.Equal(Serializer.SizeOf<SystemTime>() * bufferCount, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Object(bool bigEndian)
        {
            TestObject x0 = TestObject.Generate();
            byte[] data = Serializer.Write(x0, bigEndian);
            TestObject x1 = Serializer.Read<TestObject>(data, bigEndian);

            Assert.Equal(x0, x1);
            Assert.Equal(Serializer.SizeOf<TestObject>(), data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ObjectArray(bool bigEndian)
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            TestObject[] x0 = Generator.Array(count, g => TestObject.Generate());
            byte[] data = ArrayToBytes(x0, bigEndian: bigEndian);
            TestObject[] x1 = BytesToArray<TestObject>(data, count, bigEndian: bigEndian);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(Serializer.SizeOf<TestObject>() * count, data.Length);
        }

        [Theory]
        [InlineData(false, 8, 7, 8)]       // Shorter than buffer count
        [InlineData(false, 8, 8, 8)]       // Equal to buffer count
        [InlineData(false, 8, 9, 8)]       // Larger than buffer count
        [InlineData(true, 8, 7, 8)]
        [InlineData(true, 8, 8, 8)]
        [InlineData(true, 8, 9, 8)]
        public void ObjectArrayFixedLength(bool bigEndian, int bufferCount, int initialCount, int expectedCount)
        {
            Faker f = new Faker();

            TestObject[] x0 = Generator.Array(initialCount, g => TestObject.Generate());
            byte[] data = ArrayToBytes(x0, bufferCount, bigEndian: bigEndian);
            TestObject[] x1 = BytesToArray<TestObject>(data, bufferCount, bigEndian: bigEndian);

            Assert.Equal(initialCount, x0.Length);
            Assert.Equal(expectedCount, x1.Length);
            Assert.Equal(x0.Take(Math.Min(bufferCount, initialCount)), x1.Take(Math.Min(bufferCount, initialCount)));
            Assert.Equal(Serializer.SizeOf<TestObject>() * bufferCount, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void BoolArrayMultiByte(bool bigEndian)
        {
            Faker f = new Faker();
            int count = f.Random.Int(10, 100);
            int numBytes = f.Random.Int(1, 8);

            bool[] x0 = Generator.Array(count, g => f.Random.Bool());
            byte[] data = ArrayToBytes(x0, itemLength: numBytes, bigEndian: bigEndian);
            bool[] x1 = BytesToArray<bool>(data, count, itemLength: numBytes, bigEndian: bigEndian);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(numBytes * count, data.Length);
        }

        [Fact]
        public void ArrayWithNullItem()
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            TestObject[] x0 = Generator.Array(count, g => TestObject.Generate());
            x0[count - 1] = null;
            Assert.Throws<ArgumentNullException>(() => ArrayToBytes(x0));
        }

        [Fact]
        public void NullObject()
        {
            TestObject x = null;
            Assert.Throws<ArgumentNullException>(() => Serializer.Write(x));
        }

        [Fact]
        public void NullString()
        {
            string x = null;
            Assert.Throws<ArgumentNullException>(() => Serializer.Write(x));
        }

        [Fact]
        public void NullArray()
        {
            TestObject[] x = null;
            Assert.Throws<ArgumentNullException>(() => Serializer.Write(x));
        }

        [Fact]
        public void NonSerializableObject()
        {
            TestObject2 x = new TestObject2();
            Assert.Throws<NotSupportedException>(() => Serializer.Write(x));
        }

        #region Test Objects
        public class TestObject : SaveDataObject, IEquatable<TestObject>
        {
            public char Ascii { get; set; }
            public int Integer { get; set; }
            public bool Boolean { get; set; }
            public char Unicode { get; set; }
            public float Single { get; set; }

            public TestObject()
            { }

            protected override void ReadData(DataBuffer buf, FileType fmt)
            {
                Ascii = buf.ReadChar();         // 0x00
                Integer = buf.ReadInt32();      // 0x01
                Boolean = buf.ReadBool();       // 0x05
                Unicode = buf.ReadChar(true);   // 0x06
                Single = buf.ReadFloat();       // 0x08
            }

            protected override void WriteData(DataBuffer buf, FileType fmt)
            {
                buf.Write(Ascii);
                buf.Write(Integer);
                buf.Write(Boolean);
                buf.Write(Unicode, true);
                buf.Write(Single);
            }

            protected override int GetSize(FileType fmt)
            {
                return 1 + sizeof(int) + 1 + 2 + sizeof(float);
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash += 23 * Ascii.GetHashCode();
                hash += 23 * Integer.GetHashCode();
                hash += 23 * Boolean.GetHashCode();
                hash += 23 * Unicode.GetHashCode();
                hash += 23 * Single.GetHashCode();

                return hash;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TestObject);
            }

            public bool Equals(TestObject other)
            {
                if (other == null)
                {
                    return false;
                }

                return Ascii.Equals(other.Ascii)
                    && Integer.Equals(other.Integer)
                    && Boolean.Equals(other.Boolean)
                    && Unicode.Equals(other.Unicode)
                    && Single.Equals(other.Single);
            }

            public static TestObject Generate()
            {
                Faker<TestObject> model = new Faker<TestObject>()
                    .RuleFor(x => x.Ascii, f => Generator.Ascii(f))
                    .RuleFor(x => x.Integer, f => f.Random.Int())
                    .RuleFor(x => x.Boolean, f => f.Random.Bool())
                    .RuleFor(x => x.Unicode, f => Generator.Unicode(f))
                    .RuleFor(x => x.Single, f => f.Random.Float());

                return model.Generate();
            }
        }

        public class TestObject2 { }
        #endregion

        #region Helper Functions
        public static byte[] StringToBytes(string x,
            int? length = null,
            bool unicode = false,
            bool zeroTerminate = true,
            bool bigEndian = false)
        {
            using (DataBuffer wb = new DataBuffer() { BigEndian = bigEndian })
            {
                wb.Write(x, length, unicode, zeroTerminate);
                return wb.GetBuffer();
            }
        }

        public static string BytesToString(byte[] data,
            int length = 0,
            bool unicode = false,
            bool bigEndian = false)
        {
            using (DataBuffer wb = new DataBuffer(data) { BigEndian = bigEndian })
            {
                return wb.ReadString(length, unicode);
            }
        }

        public static byte[] ArrayToBytes<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false,
            bool bigEndian = false)
            where T : new()
        {
            using (DataBuffer wb = new DataBuffer() { BigEndian = bigEndian })
            {
                wb.Write(items, count, itemLength, unicode);
                return wb.GetBuffer();
            }
        }

        public static T[] BytesToArray<T>(byte[] data, int count,
            int itemLength = 0,
            bool unicode = false,
            bool bigEndian = false)
        {
            using (DataBuffer wb = new DataBuffer(data) { BigEndian = bigEndian })
            {
                return wb.ReadArray<T>(count, itemLength, unicode);
            }
        }
        #endregion
    }
}
