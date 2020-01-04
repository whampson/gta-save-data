using GTASaveData.Serialization;

namespace GTASaveData.Tests.TestFramework
{
    public abstract class SaveDataObjectTestBase<T>
    {
        public T CreateSerializedCopy(T x, FileFormat format = null)
        {
            return CreateSerializedCopy(x, out _, format);
        }

        public T CreateSerializedCopy(T x, out byte[] bytes, FileFormat format = null)
        {
            bytes = SaveDataSerializer.Serialize(x, format);
            T obj = SaveDataSerializer.Deserialize<T>(bytes, format);

            return obj;
        }

        public T GenerateTestVector()
        {
            return GenerateTestVector(FileFormat.Unknown);
        }

        public abstract T GenerateTestVector(FileFormat format);
    }
}
