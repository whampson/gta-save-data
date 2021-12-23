using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestBuildingSwap : SaveDataObjectTestBase<BuildingSwap>
    {
        public override BuildingSwap GenerateTestObject(FileType format)
        {
            Faker<BuildingSwap> model = new Faker<BuildingSwap>()
                .RuleFor(x => x.Type, f => f.PickRandom<EntityClassType>())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.NewModel, f => f.Random.Int())
                .RuleFor(x => x.OldModel, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            BuildingSwap x0 = GenerateTestObject();
            BuildingSwap x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.NewModel, x1.NewModel);
            Assert.Equal(x0.OldModel, x1.OldModel);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            BuildingSwap x0 = GenerateTestObject();
            BuildingSwap x1 = new BuildingSwap(x0);

            Assert.Equal(x0, x1);
        }

        public override int GetSizeOfTestObject(BuildingSwap obj)
        {
            return 16;
        }
    }
}
