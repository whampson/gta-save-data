using Bogus;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestContactInfo
    {
        public static ContactInfo Generate()
        {
            Faker<ContactInfo> model = new Faker<ContactInfo>()
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.BaseBriefId, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            ContactInfo c0 = Generate();
            ContactInfo c1 = TestHelper.CreateSerializedCopy(c0);

            Assert.Equal(c0, c1);
        }
    }
}
