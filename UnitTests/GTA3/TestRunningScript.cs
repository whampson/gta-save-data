using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestRunningScript
        : SaveDataObjectTestBase<RunningScript>
    {
        public override RunningScript GenerateTestVector(SystemType system)
        {
            int stackSize = system.HasFlag(SystemType.PS2) ? RunningScript.StackSizePS2 : RunningScript.StackSize;
            int numLocals = RunningScript.LocalVariablesCount;

            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScript, f => f.Random.UInt())
                .RuleFor(x => x.PrevScript, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => TestHelper.RandomString(f, 7))
                .RuleFor(x => x.InstructionPointer, f => f.Random.UInt())
                .RuleFor(x => x.Stack, f => TestHelper.CreateValueCollection(stackSize, e => f.Random.UInt()))
                .RuleFor(x => x.StackPointer, f => f.Random.UShort())
                .RuleFor(x => x.LocalVariables, f => TestHelper.CreateValueCollection(numLocals, e => f.Random.UInt()))
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
        [InlineData(SystemType.Android, 0x88)]
        [InlineData(SystemType.IOS, 0x88)]
        [InlineData(SystemType.PC, 0x88)]
        [InlineData(SystemType.PS2, 0x80)]
        [InlineData(SystemType.Xbox, 0x88)]
        public void Serialization(SystemType system, int expectedSize)
        {
            RunningScript x0 = GenerateTestVector(system);
            RunningScript x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, system);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }
    }
}
