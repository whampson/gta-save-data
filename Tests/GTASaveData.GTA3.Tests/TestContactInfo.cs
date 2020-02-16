using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestContactInfo : SerializableObjectTestBase<ContactInfo>
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
            ContactInfo x0 = GenerateTestVector();
            ContactInfo x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.BaseBriefId, x1.BaseBriefId);
            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }
    }
}
