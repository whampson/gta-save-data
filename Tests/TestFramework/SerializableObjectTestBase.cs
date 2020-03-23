using GTASaveData;
using GTASaveData.Types;

namespace TestFramework
{
    public abstract class SerializableObjectTestBase<T>
    {
        public T CreateSerializedCopy(T obj)
        {
            return CreateSerializedCopy(obj, SaveFileFormat.Default, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, SaveFileFormat.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, SaveFileFormat fmt)
        {
            return CreateSerializedCopy(obj, fmt, out byte[] _);
        }

        public T CreateSerializedCopy(T obj, SaveFileFormat fmt, out byte[] bytes)
        {
            bytes = Serializer.Write(obj, fmt);
            return Serializer.Read<T>(bytes, fmt);
        }

        public T GenerateTestObject()
        {
            return GenerateTestObject(SaveFileFormat.Default);
        }

        public abstract T GenerateTestObject(SaveFileFormat format);

        public static int SizeOf<TSaveDataObject>()
            where TSaveDataObject : SaveDataObject, new()
        {
            return SaveDataObject.SizeOf<TSaveDataObject>();
        }
    }
}
