using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestAudioScriptObject : Base<AudioScriptObject>
    {
        public override AudioScriptObject GenerateTestObject(DataFormat format)
        {
            Faker<AudioScriptObject> model = new Faker<AudioScriptObject>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.AudioId, f => f.Random.Short())
                .RuleFor(x => x.Position, Generator.Generate<Vector3D, TestVector3D>())
                .RuleFor(x => x.AudioEntity, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            AudioScriptObject x0 = GenerateTestObject();
            AudioScriptObject x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.AudioId, x1.AudioId);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.AudioEntity, x1.AudioEntity);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
