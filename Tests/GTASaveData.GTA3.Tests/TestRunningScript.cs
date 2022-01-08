using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRunningScript : Base<RunningScript>
    {
        public override RunningScript GenerateTestObject(GTA3SaveParams p)
        {
            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.PrevScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => Generator.Words(f, RunningScript.MaxNameLength - 1))
                .RuleFor(x => x.IP, f => f.Random.Int())
                .RuleFor(x => x.Stack, f => Generator.Array(p.StackDepth, g => f.Random.Int()))
                .RuleFor(x => x.StackIndex, f => f.Random.Short())
                .RuleFor(x => x.Locals, f => Generator.Array(p.NumLocalVariables, g => f.Random.Int()))
                .RuleFor(x => x.TimerA, f => f.Random.UInt())
                .RuleFor(x => x.TimerB, f => f.Random.UInt())
                .RuleFor(x => x.CompareFlag, f => f.Random.Bool())
                .RuleFor(x => x.IsMissionScript, f => f.Random.Bool())
                .RuleFor(x => x.SkipWakeTime, f => f.Random.Bool())
                .RuleFor(x => x.WakeTime, f => f.Random.UInt())
                .RuleFor(x => x.AndOrState, f => f.PickRandom<AndOrState>())
                .RuleFor(x => x.NotFlag, f => f.Random.Bool())
                .RuleFor(x => x.DeathArrestEnabled, f => f.Random.Bool())
                .RuleFor(x => x.DeathArrestExecuted, f => f.Random.Bool())
                .RuleFor(x => x.MissionFlag, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RunningScript x0 = GenerateTestObject(p);
            RunningScript x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.NextScriptPointer, x1.NextScriptPointer);
            Assert.Equal(x0.PrevScriptPointer, x1.PrevScriptPointer);
            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.IP, x1.IP);
            Assert.Equal(x0.Stack, x1.Stack);
            Assert.Equal(x0.StackIndex, x1.StackIndex);
            Assert.Equal(x0.Locals, x1.Locals);
            Assert.Equal(x0.TimerA, x1.TimerA);
            Assert.Equal(x0.TimerB, x1.TimerB);
            Assert.Equal(x0.CompareFlag, x1.CompareFlag);
            Assert.Equal(x0.IsMissionScript, x1.IsMissionScript);
            Assert.Equal(x0.SkipWakeTime, x1.SkipWakeTime);
            Assert.Equal(x0.WakeTime, x1.WakeTime);
            Assert.Equal(x0.AndOrState, x1.AndOrState);
            Assert.Equal(x0.NotFlag, x1.NotFlag);
            Assert.Equal(x0.DeathArrestEnabled, x1.DeathArrestEnabled);
            Assert.Equal(x0.DeathArrestExecuted, x1.DeathArrestExecuted);
            Assert.Equal(x0.MissionFlag, x1.MissionFlag);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RunningScript x0 = GenerateTestObject(p);
            RunningScript x1 = new RunningScript(x0);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void Stack()
        {
            int i0 = 1337;
            int i1 = 69420;
            int i2 = 1234;
            RunningScript x0 = new RunningScript();

            Assert.Throws<InvalidOperationException>(() => x0.PeekStack());

            x0.PushStack(i0);
            x0.PushStack(i1);
            Assert.Equal(2, x0.StackIndex);

            Assert.Equal(i1, x0.PeekStack());

            int j0 = x0.PopStack();
            int j1 = x0.PopStack();
            Assert.Equal(i1, j0);
            Assert.Equal(i0, j1);
            Assert.Equal(0, x0.StackIndex);

            Assert.Throws<InvalidOperationException>(() => x0.PopStack());

            x0.PushStack(i2);
            int j2 = x0.PopStack();
            Assert.Equal(i2, j2);
        }

        public override int GetSizeOfTestObject(RunningScript obj, GTA3SaveParams p)
        {
            return p.FileType.IsPS2 ? 0x80 : 0x88;
        }
    }
}
