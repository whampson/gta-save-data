using GTASaveData;

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

        public T GenerateTestVector()
        {
            return GenerateTestVector(SaveFileFormat.Default);
        }

        public abstract T GenerateTestVector(SaveFileFormat format);
    }
}
