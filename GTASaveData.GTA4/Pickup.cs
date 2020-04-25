using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA4
{
    [Size(0x54)]
    public class Pickup : SaveDataObject, IEquatable<Pickup>
    {
        private int m_index;
        private int m_unknown04h;
        private int m_unknown08h;
        private int m_amount;
        private int m_unknown10h;
        private int m_unknown14h;
        private int blip;
        private uint timer;
        private Vector3D m_position;
        private int m_unknown2Ch;
        private int m_unknown30h;
        private Vector3D m_rotation;  // maybe
        private int m_unknown40h;
        private short m_objectId;
        private short m_refNum;
        private byte m_pickupType;   // TODO: enum
        private byte m_flags;
        private byte m_flags2;
        private byte m_unknown4Bh;
        private int m_unknown4Ch;
        private int m_unknown50h;

        public int Index
        { 
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public int Unknown04h
        { 
            get { return m_unknown04h; }
            set { m_unknown04h = value; OnPropertyChanged(); }
        }

        public int Unknown08h
        { 
            get { return m_unknown08h; }
            set { m_unknown08h = value; OnPropertyChanged(); }
        }

        public int Amount
        { 
            get { return m_amount; }
            set { m_amount = value; OnPropertyChanged(); }
        }

        public int Unknown10h
        { 
            get { return m_unknown10h; }
            set { m_unknown10h = value; OnPropertyChanged(); }
        }

        public int Unknown14h
        { 
            get { return m_unknown14h; }
            set { m_unknown14h = value; OnPropertyChanged(); }
        }

        public int Blip
        { 
            get { return blip; }
            set { blip = value; OnPropertyChanged(); }
        }

        public uint Timer
        { 
            get { return timer; }
            set { timer = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        { 
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public int Unknown2Ch
        { 
            get { return m_unknown2Ch; }
            set { m_unknown2Ch = value; OnPropertyChanged(); }
        }

        public int Unknown30h
        { 
            get { return m_unknown30h; }
            set { m_unknown30h = value; OnPropertyChanged(); }
        }

        public Vector3D Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; OnPropertyChanged(); }
        }

        public int Unknown40h
        { 
            get { return m_unknown40h; }
            set { m_unknown40h = value; OnPropertyChanged(); }
        }

        public short ObjectId
        { 
            get { return m_objectId; }
            set { m_objectId = value; OnPropertyChanged(); }
        }

        public short RefNum
        { 
            get { return m_refNum; }
            set { m_refNum = value; OnPropertyChanged(); }
        }

        public byte PickupType
        { 
            get { return m_pickupType; }
            set { m_pickupType = value; OnPropertyChanged(); }
        }

        public byte Flags
        { 
            get { return m_flags; }
            set { m_flags = value; OnPropertyChanged(); }
        }

        public byte Flags2
        { 
            get { return m_flags2; }
            set { m_flags2 = value; OnPropertyChanged(); }
        }

        public byte Unknown4Bh
        {
            get { return m_unknown4Bh; }
            set { m_unknown4Bh = value; OnPropertyChanged(); }
        }

        public int Unknown4Ch
        {
            get { return m_unknown4Ch; }
            set { m_unknown4Ch = value; OnPropertyChanged(); }
        }

        public int Unknown50h
        { 
            get { return m_unknown50h; }
            set { m_unknown50h = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            Position = new Vector3D();
            Rotation = new Vector3D();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Index = buf.ReadInt32();
            Unknown04h = buf.ReadInt32();
            Unknown08h = buf.ReadInt32();
            Amount = buf.ReadInt32();
            Unknown10h = buf.ReadInt32();
            Unknown14h = buf.ReadInt32();
            Blip = buf.ReadInt32();
            Timer = buf.ReadUInt32();
            Position = buf.Read<Vector3D>();
            Unknown2Ch = buf.ReadInt32();
            Unknown30h = buf.ReadInt32();
            Rotation = buf.Read<Vector3D>();
            Unknown40h = buf.ReadInt32();
            ObjectId = buf.ReadInt16();
            RefNum = buf.ReadInt16();
            PickupType = buf.ReadByte();
            Flags = buf.ReadByte();
            Flags2 = buf.ReadByte();
            Unknown4Bh = buf.ReadByte();
            Unknown4Ch = buf.ReadInt32();
            Unknown50h = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Pickup>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Index);
            buf.Write(Unknown04h);
            buf.Write(Unknown08h);
            buf.Write(Amount);
            buf.Write(Unknown10h);
            buf.Write(Unknown14h);
            buf.Write(Blip);
            buf.Write(Timer);
            buf.Write(Position);
            buf.Write(Unknown2Ch);
            buf.Write(Unknown30h);
            buf.Write(Rotation);
            buf.Write(Unknown40h);
            buf.Write(ObjectId);
            buf.Write(RefNum);
            buf.Write(PickupType);
            buf.Write(Flags);
            buf.Write(Flags2);
            buf.Write(Unknown4Bh);
            buf.Write(Unknown4Ch);
            buf.Write(Unknown50h);

            Debug.Assert(buf.Offset == SizeOf<Pickup>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pickup);
        }

        public bool Equals(Pickup other)
        {
            if (other == null)
            {
                return false;
            }

            return Index.Equals(other.Index)
                && Unknown04h.Equals(other.Unknown04h)
                && Unknown08h.Equals(other.Unknown08h)
                && Amount.Equals(other.Amount)
                && Unknown10h.Equals(other.Unknown10h)
                && Unknown14h.Equals(other.Unknown14h)
                && Blip.Equals(other.Blip)
                && Timer.Equals(other.Timer)
                && Position.Equals(other.Position)
                && Unknown2Ch.Equals(other.Unknown2Ch)
                && Unknown30h.Equals(other.Unknown30h)
                && Rotation.Equals(other.Rotation)
                && Unknown40h.Equals(other.Unknown40h)
                && ObjectId.Equals(other.ObjectId)
                && RefNum.Equals(other.RefNum)
                && PickupType.Equals(other.PickupType)
                && Flags.Equals(other.Flags)
                && Flags2.Equals(other.Flags2)
                && Unknown4Bh.Equals(other.Unknown4Bh)
                && Unknown4Ch.Equals(other.Unknown4Ch)
                && Unknown50h.Equals(other.Unknown50h);
        }
    }
}
