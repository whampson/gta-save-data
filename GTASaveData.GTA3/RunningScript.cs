using GTASaveData.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class RunningScript : SaveDataObject, IRunningScript,
        IEquatable<RunningScript>, IDeepClonable<RunningScript>
    {
        public const int MaxNameLength = 8;
        public const int MaxStackDepth = 6;
        public const int MaxStackDepthPS2 = 4;
        public const int NumLocalVariables = 16;

        private uint m_pNextScript; // not loaded
        private uint m_pPrevScript; // not loaded
        private string m_name;
        private int m_ip;
        private Array<int> m_stack;
        private short m_stackPointer;
        private Array<int> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private bool m_condResult;
        private bool m_isMissionScript;
        private bool m_clearMessages;
        private uint m_wakeTime;
        private short m_andOrState;
        private bool m_notFlag;
        private bool m_deathArrestEnabled;
        private bool m_deathArrestExecuted;
        private bool m_missionFlag;

        [Obsolete("Value overridden by the game.")]
        public uint NextScriptPointer
        {
            get { return m_pNextScript; }
            set { m_pNextScript = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint PrevScriptPointer
        {
            get { return m_pPrevScript; }
            set { m_pPrevScript = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public int InstructionPointer
        {
            get { return m_ip; }
            set { m_ip = value; OnPropertyChanged(); }
        }

        public Array<int> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        public short StackPointer
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

        public Array<int> Locals
        {
            get { return m_localVariables; }
            set { m_localVariables = value; OnPropertyChanged(); }
        }

        public uint TimerA
        {
            get { return m_timerA; }
            set { m_timerA = value; OnPropertyChanged(); }
        }

        public uint TimerB
        {
            get { return m_timerB; }
            set { m_timerB = value; OnPropertyChanged(); }
        }

        public bool ConditionResult
        {
            get { return m_condResult; }
            set { m_condResult = value; OnPropertyChanged(); }
        }

        public bool IsMissionScript
        {
            get { return m_isMissionScript; }
            set { m_isMissionScript = value; OnPropertyChanged(); }
        }

        public bool ClearMessages
        {
            get { return m_clearMessages; }
            set { m_clearMessages = value; OnPropertyChanged(); }
        }

        public uint WakeTime
        {
            get { return m_wakeTime; }
            set { m_wakeTime = value; OnPropertyChanged(); }
        }

        public short AndOrState
        {
            get { return m_andOrState; }
            set { m_andOrState = value; OnPropertyChanged(); }
        }

        public bool NotFlag
        {
            get { return m_notFlag; }
            set { m_notFlag = value; OnPropertyChanged(); }
        }

        public bool WastedBustedCheckEnabled
        {
            get { return m_deathArrestEnabled; }
            set { m_deathArrestEnabled = value; OnPropertyChanged(); }
        }

        public bool WastedBustedCheckResult
        {
            get { return m_deathArrestExecuted; }
            set { m_deathArrestExecuted = value; OnPropertyChanged(); }
        }

        public bool MissionFlag
        {
            get { return m_missionFlag; }
            set { m_missionFlag = value; OnPropertyChanged(); }
        }

        IEnumerable<int> IRunningScript.Stack => m_stack;

        IEnumerable<int> IRunningScript.Locals => m_localVariables;

        public RunningScript()
        {
            m_name = "noname";
            m_stack = ArrayHelper.CreateArray<int>(MaxStackDepth);
            m_localVariables = ArrayHelper.CreateArray<int>(NumLocalVariables);
        }

        public RunningScript(RunningScript other)
        {
            NextScriptPointer = other.NextScriptPointer;
            PrevScriptPointer = other.PrevScriptPointer;
            Name = other.Name;
            InstructionPointer = other.InstructionPointer;
            Stack = ArrayHelper.DeepClone(other.Stack);
            StackPointer = other.StackPointer;
            Locals = ArrayHelper.DeepClone(other.Locals);
            TimerA = other.TimerA;
            TimerB = other.TimerB;
            ConditionResult = other.ConditionResult;
            IsMissionScript = other.IsMissionScript;
            ClearMessages = other.ClearMessages;
            WakeTime = other.WakeTime;
            AndOrState = other.AndOrState;
            NotFlag = other.NotFlag;
            WastedBustedCheckEnabled = other.WastedBustedCheckEnabled;
            WastedBustedCheckResult = other.WastedBustedCheckResult;
            MissionFlag = other.MissionFlag;
        }

        public void PushStack(int value)
        {
            if (StackPointer + 1 >= Stack.Count)
            {
                Stack.Add(value);
                StackPointer++;
            }
            else
            {
                Stack[StackPointer++] = value;
            }
        }

        public int PopStack()
        {
            if (StackPointer == 0)
            {
                throw new InvalidOperationException(Strings.Error_InvalidOperation_StackEmpty);
            }
            return Stack[--StackPointer];
        }

        public int PeekStack()
        {
            if (StackPointer == 0)
            {
                throw new InvalidOperationException(Strings.Error_InvalidOperation_StackEmpty);
            }
            return Stack[StackPointer - 1];
        }

        public void SetLocal(int index, int value)
        {
            Locals[index] = value;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            NextScriptPointer = buf.ReadUInt32();
            PrevScriptPointer = buf.ReadUInt32();
            Name = buf.ReadString(MaxNameLength);
            InstructionPointer = buf.ReadInt32();
            Stack = buf.ReadArray<int>(GetMaxStackDepth(fmt));
            StackPointer = buf.ReadInt16();
            buf.Align4();
            Locals = buf.ReadArray<int>(NumLocalVariables);
            TimerA = buf.ReadUInt32();
            TimerB = buf.ReadUInt32();
            ConditionResult = buf.ReadBool();
            IsMissionScript = buf.ReadBool();
            ClearMessages = buf.ReadBool();
            buf.Align4();
            WakeTime = buf.ReadUInt32();
            AndOrState = buf.ReadInt16();
            NotFlag = buf.ReadBool();
            WastedBustedCheckEnabled = buf.ReadBool();
            WastedBustedCheckResult = buf.ReadBool();
            MissionFlag = buf.ReadBool();
            buf.Align4();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(NextScriptPointer);
            buf.Write(PrevScriptPointer);
            buf.Write(Name, MaxNameLength);
            buf.Write(InstructionPointer);
            buf.Write(Stack, GetMaxStackDepth(fmt));
            buf.Write(StackPointer);
            buf.Align4();
            buf.Write(Locals, NumLocalVariables);
            buf.Write(TimerA);
            buf.Write(TimerB);
            buf.Write(ConditionResult);
            buf.Write(IsMissionScript);
            buf.Write(ClearMessages);
            buf.Align4();
            buf.Write(WakeTime);
            buf.Write(AndOrState);
            buf.Write(NotFlag);
            buf.Write(WastedBustedCheckEnabled);
            buf.Write(WastedBustedCheckResult);
            buf.Write(MissionFlag);
            buf.Align4();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        public static int GetMaxStackDepth(FileFormat fmt)
        {
            return (fmt.IsPS2) ? MaxStackDepthPS2 : MaxStackDepth;
        }

        protected override int GetSize(FileFormat fmt)
        {
            return (fmt.IsPS2) ? 0x80 : 0x88;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RunningScript);
        }

        public bool Equals(RunningScript other)
        {
            if (other == null)
            {
                return false;
            }

            return NextScriptPointer.Equals(other.NextScriptPointer)
                && PrevScriptPointer.Equals(other.PrevScriptPointer)
                && Name.Equals(other.Name)
                && InstructionPointer.Equals(other.InstructionPointer)
                && Stack.SequenceEqual(other.Stack)
                && StackPointer.Equals(other.StackPointer)
                && Locals.SequenceEqual(other.Locals)
                && TimerA.Equals(other.TimerA)
                && TimerB.Equals(other.TimerB)
                && ConditionResult.Equals(other.ConditionResult)
                && IsMissionScript.Equals(other.IsMissionScript)
                && ClearMessages.Equals(other.ClearMessages)
                && WakeTime.Equals(other.WakeTime)
                && AndOrState.Equals(other.AndOrState)
                && NotFlag.Equals(other.NotFlag)
                && WastedBustedCheckEnabled.Equals(other.WastedBustedCheckEnabled)
                && WastedBustedCheckResult.Equals(other.WastedBustedCheckResult)
                && MissionFlag.Equals(other.MissionFlag);
        }

        public RunningScript DeepClone()
        {
            return new RunningScript(this);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
