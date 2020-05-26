using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestStreaming : Base<Streaming>
    {
        public override Streaming GenerateTestObject(FileFormat format)
        {
            Faker<Streaming> model = new Faker<Streaming>()
                .RuleFor(x => x.ModelFlags, f => Generator.Array(Streaming.Limits.NumberOfModels, g => f.PickRandom<StreamingFlags>()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Streaming x0 = GenerateTestObject();
            Streaming x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.ModelFlags, x1.ModelFlags);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
