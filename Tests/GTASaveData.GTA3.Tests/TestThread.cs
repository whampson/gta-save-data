using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using System.Collections.Generic;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestThread : SerializableObjectTestBase<Thread>
    {
        public override Thread GenerateTestVector(FileFormat format)
        {
            int numLocals = Thread.Limits.LocalVariablesCount;
            int stackSize = format.SupportsPS2
                ? Thread.Limits.StackSizePS2
                : Thread.Limits.StackSize;

            Faker<Thread> model = new Faker<Thread>()
                .RuleFor(x => x.NextScript, f => f.Random.UInt())
                .RuleFor(x => x.PrevScript, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => Generator.RandomWords(f, 7))
                .RuleFor(x => x.InstructionPointer, f => f.Random.UInt())
                .RuleFor(x => x.Stack, f => Generator.CreateArray(stackSize, g => f.Random.UInt()))
                .RuleFor(x => x.StackPointer, f => f.Random.UShort())
                .RuleFor(x => x.LocalVariables, f => Generator.CreateArray(numLocals, g => f.Random.UInt()))
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
            Thread x0 = GenerateTestVector(format);
            Thread x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0.NextScript, x1.NextScript);
            Assert.Equal(x0.PrevScript, x1.PrevScript);
            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.InstructionPointer, x1.InstructionPointer);
            Assert.Equal(x0.Stack, x1.Stack);
            Assert.Equal(x0.StackPointer, x1.StackPointer);
            Assert.Equal(x0.LocalVariables, x1.LocalVariables);
            Assert.Equal(x0.TimerA, x1.TimerA);
            Assert.Equal(x0.TimerB, x1.TimerB);
            Assert.Equal(x0.IfResult, x1.IfResult);
            Assert.Equal(x0.IsMissionScript, x1.IsMissionScript);
            Assert.Equal(x0.IsActive, x1.IsActive);
            Assert.Equal(x0.WakeTime, x1.WakeTime);
            Assert.Equal(x0.IfNumber, x1.IfNumber);
            Assert.Equal(x0.NotFlag, x1.NotFlag);
            Assert.Equal(x0.IsWastedOrBustedCheckEnabled, x1.IsWastedOrBustedCheckEnabled);
            Assert.Equal(x0.IsWastedOrBusted, x1.IsWastedOrBusted);
            Assert.Equal(x0.IsMission, x1.IsMission);
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
