using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class BuildingSwap : SaveDataObject,
        IEquatable<BuildingSwap>, IDeepClonable<BuildingSwap>
    {
        private PoolType m_type;
        private int m_handle;
        private int m_newModel;
        private int m_oldModel;

        public PoolType Type
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
            NewModel = -1;
            OldModel = -1;
        }

        public BuildingSwap(BuildingSwap other)
        {
            Type = other.Type;
            Handle = other.Handle;
            NewModel = other.NewModel;
            OldModel = other.OldModel;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Type = (PoolType) buf.ReadInt32();
            Handle = buf.ReadInt32();
            NewModel = buf.ReadInt32();
            OldModel = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<BuildingSwap>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
            buf.Write(NewModel);
            buf.Write(OldModel);

            Debug.Assert(buf.Offset == SizeOfType<BuildingSwap>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 16;
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

        public BuildingSwap DeepClone()
        {
            return new BuildingSwap(this);
        }
    }
}
