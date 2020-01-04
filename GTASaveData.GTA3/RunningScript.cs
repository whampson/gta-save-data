using GTASaveData.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTASaveData.GTA3
{
    public sealed class RunningScript : SaveDataObject,
        IEquatable<RunningScript>
    {
        public static class Limits
        {
            public const int NameLength = 6;
            public const int StackSize = 6;
            public const int StackSizePS2 = 4;
            public const int LocalVariablesCount = 16;
        }

        private uint m_nextScript;
        private uint m_prevScript;
        private string m_name;
        private uint m_instructionPointer;
        private ObservableCollection<uint> m_stack;
        private ushort m_stackPointer;
        private ObservableCollection<uint> m_localVariables;
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

        public ObservableCollection<uint> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        public ushort StackPointer
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

        public ObservableCollection<uint> LocalVariables
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

        public RunningScript()
        {
            m_name = string.Empty;
            m_stack = new ObservableCollection<uint>();
            m_localVariables = new ObservableCollection<uint>();
        }

        private RunningScript(SaveDataSerializer serializer, FileFormat format)
        {
            int stackSize = format.IsPS2
                ? Limits.StackSizePS2
                : Limits.StackSize;

            m_nextScript = serializer.ReadUInt32();
            m_prevScript = serializer.ReadUInt32();
            m_name = serializer.ReadString(Limits.NameLength);
            m_instructionPointer = serializer.ReadUInt32();
            m_stack = new ObservableCollection<uint>(serializer.ReadArray<uint>(stackSize));
            m_stackPointer = serializer.ReadUInt16();
            serializer.Align();
            m_localVariables = new ObservableCollection<uint>(serializer.ReadArray<uint>(Limits.LocalVariablesCount));
            m_timerA = serializer.ReadUInt32();
            m_timerB = serializer.ReadUInt32();
            m_ifResult = serializer.ReadBool();
            m_isMissionScript = serializer.ReadBool();
            m_isActive = serializer.ReadBool();
            serializer.Align();
            m_wakeTime = serializer.ReadUInt32();
            m_ifNumber = serializer.ReadUInt16();
            m_notFlag = serializer.ReadBool();
            m_isWastedOrBustedCheckEnabled = serializer.ReadBool();
            m_isWastedOrBusted = serializer.ReadBool();
            m_isMission = serializer.ReadBool();
            serializer.Align();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            int stackSize = format.IsPS2
                ? Limits.StackSizePS2
                : Limits.StackSize;

            serializer.Write(m_nextScript);
            serializer.Write(m_prevScript);
            serializer.Write(m_name, Limits.NameLength);
            serializer.Write(m_instructionPointer);
            serializer.WriteArray(m_stack, stackSize);
            serializer.Write(m_stackPointer);
            serializer.Align();
            serializer.WriteArray(m_localVariables, Limits.LocalVariablesCount);
            serializer.Write(m_timerA);
            serializer.Write(m_timerB);
            serializer.Write(m_ifResult);
            serializer.Write(m_isMissionScript);
            serializer.Write(m_isActive);
            serializer.Align();
            serializer.Write(m_wakeTime);
            serializer.Write(m_ifNumber);
            serializer.Write(m_notFlag);
            serializer.Write(m_isWastedOrBustedCheckEnabled);
            serializer.Write(m_isWastedOrBusted);
            serializer.Write(m_isMission);
            serializer.Align();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
