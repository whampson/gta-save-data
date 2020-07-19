using System;
using System.Collections.Generic;
using System.Text;

namespace GTASaveData.Types.Interfaces
{
    public interface IRunningScript
    {
        string Name { get; set; }
        uint IP { get; set; }
        IEnumerable<int> Stack { get; }
        ushort StackPosition { get; set; }
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
