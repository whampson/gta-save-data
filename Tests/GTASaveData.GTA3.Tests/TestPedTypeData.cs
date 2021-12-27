using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedTypeData : Base<PedTypeData>
    {
        public override PedTypeData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<PedTypeData> model = new Faker<PedTypeData>()
                .RuleFor(x => x.PedTypes, f => Generator.Array(PedTypeData.NumPedTypes, g => Generator.Generate<PedType, TestPedType, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedTypeData x0 = GenerateTestObject(p);
            PedTypeData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.PedTypes, x1.PedTypes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedTypeData x0 = GenerateTestObject(p);
            PedTypeData x1 = new PedTypeData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
