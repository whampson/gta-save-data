using GTASaveData;

namespace UnitTest
{
    public abstract class SaveDataObjectTestBase<T>
    {
        public T GenerateTestVector()
        {
            return GenerateTestVector(SystemType.Unspecified);
        }

        public abstract T GenerateTestVector(SystemType system);
    }
}
