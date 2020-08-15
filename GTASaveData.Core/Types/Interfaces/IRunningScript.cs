using System;
using System.Collections.Generic;
using System.Text;

namespace GTASaveData.Types.Interfaces
{
    public interface IRunningScript
    {
        string Name { get; set; }
        int IP { get; set; }
        IEnumerable<int> Stack { get; }
        short StackPosition { get; set; }
        IEnumerable<int> LocalVariables { get; }
        uint TimerA { get; set; }
        uint TimerB { get; set; }
        uint WakeTime { get; set; }

        void PushStack(int value);
        int PopStack();
        int PeekStack();
        void SetLocal(int index, int value);
    }
}
