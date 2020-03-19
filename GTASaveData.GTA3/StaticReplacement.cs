using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(16)]
    public class StaticReplacement : GTAObject,
        IEquatable<StaticReplacement>
    {
        private ObjectType m_type;
        private int m_staticIndex;
        private int m_newModelId;
        private int m_oldModelId;

        public ObjectType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int StaticIndex
        {
            get { return m_staticIndex; }
            set { m_staticIndex = value; OnPropertyChanged(); }
        }

        public int NewModelId
        {
            get { return m_newModelId; }
            set { m_newModelId = value; OnPropertyChanged(); }
        }

        public int OldModelId
        {
            get { return m_oldModelId; }
            set { m_oldModelId = value; OnPropertyChanged(); }
        }

        public StaticReplacement()
        {
            m_type = ObjectType.None;
            m_staticIndex = 0;
            m_newModelId = -1;
            m_oldModelId = -1;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_type = (ObjectType) buf.ReadInt32();
            m_staticIndex = buf.ReadInt32();
            m_newModelId = buf.ReadInt32();
            m_oldModelId = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<StaticReplacement>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((int) m_type);
            buf.Write(m_staticIndex);
            buf.Write(m_newModelId);
            buf.Write(m_oldModelId);

            Debug.Assert(buf.Offset == SizeOf<StaticReplacement>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StaticReplacement);
        }

        public bool Equals(StaticReplacement other)
        {
            if (other == null)
            {
                return false;
            }

            return m_type.Equals(other.m_type)
                && m_staticIndex.Equals(other.m_staticIndex)
                && m_newModelId.Equals(other.m_newModelId)
                && m_oldModelId.Equals(other.m_oldModelId);
        }
    }
}
