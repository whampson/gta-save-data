using GTASaveData.JsonConverters;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.LCS
{
    public class RunningScript : SaveDataObject, IRunningScript,
        IEquatable<RunningScript>, IDeepClonable<RunningScript>
    {
        public const int MaxNameLength = 8;
        public const int MaxStackDepth = 16;
        public const int NumLocalVariables = 104;

        private uint m_pNextScript; // not loaded
        private uint m_pPrevScript; // not loaded
        private int m_id;
        private string m_name;
        private int m_ip;
        private Array<int> m_stack;
        private short m_stackPointer;
        private Array<int> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private int m_field210h;
        private byte m_field214h;
        private bool m_condResult;
        private bool m_isMissionScript;
        private bool m_clearMessages;
        private uint m_wakeTime;
        private short m_andOrState;
        private bool m_notFlag;
        private bool m_deathArrestEnabled;
        private bool m_deathArrestExecuted;
        private bool m_missionFlag;

        public uint NextScriptPointer
        {
            get { return m_pNextScript; }
            set { m_pNextScript = value; OnPropertyChanged(); }
        }

        public uint PrevScriptPointer
        {
            get { return m_pPrevScript; }
            set { m_pPrevScript = value; OnPropertyChanged(); }
        }

        public int Id
        {
            get { return m_id; }
            set { m_id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public int IP
        {
            get { return m_ip; }
            set { m_ip = value; OnPropertyChanged(); }
        }

        public Array<int> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        public short StackPosition
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

        [JsonConverter(typeof(IntArrayConverter))]
        public Array<int> LocalVariables
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

        public int Field210h
        {
            get { return m_field210h; }
            set { m_field210h = value; OnPropertyChanged(); }
        }

        public byte Field214h
        {
            get { return m_field214h; }
            set { m_field214h = value; OnPropertyChanged(); }
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

        IEnumerable<int> IRunningScript.LocalVariables => m_localVariables;

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
            Id = other.Id;
            Name = other.Name;
            IP = other.IP;
            Stack = ArrayHelper.DeepClone(other.Stack);
            StackPosition = other.StackPosition;
            LocalVariables = ArrayHelper.DeepClone(other.LocalVariables);
            TimerA = other.TimerA;
            TimerB = other.TimerB;
            Field210h = other.Field210h;
            Field214h = other.Field214h;
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
            if (StackPosition + 1 >= Stack.Count)
            {
                Stack.Add(value);
                StackPosition++;
            }
            else
            {
                Stack[StackPosition++] = value;
            }
        }

        public int PopStack()
        {
            if (StackPosition == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }
            return Stack[--StackPosition];
        }

        public int PeekStack()
        {
            if (StackPosition == 0)
            {
                throw new InvalidOperationException("The stack is full.");
            }
            return Stack[StackPosition - 1];
        }

        public void SetLocal(int index, int value)
        {
            LocalVariables[index] = value;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            NextScriptPointer = buf.ReadUInt32();
            if (fmt.IsiOS) buf.ReadInt32();
            PrevScriptPointer = buf.ReadUInt32();
            if (fmt.IsiOS) buf.ReadInt32();
            Id = buf.ReadInt32();
            buf.ReadInt32();
            Name = buf.ReadString(MaxNameLength);
            IP = buf.ReadInt32();
            Stack = buf.Read<int>(MaxStackDepth);
            StackPosition = buf.ReadInt16();
            buf.Skip(2);
            LocalVariables = buf.Read<int>(NumLocalVariables);
            TimerA = buf.ReadUInt32();
            TimerB = buf.ReadUInt32();
            Field210h = buf.ReadInt32();
            Field214h = buf.ReadByte();
            ConditionResult = buf.ReadBool();
            IsMissionScript = buf.ReadBool();
            ClearMessages = buf.ReadBool();
            WakeTime = buf.ReadUInt32();
            AndOrState = buf.ReadInt16();
            NotFlag = buf.ReadBool();
            WastedBustedCheckEnabled = buf.ReadBool();
            WastedBustedCheckResult = buf.ReadBool();
            MissionFlag = buf.ReadBool();
            buf.Skip(2);
            if (fmt.IsiOS) buf.ReadInt32();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(NextScriptPointer);
            if (fmt.IsiOS) buf.Write(0);
            buf.Write(PrevScriptPointer);
            if (fmt.IsiOS) buf.Write(0);
            buf.Write(Id);
            buf.Write(0);
            buf.Write(Name, MaxNameLength);
            buf.Write(IP);
            buf.Write(Stack, MaxStackDepth);
            buf.Write(StackPosition);
            buf.Skip(2);
            buf.Write(LocalVariables, NumLocalVariables);
            buf.Write(TimerA);
            buf.Write(TimerB);
            buf.Write(Field210h);
            buf.Write(Field214h);
            buf.Write(ConditionResult);
            buf.Write(IsMissionScript);
            buf.Write(ClearMessages);
            buf.Write(WakeTime);
            buf.Write(AndOrState);
            buf.Write(NotFlag);
            buf.Write(WastedBustedCheckEnabled);
            buf.Write(WastedBustedCheckResult);
            buf.Write(MissionFlag);
            buf.Skip(2);
            if (fmt.IsiOS) buf.Write(0);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return (fmt.IsiOS) ? 0x228 : 0x21C;
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
                && Id.Equals(other.Id)
                && Name.Equals(other.Name)
                && IP.Equals(other.IP)
                && Stack.SequenceEqual(other.Stack)
                && StackPosition.Equals(other.StackPosition)
                && LocalVariables.SequenceEqual(other.LocalVariables)
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
