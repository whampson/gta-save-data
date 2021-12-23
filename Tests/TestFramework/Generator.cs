using Bogus;
using GTASaveData;
using GTASaveData.Types;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace TestFramework
{
    public static class Generator
    {
        public static T[] Array<T>(int count, Func<int, T> itemGenerator)
        {
            return Enumerable.Range(0, count).Select(itemGenerator).ToArray();
        }

        public static Vector2 Vector2(Faker f)
        {
            return new Vector2(f.Random.Float(), f.Random.Float());
        }

        public static Vector3 Vector3(Faker f)
        {
            return new Vector3(f.Random.Float(), f.Random.Float(), f.Random.Float());
        }

        public static Quaternion Quaternion(Faker f)
        {
            return new Quaternion(f.Random.Float(), f.Random.Float(), f.Random.Float(), f.Random.Float());
        }

        public static Matrix Matrix(Faker f)
        {
            return new Matrix(Vector3(f), Vector3(f), Vector3(f), Vector3(f));
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

        public static char Ascii(Faker f)
        {
            return f.Random.Char('\u0020', '\u007E');
        }

        public static char Unicode(Faker f)
        {
            return f.Random.Char('\u0000', '\uD7FF');
        }

        public static string AsciiString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0020', '\u007E', length));
        }

        public static string UnicodeString(Faker f, int length)
        {
            return new string(f.Random.Chars('\u0000', '\uD7FF', length));
        }

        public static T Generate<T, U>()
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            return Generate<T, U>(FileType.Default);
        }

        public static T Generate<T, U>(FileType format)
            where T : SaveDataObject, new()
            where U : SaveDataObjectTestBase<T>
        {
            var testGen = Activator.CreateInstance(typeof(U));
            MethodInfo m = typeof(U).GetMethod(
                nameof(SaveDataObjectTestBase<T>.GenerateTestObject),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(FileType) },
                null);
            
            return (T) m.Invoke(testGen, new object[] { format });
        }
    }
}
