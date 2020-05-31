using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Placeable : SaveDataObject, IEquatable<Placeable>
    {
        // Almost all of this gets overridden by the game
        private int m_pVtbl;
        private Vector3D m_right;
        private int m_flags;
        private Vector3D m_up;
        private Vector3D m_at;
        private Vector3D m_pos; // not this though
        private int m_pAttachment;
        private bool m_hasAttachment;
        private int[] m_unknown0;

        public Vector3D Position
        {
            get { return m_pos; }
            set { m_pos = value; OnPropertyChanged(); }
        }

        public Placeable()
        {
            m_unknown0 = new int[0];
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            bool ps2 = fmt.IsPS2;
            bool ps2jp = fmt.IsPS2 && fmt.IsJapanese;

            if (!ps2)
            {
                m_pVtbl = buf.ReadInt32();
            }

            // RwMatrix
            m_right = buf.Read<Vector3D>();
            m_flags = buf.ReadInt32();
            m_up = buf.Read<Vector3D>();
            buf.ReadInt32();
            m_at = buf.Read<Vector3D>();
            buf.ReadInt32();
            Position = buf.Read<Vector3D>();
            buf.ReadInt32();

            if (!ps2jp)
            {
                m_pAttachment = buf.ReadInt32();
                m_hasAttachment = buf.ReadBool();
                buf.ReadBytes(3);
            }
            if (ps2 && !ps2jp)
            {
                // no idea what this is
                m_unknown0 = buf.Read<int>(6);
            }

            Debug.Assert(buf.Offset == SizeOfType<Placeable>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            bool ps2 = fmt.IsPS2;
            bool ps2jp = fmt.IsPS2 && fmt.IsJapanese;

            if (!ps2)
            {
                buf.Write(m_pVtbl);
            }

            // RwMatrix
            buf.Write(m_right);
            buf.Write(m_flags);
            buf.Write(m_up);
            buf.Write(0);
            buf.Write(m_at);
            buf.Write(0);
            buf.Write(Position);
            buf.Write(0);

            if (!ps2jp)
            {
                buf.Write(m_pAttachment);
                buf.Write(m_hasAttachment);
                buf.Write(new byte[3]);
            }
            if (ps2 && !ps2jp)
            {
                // no idea what this is
                buf.Write(m_unknown0);
            }

            Debug.Assert(buf.Offset == SizeOfType<Placeable>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2 && fmt.IsJapanese)
            {
                return 0x40;
            }
            if (fmt.IsPS2)
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
