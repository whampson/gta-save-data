using System;

namespace GTASaveData.GTA3
{
    public sealed class BuildingSwap : SaveDataObject,
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
            m_type = ObjectType.Static;
            m_staticIndex = 0;
            m_newModelId = -1;
            m_oldModelId = -1;
        }

        private BuildingSwap(SaveDataSerializer serializer, SystemType system)
            : base(serializer, system)
        { }

        protected override void ReadObjectData(SaveDataSerializer serializer, SystemType system)
        {
            m_type = (ObjectType) serializer.ReadInt32();
            m_staticIndex = serializer.ReadInt32();
            m_newModelId = serializer.ReadInt32();
            m_oldModelId = serializer.ReadInt32();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, SystemType system)
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

            return m_type.Equals(other.m_type)
                && m_staticIndex.Equals(other.m_staticIndex)
                && m_newModelId.Equals(other.m_newModelId)
                && m_oldModelId.Equals(other.m_oldModelId);
        }
    }
}
