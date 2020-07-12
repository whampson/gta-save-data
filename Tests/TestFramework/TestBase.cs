using GTASaveData;
using System;

namespace TestFramework
{
    public abstract class TestBase : IDisposable
    {
        public TestBase()
        { }

        public virtual void Dispose()
        { }
    }

    public abstract class TestBase<T> where T : new()
    {
        public int GetSizeOfTestObject(T obj)
        {
            return Serializer.SizeOfObject(obj, FileFormat.Default);
        }

        public int GetSizeOfTestObject(T obj, FileFormat format)
        {
            return Serializer.SizeOfObject(obj, format);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, FileFormat.Default, out bytes);
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
