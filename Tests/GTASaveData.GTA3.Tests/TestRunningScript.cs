using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRunningScript : Base<RunningScript>
    {
        public override RunningScript GenerateTestObject(DataFormat format)
        {
            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.PrevScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.Name, f => Generator.RandomWords(f, 7))
                .RuleFor(x => x.IP, f => f.Random.UInt())
                .RuleFor(x => x.Stack, f => Generator.CreateArray(RunningScript.GetMaxStackDepth(format), g => f.Random.UInt()))
                .RuleFor(x => x.StackPointer, f => f.Random.UShort())
                .RuleFor(x => x.LocalVariables, f => Generator.CreateArray(RunningScript.Limits.NumberOfLocalVariables, g => f.Random.UInt()))
                .RuleFor(x => x.TimerA, f => f.Random.UInt())
                .RuleFor(x => x.TimerB, f => f.Random.UInt())
                .RuleFor(x => x.ConditionResult, f => f.Random.Bool())
                .RuleFor(x => x.IsMissionScript, f => f.Random.Bool())
                .RuleFor(x => x.SkipWakeTime, f => f.Random.Bool())
                .RuleFor(x => x.WakeTime, f => f.Random.UInt())
                .RuleFor(x => x.AndOrState, f => f.Random.UShort())
                .RuleFor(x => x.NotFlag, f => f.Random.Bool())
                .RuleFor(x => x.DeathArrestCheckEnabled, f => f.Random.Bool())
                .RuleFor(x => x.DeathArrestCheckExecuted, f => f.Random.Bool())
                .RuleFor(x => x.MissionFlag, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void Serialization(DataFormat format)
        {
            RunningScript x0 = GenerateTestObject(format);
            RunningScript x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.NextScriptPointer, x1.NextScriptPointer);
            Assert.Equal(x0.PrevScriptPointer, x1.PrevScriptPointer);
            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.IP, x1.IP);
            Assert.Equal(x0.Stack, x1.Stack);
            Assert.Equal(x0.StackPointer, x1.StackPointer);
            Assert.Equal(x0.LocalVariables, x1.LocalVariables);
            Assert.Equal(x0.TimerA, x1.TimerA);
            Assert.Equal(x0.TimerB, x1.TimerB);
            Assert.Equal(x0.ConditionResult, x1.ConditionResult);
            Assert.Equal(x0.IsMissionScript, x1.IsMissionScript);
            Assert.Equal(x0.SkipWakeTime, x1.SkipWakeTime);
            Assert.Equal(x0.WakeTime, x1.WakeTime);
            Assert.Equal(x0.AndOrState, x1.AndOrState);
            Assert.Equal(x0.NotFlag, x1.NotFlag);
            Assert.Equal(x0.DeathArrestCheckEnabled, x1.DeathArrestCheckEnabled);
            Assert.Equal(x0.DeathArrestCheckExecuted, x1.DeathArrestCheckExecuted);
            Assert.Equal(x0.MissionFlag, x1.MissionFlag);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(format), data.Length);
        }
    }
}
