using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestScripts : SaveDataObjectTestBase<Scripts>
    {
        public override Scripts GenerateTestVector(FileFormat format)
        {
            Faker faker = new Faker();

            int numGlobals = faker.Random.Int(1, 10);
            int numRunningScripts = faker.Random.Int(1, 10);

            Faker<Scripts> model = new Faker<Scripts>()
                .RuleFor(x => x.GlobalVariables, f => Generator.CreateValueCollection(numGlobals, g => f.Random.UInt()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => Generator.CreateObjectCollection(Scripts.Limits.ContactsCount, g => Generator.Generate<ContactInfo, TestContactInfo>()))
                .RuleFor(x => x.Collectives, f => Generator.CreateObjectCollection(Scripts.Limits.CollectivesCount, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.CreateObjectCollection(Scripts.Limits.BuildingSwapsCount, g => Generator.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.CreateObjectCollection(Scripts.Limits.InvisibilitySettingsCount, g => Generator.Generate<InvisibilitySetting, TestInvisibilitySetting>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => Generator.CreateObjectCollection(numRunningScripts, g => Generator.Generate<RunningScript, TestRunningScript>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format)
        {
            Scripts x0 = GenerateTestVector(format);
            Scripts x1 = CreateSerializedCopy(x0, out byte[] data, format);

            int runningScriptSize = format.IsPS2 ? 0x80 : 0x88;
            int expectedSize = 0x3D4 + (x0.GlobalVariables.Count * 4) + (x0.RunningScripts.Count * runningScriptSize);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android },
            new object[] { GTA3Save.FileFormats.IOS },
            new object[] { GTA3Save.FileFormats.PC },
            new object[] { GTA3Save.FileFormats.PS2NAEU },
            new object[] { GTA3Save.FileFormats.PS2AU },
            new object[] { GTA3Save.FileFormats.PS2JP },
            new object[] { GTA3Save.FileFormats.Xbox },
        };
    }
}
