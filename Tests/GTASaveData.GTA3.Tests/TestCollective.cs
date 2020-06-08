using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCollective : Base<Collective>
    {
        public override Collective GenerateTestObject(FileFormat format)
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.Field04h, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Collective x0 = GenerateTestObject(format);
            Collective x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.Field04h, x1.Field04h);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = new Collective(x0);

            Assert.Equal(x0, x1);
        }
    }
}
