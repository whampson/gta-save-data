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

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Flag = (PedTypeFlags) buf.ReadInt32();
            Unknown0 = buf.ReadFloat();
            Unknown1 = buf.ReadFloat();
            Unknown2 = buf.ReadFloat();
            Unknown3 = buf.ReadFloat();
            Unknown4 = buf.ReadFloat();
            Threats = (PedTypeFlags) buf.ReadInt32();
            Avoid = (PedTypeFlags) buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<PedType>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((int) Flag);
            buf.Write(Unknown0);
            buf.Write(Unknown1);
            buf.Write(Unknown2);
            buf.Write(Unknown3);
            buf.Write(Unknown4);
            buf.Write((int) Threats);
            buf.Write((int) Avoid);

            Debug.Assert(buf.Offset == SizeOf<PedType>());
        }

        protected override int GetSize(DataFormat fmt)
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
}
