using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// The saved state of a running mission thread.
    /// </summary>
    public class RunningScript : SaveDataObject,
        IEquatable<RunningScript>, IDeepClonable<RunningScript>
    {
        public const int MaxNameLength = 8;

        private uint m_pNextScript; // not loaded
        private uint m_pPrevScript; // not loaded
        private string m_name;
        private int m_ip;
        private ObservableArray<int> m_stack;
        private short m_stackPointer;
        private ObservableArray<int> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private bool m_condResult;
        private bool m_isMissionScript;
        private bool m_skipWakeTime;
        private uint m_wakeTime;
        private short m_andOrState;
        private bool m_notFlag;
        private bool m_deathArrestEnabled;
        private bool m_deathArrestExecuted;
        private bool m_missionFlag;

        /// <summary>
        /// Pointer to the next CRunningScript in the list.
        /// </summary>
        /// <remarks>
        /// Useless value. Game always re-creates the list and does not use
        /// old pointers.
        /// </remarks>
        public uint NextScriptPointer
        {
            get { return m_pNextScript; }
            set { m_pNextScript = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Pointer to the previous CRunningScript in the list.
        /// </summary>
        /// <remarks>
        /// Useless value. Game always re-creates the list and does not use
        /// old pointers.
        /// </remarks>
        public uint PrevScriptPointer
        {
            get { return m_pPrevScript; }
            set { m_pPrevScript = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Script name, as set by opcode 03A4. Length: 8 (NUL-terminated)
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Current instruction pointer.
        /// </summary>
        public int IP
        {
            get { return m_ip; }
            set { m_ip = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Return address stack.
        /// </summary>
        public ObservableArray<int> Stack
        {
            get { return m_stack; }
            set { m_stack = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Current stack pointer.
        /// </summary>
        public short StackIndex
        {
            get { return m_stackPointer; }
            set { m_stackPointer = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Local variables.
        /// </summary>
        public ObservableArray<int> Locals
        {
            get { return m_localVariables; }
            set { m_localVariables = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Timer #1.
        /// </summary>
        public uint TimerA
        {
            get { return m_timerA; }
            set { m_timerA = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Timer #2.
        /// </summary>
        public uint TimerB
        {
            get { return m_timerB; }
            set { m_timerB = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Comparison flag.
        /// </summary>
        public bool CompareFlag
        {
            get { return m_condResult; }
            set { m_condResult = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indiciates that this is a mission script.
        /// </summary>
        public bool IsMissionScript
        {
            get { return m_isMissionScript; }
            set { m_isMissionScript = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// When set, bypasses the wake timer, effectively breaking
        /// the script from a wait loop.
        /// </summary>
        public bool SkipWakeTime
        {
            get { return m_skipWakeTime; }
            set { m_skipWakeTime = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The wakeup timer. The script will resume execution after
        /// the global timer has passed this value, otherwise the
        /// script is considered to be in a wait state.
        /// </summary>
        public uint WakeTime
        {
            get { return m_wakeTime; }
            set { m_wakeTime = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The state of the current if..and or if..or chain.
        /// 0 = none, 1-8 = and state, 21-28 = or state.
        /// </summary>
        public short AndOrState
        {
            get { return m_andOrState; }
            set { m_andOrState = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Negated instruction flag. True if the MSB of the current opcode
        /// is set.
        /// </summary>
        public bool NotFlag
        {
            get { return m_notFlag; }
            set { m_notFlag = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Sets whether the Wasted/Busted check is enabled. Only valid if
        /// <see cref="IsMissionScript"/> is true and <c>$ONMISSION = 1</c>.
        /// </summary>
        public bool DeathArrestEnabled
        {
            get { return m_deathArrestEnabled; }
            set { m_deathArrestEnabled = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Set if the Wasted/Busted check result was true (player is Wasted or Busted).
        /// </summary>
        public bool DeathArrestExecuted
        {
            get { return m_deathArrestExecuted; }
            set { m_deathArrestExecuted = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Similar to <see cref="IsMissionScript"/>. Not sure why R* used two
        /// mission flags. Best to keep this in-line with <see cref="IsMissionScript"/>.
        /// </summary>
        public bool MissionFlag
        {
            get { return m_missionFlag; }
            set { m_missionFlag = value; OnPropertyChanged(); }
        }

        public RunningScript()
        {
            Name = "noname";
            Stack = new ObservableArray<int>();
            Locals = new ObservableArray<int>();
            DeathArrestEnabled = true;
        }

        public RunningScript(RunningScript other)
        {
            NextScriptPointer = other.NextScriptPointer;
            PrevScriptPointer = other.PrevScriptPointer;
            Name = other.Name;
            IP = other.IP;
            Stack = ArrayHelper.DeepClone(other.Stack);
            StackIndex = other.StackIndex;
            Locals = ArrayHelper.DeepClone(other.Locals);
            TimerA = other.TimerA;
            TimerB = other.TimerB;
            CompareFlag = other.CompareFlag;
            IsMissionScript = other.IsMissionScript;
            SkipWakeTime = other.SkipWakeTime;
            WakeTime = other.WakeTime;
            AndOrState = other.AndOrState;
            NotFlag = other.NotFlag;
            DeathArrestEnabled = other.DeathArrestEnabled;
            DeathArrestExecuted = other.DeathArrestExecuted;
            MissionFlag = other.MissionFlag;
        }

        /// <summary>
        /// Checks whether the stack is empty.
        /// </summary>
        public bool IsStackEmpty()
        {
            return StackIndex == 0;
        }

        /// <summary>
        /// Pushes a value onto the stack.
        /// </summary>
        public void PushStack(int value)
        {
            if (Stack.Count <= StackIndex)
            {
                Stack.Add(value);
            }
            else
            {
                Stack[StackIndex] = value;
            }
            StackIndex++;
        }

        /// <summary>
        /// Pops a value from the stack.
        /// </summary>
        public int PopStack()
        {
            return IsStackEmpty()
                ? throw new InvalidOperationException(Strings.Error_InvalidOperation_StackEmpty)
                : Stack[--StackIndex];
        }

        /// <summary>
        /// Gets the value on the top of the stack.
        /// </summary>
        public int PeekStack()
        {
            return IsStackEmpty()
                ? throw new InvalidOperationException(Strings.Error_InvalidOperation_StackEmpty)
                : Stack[StackIndex - 1];
        }

        /// <summary>
        /// Gets the value of a local variable.
        /// </summary>
        public int GetLocal(int index)
        {
            return Locals[index];
        }

        /// <summary>
        /// Gets the value of a local variable as a float.
        /// </summary>
        public float GetLocalAsFloat(int index)
        {
            return BitConverter.Int32BitsToSingle(Locals[index]);
        }

        /// <summary>
        /// Sets the value of a local variable.
        /// </summary>
        public void SetLocal(int index, int value)
        {
            Locals[index] = value;
        }

        /// <summary>
        /// Sets the value of a local variable as a float.
        /// </summary>
        public void SetLocal(int index, float value)
        {
            Locals[index] = BitConverter.SingleToInt32Bits(value);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;

            NextScriptPointer = buf.ReadUInt32();
            PrevScriptPointer = buf.ReadUInt32();
            Name = buf.ReadString(MaxNameLength);
            IP = buf.ReadInt32();
            Stack = buf.ReadArray<int>(p.MaxStackDepth);
            StackIndex = buf.ReadInt16();
            buf.Align4();
            Locals = buf.ReadArray<int>(p.NumLocalVariables);
            TimerA = buf.ReadUInt32();
            TimerB = buf.ReadUInt32();
            CompareFlag = buf.ReadBool();
            IsMissionScript = buf.ReadBool();
            SkipWakeTime = buf.ReadBool();
            buf.Align4();
            WakeTime = buf.ReadUInt32();
            AndOrState = buf.ReadInt16();
            NotFlag = buf.ReadBool();
            DeathArrestEnabled = buf.ReadBool();
            DeathArrestExecuted = buf.ReadBool();
            MissionFlag = buf.ReadBool();
            buf.Align4();

            Debug.Assert(buf.Offset == GetSize(p));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;

            buf.Write(NextScriptPointer);
            buf.Write(PrevScriptPointer);
            buf.Write(Name, MaxNameLength);
            buf.Write(IP);
            buf.Write(Stack, p.MaxStackDepth);
            buf.Write(StackIndex);
            buf.Align4();
            buf.Write(Locals, p.NumLocalVariables);
            buf.Write(TimerA);
            buf.Write(TimerB);
            buf.Write(CompareFlag);
            buf.Write(IsMissionScript);
            buf.Write(SkipWakeTime);
            buf.Align4();
            buf.Write(WakeTime);
            buf.Write(AndOrState);
            buf.Write(NotFlag);
            buf.Write(DeathArrestEnabled);
            buf.Write(DeathArrestExecuted);
            buf.Write(MissionFlag);
            buf.Align4();

            Debug.Assert(buf.Offset == GetSize(p));
        }

        protected override int GetSize(SerializationParams prm)
        {
            return (prm.FileType.IsPS2) ? 0x80 : 0x88;
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
                && StackIndex.Equals(other.StackIndex)
                && Locals.SequenceEqual(other.Locals)
                && TimerA.Equals(other.TimerA)
                && TimerB.Equals(other.TimerB)
                && CompareFlag.Equals(other.CompareFlag)
                && IsMissionScript.Equals(other.IsMissionScript)
                && SkipWakeTime.Equals(other.SkipWakeTime)
                && WakeTime.Equals(other.WakeTime)
                && AndOrState.Equals(other.AndOrState)
                && NotFlag.Equals(other.NotFlag)
                && DeathArrestEnabled.Equals(other.DeathArrestEnabled)
                && DeathArrestExecuted.Equals(other.DeathArrestExecuted)
                && MissionFlag.Equals(other.MissionFlag);
        }

        public RunningScript DeepClone()
        {
            return new RunningScript(this);
        }

        public override string ToString()
        {
            return $"{Name}, {IP}, Mission = {IsMissionScript}";
        }
    }
}
