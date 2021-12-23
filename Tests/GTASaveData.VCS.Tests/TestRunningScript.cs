using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.VCS.Tests
{
    public class TestRunningScript : Base<RunningScript>
    {
        public override RunningScript GenerateTestObject(FileType format)
        {
            Faker<RunningScript> model = new Faker<RunningScript>()
                .RuleFor(x => x.NextScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.PrevScriptPointer, f => f.Random.UInt())
                .RuleFor(x => x.Id, f => f.Random.Int())
                .RuleFor(x => x.Field10h, f => f.Random.Int())
                .RuleFor(x => x.InstructionPointer, f => f.Random.Int())
                .RuleFor(x => x.Stack, f => Generator.Array(RunningScript.MaxStackDepth, g => f.Random.Int()))
                .RuleFor(x => x.StackPointer, f => f.Random.Short())
                .RuleFor(x => x.Locals, f => Generator.Array(RunningScript.NumLocalVariables, g => f.Random.Int()))
                .RuleFor(x => x.TimerA, f => f.Random.UInt())
                .RuleFor(x => x.TimerB, f => f.Random.UInt())
                .RuleFor(x => x.Field1FCh, f => f.Random.Int())
                .RuleFor(x => x.WakeTime, f => f.Random.UInt())
                .RuleFor(x => x.Field204h, f => f.Random.Int())
                .RuleFor(x => x.Field208h, f => f.Random.Int())
                .RuleFor(x => x.Field20Ch, f => f.Random.Byte())
                .RuleFor(x => x.Field20Dh, f => f.Random.Byte())
                .RuleFor(x => x.Field20Eh, f => f.Random.Byte())
                .RuleFor(x => x.Name, f => Generator.Words(f, RunningScript.MaxNameLength - 1))
                .RuleFor(x => x.Field217h, f => f.Random.Byte());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            RunningScript x0 = GenerateTestObject(format);
            RunningScript x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.NextScriptPointer, x1.NextScriptPointer);
            Assert.Equal(x0.PrevScriptPointer, x1.PrevScriptPointer);
            Assert.Equal(x0.Id, x1.Id);
            Assert.Equal(x0.Field10h, x1.Field10h);
            Assert.Equal(x0.InstructionPointer, x1.InstructionPointer);
            Assert.Equal(x0.Stack, x1.Stack);
            Assert.Equal(x0.StackPointer, x1.StackPointer);
            Assert.Equal(x0.Locals, x1.Locals);
            Assert.Equal(x0.TimerA, x1.TimerA);
            Assert.Equal(x0.TimerB, x1.TimerB);
            Assert.Equal(x0.Field1FCh, x1.Field1FCh);
            Assert.Equal(x0.WakeTime, x1.WakeTime);
            Assert.Equal(x0.Field204h, x1.Field204h);
            Assert.Equal(x0.Field208h, x1.Field208h);
            Assert.Equal(x0.Field20Ch, x1.Field20Ch);
            Assert.Equal(x0.Field20Dh, x1.Field20Dh);
            Assert.Equal(x0.Field20Eh, x1.Field20Eh);
            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.Field217h, x1.Field217h);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            RunningScript x0 = GenerateTestObject();
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
            Assert.Equal(2, x0.StackPointer);

            Assert.Equal(i1, x0.PeekStack());

            int j0 = x0.PopStack();
            int j1 = x0.PopStack();
            Assert.Equal(i1, j0);
            Assert.Equal(i0, j1);
            Assert.Equal(0, x0.StackPointer);

            Assert.Throws<InvalidOperationException>(() => x0.PopStack());

            x0.PushStack(i2);
            int j2 = x0.PopStack();
            Assert.Equal(i2, j2);
        }
    }
}
