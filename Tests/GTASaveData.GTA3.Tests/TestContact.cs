using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestContact : Base<Contact>
    {
        public override Contact GenerateTestObject(FileType format)
        {
            Faker<Contact> model = new Faker<Contact>()
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.BaseBriefId, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            Contact x0 = GenerateTestObject(format);
            Contact x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.BaseBriefId, x1.BaseBriefId);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Contact x0 = GenerateTestObject();
            Contact x1 = new Contact(x0);

            Assert.Equal(x0, x1);
        }
    }
}
