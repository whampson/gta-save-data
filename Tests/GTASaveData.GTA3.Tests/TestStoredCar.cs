using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestStoredCar : Base<StoredCar>
    {
        public override StoredCar GenerateTestObject(GTA3SaveParams p)
        {
            Faker<StoredCar> model = new Faker<StoredCar>()
                .RuleFor(x => x.Model, f => f.Random.Int())
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Heading, f => f.Random.Float())
                .RuleFor(x => x.Flags, f => f.PickRandom<StoredCarFlags>())
                .RuleFor(x => x.Color1, f => f.Random.Byte())
                .RuleFor(x => x.Color2, f => f.Random.Byte())
                .RuleFor(x => x.Radio, f => f.PickRandom<RadioStation>())
                .RuleFor(x => x.Extra1, f => f.Random.SByte())
                .RuleFor(x => x.Extra2, f => f.Random.SByte())
                .RuleFor(x => x.Bomb, f => f.PickRandom<BombType>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            StoredCar x0 = GenerateTestObject(p);
            StoredCar x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Model, x1.Model);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Heading, x1.Heading);
            Assert.Equal(x0.Flags, x1.Flags);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.Radio, x1.Radio);
            Assert.Equal(x0.Extra1, x1.Extra1);
            Assert.Equal(x0.Extra2, x1.Extra2);
            Assert.Equal(x0.Bomb, x1.Bomb);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            StoredCar x0 = GenerateTestObject(p);
            StoredCar x1 = new StoredCar(x0);

            Assert.Equal(x0, x1);
        }
    }
}
