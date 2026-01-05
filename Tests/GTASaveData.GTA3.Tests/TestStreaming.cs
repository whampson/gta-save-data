using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestStreaming : Base<Streaming>
    {
        public override Streaming GenerateTestObject(GTA3SaveParams p)
        {
            Faker<Streaming> model = new Faker<Streaming>()
                .RuleFor(x => x.ModelFlags, f => Generator.Array(Streaming.NumModels, g => f.PickRandom<StreamingFlags>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Streaming x0 = GenerateTestObject(p);
            Streaming x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.ModelFlags, x1.ModelFlags);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Streaming x0 = GenerateTestObject(p);
            Streaming x1 = new Streaming(x0);

            Assert.Equal(x0, x1);
        }
    }
}
