using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class Thread : SerializableObject,
        IEquatable<Thread>
    {
        public static class Limits
        {
            public const int NameLength = 8;
            public const int StackSize = 6;
            public const int StackSizePS2 = 4;
            public const int LocalVariablesCount = 16;
        }

        private uint m_nextScript;
        private uint m_prevScript;
        private string m_name;
        private uint m_instructionPointer;
        private Array<uint> m_stack;
        private ushort m_stackPointer;
        private Array<uint> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private bool m_ifResult;
        private bool m_isMissionScript;
        private bool m_isActive;
        private uint m_wakeTime;
        private ushort m_ifNumber;
        private bool m_notFlag;
        private bool m_isWastedOrBustedCheckEnabled;
        private bool m_isWastedOrBusted;
        private bool m_isMission;

        public uint NextScript
        {
            get { return m_nextScript; }
            set { m_nextScript = value; OnPropertyChanged(); }
        }

        public uint PrevScript
        {
            get { return m_prevScript; }
            set { m_prevScript = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public uint InstructionPointer
        {
            get { return m_instructionPointer; }
            set { m_instructionPointer = value; OnPropertyChanged(); }
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

        public bool IfResult
        {
            get { return m_ifResult; }
            set { m_ifResult = value; OnPropertyChanged(); }
        }

        public bool IsMissionScript
        {
            get { return m_isMissionScript; }
            set { m_isMissionScript = value; OnPropertyChanged(); }
        }

        public bool IsActive
        {
            get { return m_isActive; }
            set { m_isActive = value; OnPropertyChanged(); }
        }

        public uint WakeTime
        {
            get { return m_wakeTime; }
            set { m_wakeTime = value; OnPropertyChanged(); }
        }

        public ushort IfNumber
        {
            get { return m_ifNumber; }
            set { m_ifNumber = value; OnPropertyChanged(); }
        }

        public bool NotFlag
        {
            get { return m_notFlag; }
            set { m_notFlag = value; OnPropertyChanged(); }
        }

        public bool IsWastedOrBustedCheckEnabled
        {
            get { return m_isWastedOrBustedCheckEnabled; }
            set { m_isWastedOrBustedCheckEnabled = value; OnPropertyChanged(); }
        }

        public bool IsWastedOrBusted
        {
            get { return m_isWastedOrBusted; }
            set { m_isWastedOrBusted = value; OnPropertyChanged(); }
        }

        public bool IsMission
        {
            get { return m_isMission; }
            set { m_isMission = value; OnPropertyChanged(); }
        }

        public Thread()
        {
            m_name = string.Empty;
            m_stack = new Array<uint>();
            m_localVariables = new Array<uint>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            int stackSize = fmt.SupportsPS2
                ? Limits.StackSizePS2
                : Limits.StackSize;

            m_nextScript = r.ReadUInt32();
            m_prevScript = r.ReadUInt32();
            m_name = r.ReadString(Limits.NameLength);
            m_instructionPointer = r.ReadUInt32();
            m_stack = r.ReadArray<uint>(stackSize);
            m_stackPointer = r.ReadUInt16();
            r.Align();
            m_localVariables = r.ReadArray<uint>(Limits.LocalVariablesCount);
            m_timerA = r.ReadUInt32();
            m_timerB = r.ReadUInt32();
            m_ifResult = r.ReadBool();
            m_isMissionScript = r.ReadBool();
            m_isActive = r.ReadBool();
            r.Align();
            m_wakeTime = r.ReadUInt32();
            m_ifNumber = r.ReadUInt16();
            m_notFlag = r.ReadBool();
            m_isWastedOrBustedCheckEnabled = r.ReadBool();
            m_isWastedOrBusted = r.ReadBool();
            m_isMission = r.ReadBool();
            r.Align();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            int stackSize = fmt.SupportsPS2
                ? Limits.StackSizePS2
                : Limits.StackSize;

            w.Write(m_nextScript);
            w.Write(m_prevScript);
            w.Write(m_name, Limits.NameLength);
            w.Write(m_instructionPointer);
            w.Write(m_stack.ToArray(), stackSize);
            w.Write(m_stackPointer);
            w.Align();
            w.Write(m_localVariables.ToArray(), Limits.LocalVariablesCount);
            w.Write(m_timerA);
            w.Write(m_timerB);
            w.Write(m_ifResult);
            w.Write(m_isMissionScript);
            w.Write(m_isActive);
            w.Align();
            w.Write(m_wakeTime);
            w.Write(m_ifNumber);
            w.Write(m_notFlag);
            w.Write(m_isWastedOrBustedCheckEnabled);
            w.Write(m_isWastedOrBusted);
            w.Write(m_isMission);
            w.Align();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Thread);
        }

        public bool Equals(Thread other)
        {
            if (other == null)
            {
                return false;
            }

            return m_nextScript.Equals(other.m_nextScript)
                && m_prevScript.Equals(other.m_prevScript)
                && m_name.Equals(other.m_name)
                && m_instructionPointer.Equals(other.m_instructionPointer)
                && m_stack.SequenceEqual(other.m_stack)
                && m_stackPointer.Equals(other.m_stackPointer)
                && m_localVariables.SequenceEqual(other.m_localVariables)
                && m_timerA.Equals(other.m_timerA)
                && m_timerB.Equals(other.m_timerB)
                && m_ifResult.Equals(other.m_ifResult)
                && m_isMissionScript.Equals(other.m_isMissionScript)
                && m_isActive.Equals(other.m_isActive)
                && m_wakeTime.Equals(other.m_wakeTime)
                && m_ifNumber.Equals(other.m_ifNumber)
                && m_notFlag.Equals(other.m_notFlag)
                && m_isWastedOrBustedCheckEnabled.Equals(other.m_isWastedOrBustedCheckEnabled)
                && m_isWastedOrBusted.Equals(other.m_isWastedOrBusted)
                && m_isMission.Equals(other.m_isMission);
        }
    }
}
