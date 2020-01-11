using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestRunningScript : SaveDataObjectTestBase<RunningScript>
    {
        public override RunningScript GenerateTestVector(FileFormat format)
        {
            int numLocals = RunningScript.Limits.LocalVariablesCount;
            int stackSize = format.IsPS2
                ? RunningScript.Limits.StackSizePS2
                : RunningScript.Limits.StackSize;

            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScript, f => f.Random.UInt())
                .RuleFor(x => x.PrevScript, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => Generator.RandomWords(f, 7))
                .RuleFor(x => x.InstructionPointer, f => f.Random.UInt())
                .RuleFor(x => x.Stack, f => Generator.CreateValueCollection(stackSize, g => f.Random.UInt()))
                .RuleFor(x => x.StackPointer, f => f.Random.UShort())
                .RuleFor(x => x.LocalVariables, f => Generator.CreateValueCollection(numLocals, g => f.Random.UInt()))
                .RuleFor(x => x.TimerA, f => f.Random.UInt())
                .RuleFor(x => x.TimerB, f => f.Random.UInt())
                .RuleFor(x => x.IfResult, f => f.Random.Bool())
                .RuleFor(x => x.IsMissionScript, f => f.Random.Bool())
                .RuleFor(x => x.IsActive, f => f.Random.Bool())
                .RuleFor(x => x.WakeTime, f => f.Random.UInt())
                .RuleFor(x => x.IfNumber, f => f.Random.UShort())
                .RuleFor(x => x.NotFlag, f => f.Random.Bool())
                .RuleFor(x => x.IsWastedOrBustedCheckEnabled, f => f.Random.Bool())
                .RuleFor(x => x.IsWastedOrBusted, f => f.Random.Bool())
                .RuleFor(x => x.IsMission, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format, int expectedSize)
        {
            RunningScript x0 = GenerateTestVector(format);
            RunningScript x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 0x88 },
            new object[] { GTA3Save.FileFormats.IOS, 0x88 },
            new object[] { GTA3Save.FileFormats.PC, 0x88 },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 0x80 },
            new object[] { GTA3Save.FileFormats.PS2AU, 0x80 },
            new object[] { GTA3Save.FileFormats.PS2JP, 0x80 },
            new object[] { GTA3Save.FileFormats.Xbox, 0x88 },
        };
    }
}
