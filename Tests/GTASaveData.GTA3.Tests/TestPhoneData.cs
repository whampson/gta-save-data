using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPhoneData : Base<PhoneData>
    {
        public override PhoneData GenerateTestObject(FileType format)
        {
            Faker<PhoneData> model = new Faker<PhoneData>()
                .RuleFor(x => x.NumPhones, f => f.Random.Int())
                .RuleFor(x => x.NumActivePhones, f => f.Random.Int())
                .RuleFor(x => x.Phones, f => Generator.Array(PhoneData.MaxNumPhones, g => Generator.Generate<Phone, TestPhone>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            PhoneData x0 = GenerateTestObject(format);
            PhoneData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.NumPhones, x1.NumPhones);
            Assert.Equal(x0.NumActivePhones, x1.NumActivePhones);
            Assert.Equal(x0.Phones, x1.Phones);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            PhoneData x0 = GenerateTestObject();
            PhoneData x1 = new PhoneData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
