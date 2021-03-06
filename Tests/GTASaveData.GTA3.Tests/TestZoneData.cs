﻿using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3.Tests
{
    public class TestZoneData : Base<ZoneData>
    {
        public override ZoneData GenerateTestObject(FileFormat format)
        {
            Faker<ZoneData> model = new Faker<ZoneData>()
                .RuleFor(x => x.CurrentZoneIndex, f => f.Random.Int())
                .RuleFor(x => x.CurrentLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.FindIndex, f => f.Random.Short())
                .RuleFor(x => x.Zones, f => Generator.Array(ZoneData.MaxNumZones, g => Generator.Generate<Zone, TestZone>()))
                .RuleFor(x => x.ZoneInfo, f => Generator.Array(ZoneData.MaxNumZoneInfos, g => Generator.Generate<ZoneInfo, TestZoneInfo>()))
                .RuleFor(x => x.NumberOfZones, f => f.Random.Short())
                .RuleFor(x => x.NumberOfZoneInfos, f => f.Random.Short())
                .RuleFor(x => x.MapZones, f => Generator.Array(ZoneData.MaxNumMapZones, g => Generator.Generate<Zone, TestZone>()))
                .RuleFor(x => x.AudioZones, f => Generator.Array(ZoneData.MaxNumAudioZones, g => f.Random.Short()))
                .RuleFor(x => x.NumberOfMapZones, f => f.Random.Short())
                .RuleFor(x => x.NumberOfAudioZones, f => f.Random.Short());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ZoneData x0 = GenerateTestObject(format);
            ZoneData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.CurrentZoneIndex, x1.CurrentZoneIndex);
            Assert.Equal(x0.CurrentLevel, x1.CurrentLevel);
            Assert.Equal(x0.FindIndex, x1.FindIndex);
            Assert.Equal(x0.Zones, x1.Zones);
            Assert.Equal(x0.ZoneInfo, x1.ZoneInfo);
            Assert.Equal(x0.NumberOfZones, x1.NumberOfZones);
            Assert.Equal(x0.NumberOfZoneInfos, x1.NumberOfZoneInfos);
            Assert.Equal(x0.MapZones, x1.MapZones);
            Assert.Equal(x0.AudioZones, x1.AudioZones);
            Assert.Equal(x0.NumberOfMapZones, x1.NumberOfMapZones);
            Assert.Equal(x0.NumberOfAudioZones, x1.NumberOfAudioZones);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            ZoneData x0 = GenerateTestObject();
            ZoneData x1 = new ZoneData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
