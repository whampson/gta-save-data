using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAudioScriptObject : Base<AudioScriptObject>
    {
        public override AudioScriptObject GenerateTestObject(FileType format)
        {
            Faker<AudioScriptObject> model = new Faker<AudioScriptObject>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.AudioId, f => f.Random.Short())
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.AudioEntity, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            AudioScriptObject x0 = GenerateTestObject(format);
            AudioScriptObject x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.AudioId, x1.AudioId);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.AudioEntity, x1.AudioEntity);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            AudioScriptObject x0 = GenerateTestObject();
            AudioScriptObject x1 = new AudioScriptObject(x0);

            Assert.Equal(x0, x1);
        }
    }
}
