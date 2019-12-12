using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class BuildingSwap : GTAObject,
        IEquatable<BuildingSwap>
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

        public BuildingSwap()
        {
            m_newModelId = -1;
            m_oldModelId = -1;
        }

        private BuildingSwap(Serializer serializer)
        {
            m_type = (ObjectType) serializer.ReadInt32();
            m_staticIndex = serializer.ReadInt32();
            m_newModelId = serializer.ReadInt32();
            m_oldModelId = serializer.ReadInt32();
        }

        private void Serialize(Serializer serializer)
        {
            serializer.Write((int) m_type);
            serializer.Write(m_staticIndex);
            serializer.Write(m_newModelId);
            serializer.Write(m_oldModelId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BuildingSwap);
        }

        public bool Equals(BuildingSwap other)
        {
            if (other == null)
            {
                return false;
            }

            return m_type == other.m_type
                && m_staticIndex == other.m_staticIndex
                && m_newModelId == other.m_newModelId
                && m_oldModelId == other.m_oldModelId;
        }

        public override string ToString()
        {
            return BuildToString(new (string, object)[]
            {
                (nameof(Type), Type),
                (nameof(StaticIndex), StaticIndex),
                (nameof(NewModelId), NewModelId),
                (nameof(OldModelId), OldModelId)
            });
        }
    }
}
