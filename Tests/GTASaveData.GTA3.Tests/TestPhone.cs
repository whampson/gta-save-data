using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPhone : Base<Phone>
    {
        public override Phone GenerateTestObject(FileFormat format)
        {
            Faker<Phone> model = new Faker<Phone>()
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Messages, f => Generator.Array(Phone.MaxNumMessages, g => f.Random.UInt()))
                .RuleFor(x => x.RepeatedMessageStartTime, f => f.Random.UInt())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.State, f => f.PickRandom<PhoneState>())
                .RuleFor(x => x.VisibleToCam, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Phone x0 = GenerateTestObject(format);
            Phone x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Messages, x1.Messages);
            Assert.Equal(x0.RepeatedMessageStartTime, x1.RepeatedMessageStartTime);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.VisibleToCam, x1.VisibleToCam);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            Phone x0 = GenerateTestObject();
            Phone x1 = new Phone(x0);

            Assert.Equal(x0, x1);
        }
    }
}
