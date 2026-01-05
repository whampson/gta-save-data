using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAudioScriptData : Base<AudioScriptData>
    {
        public override AudioScriptData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<AudioScriptData> model = new Faker<AudioScriptData>()
                .RuleFor(x => x.AudioScriptObjects,
                    f => Generator.Array(f.Random.Int(1, 50),
                    g => Generator.Generate<AudioScriptObject, TestAudioScriptObject, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            AudioScriptData x0 = GenerateTestObject(p);
            AudioScriptData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            AudioScriptData x0 = GenerateTestObject(p);
            AudioScriptData x1 = new AudioScriptData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
