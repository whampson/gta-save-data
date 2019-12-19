using Bogus;
using GTASaveData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Tests.Helpers
{
    public static class TestHelper
    {
        public static T CreateSerializedCopy<T>(T x, SystemType system = SystemType.Unspecified)
        {
            return CreateSerializedCopy(x, out _, system);
        }

        public static T CreateSerializedCopy<T>(T x, out byte[] bytes, SystemType system = SystemType.Unspecified)
        {
            bytes = SaveDataSerializer.Serialize(x, system: system);
            T obj = SaveDataSerializer.Deserialize<T>(bytes, system: system);

            return obj;
        }

        public static ObservableCollection<T> CreateValueCollection<T>(int count, Func<int, T> itemGenerator)
            where T : new()
        {
            return new ObservableCollection<T>(Enumerable.Range(0, count).Select(itemGenerator));
        }

        public static FullyObservableCollection<T> CreateObjectCollection<T>(int count, Func<int, T> itemGenerator)
            where T : INotifyPropertyChanged, new()
        {
            return new FullyObservableCollection<T>(Enumerable.Range(0, count).Select(itemGenerator));
        }

        public static string RandomString(Faker f, int maxLength)
        {
            string s = f.Random.Words();
            if (s.Length > maxLength)
            {
                s = s.Substring(0, maxLength);
            }

            return s;
        }

        public static T Generate<T, U>()
            where T : SaveDataObject
            where U : SaveDataObjectTestBase<T>
        {
            return Generate<T, U>(SystemType.Unspecified);
        }

        public static T Generate<T, U>(SystemType system)
            where T : SaveDataObject
            where U : SaveDataObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                "GenerateTestVector",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(SystemType) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { system });
        }
    }
}
