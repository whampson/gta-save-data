using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestParticleData : Base<ParticleData>
    {
        public override ParticleData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<ParticleData> model = new Faker<ParticleData>()
                .RuleFor(x => x.Objects,
                    f => Generator.Array(f.Random.Int(1, 50), g => Generator.Generate<ParticleObject, TestParticleObject, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ParticleData x0 = GenerateTestObject(p);
            ParticleData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Objects, x1.Objects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ParticleData x0 = GenerateTestObject(p);
            ParticleData x1 = new ParticleData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
