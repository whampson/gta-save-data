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
            return Serializer.SizeOf(obj, FileFormat.Default);
        }

        public int GetSizeOfTestObject(FileFormat format)
        {
            return Serializer.SizeOf<T>(format);
        }

        public int GetSizeOfTestObject(T obj, FileFormat format)
        {
            return Serializer.SizeOf(obj, format);
        }

        public T CreateSerializedCopy(T obj)
        {
            return CreateSerializedCopy(obj, FileFormat.Default, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, FileFormat.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, FileFormat format)
        {
            return CreateSerializedCopy(obj, format, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, FileFormat format, out byte[] bytes)
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
            return GenerateTestObject(FileFormat.Default);
        }

        public abstract T GenerateTestObject(FileFormat format);
    }
}
