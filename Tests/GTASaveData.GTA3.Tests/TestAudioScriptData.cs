using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAudioScriptData : Base<AudioScriptData>
    {
        public override AudioScriptData GenerateTestObject(FileFormat format)
        {
            Faker<AudioScriptData> model = new Faker<AudioScriptData>()
                .RuleFor(x => x.AudioScriptObjects,
                    f => Generator.Array(f.Random.Int(1, 50), g => Generator.Generate<AudioScriptObject, TestAudioScriptObject>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            AudioScriptData x0 = GenerateTestObject(format);
            AudioScriptData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            AudioScriptData x0 = GenerateTestObject();
            AudioScriptData x1 = new AudioScriptData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
