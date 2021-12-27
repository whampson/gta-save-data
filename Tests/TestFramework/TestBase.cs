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

    public abstract class TestBase<T, P>
        where T : new()
        where P : SerializationParams, new()
    {
        public virtual int GetSizeOfTestObject(T obj)
        {
            return Serializer.SizeOf(obj, new P());
        }

        public virtual int GetSizeOfTestObject(T obj, P p)
        {
            return Serializer.SizeOf(obj, p);
        }

        public T CreateSerializedCopy(T obj, out byte[] bytes)
        {
            return CreateSerializedCopy(obj, new P(), out bytes);
        }

        public T CreateSerializedCopy(T obj, P p, out byte[] bytes)
        {
            bytes = Serializer.Write(obj, p);
            return Serializer.Read<T>(bytes, p);
        }
    }

    public abstract class SaveDataObjectTestBase<T, P> : TestBase<T, P>
        where T : SaveDataObject, new()
        where P : SerializationParams, new()
    {
        public T GenerateTestObject()
        {
            return GenerateTestObject(new P());
        }

        public abstract T GenerateTestObject(P p);
    }
}
