using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPhone : Base<Phone>
    {
        public override Phone GenerateTestObject(GTA3SaveParams p)
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
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Phone x0 = GenerateTestObject(p);
            Phone x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Messages, x1.Messages);
            Assert.Equal(x0.RepeatedMessageStartTime, x1.RepeatedMessageStartTime);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.VisibleToCam, x1.VisibleToCam);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Phone x0 = GenerateTestObject(p);
            Phone x1 = new Phone(x0);

            Assert.Equal(x0, x1);
        }
    }
}
