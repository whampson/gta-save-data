using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.GTA3
{
    public class TestContactInfo
        : SaveDataObjectTestBase<ContactInfo>
    {
        public override ContactInfo GenerateTestVector(SystemType system)
        {
            Faker<ContactInfo> model = new Faker<ContactInfo>()
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.BaseBriefId, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            ContactInfo c0 = GenerateTestVector();
            ContactInfo c1 = TestHelper.CreateSerializedCopy(c0, out byte[] data);

            Assert.Equal(c0, c1);
            Assert.Equal(8, data.Length);
        }
    }
}
