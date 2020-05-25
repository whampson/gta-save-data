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
            return Serializer.SizeOf(obj, SaveDataFormat.Default);
        }

        public int GetSizeOfTestObject(SaveDataFormat format)
        {
            return Serializer.SizeOf<T>(format);
        }

        public int GetSizeOfTestObject(T obj, SaveDataFormat format)
        {
            return Serializer.SizeOf(obj, format);
        }

        public T CreateSerializedCopy(T obj)
        {
            return CreateSerializedCopy(obj, SaveDataFormat.Default, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, SaveDataFormat.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, SaveDataFormat format)
        {
            return CreateSerializedCopy(obj, format, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, SaveDataFormat format, out byte[] bytes)
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
            return GenerateTestObject(SaveDataFormat.Default);
        }

        public abstract T GenerateTestObject(SaveDataFormat format);
    }
}
