using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestZone : Base<Zone>
    {
        public override Zone GenerateTestObject(FileFormat format)
        {
            Faker<Zone> model = new Faker<Zone>()
                .RuleFor(x => x.Name, f => Generator.Words(f, Zone.MaxNameLength - 1))
                .RuleFor(x => x.Min, f => Generator.Vector3D(f))
                .RuleFor(x => x.Max, f => Generator.Vector3D(f))
                .RuleFor(x => x.Type, f => f.PickRandom<ZoneType>())
                .RuleFor(x => x.Level, f => f.PickRandom<Level>())
                .RuleFor(x => x.ZoneInfoDay, f => f.Random.Short())
                .RuleFor(x => x.ZoneInfoNight, f => f.Random.Short())
                .RuleFor(x => x.ChildZoneIndex, f => f.Random.Int())
                .RuleFor(x => x.ParentZoneIndex, f => f.Random.Int())
                .RuleFor(x => x.NextZoneIndex, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Zone x0 = GenerateTestObject(format);
            Zone x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.Min, x1.Min);
            Assert.Equal(x0.Max, x1.Max);
            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Level, x1.Level);
            Assert.Equal(x0.ZoneInfoDay, x1.ZoneInfoDay);
            Assert.Equal(x0.ZoneInfoNight, x1.ZoneInfoNight);
            Assert.Equal(x0.ChildZoneIndex, x1.ChildZoneIndex);
            Assert.Equal(x0.ParentZoneIndex, x1.ParentZoneIndex);
            Assert.Equal(x0.NextZoneIndex, x1.NextZoneIndex);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            Zone x0 = GenerateTestObject();
            Zone x1 = new Zone(x0);

            Assert.Equal(x0, x1);
        }
    }
}