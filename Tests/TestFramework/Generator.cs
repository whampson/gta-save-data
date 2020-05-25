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
        public static T[] Array<T>(int count, Func<int, T> itemGenerator)
        {
            return Enumerable.Range(0, count).Select(itemGenerator).ToArray();
        }

        public static Vector2D Vector2D(Faker f)
        {
            return new Vector2D(f.Random.Float(), f.Random.Float());
        }

        public static Vector3D Vector3D(Faker f)
        {
            return new Vector3D(f.Random.Float(), f.Random.Float(), f.Random.Float());
        }

        public static DateTime Date(Faker f)
        {
            return f.Date.Between(new DateTime(1970, 1, 1), DateTime.Now);
        }

        public static string Words(Faker f, int maxLength)
        {
            string s = f.Random.Words();
            if (s.Length > maxLength)
            {
                s = s.Substring(0, maxLength);
            }

            return s;
        }

        public static string AsciiString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0020', '\u007E', length));
        }

        public static string UnicodeString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0000', '\uD7FF', length));  // exclude surrogates
        }

        public static T Generate<T, U>()
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            return Generate<T, U>(SaveDataFormat.Default);
        }

        public static T Generate<T, U>(SaveDataFormat format)
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                nameof(SaveDataObjectTestBase<T>.GenerateTestObject),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(SaveDataFormat) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { format });
        }
    }
}
