using GTASaveData;

namespace TestFramework
{
    public abstract class TestBase<T> where T : new()
    {
        public int GetSizeOfTestObject()
        {
            return Serializer.SizeOf<T>();
        }

        public int GetSizeOfTestObject(T obj)
        {
            return Serializer.SizeOf(obj, DataFormat.Default);
        }

        public int GetSizeOfTestObject(DataFormat format)
        {
            return Serializer.SizeOf<T>(format);
        }

        public int GetSizeOfTestObject(T obj, DataFormat format)
        {
            return Serializer.SizeOf(obj, format);
        }

        public T CreateSerializedCopy(T obj)
        {
            return CreateSerializedCopy(obj, DataFormat.Default, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, DataFormat.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, DataFormat format)
        {
            return CreateSerializedCopy(obj, format, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, DataFormat format, out byte[] bytes)
        {
            bytes = Serializer.Write(obj, format);
            return Serializer.Read<T>(bytes, format);
        }
    }

    public abstract class SaveDataObjectTestBase<T> : TestBase<T>
        where T : SaveDataObject, new()
    {
        public T GenerateTestObject()
        {
            return GenerateTestObject(DataFormat.Default);
        }

        public abstract T GenerateTestObject(DataFormat format);
    }
}
