using GTASaveData.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    /// <summary>
    /// Data structure containing building model swap info.
    /// </summary>
    public class BuildingSwap : SaveDataObject,
        IEquatable<BuildingSwap>, IDeepClonable<BuildingSwap>
    {
        private EntityClassType m_type;      // can be Treadable or Building
        private int m_handle;
        private int m_newModel;
        private int m_oldModel;

        /// <summary>
        /// Entity class type. Controls which entity pool the <see cref="BuildingSwap"/>
        /// goes into when loaded into the game.
        /// </summary>
        /// <remarks>
        /// Can be <see cref="EntityClassType.Treadable"/> or <see cref="EntityClassType.Building"/>.
        /// </remarks>
        public EntityClassType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Entity handle; pool index + 1.
        /// </summary>
        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// New (current) building model index.
        /// </summary>
        public int NewModel
        {
            get { return m_newModel; }
            set { m_newModel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Old building model index.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether this building swap is valid.
        /// </summary>
        public bool IsUsed()
        {
            return Handle != 0;
        }

        /// <summary>
        /// Invalidates this building swap.
        /// </summary>
        public void Clear()
        {
            Type = EntityClassType.None;
            Handle = 0;
            OldModel = -1;
            NewModel = -1;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            Type = (EntityClassType) buf.ReadInt32();
            Handle = buf.ReadInt32();
            NewModel = buf.ReadInt32();
            OldModel = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<BuildingSwap>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
            buf.Write(NewModel);
            buf.Write(OldModel);

            Debug.Assert(buf.Offset == SizeOf<BuildingSwap>());
        }

        protected override int GetSize(SerializationParams p)
        {
            return sizeof(int)
                + sizeof(int)
                + sizeof(int)
                + sizeof(int);
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

    public enum EntityClassType
    {
        None,

        /// <summary>CTreadable</summary>
        Treadable,

        /// <summary>CBuilding</summary>
        Building,

        /// <summary>CObject</summary>
        Object,

        /// <summary>CDummy</summary>
        Dummy
    }
}
