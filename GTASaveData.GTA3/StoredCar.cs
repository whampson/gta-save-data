using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x28)]
    public class StoredCar : SerializableObject,
        IEquatable<StoredCar>
    {
        private VehicleType m_model;
        private Vector m_vecPos;
        private Vector m_vecAngle;
        private StoredCarFlags m_flags;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;
        private BombType m_carBombType;

        public VehicleType Model
        {
            get { return m_model; }
            set { m_model = value; OnPropertyChanged(); }
        }

        public Vector Position
        {
            get { return m_vecPos; }
            set { m_vecPos = value; OnPropertyChanged(); }
        }

        public Vector Angle
        {
            get { return m_vecAngle; }
            set { m_vecAngle = value; OnPropertyChanged(); }
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
            get { return m_carBombType; }
            set { m_carBombType = value; OnPropertyChanged(); }
        }


        public StoredCar()
        {
            m_vecPos = new Vector();
            m_vecAngle = new Vector();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_model = (VehicleType) r.ReadInt32();
            m_vecPos = r.ReadObject<Vector>();
            m_vecAngle = r.ReadObject<Vector>();
            m_flags = (StoredCarFlags) r.ReadInt32();
            m_color1 = r.ReadByte();
            m_color2 = r.ReadByte();
            m_radio = (RadioStation) r.ReadByte();
            m_extra1 = r.ReadSByte();
            m_extra2 = r.ReadSByte();
            m_carBombType = (BombType) r.ReadByte();
            r.Align();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<StoredCar>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_model);
            w.Write(m_vecPos);
            w.Write(m_vecAngle);
            w.Write((int) m_flags);
            w.Write(m_color1);
            w.Write(m_color2);
            w.Write((byte) m_radio);
            w.Write(m_extra1);
            w.Write(m_extra2);
            w.Write((byte) m_carBombType);
            w.Align();

            Debug.Assert(w.Position() - w.Marked() == SizeOf<StoredCar>());
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

            return m_model.Equals(other.m_model)
                && m_vecPos.Equals(other.m_vecPos)
                && m_vecAngle.Equals(other.m_vecAngle)
                && m_flags.Equals(other.m_flags)
                && m_color1.Equals(other.m_color1)
                && m_color2.Equals(other.m_color2)
                && m_radio.Equals(other.m_radio)
                && m_extra1.Equals(other.m_extra1)
                && m_extra2.Equals(other.m_extra2)
                && m_carBombType.Equals(other.m_carBombType);
        }
    }
}
