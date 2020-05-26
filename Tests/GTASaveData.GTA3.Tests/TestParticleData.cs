using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestParticleData : Base<ParticleData>
    {
        public override ParticleData GenerateTestObject(FileFormat format)
        {
            Faker<ParticleData> model = new Faker<ParticleData>()
                .RuleFor(x => x.ParticleObjects,
                    f => Generator.Array(f.Random.Int(1, 50), g => Generator.Generate<ParticleObject, TestParticleObject>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat fmt)
        {
            ParticleData x0 = GenerateTestObject(fmt);
            ParticleData x1 = CreateSerializedCopy(x0, fmt, out byte[] data);

            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, fmt), data.Length);
        }
    }
}
