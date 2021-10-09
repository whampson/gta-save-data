using System.Linq;
using Bogus;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.JsonConverters
{
    public class TestIntArrayConverter : TestBase
    {
        [Fact]
        public void ConvertBelowThreshold()
        {
            Faker f = new Faker();
            TestClass x = new TestClass
            {
                Data = Generator.Array(f.Random.Int(0, IntArrayConverter.Threshold), g => f.Random.Int())
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
                Data = Generator.Array(f.Random.Int(IntArrayConverter.Threshold, 1000), g => f.Random.Int())
            };

            string json = JsonConvert.SerializeObject(x);
            TestClass y = JsonConvert.DeserializeObject<TestClass>(json);

            Assert.Equal(x.Data, y.Data);
        }

        public class TestClass
        {
            [JsonConverter(typeof(IntArrayConverter))]
            public ObservableArray<int> Data;
        }
    }
}
