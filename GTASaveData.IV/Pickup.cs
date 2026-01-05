using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.IV
{
    public enum ObjectType : short
    {
        // weapons
        w_nstick = 3,               // probably not used as pickup
        w_bat = 4,
        w_cue = 5,                  // probably not used as pickup
        w_knife = 6,
        w_grenade = 7,
        w_molotov = 8,
        cj_rpg_rocket = 9,          // probably not used as pickup
        w_glock = 10,
        w_eagle = 11,
        w_pumpshot = 12,
        w_shotgun = 13,
        w_uzi = 14,
        w_mp5 = 15,
        w_ak47 = 16,
        w_m4 = 17,
        w_psg1 = 18,
        w_rifle = 19,
        rpg = 20,

        // pickups
        cj_cash_pile_1 = 0x83B,     // cash pickup
        cj_first_aid_pickup = 0x84D,// health pickup
        ec_bpjacket = 0x8AF,        // body armor pickup
        cj_pigeon_05 = 0x8DC,       // flying rat
        cj_lib_pickup = 0xC56,      // happiness is...

        // episodes
        tlad_cj_seagull = 0x1AF,
        tbogt_cj_seagull = 0x574,
    }

    public enum PickupType : byte
    {
        PickupOnStreet = 2,
        PickupOnce = 3,
        PickupOnStreetSlow = 15,
        PickupMoneyNoDisappear = 19,
    }

    public class Pickup : SaveDataObject,
        IEquatable<Pickup>, IDeepClonable<Pickup>
    {
        public const int SizeOfPickup = 0x54;

        private int m_index;
        private int m_unknown04h;
        private int m_unknown08h;
        private int m_amount;
        private int m_unknown10h;
        private int m_unknown14h;
        private int blip;
        private uint timer;
        private Vector3 m_position;
        private int m_unknown2Ch;
        private int m_unknown30h;
        private Vector3 m_unknown34h;
        private int m_unknown40h;
        private ObjectType m_objectId;
        private short m_refNum;
        private PickupType m_pickupType;
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

        public Vector3 Position
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

        public Vector3 Unknown34h
        {
            get { return m_unknown34h; }
            set { m_unknown34h = value; OnPropertyChanged(); }
        }

        public int Unknown40h
        {
            get { return m_unknown40h; }
            set { m_unknown40h = value; OnPropertyChanged(); }
        }

        public ObjectType ObjectId
        {
            get { return m_objectId; }
            set { m_objectId = value; OnPropertyChanged(); }
        }

        public short RefNum
        {
            get { return m_refNum; }
            set { m_refNum = value; OnPropertyChanged(); }
        }

        public PickupType PickupType
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
        { }

        public Pickup(Pickup other)
        {
            Index = other.Index;
            Unknown04h = other.Unknown04h;
            Unknown08h = other.Unknown08h;
            Amount = other.Amount;
            Unknown10h = other.Unknown10h;
            Unknown14h = other.Unknown14h;
            Blip = other.Blip;
            Timer = other.Timer;
            Position = other.Position;
            Unknown2Ch = other.Unknown2Ch;
            Unknown30h = other.Unknown30h;
            Unknown34h = other.Unknown34h;
            Unknown40h = other.Unknown40h;
            ObjectId = other.ObjectId;
            RefNum = other.RefNum;
            PickupType = other.PickupType;
            Flags = other.Flags;
            Flags2 = other.Flags2;
            Unknown4Bh = other.Unknown4Bh;
            Unknown4Ch = other.Unknown4Ch;
            Unknown50h = other.Unknown50h;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            Index = buf.ReadInt32();
            Unknown04h = buf.ReadInt32();
            Unknown08h = buf.ReadInt32();
            Amount = buf.ReadInt32();
            Unknown10h = buf.ReadInt32();
            Unknown14h = buf.ReadInt32();
            Blip = buf.ReadStruct<int>();
            Timer = buf.ReadUInt32();
            Position = buf.ReadStruct<Vector3>();
            Unknown2Ch = buf.ReadInt32();
            Unknown30h = buf.ReadInt32();
            Unknown34h = buf.ReadStruct<Vector3>();
            Unknown40h = buf.ReadInt32();
            ObjectId = buf.ReadStruct<ObjectType>();
            RefNum = buf.ReadInt16();
            PickupType = buf.ReadStruct<PickupType>();
            Flags = buf.ReadByte();
            Flags2 = buf.ReadByte();
            Unknown4Bh = buf.ReadByte();
            Unknown4Ch = buf.ReadInt32();
            Unknown50h = buf.ReadInt32();
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
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
            buf.Write(Position.X);
            buf.Write(Position.Y);
            buf.Write(Position.Z);
            buf.Write(Unknown2Ch);
            buf.Write(Unknown30h);
            buf.Write(Unknown34h);
            buf.Write(Unknown40h);
            buf.Write(ObjectId);
            buf.Write(RefNum);
            buf.Write(PickupType);
            buf.Write(Flags);
            buf.Write(Flags2);
            buf.Write(Unknown4Bh);
            buf.Write(Unknown4Ch);
            buf.Write(Unknown50h);
        }

        protected override int GetSize(SerializationParams p)
        {
            return SizeOfPickup;
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
                && Unknown34h.Equals(other.Unknown34h)
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

        public Pickup DeepClone()
        {
            return new Pickup(this);
        }

#if DEBUG
        public override string ToString()
        {
            return $"{Index}, {PickupType}, {ObjectId}, {Amount}, {Position}";
        }
#endif
    }
}
