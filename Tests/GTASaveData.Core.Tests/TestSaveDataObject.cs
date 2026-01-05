using Bogus;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSaveDataObject : SaveDataObjectTestBase<DummySaveDataObject, SerializationParams>
    {
        public override DummySaveDataObject GenerateTestObject(SerializationParams p)
        {
            Faker<DummySaveDataObject2> model2 = new Faker<DummySaveDataObject2>()
                .RuleFor(x => x.Value, f => f.Random.Int());

            Faker<DummySaveDataObject> model = new Faker<DummySaveDataObject>()
                .RuleFor(x => x.Value, f => f.Random.Int())
                .RuleFor(x => x.Object, model2.Generate())
                .RuleFor(x => x.ValueArray, f => Generator.Array(DummySaveDataObject.ValueArrayCount, g => f.Random.Int()))
                .RuleFor(x => x.ObjectArray, f => Generator.Array(DummySaveDataObject.ObjectArrayCount, g => model2.Generate()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            DummySaveDataObject x0 = GenerateTestObject();
            DummySaveDataObject x1 = CreateSerializedCopy(x0, out byte[] data);

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
            DummySaveDataObject x0 = GenerateTestObject();
            string json = x0.ToJsonString();
            DummySaveDataObject x1 = SaveDataObject.FromJsonString<DummySaveDataObject>(json);

            Assert.Equal(x0.Value, x1.Value);
            Assert.Equal(x0.Object, x1.Object);
            Assert.Equal(x0.ValueArray, x1.ValueArray);
            Assert.Equal(x0.ObjectArray, x1.ObjectArray);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void NullObject()
        {
            DummySaveDataObject x0 = GenerateTestObject();
            x0.Object = null;
            
            Assert.Throws<ArgumentNullException>(() => CreateSerializedCopy(x0, out byte[] _));
        }

        [Fact]
        public void NullArray()
        {
            DummySaveDataObject x0 = GenerateTestObject();
            x0.ObjectArray = null;

            Assert.Throws<ArgumentNullException>(() => CreateSerializedCopy(x0, out byte[] _));
        }
    }

    public class DummySaveDataObject : SaveDataObject, IEquatable<DummySaveDataObject>
    {
        public const int ValueArrayCount = 5;
        public const int ObjectArrayCount = 5;

        private int m_value;
        private DummySaveDataObject2 m_object;
        private ObservableArray<int> m_valueArray;
        private ObservableArray<DummySaveDataObject2> m_objectArray;

        public int Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public DummySaveDataObject2 Object
        {
            get { return m_object; }
            set { m_object = value; OnPropertyChanged(); }
        }

        public ObservableArray<int> ValueArray
        {
            get { return m_valueArray; }
            set { m_valueArray = value; OnPropertyChanged(); }
        }

        public ObservableArray<DummySaveDataObject2> ObjectArray
        {
            get { return m_objectArray; }
            set { m_objectArray = value; OnPropertyChanged(); }
        }

        public DummySaveDataObject()
        {
            m_value = 0;
            m_object = new DummySaveDataObject2();
            m_valueArray = new ObservableArray<int>();
            m_objectArray = new ObservableArray<DummySaveDataObject2>();
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            Value = buf.ReadInt32();
            Object = buf.ReadObject<DummySaveDataObject2>();
            ValueArray = buf.ReadArray<int>(ValueArrayCount);
            ObjectArray = buf.ReadArray<DummySaveDataObject2>(ObjectArrayCount);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            buf.Write(Value);
            buf.Write(Object);
            buf.Write(ValueArray, ValueArrayCount);
            buf.Write(ObjectArray, ObjectArrayCount);

        }

        protected override int GetSize(SerializationParams p)
        {
            return sizeof(int)
                + sizeof(int) * ValueArrayCount
                + SizeOf<DummySaveDataObject2>()
                + SizeOf<DummySaveDataObject2>() * ObjectArrayCount;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DummySaveDataObject);
        }

        public bool Equals(DummySaveDataObject other)
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

    public class DummySaveDataObject2 : SaveDataObject, IEquatable<DummySaveDataObject2>
    {
        private int m_value;

        public int Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public DummySaveDataObject2()
        { }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            Value = buf.ReadInt32();
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            buf.Write(Value);
        }

        protected override int GetSize(SerializationParams p)
        {
            return 4;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DummySaveDataObject2);
        }

        public bool Equals(DummySaveDataObject2 other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }
    }
}
