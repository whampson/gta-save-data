using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(16)]
    public class StaticReplacement : SaveDataObject, IEquatable<StaticReplacement>
    {
        private ObjectType m_type;
        private int m_staticIndex;
        private int m_newModelIndex;
        private int m_oldModelIndex;

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

        public int NewModelIndex
        {
            get { return m_newModelIndex; }
            set { m_newModelIndex = value; OnPropertyChanged(); }
        }

        public int OldModelIndex
        {
            get { return m_oldModelIndex; }
            set { m_oldModelIndex = value; OnPropertyChanged(); }
        }

        public StaticReplacement()
        {
            Type = ObjectType.None;
            StaticIndex = 0;
            NewModelIndex = -1;
            OldModelIndex = -1;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            Type = (ObjectType) buf.ReadInt32();
            StaticIndex = buf.ReadInt32();
            NewModelIndex = buf.ReadInt32();
            OldModelIndex = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<StaticReplacement>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(StaticIndex);
            buf.Write(NewModelIndex);
            buf.Write(OldModelIndex);

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

            return Type.Equals(other.Type)
                && StaticIndex.Equals(other.StaticIndex)
                && NewModelIndex.Equals(other.NewModelIndex)
                && OldModelIndex.Equals(other.OldModelIndex);
        }
    }
}
