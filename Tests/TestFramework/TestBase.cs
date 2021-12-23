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
        public virtual int GetSizeOfTestObject(T obj)
        {
            return Serializer.SizeOf(obj, FileType.Default);
        }

        public virtual int GetSizeOfTestObject(T obj, FileType format)
        {
            return Serializer.SizeOf(obj, format);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, FileType.Default, out bytes);
        }

        public T CreateSerializedCopy(T obj, FileType format, out byte[] bytes)
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
            return GenerateTestObject(FileType.Default);
        }

        public abstract T GenerateTestObject(FileType format);
    }
}
