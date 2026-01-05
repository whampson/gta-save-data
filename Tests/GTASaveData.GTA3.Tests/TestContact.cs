using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestContact : Base<Contact>
    {
        public override Contact GenerateTestObject(GTA3SaveParams p)
        {
            Faker<Contact> model = new Faker<Contact>()
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.BaseBriefId, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Contact x0 = GenerateTestObject(p);
            Contact x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.BaseBriefId, x1.BaseBriefId);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Contact x0 = GenerateTestObject(p);
            Contact x1 = new Contact(x0);

            Assert.Equal(x0, x1);
        }
    }
}
