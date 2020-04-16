using GTASaveData;
using GTASaveData.Types;

namespace TestFramework
{
    public abstract class SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public int GetSizeOfTestObject()
        {
            return SaveDataObject.SizeOf<T>();
        }

        public int GetSizeOfTestObject(T obj)
        {
            return SaveDataObject.SizeOf<T>(obj, DataFormat.Default);
        }

        public int GetSizeOfTestObject(DataFormat format)
        {
            return SaveDataObject.SizeOf<T>(format);
        }

        public int GetSizeOfTestObject(T obj, DataFormat format)
        {
            return SaveDataObject.SizeOf<T>(obj, format);
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

        public T GenerateTestObject()
        {
            return GenerateTestObject(DataFormat.Default);
        }

        public abstract T GenerateTestObject(DataFormat format);
    }
}
