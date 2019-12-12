using Bogus;
using GTASaveData.Common;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
            bytes = Serializer.Serialize(x, system: system);
            T obj = Serializer.Deserialize<T>(bytes, system: system);

            return obj;
        }

        public static ObservableCollection<T> CreateCollection<T>(int count, Func<int, T> itemGenerator)
            where T : new()
        {
            return new ObservableCollection<T>(Enumerable.Range(0, count).Select(itemGenerator));
        }

        public static FullyObservableCollection<T> CreateStructCollection<T>(int count, Func<int, T> itemGenerator)
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
    }
}
