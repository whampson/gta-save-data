using Bogus;
using GTASaveData;
using GTASaveData.Types;
using System;
using System.Linq;
using System.Reflection;

namespace TestFramework
{
    public static class Generator
    {
        public static Array<T> CreateArray<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(x => new T()).ToArray();
        }

        public static Array<T> CreateArray<T>(int count, Func<int, T> itemGenerator)
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
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            return Generate<T, U>(DataFormat.Default);
        }

        public static T Generate<T, U>(DataFormat format)
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                nameof(SaveDataObjectTestBase<T>.GenerateTestObject),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(DataFormat) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { format });
        }
    }
}
