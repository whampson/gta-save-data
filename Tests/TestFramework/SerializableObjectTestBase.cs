using GTASaveData.Serialization;

namespace TestFramework
{
    public abstract class SerializableObjectTestBase<T>
    {
        public T CreateSerializedCopy(T x, FileFormat format = null)
        {
            return CreateSerializedCopy(x, out _, format);
        }

        public T CreateSerializedCopy(T x, out byte[] bytes, FileFormat format = null)
        {
            bytes = Serializer.Serialize(x, format);
            T obj = Serializer.Deserialize<T>(bytes, format);

            return obj;
        }

        public T GenerateTestVector()
        {
            return GenerateTestVector(FileFormat.Unknown);
        }

        public abstract T GenerateTestVector(FileFormat format);
    }
}
