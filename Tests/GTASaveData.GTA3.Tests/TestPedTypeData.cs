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

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            PedTypeData x0 = GenerateTestObject(format);
            PedTypeData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.PedTypes, x1.PedTypes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            PedTypeData x0 = GenerateTestObject();
            PedTypeData x1 = new PedTypeData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
