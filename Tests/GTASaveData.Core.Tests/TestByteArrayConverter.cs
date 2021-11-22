using Bogus;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestByteArrayConverter : TestBase
    {
        [Fact]
        public void ConvertBelowThreshold()
        {
            Faker f = new Faker();
            TestClass x = new TestClass
            {
                Data = f.Random.Bytes(f.Random.Int(0, ByteArrayConverter.DefaultThreshold))
            };

            string json = JsonConvert.SerializeObject(x);
            TestClass y = JsonConvert.DeserializeObject<TestClass>(json);

            Assert.Equal(x.Data, y.Data);
        }

        [Fact]
        public void ConvertAboveThreshold()
        {
            Faker f = new Faker();
            TestClass x = new TestClass
            {
                Data = f.Random.Bytes(f.Random.Int(ByteArrayConverter.DefaultThreshold, 1000))
            };

            string json = JsonConvert.SerializeObject(x);
            TestClass y = JsonConvert.DeserializeObject<TestClass>(json);

            Assert.Equal(x.Data, y.Data);
        }
    }

    public class TestClass
    {
        [JsonConverter(typeof(ByteArrayConverter))]
        public ObservableArray<byte> Data;
    }
}
