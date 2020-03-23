using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x28)]
    public class StoredCar : SaveDataObject, IEquatable<StoredCar>
    {
        private int m_modelIndex;
        private Vector m_vecPos;
        private Vector m_vecAngle;
        private StoredCarFlags m_flags;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;
        private BombType m_carBombType;

        public int ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
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

        public BombType CarBombType
        {
            get { return m_carBombType; }
            set { m_carBombType = value; OnPropertyChanged(); }
        }

        public StoredCar()
        {
            Position = new Vector();
            Angle = new Vector();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            ModelIndex = buf.ReadInt32();
            Position = buf.ReadObject<Vector>();
            Angle = buf.ReadObject<Vector>();
            Flags = (StoredCarFlags) buf.ReadInt32();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            Radio = (RadioStation) buf.ReadByte();
            Extra1 = buf.ReadSByte();
            Extra2 = buf.ReadSByte();
            CarBombType = (BombType) buf.ReadByte();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<StoredCar>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(ModelIndex);
            buf.Write(Position);
            buf.Write(Angle);
            buf.Write((int) Flags);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write((byte) Radio);
            buf.Write(Extra1);
            buf.Write(Extra2);
            buf.Write((byte) CarBombType);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == SizeOf<StoredCar>());
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

            return ModelIndex.Equals(other.ModelIndex)
                && Position.Equals(other.Position)
                && Angle.Equals(other.Angle)
                && Flags.Equals(other.Flags)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && Radio.Equals(other.Radio)
                && Extra1.Equals(other.Extra1)
                && Extra2.Equals(other.Extra2)
                && CarBombType.Equals(other.CarBombType);
        }
    }
}
