using Bogus;
using GTASaveData.Serialization;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WpfEssentials;

namespace GTASaveData.Tests.TestFramework
{
    public static class Generator
    {
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

        public static T[] CreateArray<T>(int count, Func<int, T> itemGenerator)
            where T : new()
        {
            return Enumerable.Range(0, count).Select(itemGenerator).ToArray();
        }

        public static string RandomWords(Faker f, int maxLength)
        {
            string s = f.Random.Words();
            if (s.Length > maxLength)
            {
                s = s.Substring(0, maxLength);
            }

            return s;
        }

        public static string RandomAsciiString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0020', '\u007E', length));
        }

        public static string RandomUnicodeString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0000', '\uD7FF', length));  // exclude surrogates
        }

        public static T Generate<T, U>()
            where T : SaveDataObject
            where U : SaveDataObjectTestBase<T>
        {
            return Generate<T, U>(FileFormat.Unknown);
        }

        public static T Generate<T, U>(FileFormat format)
            where T : SaveDataObject
            where U : SaveDataObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                nameof(SaveDataObjectTestBase<T>.GenerateTestVector),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(FileFormat) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { format ?? FileFormat.Unknown });
        }
    }
}
