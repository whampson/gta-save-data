using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedTypeData : Base<PedTypeData>
    {
        public override PedTypeData GenerateTestObject(FileFormat format)
        {
            Faker<PedTypeData> model = new Faker<PedTypeData>()
                .RuleFor(x => x.PedTypes, f => Generator.Array(PedTypeData.NumPedTypes, g => Generator.Generate<PedType, TestPedType>()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            PedTypeData x0 = GenerateTestObject();
            PedTypeData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.PedTypes, x1.PedTypes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
