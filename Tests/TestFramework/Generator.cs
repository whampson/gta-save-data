using Bogus;
using GTASaveData;
using GTASaveData.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace TestFramework
{
    public static class Generator
    {
        //public static ObservableCollection<T> CreateValueCollection<T>(int count, Func<int, T> itemGenerator)
        //    where T : new()
        //{
        //    return new ObservableCollection<T>(Enumerable.Range(0, count).Select(itemGenerator));
        //}

        //public static FullyObservableCollection<T> CreateObjectCollection<T>(int count, Func<int, T> itemGenerator)
        //    where T : INotifyPropertyChanged, new()
        //{
        //    return new FullyObservableCollection<T>(Enumerable.Range(0, count).Select(itemGenerator));
        //}

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

        public static TChunk Generate<TChunk, TChunkTest>()
            where TChunk : Chunk
            where TChunkTest : ChunkTestBase<TChunk>
        {
            return Generate<TChunk, TChunkTest>(FileFormat.Unknown);
        }

        public static TChunk Generate<TChunk, TChunkTest>(FileFormat format)
            where TChunk : Chunk
            where TChunkTest : ChunkTestBase<TChunk>
        {
            var testGen = Activator.CreateInstance(typeof(TChunkTest));
            MethodInfo m = typeof(TChunkTest).GetMethod(
                nameof(ChunkTestBase<TChunk>.GenerateTestVector),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(FileFormat) },
                null);
            
            return (TChunk) m.Invoke(testGen, new object[] { format ?? FileFormat.Unknown });
        }
    }
}
