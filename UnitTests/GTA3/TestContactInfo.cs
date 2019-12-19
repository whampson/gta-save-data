using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
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
        public void Sanity()
        {
            ContactInfo c0 = GenerateTestVector();
            ContactInfo c1 = TestHelper.CreateSerializedCopy(c0);

            Assert.Equal(c0, c1);
        }
    }
}
