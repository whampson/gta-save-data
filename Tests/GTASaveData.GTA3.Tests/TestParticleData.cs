using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestParticleData : Base<ParticleData>
    {
        public override ParticleData GenerateTestObject(DataFormat format)
        {
            Faker<ParticleData> model = new Faker<ParticleData>()
                .RuleFor(x => x.ParticleObjects,
                    f => Generator.CreateArray(f.Random.Int(1, 50), g => Generator.Generate<ParticleObject, TestParticleObject>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            ParticleData x0 = GenerateTestObject();
            ParticleData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
