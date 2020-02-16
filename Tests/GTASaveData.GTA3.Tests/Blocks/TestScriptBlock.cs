using Bogus;
using GTASaveData.GTA3;
using GTASaveData.GTA3.Blocks;
using GTASaveData.Serialization;
using System.Collections.Generic;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3.Blocks
{
    public class TestScriptBlock : SerializableObjectTestBase<ScriptBlock>
    {
        public override ScriptBlock GenerateTestVector(FileFormat format)
        {
            Faker faker = new Faker();

            int numGlobals = faker.Random.Int(1, 10);
            int numRunningScripts = faker.Random.Int(1, 10);

            Faker<ScriptBlock> model = new Faker<ScriptBlock>()
                .RuleFor(x => x.GlobalVariables, f => Generator.CreateArray(numGlobals, g => f.Random.UInt()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => Generator.CreateArray(ScriptBlock.Limits.ContactsCount, g => Generator.Generate<ContactInfo, TestContactInfo>()))
                .RuleFor(x => x.Collectives, f => Generator.CreateArray(ScriptBlock.Limits.CollectivesCount, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.CreateArray(ScriptBlock.Limits.BuildingSwapsCount, g => Generator.Generate<StaticReplacement, TestStaticReplacement>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.CreateArray(ScriptBlock.Limits.InvisibilitySettingsCount, g => Generator.Generate<InvisibleObject, TestInvisibleObject>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => Generator.CreateArray(numRunningScripts, g => Generator.Generate<Thread, TestThread>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format)
        {
            ScriptBlock x0 = GenerateTestVector(format);
            ScriptBlock x1 = CreateSerializedCopy(x0, out byte[] data, format);

            int runningScriptSize = format.IsPS2 ? 0x80 : 0x88;
            int expectedSize = 0x3D4 + (x0.GlobalVariables.Count * 4) + (x0.RunningScripts.Count * runningScriptSize);

            Assert.Equal(x0.GlobalVariables, x1.GlobalVariables);
            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.Contacts, x1.Contacts);
            Assert.Equal(x0.Collectives, x1.Collectives);
            Assert.Equal(x0.NextFreeCollectiveIndex, x1.NextFreeCollectiveIndex);
            Assert.Equal(x0.BuildingSwaps, x1.BuildingSwaps);
            Assert.Equal(x0.InvisibilitySettings, x1.InvisibilitySettings);
            Assert.Equal(x0.UsingAMultiScriptFile, x1.UsingAMultiScriptFile);
            Assert.Equal(x0.MainScriptSize, x1.MainScriptSize);
            Assert.Equal(x0.LargestMissionScriptSize, x1.LargestMissionScriptSize);
            Assert.Equal(x0.NumberOfMissionScripts, x1.NumberOfMissionScripts);
            Assert.Equal(x0.RunningScripts, x1.RunningScripts);
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
