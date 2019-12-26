using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.GTA3
{
    public class TestStoredCarSlot
        : SaveDataObjectTestBase<StoredCarSlot>
    {
        public override StoredCarSlot GenerateTestVector(SystemType system)
        {
            Faker<StoredCarSlot> model = new Faker<StoredCarSlot>()
                .RuleFor(x => x.Portland, f => TestHelper.Generate<StoredCar, TestStoredCar>())
                .RuleFor(x => x.Staunton, f => TestHelper.Generate<StoredCar, TestStoredCar>())
                .RuleFor(x => x.Shoreside, f => TestHelper.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            StoredCarSlot x0 = GenerateTestVector();
            StoredCarSlot x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(120, data.Length);
        }
    }
}
