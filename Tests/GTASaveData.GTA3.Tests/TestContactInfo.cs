using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestContact : Base<Contact>
    {
        public override Contact GenerateTestObject(FileFormat format)
        {
            Faker<Contact> model = new Faker<Contact>()
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.BaseBriefId, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Contact x0 = GenerateTestObject();
            Contact x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.BaseBriefId, x1.BaseBriefId);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
