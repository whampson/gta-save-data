using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class StoredCar : SaveDataObject, IEquatable<StoredCar>
    {
        private int m_model;
        private Vector3D m_position;
        private Vector3D m_angle;
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

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Vector3D Angle
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

        public StoredCar()
        { }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.Read<Vector3D>();
            Angle = buf.Read<Vector3D>();
            Flags = (StoredCarFlags) buf.ReadInt32();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            Radio = (RadioStation) buf.ReadSByte();
            Extra1 = buf.ReadSByte();
            Extra2 = buf.ReadSByte();
            Bomb = (BombType) buf.ReadSByte();
            buf.Align4();

            Debug.Assert(buf.Offset == SizeOfType<StoredCar>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
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
            buf.Align4();

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
