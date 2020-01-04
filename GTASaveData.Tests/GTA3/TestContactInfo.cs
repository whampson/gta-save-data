using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestContactInfo : SaveDataObjectTestBase<ContactInfo>
    {
        public override ContactInfo GenerateTestVector(FileFormat format)
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
            ContactInfo c1 = CreateSerializedCopy(c0, out byte[] data);

            Assert.Equal(c0, c1);
            Assert.Equal(8, data.Length);
        }
    }
}
