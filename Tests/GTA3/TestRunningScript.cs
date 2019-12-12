using Bogus;
using GTASaveData.Common;
using GTASaveData.GTA3;
using System.Collections.ObjectModel;
using System.Linq;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestRunningScript
    {
        private const int SizePS2 = 0x80;
        private const int SizeNonPS2 = 0x88;

        public static RunningScript Generate(bool isPS2 = false)
        {
            int stackSize = (isPS2) ? RunningScript.StackSizePS2 : RunningScript.StackSize;
            int numLocals = RunningScript.LocalVariablesCount;

            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScript, f => f.Random.UInt())
                .RuleFor(x => x.PrevScript, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => TestHelper.RandomString(f, 7))
                .RuleFor(x => x.InstructionPointer, f => f.Random.UInt())
                .RuleFor(x => x.Stack, f => TestHelper.CreateCollection(stackSize, e => f.Random.UInt()))
                .RuleFor(x => x.StackPointer, f => f.Random.UShort())
                .RuleFor(x => x.LocalVariables, f => TestHelper.CreateCollection(numLocals, e => f.Random.UInt()))
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
            RunningScript x0 = Generate();
            RunningScript x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(SizeNonPS2, data.Length);
        }

        [Fact]
        public void SanityPS2()
        {
            RunningScript x0 = Generate(isPS2: true);
            RunningScript x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, SystemType.PS2);
            
            Assert.Equal(x0, x1);
            Assert.Equal(SizePS2, data.Length);

        }
    }
}
