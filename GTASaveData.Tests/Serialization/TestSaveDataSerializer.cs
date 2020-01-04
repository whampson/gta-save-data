using Bogus;
using GTASaveData.Serialization;
using System.IO;
using Xunit;

namespace GTASaveData.Tests.Serialization
{
    public class TestSaveDataSerializer
    {
        [Fact]
        public void AsciiString()
        {
            string s0 = new Faker().Random.Words();
            byte[] data = StringToBytes(s0);
            string s1 = BytesToString(data);

            Assert.Equal(s0.Length, s1.Length);
            Assert.Equal(s0.Length + 1, data.Length);
            Assert.Equal(s0, s1);
        }

        [Fact]
        public void AsciiStringFixed()
        {
            Faker f = new Faker();

            int len = f.Random.Int(4, 24);
            string s0 = f.Random.Words();
            byte[] data = StringToBytes(s0, length: len);
            string s1 = BytesToString(data, length: len);

            Assert.True(data.Length > s1.Length);
            Assert.Equal(len, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }

        [Fact]
        public void AsciiStringFixedNoZero()
        {
            Faker f = new Faker();

            int len = f.Random.Int(4, 24);
            string s0 = f.Random.Words();
            byte[] data = StringToBytes(s0, length: len, zeroTerminate: false);
            string s1 = BytesToString(data, length: len);

            Assert.True(data.Length >= s1.Length);
            Assert.Equal(len, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }

        [Fact]
        public void UnicodeString()
        {
            string s0 = new Faker().Random.Words();
            byte[] data = StringToBytes(s0, unicode: true);
            string s1 = BytesToString(data, unicode: true);

            Assert.Equal(s0.Length, s1.Length);
            Assert.Equal((s0.Length + 1) * 2, data.Length);
            Assert.Equal(s0, s1);
        }

        [Fact]
        public void UnicodeStringFixed()
        {
            Faker f = new Faker();

            int len = f.Random.Int(4, 24);
            string s0 = f.Random.Words();
            byte[] data = StringToBytes(s0, length: len, unicode: true);
            string s1 = BytesToString(data, length: len, unicode: true);

            Assert.True(data.Length > s1.Length * 2);
            Assert.Equal(len * 2, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }

        [Fact]
        public void UnicodeStringFixedNoZero()
        {
            Faker f = new Faker();

            int len = f.Random.Int(4, 24);
            string s0 = f.Random.Words();
            byte[] data = StringToBytes(s0, length: len, unicode: true, zeroTerminate: false);
            string s1 = BytesToString(data, length: len, unicode: true);

            Assert.True(data.Length >= s1.Length * 2);
            Assert.Equal(len * 2, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }

        public static byte[] StringToBytes(string x,
            int? length = null,
            bool unicode = false,
            bool zeroTerminate = true)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = new SaveDataSerializer(m))
                {
                    s.Write(x, length, unicode, zeroTerminate);
                }

                return m.ToArray();
            }
        }

        public static string BytesToString(byte[] data, int length = 0, bool unicode = false)
        {
            using (SaveDataSerializer s = new SaveDataSerializer(new MemoryStream(data)))
            {
                return s.ReadString(length, unicode);
            }
        }
    }
}
