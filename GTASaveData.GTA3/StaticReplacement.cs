using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(16)]
    public class StaticReplacement : SerializableObject,
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

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_type = (ObjectType) r.ReadInt32();
            m_staticIndex = r.ReadInt32();
            m_newModelId = r.ReadInt32();
            m_oldModelId = r.ReadInt32();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<StaticReplacement>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_type);
            w.Write(m_staticIndex);
            w.Write(m_newModelId);
            w.Write(m_oldModelId);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<StaticReplacement>());
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
