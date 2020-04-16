using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(16)]
    public class BuildingSwap : SaveDataObject, IEquatable<BuildingSwap>
    {
        private ObjectType m_type;
        private int m_handle;
        private int m_newModel;
        private int m_oldModel;

        public ObjectType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public int NewModel
        {
            get { return m_newModel; }
            set { m_newModel = value; OnPropertyChanged(); }
        }

        public int OldModel
        {
            get { return m_oldModel; }
            set { m_oldModel = value; OnPropertyChanged(); }
        }

        public BuildingSwap()
        {
            Type = ObjectType.None;
            Handle = 0;
            NewModel = -1;
            OldModel = -1;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Type = (ObjectType) buf.ReadInt32();
            Handle = buf.ReadInt32();
            NewModel = buf.ReadInt32();
            OldModel = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<BuildingSwap>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
            buf.Write(NewModel);
            buf.Write(OldModel);

            Debug.Assert(buf.Offset == SizeOf<BuildingSwap>());
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

            return Type.Equals(other.Type)
                && Handle.Equals(other.Handle)
                && NewModel.Equals(other.NewModel)
                && OldModel.Equals(other.OldModel);
        }
    }
}
