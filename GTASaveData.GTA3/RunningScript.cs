using System;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class RunningScript : SaveDataObject, IEquatable<RunningScript>
    {
        public static class Limits
        {
            public const int MaxNameLength = 8;
            public const int MaxStackDepth = 6;
            public const int MaxStackDepthPS2 = 4;
            public const int NumberOfLocalVariables = 16;
        }

        private const int SizeOfRunningScript = 136;
        private const int SizeOfRunningScriptPS2 = 128;

        private uint m_pNextScript; // not loaded
        private uint m_pPrevScript; // not loaded
        private string m_name;
        private uint m_ip;
        private Array<int> m_stack;
        private ushort m_stackPointer;
        private Array<int> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private bool m_condResult;
        private bool m_isMissionScript;
        private bool m_clearMessages;
        private uint m_wakeTime;
        private ushort m_andOrState;
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

        public uint IP
        {
            get { return m_ip; }
            set { m_ip = value; OnPropertyChanged(); }
        }

        public Array<int> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        public ushort StackPointer
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

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

        public ushort AndOrState
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

        public RunningScript()
        {
            m_name = "noname";
            m_stack = new Array<int>();
            m_localVariables = new Array<int>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            NextScriptPointer = buf.ReadUInt32();
            PrevScriptPointer = buf.ReadUInt32();
            Name = buf.ReadString(Limits.MaxNameLength);
            IP = buf.ReadUInt32();
            Stack = buf.ReadArray<int>(GetMaxStackDepth(fmt));
            StackPointer = buf.ReadUInt16();
            buf.Align4Bytes();
            LocalVariables = buf.ReadArray<int>(Limits.NumberOfLocalVariables);
            TimerA = buf.ReadUInt32();
            TimerB = buf.ReadUInt32();
            ConditionResult = buf.ReadBool();
            IsMissionScript = buf.ReadBool();
            ClearMessages = buf.ReadBool();
            buf.Align4Bytes();
            WakeTime = buf.ReadUInt32();
            AndOrState = buf.ReadUInt16();
            NotFlag = buf.ReadBool();
            WastedBustedCheckEnabled = buf.ReadBool();
            WastedBustedCheckResult = buf.ReadBool();
            MissionFlag = buf.ReadBool();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(NextScriptPointer);
            buf.Write(PrevScriptPointer);
            buf.Write(Name, Limits.MaxNameLength);
            buf.Write(IP);
            buf.Write(Stack.ToArray(), GetMaxStackDepth(fmt));
            buf.Write(StackPointer);
            buf.Align4Bytes();
            buf.Write(LocalVariables.ToArray(), Limits.NumberOfLocalVariables);
            buf.Write(TimerA);
            buf.Write(TimerB);
            buf.Write(ConditionResult);
            buf.Write(IsMissionScript);
            buf.Write(ClearMessages);
            buf.Align4Bytes();
            buf.Write(WakeTime);
            buf.Write(AndOrState);
            buf.Write(NotFlag);
            buf.Write(WastedBustedCheckEnabled);
            buf.Write(WastedBustedCheckResult);
            buf.Write(MissionFlag);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            return (fmt.IsSupportedOnPS2)
                ? SizeOfRunningScriptPS2
                : SizeOfRunningScript;
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
                && IP.Equals(other.IP)
                && Stack.SequenceEqual(other.Stack)
                && StackPointer.Equals(other.StackPointer)
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

        public static int GetMaxStackDepth(DataFormat fmt)
        {
            return (fmt.IsSupportedOnPS2)
                ? Limits.MaxStackDepthPS2
                : Limits.MaxStackDepth;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
