using GTASaveData.Interfaces;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace GTASaveData.LCS
{
    public class StoredCar : SaveDataObject, IStoredCar,
        IEquatable<StoredCar>, IDeepClonable<StoredCar>
    {
        private int m_model;
        private Vector3D m_position;
        private float m_heading;
        private float m_pitch;
        private float m_traction;
        private StoredCarFlags m_flags;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;

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

        public float Heading
        {
            get { return m_heading; }
            set { m_heading = value; OnPropertyChanged(); }
        }

        public float Pitch
        {
            get { return m_pitch; }
            set { m_pitch = value; OnPropertyChanged(); }
        }

        public float Traction
        {
            get { return m_traction; }
            set { m_traction = value; OnPropertyChanged(); }
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
            Position = new Vector3D();
        }

        public StoredCar(StoredCar other)
        {
            Model = other.Model;
            Position = new Vector3D(other.Position);
            Heading = other.Heading;
            Pitch = other.Pitch;
            Traction = other.Traction;
            Flags = other.Flags;
            Color1 = other.Color1;
            Color2 = other.Color2;
            Radio = other.Radio;
            Extra1 = other.Extra1;
            Extra2 = other.Extra2;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.Read<Vector3D>();
            float headingX = buf.ReadFloat();
            float headingY = buf.ReadFloat();
            float heading = (float) RadToDeg(Math.Atan2(headingY, headingX)) - 90;
            if (heading < 0) heading += 360;
            Heading = heading;
            Pitch = (float) RadToDeg(Math.Atan(buf.ReadFloat()));
            Traction = buf.ReadFloat();
            Flags = (StoredCarFlags) buf.ReadInt32();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            Radio = (RadioStation) buf.ReadSByte();
            Extra1 = buf.ReadSByte();
            Extra2 = buf.ReadSByte();
            buf.Skip(3);

            Debug.Assert(buf.Offset == SizeOfType<StoredCar>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write((float) Math.Cos(DegToRad(Heading + 90)));
            buf.Write((float) Math.Sin(DegToRad(Heading + 90)));
            buf.Write((float) Math.Tan(DegToRad(Pitch)));
            buf.Write(Traction);
            buf.Write((int) Flags);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write((sbyte) Radio);
            buf.Write(Extra1);
            buf.Write(Extra2);
            buf.Skip(3);

            Debug.Assert(buf.Offset == SizeOfType<StoredCar>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x2C;
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
                && Math.Abs(Heading - other.Heading) < 0.001
                && Math.Abs(Pitch - other.Pitch) < 0.001
                && Traction.Equals(other.Traction)
                && Flags.Equals(other.Flags)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && Radio.Equals(other.Radio)
                && Extra1.Equals(other.Extra1)
                && Extra2.Equals(other.Extra2);
        }

        public StoredCar DeepClone()
        {
            return new StoredCar(this);
        }

        private static double RadToDeg(double r)
        {
            return r * (180 / Math.PI);
        }

        private static double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }

    [Flags]
    public enum StoredCarFlags
    {
        [Description("Bullet Proof")]
        BulletProof     = 0b_00000000_00000001,     // Won't take bullet damage

        [Description("Fire Proof")]
        FireProof       = 0b_00000000_00000010,     // Won't take fire damage

        [Description("Explosion Proof")]
        ExplosionProof  = 0b_00000000_00000100,     // Won't take explosion damage

        [Description("Collision Proof")]
        CollisionProof  = 0b_00000000_00001000,     // Won't take collision damage

        [Description("Melee Proof (broken)")]
        MeleeProof      = 0b_00000000_00010000,     // Won't take melee damage (doesn't work)

        [Description("Pop Proof")]
        PopProof        = 0b_00000000_00100000,     // Tires won't pop

        [Description("Strong")]
        Strong          = 0b_00000000_01000000,     // Car can take about 4x more collision damage

        [Description("Heavy")]
        Heavy           = 0b_00000000_10000000,     // Car has higher mass

        [Description("Permanent Color")]
        PermanentColor  = 0b_00000001_00000000,     // Can't be painted a different color

        [Description("Time Bomb")]
        TimeBomb        = 0b_00000010_00000000,     // Fitted with a time bomb

        [Description("Tip Proof")]
        TipProof        = 0b_00000100_00000000,     // Won't explode if tipped over

        [Description("Marked")]
        Marked          = 0b_10000000_00000000,     // Marked for mission; game won't delete if left stranded
    }

    public enum BombType
    {
        [Description("(none)")]
        None,

        [Description("Timer")]
        Timer,

        [Description("Ignition")]
        Ignition,

        [Description("Remote")]
        Remote,

        [Description("Timer (armed)")]
        TimerArmed,

        [Description("Ignition (armed)")]
        IgnitionArmed
    }

    public enum RadioStation
    {
        [Description("Head Radio")]
        HeadRadio,

        [Description("Double Clef FM")]
        DoubleClefFM,

        [Description("K-Jah")]
        KJah,

        [Description("Rise FM")]
        RiseFM,

        [Description("Lips 106")]
        Lips106,

        [Description("Radio Del Mundo")]
        RadioDelMundo,

        [Description("MSX 98")]
        Msx98,

        [Description("Flashback FM")]
        FlashbackFM,

        [Description("The Liberty Jam")]
        TheLibertyJam,

        [Description("LCFR")]
        LCFR,

        [Description("Mix Tape")]
        MixTape,

        [Description("(radio off)")]
        None
    }
}
