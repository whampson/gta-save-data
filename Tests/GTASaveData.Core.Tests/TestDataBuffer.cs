using Bogus;
using GTASaveData.Types;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestDataBuffer
    {
        [Fact]
        public void Alignment()
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
                wb.Align4Bytes();
                wb.Write(i0);
                wb.Write(f0);
                data = wb.GetBytes();
            }

            using (DataBuffer wb = new DataBuffer(data))
            {
                b1 = wb.ReadBool();
                wb.Align4Bytes();
                i1 = wb.ReadInt32();
                f1 = wb.ReadSingle();
            }

            Assert.Equal(b0, b1);
            Assert.Equal(i0, i1);
            Assert.Equal(f0, f1);
            Assert.Equal(12, data.Length);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void Bool(bool value, bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;

            bool x0 = value;
            byte[] data = Serializer.Write(x0);
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
            Serializer.BigEndian = bigEndian;

            Faker f = new Faker();
            int numBytes = f.Random.Int(2, 8);

            bool x0 = value;
            byte[] data;
            bool x1;

            using (DataBuffer wb = new DataBuffer())
            {
                wb.Write(x0, numBytes);
                data = wb.GetBytes();
            }

            using (DataBuffer wb = new DataBuffer(data))
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
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            char x0 = f.Random.Char('\u0000', '\u00FF');
            byte[] data = Serializer.Write(x0);
            char x1 = Serializer.Read<char>(data);

            Assert.Equal(x0, x1);
            Assert.Single(data);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CharUnicode(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            char x0 = f.Random.Char('\u0000', '\uD7FF');
            byte[] data;
            char x1;

            using (DataBuffer wb = new DataBuffer())
            {
                wb.Write(x0, true);
                data = wb.GetBytes();
            }

            using (DataBuffer wb = new DataBuffer(data))
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
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            double x0 = f.Random.Double();
            byte[] data = Serializer.Write(x0);
            double x1 = Serializer.Read<double>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Float(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            float x0 = f.Random.Float();
            byte[] data = Serializer.Write(x0);
            float x1 = Serializer.Read<float>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int16(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            short x0 = f.Random.Short();
            byte[] data = Serializer.Write(x0);
            short x1 = Serializer.Read<short>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(2, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt16(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            ushort x0 = f.Random.UShort();
            byte[] data = Serializer.Write(x0);
            ushort x1 = Serializer.Read<ushort>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(2, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int32(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            int x0 = f.Random.Int();
            byte[] data = Serializer.Write(x0);
            int x1 = Serializer.Read<int>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt32(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            uint x0 = f.Random.UInt();
            byte[] data = Serializer.Write(x0);
            uint x1 = Serializer.Read<uint>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(4, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Int64(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            long x0 = f.Random.Long();
            byte[] data = Serializer.Write(x0);
            long x1 = Serializer.Read<long>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void UInt64(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            Faker f = new Faker();

            ulong x0 = f.Random.ULong();
            byte[] data = Serializer.Write(x0);
            ulong x1 = Serializer.Read<ulong>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Object(bool bigEndian)
        {
            Serializer.BigEndian = bigEndian;
            TestObject x0 = TestObject.Generate();
            byte[] data = Serializer.Write(x0);
            TestObject x1 = Serializer.Read<TestObject>(data);

            Assert.Equal(x0, x1);
            Assert.Equal(SaveDataObject.SizeOf<TestObject>(), data.Length);
        }

        [Fact]
        public void NonSerializableObject()
        {
            TestObject2 x = new TestObject2();

            Assert.Throws<SerializationException>(() => Serializer.Write(x));
        }

        [Fact]
        public void AsciiString()
        {
            Faker f = new Faker();
            string s0 = Generator.RandomAsciiString(f, f.Random.Int(1, 100));
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
            string s0 = Generator.RandomAsciiString(f, initialLength);
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
            string s0 = Generator.RandomAsciiString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, zeroTerminate: false);
            string s1 = BytesToString(data, bufferLength);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Fact]
        public void UnicodeString()
        {
            Faker f = new Faker();
            string s0 = Generator.RandomUnicodeString(f, f.Random.Int(1, 100));
            byte[] data = StringToBytes(s0, unicode: true);
            string s1 = BytesToString(data, unicode: true);

            Assert.Equal(s0, s1);
            Assert.Equal(s0.Length + 1, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
        }

        [Theory]
        [InlineData(24, 23, 23)]        // Shorter than buffer length
        [InlineData(24, 24, 23)]        // Equal to buffer length
        [InlineData(24, 25, 23)]        // Larger than buffer length
        public void UnicodeStringFixedLength(int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.RandomUnicodeString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, true);
            string s1 = BytesToString(data, bufferLength, true);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Theory]
        [InlineData(24, 23, 23)]        // Shorter than buffer length
        [InlineData(24, 24, 24)]        // Equal to buffer length
        [InlineData(24, 25, 24)]        // Larger than buffer length
        public void UnicodeStringFixedLengthNoZero(int bufferLength, int initialLength, int expectedLength)
        {
            Faker f = new Faker();
            string s0 = Generator.RandomUnicodeString(f, initialLength);
            byte[] data = StringToBytes(s0, bufferLength, true, false);
            string s1 = BytesToString(data, bufferLength, true);

            Assert.Equal(s0.Length, initialLength);
            Assert.Equal(s0.Substring(0, expectedLength), s1);
            Assert.Equal(bufferLength, data.Length / 2);
            Assert.True((data.Length % 2) == 0);
            Assert.Equal(expectedLength, s1.Length);
        }

        [Fact]
        public void ValueArray()
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            int[] x0 = Generator.CreateArray(count, g => f.Random.Int());
            byte[] data = ArrayToBytes(x0);
            int[] x1 = BytesToArray<int>(data, count);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(4 * count, data.Length);
        }

        [Theory]
        [InlineData(8, 7, 8)]       // Shorter than buffer count
        [InlineData(8, 8, 8)]       // Equal to buffer count
        [InlineData(8, 9, 8)]       // Larger than buffer count
        public void ValueArrayFixedLength(int bufferCount, int initialCount, int expectedCount)
        {
            Faker f = new Faker();

            int[] x0 = Generator.CreateArray(initialCount, g => f.Random.Int());
            byte[] data = ArrayToBytes(x0, bufferCount);
            int[] x1 = BytesToArray<int>(data, bufferCount);

            Assert.Equal(initialCount, x0.Length);
            Assert.Equal(expectedCount, x1.Length);
            Assert.Equal(x0.Take(Math.Min(bufferCount, initialCount)), x1.Take(Math.Min(bufferCount, initialCount)));
            Assert.Equal(4 * bufferCount, data.Length);
        }

        [Fact]
        public void ObjectArray()
        {
            Faker f = new Faker();
            int count = f.Random.Int(1, 10);

            TestObject[] x0 = Generator.CreateArray(count, g => TestObject.Generate());
            byte[] data = ArrayToBytes(x0);
            TestObject[] x1 = BytesToArray<TestObject>(data, count);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(SaveDataObject.SizeOf<TestObject>() * count, data.Length);
        }

        [Theory]
        [InlineData(8, 7, 8)]       // Shorter than buffer count
        [InlineData(8, 8, 8)]       // Equal to buffer count
        [InlineData(8, 9, 8)]       // Larger than buffer count
        public void ObjectArrayFixedLength(int bufferCount, int initialCount, int expectedCount)
        {
            Faker f = new Faker();

            TestObject[] x0 = Generator.CreateArray(initialCount, g => TestObject.Generate());
            byte[] data = ArrayToBytes(x0, bufferCount);
            TestObject[] x1 = BytesToArray<TestObject>(data, bufferCount);

            Assert.Equal(initialCount, x0.Length);
            Assert.Equal(expectedCount, x1.Length);
            Assert.Equal(x0.Take(Math.Min(bufferCount, initialCount)), x1.Take(Math.Min(bufferCount, initialCount)));
            Assert.Equal(SaveDataObject.SizeOf<TestObject>() * bufferCount, data.Length);
        }

        [Fact]
        public void BoolArrayMultiByte()
        {
            Faker f = new Faker();
            int count = f.Random.Int(10, 100);
            int numBytes = f.Random.Int(1, 8);

            bool[] x0 = Generator.CreateArray(count, g => f.Random.Bool());
            byte[] data = ArrayToBytes(x0, itemLength: numBytes);
            bool[] x1 = BytesToArray<bool>(data, count, itemLength: numBytes);

            Assert.Equal(count, x1.Length);
            Assert.Equal(x0, x1);
            Assert.Equal(numBytes * count, data.Length);
        }

        #region Test Objects
        [Size(9)]
        public class TestObject : SaveDataObject, IEquatable<TestObject>
        {
            public int Integer { get; set; }
            public bool Boolean { get; set; }
            public float Single { get; set; }

            public TestObject()
            { }

            protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
            {
                Integer = buf.ReadInt32();
                Boolean = buf.ReadBool();
                Single = buf.ReadSingle();
            }

            protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
            {
                buf.Write(Integer);
                buf.Write(Boolean);
                buf.Write(Single);
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash *= Integer.GetHashCode();
                hash *= Boolean.GetHashCode();
                hash *= Single.GetHashCode();

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

                return Integer.Equals(other.Integer)
                    && Boolean.Equals(other.Boolean)
                    && Single.Equals(other.Single);
            }

            public static TestObject Generate()
            {
                Faker<TestObject> model = new Faker<TestObject>()
                    .RuleFor(x => x.Integer, f => f.Random.Int())
                    .RuleFor(x => x.Boolean, f => f.Random.Bool())
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
            bool zeroTerminate = true)
        {
            using (DataBuffer wb = new DataBuffer())
            {
                wb.Write(x, length, unicode, zeroTerminate);
                return wb.GetBytes();
            }
        }

        public static string BytesToString(byte[] data, int length = 0, bool unicode = false)
        {
            using (DataBuffer wb = new DataBuffer(data))
            {
                return wb.ReadString(length, unicode);
            }
        }

        public static byte[] ArrayToBytes<T>(T[] items,
            int? count = null,
            int itemLength = 0,
            bool unicode = false)
            where T : new()
        {
            using (DataBuffer wb = new DataBuffer())
            {
                wb.Write(items, count, itemLength, unicode);
                return wb.GetBytes();
            }
        }

        public static T[] BytesToArray<T>(byte[] data, int count,
            int itemLength = 0,
            bool unicode = false)
        {
            using (DataBuffer wb = new DataBuffer(data))
            {
                return wb.ReadArray<T>(count, itemLength, unicode);
            }
        }
        #endregion
    }
}
