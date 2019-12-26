using Bogus;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Common
{
    public class TestGTASerializer
    {
        [Fact]
        public void AsciiString()
        {
            string s0 = new Faker().Random.Words();
            byte[] data = SerializationHelper.GetBytes(s0);
            string s1 = SerializationHelper.FromBytes(data);

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
            byte[] data = SerializationHelper.GetBytes(s0, length: len);
            string s1 = SerializationHelper.FromBytes(data, length: len);

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
            byte[] data = SerializationHelper.GetBytes(s0, length: len, zeroTerminate: false);
            string s1 = SerializationHelper.FromBytes(data, length: len);

            Assert.True(data.Length >= s1.Length);
            Assert.Equal(len, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }

        [Fact]
        public void UnicodeString()
        {
            string s0 = new Faker().Random.Words();
            byte[] data = SerializationHelper.GetBytes(s0, unicode: true);
            string s1 = SerializationHelper.FromBytes(data, unicode: true);

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
            byte[] data = SerializationHelper.GetBytes(s0, length: len, unicode: true);
            string s1 = SerializationHelper.FromBytes(data, length: len, unicode: true);

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
            byte[] data = SerializationHelper.GetBytes(s0, length: len, unicode: true, zeroTerminate: false);
            string s1 = SerializationHelper.FromBytes(data, length: len, unicode: true);

            Assert.True(data.Length >= s1.Length * 2);
            Assert.Equal(len * 2, data.Length);
            Assert.Equal(s0.Substring(0, s1.Length), s1);
        }
    }
}
