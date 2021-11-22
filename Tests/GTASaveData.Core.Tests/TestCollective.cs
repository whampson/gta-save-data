using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestCollective : SaveDataObjectTestBase<Collective>
    {
        public override Collective GenerateTestObject(FileFormat format)
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.PedIndex, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.PedIndex, x1.PedIndex);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = new Collective(x0);

            Assert.Equal(x0, x1);
        }

        public override int GetSizeOfTestObject(Collective obj)
        {
            return 8;
        }
    }
}
