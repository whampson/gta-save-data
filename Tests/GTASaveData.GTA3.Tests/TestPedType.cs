using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedType : Base<PedType>
    {
        public override PedType GenerateTestObject(GTA3SaveParams p)
        {
            Faker<PedType> model = new Faker<PedType>()
                .RuleFor(x => x.Flag, f => f.PickRandom<PedTypeFlags>())
                .RuleFor(x => x.Unknown0, f => f.Random.Float())
                .RuleFor(x => x.Unknown1, f => f.Random.Float())
                .RuleFor(x => x.Unknown2, f => f.Random.Float())
                .RuleFor(x => x.Unknown3, f => f.Random.Float())
                .RuleFor(x => x.Unknown4, f => f.Random.Float())
                .RuleFor(x => x.Threats, f => f.PickRandom<PedTypeFlags>())
                .RuleFor(x => x.Avoids, f => f.PickRandom<PedTypeFlags>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedType x0 = GenerateTestObject(p);
            PedType x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Flag, x1.Flag);
            Assert.Equal(x0.Unknown0, x1.Unknown0);
            Assert.Equal(x0.Unknown1, x1.Unknown1);
            Assert.Equal(x0.Unknown2, x1.Unknown2);
            Assert.Equal(x0.Unknown3, x1.Unknown3);
            Assert.Equal(x0.Unknown4, x1.Unknown4);
            Assert.Equal(x0.Threats, x1.Threats);
            Assert.Equal(x0.Avoids, x1.Avoids);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedType x0 = GenerateTestObject(p);
            PedType x1 = new PedType(x0);

            Assert.Equal(x0, x1);
        }
    }
}
