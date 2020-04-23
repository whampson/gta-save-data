using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x34)]
    public class Phone : SaveDataObject, IEquatable<Phone>
    {
        public static class Limits
        {
            public const int MaxNumMessages = 6;
        }

        private Vector m_position;
        private Array<uint> m_messages;     // wchar pointers
        private uint m_repeatedMessageStartTime;
        private int m_handle;
        private PhoneState m_state;
        private bool m_visibleToCam;

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Array<uint> Messages
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
            Position = new Vector();
            Messages = new Array<uint>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Position = buf.Read<Vector>();
            Messages = buf.ReadArray<uint>(Limits.MaxNumMessages);
            RepeatedMessageStartTime = buf.ReadUInt32();
            Handle = buf.ReadInt32();
            State = (PhoneState) buf.ReadUInt32();
            VisibleToCam = buf.ReadBool();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<Phone>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Position);
            buf.Write(Messages.ToArray(), Limits.MaxNumMessages);
            buf.Write(RepeatedMessageStartTime);
            buf.Write(Handle);
            buf.Write((int) State);
            buf.Write(VisibleToCam);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<Phone>());
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
    }
}
