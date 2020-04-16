using System;
using System.Diagnostics;
using System.Linq;

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

        private uint m_pNextScript;
        private uint m_pPrevScript;
        private string m_scriptName;
        private uint m_ip;
        private Array<uint> m_stack;
        private ushort m_stackPointer;
        private Array<uint> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private bool m_condResult;
        private bool m_isMissionScript;
        private bool m_skipWakeTime;
        private uint m_wakeTime;
        private ushort m_andOrState;
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

        public string Name
        {
            get { return m_scriptName; }
            set { m_scriptName = value; OnPropertyChanged(); }
        }

        public uint IP
        {
            get { return m_ip; }
            set { m_ip = value; OnPropertyChanged(); }
        }

        public Array<uint> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        public ushort StackPointer
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

        public Array<uint> LocalVariables
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

        public bool SkipWakeTime
        {
            get { return m_skipWakeTime; }
            set { m_skipWakeTime = value; OnPropertyChanged(); }
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

        public bool DeathArrestCheckEnabled
        {
            get { return m_deathArrestEnabled; }
            set { m_deathArrestEnabled = value; OnPropertyChanged(); }
        }

        public bool DeathArrestCheckExecuted
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
            m_scriptName = string.Empty;
            m_stack = new Array<uint>();
            m_localVariables = new Array<uint>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            m_pNextScript = buf.ReadUInt32();
            m_pPrevScript = buf.ReadUInt32();
            m_scriptName = buf.ReadString(Limits.MaxNameLength);
            m_ip = buf.ReadUInt32();
            m_stack = buf.ReadArray<uint>(GetMaxStackDepth(fmt));
            m_stackPointer = buf.ReadUInt16();
            buf.Align4Bytes();
            m_localVariables = buf.ReadArray<uint>(Limits.NumberOfLocalVariables);
            m_timerA = buf.ReadUInt32();
            m_timerB = buf.ReadUInt32();
            m_condResult = buf.ReadBool();
            m_isMissionScript = buf.ReadBool();
            m_skipWakeTime = buf.ReadBool();
            buf.Align4Bytes();
            m_wakeTime = buf.ReadUInt32();
            m_andOrState = buf.ReadUInt16();
            m_notFlag = buf.ReadBool();
            m_deathArrestEnabled = buf.ReadBool();
            m_deathArrestExecuted = buf.ReadBool();
            m_missionFlag = buf.ReadBool();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(m_pNextScript);
            buf.Write(m_pPrevScript);
            buf.Write(m_scriptName, Limits.MaxNameLength);
            buf.Write(m_ip);
            buf.Write(m_stack.ToArray(), GetMaxStackDepth(fmt));
            buf.Write(m_stackPointer);
            buf.Align4Bytes();
            buf.Write(m_localVariables.ToArray(), Limits.NumberOfLocalVariables);
            buf.Write(m_timerA);
            buf.Write(m_timerB);
            buf.Write(m_condResult);
            buf.Write(m_isMissionScript);
            buf.Write(m_skipWakeTime);
            buf.Align4Bytes();
            buf.Write(m_wakeTime);
            buf.Write(m_andOrState);
            buf.Write(m_notFlag);
            buf.Write(m_deathArrestEnabled);
            buf.Write(m_deathArrestExecuted);
            buf.Write(m_missionFlag);
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

            return m_pNextScript.Equals(other.m_pNextScript)
                && m_pPrevScript.Equals(other.m_pPrevScript)
                && m_scriptName.Equals(other.m_scriptName)
                && m_ip.Equals(other.m_ip)
                && m_stack.SequenceEqual(other.m_stack)
                && m_stackPointer.Equals(other.m_stackPointer)
                && m_localVariables.SequenceEqual(other.m_localVariables)
                && m_timerA.Equals(other.m_timerA)
                && m_timerB.Equals(other.m_timerB)
                && m_condResult.Equals(other.m_condResult)
                && m_isMissionScript.Equals(other.m_isMissionScript)
                && m_skipWakeTime.Equals(other.m_skipWakeTime)
                && m_wakeTime.Equals(other.m_wakeTime)
                && m_andOrState.Equals(other.m_andOrState)
                && m_notFlag.Equals(other.m_notFlag)
                && m_deathArrestEnabled.Equals(other.m_deathArrestEnabled)
                && m_deathArrestExecuted.Equals(other.m_deathArrestExecuted)
                && m_missionFlag.Equals(other.m_missionFlag);
        }

        public static int GetMaxStackDepth(DataFormat fmt)
        {
            return (fmt.IsSupportedOnPS2)
                ? Limits.MaxStackDepthPS2
                : Limits.MaxStackDepth;
        }
    }
}
