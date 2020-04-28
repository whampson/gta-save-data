using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAudioScriptData : Base<AudioScriptData>
    {
        public override AudioScriptData GenerateTestObject(DataFormat format)
        {
            Faker<AudioScriptData> model = new Faker<AudioScriptData>()
                .RuleFor(x => x.AudioScriptObjects,
                    f => Generator.Array(f.Random.Int(1, 50), g => Generator.Generate<AudioScriptObject, TestAudioScriptObject>()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            AudioScriptData x0 = GenerateTestObject();
            AudioScriptData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
