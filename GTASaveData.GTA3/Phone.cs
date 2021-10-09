using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Phone : SaveDataObject,
        IEquatable<Phone>, IDeepClonable<Phone>
    {
        public const int MaxNumMessages = 6;

        private Vector3 m_position;
        private ObservableArray<uint> m_messages;     // wchar pointers
        private uint m_repeatedMessageStartTime;
        private int m_handle;
        private PhoneState m_state;
        private bool m_visibleToCam;

        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public ObservableArray<uint> Messages
        {
            get { return m_messages; }
            set { m_messages = value; OnPropertyChanged(); }
        }

        public uint RepeatedMessageStartTime
        {
            get { return m_repeatedMessageStartTime; }
            set { m_repeatedMessageStartTime = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public PhoneState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public bool VisibleToCam
        {
            get { return m_visibleToCam; }
            set { m_visibleToCam = value; OnPropertyChanged(); }
        }

        public Phone()
        {
            Position = new Vector3();
            Messages = ArrayHelper.CreateArray<uint>(MaxNumMessages);
        }

        public Phone(Phone other)
        {
            Position = other.Position;
            Messages = ArrayHelper.DeepClone(other.Messages);
            RepeatedMessageStartTime = other.RepeatedMessageStartTime;
            Handle = other.Handle;
            State = other.State;
            VisibleToCam = other.VisibleToCam;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Position = buf.ReadStruct<Vector3>();
            Messages = buf.ReadArray<uint>(MaxNumMessages);
            RepeatedMessageStartTime = buf.ReadUInt32();
            Handle = buf.ReadInt32();
            State = (PhoneState) buf.ReadUInt32();
            VisibleToCam = buf.ReadBool();
            buf.Align4();

            Debug.Assert(buf.Offset == SizeOfType<Phone>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Position);
            buf.Write(Messages, MaxNumMessages);
            buf.Write(RepeatedMessageStartTime);
            buf.Write(Handle);
            buf.Write((int) State);
            buf.Write(VisibleToCam);
            buf.Align4();

            Debug.Assert(buf.Offset == SizeOfType<Phone>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x34;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Phone);
        }

        public bool Equals(Phone other)
        {
            if (other == null)
            {
                return false;
            }

            return Position.Equals(other.Position)
                && Messages.SequenceEqual(other.Messages)
                && RepeatedMessageStartTime.Equals(other.RepeatedMessageStartTime)
                && Handle.Equals(other.Handle)
                && State.Equals(other.State)
                && VisibleToCam.Equals(other.VisibleToCam);
        }

        public Phone DeepClone()
        {
            return new Phone(this);
        }
    }

    public enum PhoneState
    {
        Free,
        ReportingCrime,
        MessageRemoved = 3,
        OneTimeMessageSet,
        RepeatedMessageSet,
        RepeatedMessageShownOnce,
        OneTimeMessageStarted,
        RepeatedMessageStarted,
        Ringing
    }
}
