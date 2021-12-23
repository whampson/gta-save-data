using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestStreaming : Base<Streaming>
    {
        public override Streaming GenerateTestObject(FileType format)
        {
            Faker<Streaming> model = new Faker<Streaming>()
                .RuleFor(x => x.ModelFlags, f => Generator.Array(Streaming.NumModels, g => f.PickRandom<StreamingFlags>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            Streaming x0 = GenerateTestObject(format);
            Streaming x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.ModelFlags, x1.ModelFlags);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            Streaming x0 = GenerateTestObject();
            Streaming x1 = new Streaming(x0);

            Assert.Equal(x0, x1);
        }
    }
}
