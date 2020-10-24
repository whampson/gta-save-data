using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VCS
{
    public class RunningScript : SaveDataObject, IRunningScript,
        IEquatable<RunningScript>, IDeepClonable<RunningScript>
    {
        public const int MaxNameLength = 8;
        public const int MaxStackDepth = 16;
        public const int NumLocalVariables = 103;       // hmmm... 103 is a weird number

        private uint m_pNextScript;
        private uint m_pPrevScript;
        private int m_id;
        private int m_unknown10h;
        private int m_ip;
        private Array<int> m_stack;
        private short m_stackPointer;
        private Array<int> m_localVariables;
        private uint m_timerA;
        private uint m_timerB;
        private int m_unknown1FCh;
        private uint m_wakeTime;
        private int m_unknown204h;
        private int m_unknown208h;
        private byte m_unknown20Ch;
        private byte m_unknown20Dh;
        private byte m_unknown20Eh;
        private string m_name;
        private byte m_unknown217h;

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

        public int Field10h
        {
            get { return m_unknown10h; }
            set { m_unknown10h = value; OnPropertyChanged(); }
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

        public int Field1FCh
        {
            get { return m_unknown1FCh; }
            set { m_unknown1FCh = value; OnPropertyChanged(); }
        }

        public uint WakeTime
        {
            get { return m_wakeTime; }
            set { m_wakeTime = value; OnPropertyChanged(); }
        }

        public int Field204h
        {
            get { return m_unknown204h; }
            set { m_unknown204h = value; OnPropertyChanged(); }
        }

        public int Field208h
        {
            get { return m_unknown208h; }
            set { m_unknown208h = value; OnPropertyChanged(); }
        }

        public byte Field20Ch
        {
            get { return m_unknown20Ch; }
            set { m_unknown20Ch = value; OnPropertyChanged(); }
        }

        public byte Field20Dh
        {
            get { return m_unknown20Dh; }
            set { m_unknown20Dh = value; OnPropertyChanged(); }
        }

        public byte Field20Eh
        {
            get { return m_unknown20Eh; }
            set { m_unknown20Eh = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public byte Field217h
        {
            get { return m_unknown217h; }
            set { m_unknown217h = value; OnPropertyChanged(); }
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
            Field10h = other.Field10h;
            IP = other.IP;
            Stack = ArrayHelper.DeepClone(other.Stack);
            StackPosition = other.StackPosition;
            LocalVariables = ArrayHelper.DeepClone(other.LocalVariables);
            TimerA = other.TimerA;
            TimerB = other.TimerB;
            Field1FCh = other.Field1FCh;
            WakeTime = other.WakeTime;
            Field204h = other.Field204h;
            Field208h = other.Field208h;
            Field20Ch = other.Field20Ch;
            Field20Dh = other.Field20Dh;
            Field20Eh = other.Field20Eh;
            Name = other.Name;
            Field217h = other.Field217h;
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

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            NextScriptPointer = buf.ReadUInt32();
            PrevScriptPointer = buf.ReadUInt32();
            Id = buf.ReadInt32();
            Field10h = buf.ReadInt32();
            IP = buf.ReadInt32();
            Stack = buf.ReadArray<int>(MaxStackDepth);
            StackPosition = buf.ReadInt16();
            buf.Skip(2);
            LocalVariables = buf.ReadArray<int>(NumLocalVariables);
            TimerA = buf.ReadUInt32();
            TimerB = buf.ReadUInt32();
            Field1FCh = buf.ReadInt32();
            WakeTime = buf.ReadUInt32();
            Field204h = buf.ReadInt32();
            Field208h = buf.ReadInt32();
            Field20Ch = buf.ReadByte();
            Field20Dh = buf.ReadByte();
            Field20Eh = buf.ReadByte();
            Name = buf.ReadString(MaxNameLength);
            Field217h = buf.ReadByte();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(NextScriptPointer);
            buf.Write(PrevScriptPointer);
            buf.Write(Id);
            buf.Write(Field10h);
            buf.Write(IP);
            buf.Write(Stack, MaxStackDepth);
            buf.Write(StackPosition);
            buf.Skip(2);
            buf.Write(LocalVariables, NumLocalVariables);
            buf.Write(TimerA);
            buf.Write(TimerB);
            buf.Write(Field1FCh);
            buf.Write(WakeTime);
            buf.Write(Field204h);
            buf.Write(Field208h);
            buf.Write(Field20Ch);
            buf.Write(Field20Dh);
            buf.Write(Field20Eh);
            buf.Write(Name, MaxNameLength);
            buf.Write(Field217h);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x218;
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
                && Field10h.Equals(other.Field10h)
                && IP.Equals(other.IP)
                && Stack.SequenceEqual(other.Stack)
                && StackPosition.Equals(other.StackPosition)
                && LocalVariables.SequenceEqual(other.LocalVariables)
                && TimerA.Equals(other.TimerA)
                && TimerB.Equals(other.TimerB)
                && Field1FCh.Equals(other.Field1FCh)
                && WakeTime.Equals(other.WakeTime)
                && Field204h.Equals(other.Field204h)
                && Field208h.Equals(other.Field208h)
                && Field20Ch.Equals(other.Field20Ch)
                && Field20Dh.Equals(other.Field20Dh)
                && Field20Eh.Equals(other.Field20Eh)
                && Name.Equals(other.Name)
                && Field217h.Equals(other.Field217h);
        }

        public RunningScript DeepClone()
        {
            return new RunningScript(this);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
