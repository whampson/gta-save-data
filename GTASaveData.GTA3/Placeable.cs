using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Placeable : SaveDataObject, IEquatable<Placeable>
    {
        // Almost none of this is editable
        private int m_pVtbl;
        private Vector m_right;
        private int m_flags;
        private Vector m_up;
        private Vector m_at;
        private Vector m_pos;
        private int m_pAttachment;
        private bool m_hasAttachment;
        private int[] m_unknown0;

        public Vector Position
        {
            get { return m_pos; }
            set { m_pos = value; OnPropertyChanged(); }
        }

        public Placeable()
        {
            m_right = new Vector();
            m_up = new Vector();
            m_at = new Vector();
            m_pos = new Vector();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            bool ps2 = fmt.PS2;
            bool ps2jp = GTA3Save.IsJapanesePS2(fmt);

            if (!ps2)
            {
                m_pVtbl = buf.ReadInt32();
            }

            // RwMatrix
            m_right = buf.Read<Vector>();
            m_flags = buf.ReadInt32();
            m_up = buf.Read<Vector>();
            buf.Skip(4);
            m_at = buf.Read<Vector>();
            buf.Skip(4);
            Position = buf.Read<Vector>();
            buf.Skip(4);

            if (!ps2jp)
            {
                m_pAttachment = buf.ReadInt32();
                m_hasAttachment = buf.ReadBool();
                buf.Align4Bytes();
            }
            if (ps2 && !ps2jp)
            {
                // no idea what this is
                m_unknown0 = buf.Read<int>(6);
            }

            Debug.Assert(buf.Offset == SizeOf<Placeable>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            bool ps2 = fmt.PS2;
            bool ps2jp = GTA3Save.IsJapanesePS2(fmt);

            if (!ps2)
            {
                buf.Write(m_pVtbl);
            }

            // RwMatrix
            buf.Write(m_right);
            buf.Write(m_flags);
            buf.Write(m_up);
            buf.Skip(4);
            buf.Write(m_at);
            buf.Skip(4);
            buf.Write(Position);
            buf.Skip(4);

            if (!ps2jp)
            {
                buf.Write(m_pAttachment);
                buf.Write(m_hasAttachment);
                buf.Align4Bytes();
            }
            if (ps2 && !ps2jp)
            {
                // no idea what this is
                buf.Write(m_unknown0);
            }

            Debug.Assert(buf.Offset == SizeOf<Placeable>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (GTA3Save.IsJapanesePS2(fmt))
            {
                return 0x40;
            }
            else if (fmt.PS2)
            {
                return 0x60;
            }

            return 0x4C;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Placeable);
        }

        public bool Equals(Placeable other)
        {
            if (other == null)
            {
                return false;
            }

            return Position.Equals(other.Position);
        }
    }
}
