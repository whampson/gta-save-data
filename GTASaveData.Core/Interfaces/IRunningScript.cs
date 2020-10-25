using System.Collections.Generic;

namespace GTASaveData.Interfaces
{
    public interface IRunningScript
    {
        string Name { get; set; }
        int InstructionPointer { get; set; }
        IEnumerable<int> Stack { get; }
        short StackPointer { get; set; }
        IEnumerable<int> Locals { get; }
        uint TimerA { get; set; }
        uint TimerB { get; set; }
        uint WakeTime { get; set; }

        void PushStack(int value);
        int PopStack();
        int PeekStack();
        void SetLocal(int index, int value);
    }
}
