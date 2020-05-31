using Bogus;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSaveDataObject : SaveDataObjectTestBase<TestObject>
    {
        public override TestObject GenerateTestObject(FileFormat format)
        {
            Faker<TestObject2> model2 = new Faker<TestObject2>()
                .RuleFor(x => x.Value, f => f.Random.Int());

            Faker<TestObject> model = new Faker<TestObject>()
                .RuleFor(x => x.Value, f => f.Random.Int())
                .RuleFor(x => x.Object, model2.Generate())
                .RuleFor(x => x.ValueArray, f => Generator.Array(TestObject.ValueArrayCount, g => f.Random.Int()))
                .RuleFor(x => x.ObjectArray, f => Generator.Array(TestObject.ObjectArrayCount, g => model2.Generate()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            TestObject x0 = GenerateTestObject();
            TestObject x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Value, x1.Value);
            Assert.Equal(x0.Object, x1.Object);
            Assert.Equal(x0.ValueArray, x1.ValueArray);
            Assert.Equal(x0.ObjectArray, x1.ObjectArray);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }

        [Fact]
        public void JsonSerialization()
        {
            TestObject x0 = GenerateTestObject();
            string json = x0.ToJsonString();
            TestObject x1 = SaveDataObject.FromJsonString<TestObject>(json);

            Assert.Equal(x0.Value, x1.Value);
            Assert.Equal(x0.Object, x1.Object);
            Assert.Equal(x0.ValueArray, x1.ValueArray);
            Assert.Equal(x0.ObjectArray, x1.ObjectArray);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void NullObject()
        {
            TestObject x0 = GenerateTestObject();
            x0.Object = null;
            
            Assert.Throws<ArgumentNullException>(() => CreateSerializedCopy(x0, out byte[] _));
        }

        [Fact]
        public void NullArray()
        {
            TestObject x0 = GenerateTestObject();
            x0.ObjectArray = null;

            Assert.Throws<ArgumentNullException>(() => CreateSerializedCopy(x0, out byte[] _));
        }
    }

    public class TestObject : SaveDataObject, IEquatable<TestObject>
    {
        public const int ValueArrayCount = 5;
        public const int ObjectArrayCount = 5;

        private int m_value;
        private TestObject2 m_object;
        private Array<int> m_valueArray;
        private Array<TestObject2> m_objectArray;

        public int Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public TestObject2 Object
        {
            get { return m_object; }
            set { m_object = value; OnPropertyChanged(); }
        }

        public Array<int> ValueArray
        {
            get { return m_valueArray; }
            set { m_valueArray = value; OnPropertyChanged(); }
        }

        public Array<TestObject2> ObjectArray
        {
            get { return m_objectArray; }
            set { m_objectArray = value; OnPropertyChanged(); }
        }

        public TestObject()
        {
            m_value = 0;
            m_object = new TestObject2();
            m_valueArray = new Array<int>();
            m_objectArray = new Array<TestObject2>();
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Value = buf.ReadInt32();
            Object = buf.Read<TestObject2>();
            ValueArray = buf.Read<int>(ValueArrayCount);
            ObjectArray = buf.Read<TestObject2>(ObjectArrayCount);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Value);
            buf.Write(Object);
            buf.Write(ValueArray, ValueArrayCount);
            buf.Write(ObjectArray, ObjectArrayCount);

        }

        protected override int GetSize(FileFormat fmt)
        {
            return sizeof(int)
                + sizeof(int) * ValueArrayCount
                + SizeOfType<TestObject2>()
                + SizeOfType<TestObject2>() * ObjectArrayCount;
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

            return Value.Equals(other.Value)
                && Object.Equals(other.Object)
                && ValueArray.SequenceEqual(other.ValueArray)
                && ObjectArray.SequenceEqual(other.ObjectArray);
        }
    }

    public class TestObject2 : SaveDataObject, IEquatable<TestObject2>
    {
        private int m_value;

        public int Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public TestObject2()
        { }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Value = buf.ReadInt32();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Value);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 4;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TestObject2);
        }

        public bool Equals(TestObject2 other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }
    }
}
