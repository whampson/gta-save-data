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
            return SaveDataObject.SizeOf<T>(obj, SaveFileFormat.Default);
        }

        public int GetSizeOfTestObject(SaveFileFormat format)
        {
            return SaveDataObject.SizeOf<T>(format);
        }

        public int GetSizeOfTestObject(T obj, SaveFileFormat format)
        {
            return SaveDataObject.SizeOf<T>(obj, format);
        }

        public T CreateSerializedCopy(T obj)
        {
            return CreateSerializedCopy(obj, SaveFileFormat.Default, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, SaveFileFormat.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, SaveFileFormat format)
        {
            return CreateSerializedCopy(obj, format, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, SaveFileFormat format, out byte[] bytes)
        {
            bytes = Serializer.Write(obj, format);
            return Serializer.Read<T>(bytes, format);
        }

        public T GenerateTestObject()
        {
            return GenerateTestObject(SaveFileFormat.Default);
        }

        public abstract T GenerateTestObject(SaveFileFormat format);
    }
}
