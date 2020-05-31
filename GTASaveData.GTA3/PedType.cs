using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class PedType : SaveDataObject, IEquatable<PedType>
    {
        private PedTypeFlags m_flag;
        private float m_unknown0;
        private float m_unknown1;
        private float m_unknown2;
        private float m_unknown3;
        private float m_unknown4;
        private PedTypeFlags m_threats;
        private PedTypeFlags m_avoid;

        public PedTypeFlags Flag
        {
            get { return m_flag; }
            set { m_flag = value; OnPropertyChanged(); }
        }

        public float Unknown0
        {
            get { return m_unknown0; }
            set { m_unknown0 = value; OnPropertyChanged(); }
        }

        public float Unknown1
        {
            get { return m_unknown1; }
            set { m_unknown1 = value; OnPropertyChanged(); }
        }

        public float Unknown2
        {
            get { return m_unknown2; }
            set { m_unknown2 = value; OnPropertyChanged(); }
        }

        public float Unknown3
        {
            get { return m_unknown3; }
            set { m_unknown3 = value; OnPropertyChanged(); }
        }

        public float Unknown4
        {
            get { return m_unknown4; }
            set { m_unknown4 = value; OnPropertyChanged(); }
        }

        public PedTypeFlags Threats
        {
            get { return m_threats; }
            set { m_threats = value; OnPropertyChanged(); }
        }

        public PedTypeFlags Avoid
        {
            get { return m_avoid; }
            set { m_avoid = value; OnPropertyChanged(); }
        }

        public PedType()
        { }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Flag = (PedTypeFlags) buf.ReadInt32();
            Unknown0 = buf.ReadFloat();
            Unknown1 = buf.ReadFloat();
            Unknown2 = buf.ReadFloat();
            Unknown3 = buf.ReadFloat();
            Unknown4 = buf.ReadFloat();
            Threats = (PedTypeFlags) buf.ReadInt32();
            Avoid = (PedTypeFlags) buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<PedType>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write((int) Flag);
            buf.Write(Unknown0);
            buf.Write(Unknown1);
            buf.Write(Unknown2);
            buf.Write(Unknown3);
            buf.Write(Unknown4);
            buf.Write((int) Threats);
            buf.Write((int) Avoid);

            Debug.Assert(buf.Offset == SizeOfType<PedType>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 32;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PedType);
        }

        public bool Equals(PedType other)
        {
            if (other == null)
            {
                return false;
            }

            return Flag.Equals(other.Flag)
                && Unknown0.Equals(other.Unknown0)
                && Unknown1.Equals(other.Unknown1)
                && Unknown2.Equals(other.Unknown2)
                && Unknown3.Equals(other.Unknown3)
                && Unknown4.Equals(other.Unknown4)
                && Threats.Equals(other.Threats)
                && Avoid.Equals(other.Avoid);
        }
    }

    [Flags]
    public enum PedTypeFlags
    {
        Player1 = (1 << 0),
        Player2 = (1 << 1),
        Player3 = (1 << 2),
        Player4 = (1 << 3),
        CivMale = (1 << 4),
        CivFemale = (1 << 5),
        Cop = (1 << 6),
        Gang1 = (1 << 7),
        Gang2 = (1 << 8),
        Gang3 = (1 << 9),
        Gang4 = (1 << 10),
        Gang5 = (1 << 11),
        Gang6 = (1 << 12),
        Gang7 = (1 << 13),
        Gang8 = (1 << 14),
        Gang9 = (1 << 15),
        Emergency = (1 << 16),
        Prostitute = (1 << 17),
        Criminal = (1 << 18),
        Special = (1 << 19),
        Gun = (1 << 20),
        CopCar = (1 << 21),
        FastCar = (1 << 22),
        Explosion = (1 << 23),
        Fireman = (1 << 24),
        DeadPeds = (1 << 25),
    }

    public enum PedTypeId
    {
        Player1,
        Player2,
        Player3,
        Player4,
        CivMale,
        CivFemale,
        Cop,
        Gang1,
        Gang2,
        Gang3,
        Gang4,
        Gang5,
        Gang6,
        Gang7,
        Gang8,
        Gang9,
        Emergency,
        Fireman,
        Criminal,
        Prostitute = 20,
        Special,
    }
}
