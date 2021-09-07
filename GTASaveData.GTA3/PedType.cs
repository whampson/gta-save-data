using System;
using System.Diagnostics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class PedType : SaveDataObject,
        IEquatable<PedType>, IDeepClonable<PedType>
    {
        private PedTypeFlags m_flag;
        private float m_unknown0;
        private float m_unknown1;
        private float m_unknown2;
        private float m_unknown3;
        private float m_unknown4;
        private PedTypeFlags m_threats;
        private PedTypeFlags m_avoids;

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

        public PedTypeFlags Avoids
        {
            get { return m_avoids; }
            set { m_avoids = value; OnPropertyChanged(); }
        }

        public bool IsGang
        {
            get { return Flag >= PedTypeFlags.Gang1 && Flag <= PedTypeFlags.Gang9; }
        }

        public PedType()
        { }

        public PedType(PedType other)
        {
            Flag = other.Flag;
            Unknown0 = other.Unknown0;
            Unknown1 = other.Unknown1;
            Unknown2 = other.Unknown2;
            Unknown3 = other.Unknown3;
            Unknown4 = other.Unknown4;
            Threats = other.Threats;
            Avoids = other.Avoids;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Flag = (PedTypeFlags) buf.ReadInt32();
            Unknown0 = buf.ReadFloat();
            Unknown1 = buf.ReadFloat();
            Unknown2 = buf.ReadFloat();
            Unknown3 = buf.ReadFloat();
            Unknown4 = buf.ReadFloat();
            Threats = (PedTypeFlags) buf.ReadInt32();
            Avoids = (PedTypeFlags) buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<PedType>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write((int) Flag);
            buf.Write(Unknown0);
            buf.Write(Unknown1);
            buf.Write(Unknown2);
            buf.Write(Unknown3);
            buf.Write(Unknown4);
            buf.Write((int) Threats);
            buf.Write((int) Avoids);

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
                && Avoids.Equals(other.Avoids);
        }

        public PedType DeepClone()
        {
            return new PedType(this);
        }

        public static PedTypeFlags GetFlag(PedTypeId pedType)
        {
            switch (pedType)
            {
                case PedTypeId.Player1: return PedTypeFlags.Player1;
                case PedTypeId.Player2: return PedTypeFlags.Player2;
                case PedTypeId.Player3: return PedTypeFlags.Player3;
                case PedTypeId.Player4: return PedTypeFlags.Player4;
                case PedTypeId.CivMale: return PedTypeFlags.CivMale;
                case PedTypeId.CivFemale: return PedTypeFlags.CivFemale;
                case PedTypeId.Cop: return PedTypeFlags.Cop;
                case PedTypeId.Gang1: return PedTypeFlags.Gang1;
                case PedTypeId.Gang2: return PedTypeFlags.Gang2;
                case PedTypeId.Gang3: return PedTypeFlags.Gang3;
                case PedTypeId.Gang4: return PedTypeFlags.Gang4;
                case PedTypeId.Gang5: return PedTypeFlags.Gang5;
                case PedTypeId.Gang6: return PedTypeFlags.Gang6;
                case PedTypeId.Gang7: return PedTypeFlags.Gang7;
                case PedTypeId.Gang8: return PedTypeFlags.Gang8;
                case PedTypeId.Gang9: return PedTypeFlags.Gang9;
                case PedTypeId.Emergency: return PedTypeFlags.Emergency;
                case PedTypeId.Fireman: return PedTypeFlags.Fireman;
                case PedTypeId.Criminal: return PedTypeFlags.Criminal;
                case PedTypeId.Special: return PedTypeFlags.Special;
                case PedTypeId.Prostitute: return PedTypeFlags.Prostitute;
                case PedTypeId.Unused1: return 0;
                case PedTypeId.Unused2: return 0;
            }

            throw new InvalidOperationException("Unknown ped type " + pedType);
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
        Unused1,
        Prostitute,
        Special,
        Unused2,
    }
}
