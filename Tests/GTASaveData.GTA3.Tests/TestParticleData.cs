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
        public void RandomDataSerialization(FileFormat format)
        {
            ParticleData x0 = GenerateTestObject(format);
            ParticleData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            ParticleData x0 = GenerateTestObject();
            ParticleData x1 = new ParticleData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
