using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestStoredCarSlot : SaveDataObjectTestBase<StoredCarSlot>
    {
        public override StoredCarSlot GenerateTestVector(FileFormat format)
        {
            Faker<StoredCarSlot> model = new Faker<StoredCarSlot>()
                .RuleFor(x => x.Portland, f => Generator.Generate<StoredCar, TestStoredCar>())
                .RuleFor(x => x.Staunton, f => Generator.Generate<StoredCar, TestStoredCar>())
                .RuleFor(x => x.Shoreside, f => Generator.Generate<StoredCar, TestStoredCar>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            StoredCarSlot x0 = GenerateTestVector();
            StoredCarSlot x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(120, data.Length);
        }
    }
}
