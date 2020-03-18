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
            where T : GTAObject
            where U : SerializableObjectTestBase<T>
        {
            return Generate<T, U>(SaveFileFormat.Default);
        }

        public static T Generate<T, U>(SaveFileFormat format)
            where T : GTAObject
            where U : SerializableObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                nameof(SerializableObjectTestBase<T>.GenerateTestVector),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(SaveFileFormat) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { format });
        }
    }
}
