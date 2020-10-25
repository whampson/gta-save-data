using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class StoredCar : SaveDataObject, IStoredCar,
        IEquatable<StoredCar>, IDeepClonable<StoredCar>
    {
        private int m_model;
        private Vector3 m_position;
        private Vector3 m_angle;
        private StoredCarFlags m_flags;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;
        private BombType m_bomb;

        public int Model
        {
            get { return m_model; }
            set { m_model = value; OnPropertyChanged(); }
        }

        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Vector3 Angle
        {
            get { return m_angle; }
            set { m_angle = value; OnPropertyChanged(); }
        }

        public StoredCarFlags Flags
        {
            get { return m_flags; }
            set { m_flags = value; OnPropertyChanged(); }
        }

        public byte Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public byte Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public RadioStation Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        public sbyte Extra1
        {
            get { return m_extra1; }
            set { m_extra1 = value; OnPropertyChanged(); }
        }

        public sbyte Extra2
        {
            get { return m_extra2; }
            set { m_extra2 = value; OnPropertyChanged(); }
        }

        public BombType Bomb
        {
            get { return m_bomb; }
            set { m_bomb = value; OnPropertyChanged(); }
        }

        int IStoredCar.Flags
        {
            get { return (int) Flags; }
            set { Flags = (StoredCarFlags) value; OnPropertyChanged(); }
        }

        int IStoredCar.Color1
        {
            get { return Color1; }
            set { Color1 = (byte) value; OnPropertyChanged(); }
        }

        int IStoredCar.Color2
        {
            get { return Color2; }
            set { Color2 = (byte) value; OnPropertyChanged(); }
        }

        int IStoredCar.Radio
        {
            get { return (int) Radio; }
            set { Radio = (RadioStation) value; OnPropertyChanged(); }
        }

        int IStoredCar.Extra1
        {
            get { return Extra1; }
            set { Extra1 = (sbyte) value; OnPropertyChanged(); }
        }

        int IStoredCar.Extra2
        {
            get { return Extra2; }
            set { Extra2 = (sbyte) value; OnPropertyChanged(); }
        }

        public StoredCar()
        {
            Position = new Vector3();
            Angle = new Vector3();
        }

        public StoredCar(StoredCar other)
        {
            Model = other.Model;
            Position = other.Position;
            Angle = other.Angle;
            Flags = other.Flags;
            Color1 = other.Color1;
            Color2 = other.Color2;
            Radio = other.Radio;
            Extra1 = other.Extra1;
            Extra2 = other.Extra2;
            Bomb = other.Bomb;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.ReadStruct<Vector3>();
            Angle = buf.ReadStruct<Vector3>();
            Flags = (StoredCarFlags) buf.ReadInt32();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            Radio = (RadioStation) buf.ReadSByte();
            Extra1 = buf.ReadSByte();
            Extra2 = buf.ReadSByte();
            Bomb = (BombType) buf.ReadSByte();
            buf.Skip(2);

            Debug.Assert(buf.Offset == SizeOfType<StoredCar>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write(Angle);
            buf.Write((int) Flags);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write((sbyte) Radio);
            buf.Write(Extra1);
            buf.Write(Extra2);
            buf.Write((sbyte) Bomb);
            buf.Skip(2);

            Debug.Assert(buf.Offset == SizeOfType<StoredCar>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x28;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredCar);
        }

        public bool Equals(StoredCar other)
        {
            if (other == null)
            {
                return false;
            }

            return Model.Equals(other.Model)
                && Position.Equals(other.Position)
                && Angle.Equals(other.Angle)
                && Flags.Equals(other.Flags)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && Radio.Equals(other.Radio)
                && Extra1.Equals(other.Extra1)
                && Extra2.Equals(other.Extra2)
                && Bomb.Equals(other.Bomb);
        }

        public StoredCar DeepClone()
        {
            return new StoredCar(this);
        }
    }

    [Flags]
    public enum StoredCarFlags
    {
        BulletProof     = 0b00001,
        FireProof       = 0b00010,
        ExplosionProof  = 0b00100,
        CollisionProof  = 0b01000,
        MeleeProof      = 0b10000
    }

    public enum BombType
    {
        None,
        Timer,
        Ignition,
        Remote,
        TimerArmed,
        IgnitionArmed
    }

    public enum RadioStation
    {
        HeadRadio,
        DoubleCleff,
        JahRadio,
        RiseFM,
        Lips106,
        GameFM,
        MsxFM,
        Flashback,
        Chatterbox,
        UserTrack,
        PoliceRadio,
        None
    }
}
