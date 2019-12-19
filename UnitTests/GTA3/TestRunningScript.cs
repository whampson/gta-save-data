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
        private const int SizePS2 = 0x80;
        private const int SizeNonPS2 = 0x88;

        // TODO: assert serialized size

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

        [Fact]
        public void Sanity()
        {
            RunningScript x0 = GenerateTestVector();
            RunningScript x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(SizeNonPS2, data.Length);
        }

        [Fact]
        public void SanityPS2()
        {
            RunningScript x0 = GenerateTestVector(SystemType.PS2);
            RunningScript x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, SystemType.PS2);
            
            Assert.Equal(x0, x1);
            Assert.Equal(SizePS2, data.Length);

        }
    }
}
