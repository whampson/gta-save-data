using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPhoneData : Base<PhoneData>
    {
        public override PhoneData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<PhoneData> model = new Faker<PhoneData>()
                .RuleFor(x => x.NumPhones, f => f.Random.Int())
                .RuleFor(x => x.NumActivePhones, f => f.Random.Int())
                .RuleFor(x => x.Phones, f => Generator.Array(PhoneData.MaxNumPhones, g => Generator.Generate<Phone, TestPhone, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PhoneData x0 = GenerateTestObject(p);
            PhoneData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.NumPhones, x1.NumPhones);
            Assert.Equal(x0.NumActivePhones, x1.NumActivePhones);
            Assert.Equal(x0.Phones, x1.Phones);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PhoneData x0 = GenerateTestObject(p);
            PhoneData x1 = new PhoneData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
