using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class PhysicalObject : Entity,
        IEquatable<PhysicalObject>, IDeepClonable<PhysicalObject>
    {
        private short m_modelIndex;
        private int m_handle;
        private float m_uprootLimit;
        private ViewMatrix m_objectMatrix;
        private ObjectCreatedBy m_createdBy;
        private bool m_isPickup;
        private bool m_isPickupInShop;
        private bool m_isPickupOutOfStock;
        private bool m_isGlassCracked;
        private bool m_isGlassBroken;
        private bool m_hasBeenDamaged;
        private bool m_useCarColors;
        private float m_collisionDamageMultiplier;
        private byte m_collisionDamageEffect;
        private byte m_specialCollisionResponseCases;
        private uint m_endOfLifeTime;

        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public float UprootLimit
        {
            get { return m_uprootLimit; }
            set { m_uprootLimit = value; OnPropertyChanged(); }
        }

        public ViewMatrix ObjectMatrix
        {
            get { return m_objectMatrix; }
            set { m_objectMatrix = value; OnPropertyChanged(); }
        }

        public ObjectCreatedBy CreatedBy
        {
            get { return m_createdBy; }
            set { m_createdBy = value; OnPropertyChanged(); }
        }

        public bool IsPickup
        {
            get { return m_isPickup; }
            set { m_isPickup = value; OnPropertyChanged(); }
        }

        public bool IsPickupInShop
        {
            get { return m_isPickupInShop; }
            set { m_isPickupInShop = value; OnPropertyChanged(); }
        }

        public bool IsPickupOutOfStock
        {
            get { return m_isPickupOutOfStock; }
            set { m_isPickupOutOfStock = value; OnPropertyChanged(); }
        }

        public bool IsGlassCracked
        {
            get { return m_isGlassCracked; }
            set { m_isGlassCracked = value; OnPropertyChanged(); }
        }

        public bool IsGlassBroken
        {
            get { return m_isGlassBroken; }
            set { m_isGlassBroken = value; OnPropertyChanged(); }
        }

        public bool HasBeenDamaged
        {
            get { return m_hasBeenDamaged; }
            set { m_hasBeenDamaged = value; OnPropertyChanged(); }
        }

        public bool UseCarColors
        {
            get { return m_useCarColors; }
            set { m_useCarColors = value; OnPropertyChanged(); }
        }

        public float CollisionDamageMultiplier
        {
            get { return m_collisionDamageMultiplier; }
            set { m_collisionDamageMultiplier = value; OnPropertyChanged(); }
        }

        public byte CollisionDamageEffect
        {
            get { return m_collisionDamageEffect; }
            set { m_collisionDamageEffect = value; OnPropertyChanged(); }
        }

        public byte SpecialCollisionResponseCases
        {
            get { return m_specialCollisionResponseCases; }
            set { m_specialCollisionResponseCases = value; OnPropertyChanged(); }
        }

        public uint EndOfLifeTime
        {
            get { return m_endOfLifeTime; }
            set { m_endOfLifeTime = value; OnPropertyChanged(); }
        }

        public PhysicalObject()
        {
            ObjectMatrix = ViewMatrix.Identity;
            EntityType = EntityType.Object;
        }

        public PhysicalObject(PhysicalObject other)
            : base(other)
        {
            ModelIndex = other.ModelIndex;
            Handle = other.Handle;
            UprootLimit = other.UprootLimit;
            ObjectMatrix = other.ObjectMatrix;
            CreatedBy = other.CreatedBy;
            IsPickup = other.IsPickup;
            IsPickupInShop = other.IsPickupInShop;
            IsPickupOutOfStock = other.IsPickupOutOfStock;
            IsGlassCracked = other.IsGlassCracked;
            IsGlassBroken = other.IsGlassBroken;
            HasBeenDamaged = other.HasBeenDamaged;
            UseCarColors = other.UseCarColors;
            CollisionDamageMultiplier = other.CollisionDamageMultiplier;
            CollisionDamageEffect = other.CollisionDamageEffect;
            SpecialCollisionResponseCases = other.SpecialCollisionResponseCases;
            EndOfLifeTime = other.EndOfLifeTime;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            ModelIndex = buf.ReadInt16();
            Handle = buf.ReadInt32();
            Matrix = buf.Read<CompressedViewMatrix>(fmt).Decompress();
            if ((fmt.IsPS2 && fmt.IsJapanese) || !fmt.IsPS2) buf.Skip(4);
            UprootLimit = buf.ReadFloat();
            ObjectMatrix = buf.Read<CompressedViewMatrix>(fmt).Decompress();
            if ((fmt.IsPS2 && fmt.IsJapanese) || !fmt.IsPS2) buf.Skip(4);
            CreatedBy = (ObjectCreatedBy) buf.ReadByte();
            IsPickup = buf.ReadBool();
            IsPickupInShop = buf.ReadBool();
            IsPickupOutOfStock = buf.ReadBool();
            IsGlassCracked = buf.ReadBool();
            IsGlassBroken = buf.ReadBool();
            HasBeenDamaged = buf.ReadBool();
            UseCarColors = buf.ReadBool();
            CollisionDamageMultiplier = buf.ReadFloat();
            CollisionDamageEffect = buf.ReadByte();
            SpecialCollisionResponseCases = buf.ReadByte();
            EndOfLifeTime = buf.ReadUInt32();
            LoadEntityFlags(buf, fmt);

            Debug.Assert(buf.Offset == SizeOfType<PhysicalObject>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(ModelIndex);
            buf.Write(Handle);
            buf.Write(Matrix.Compress(), fmt);
            if ((fmt.IsPS2 && fmt.IsJapanese) || !fmt.IsPS2) buf.Skip(4);
            buf.Write(UprootLimit);
            buf.Write(ObjectMatrix.Compress(), fmt);
            if ((fmt.IsPS2 && fmt.IsJapanese) || !fmt.IsPS2) buf.Skip(4);
            buf.Write((byte) CreatedBy);
            buf.Write(IsPickup);
            buf.Write(IsPickupInShop);
            buf.Write(IsPickupOutOfStock);
            buf.Write(IsGlassCracked);
            buf.Write(IsGlassBroken);
            buf.Write(HasBeenDamaged);
            buf.Write(UseCarColors);
            buf.Write(CollisionDamageMultiplier);
            buf.Write(CollisionDamageEffect);
            buf.Write(SpecialCollisionResponseCases);
            buf.Write(EndOfLifeTime);
            SaveEntityFlags(buf, fmt);

            Debug.Assert(buf.Offset == SizeOfType<PhysicalObject>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2 && !fmt.IsJapanese) return 0x4C;
            if (fmt.IsiOS) return 0x52;
            return 0x54;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhysicalObject);
        }

        public bool Equals(PhysicalObject other)
        {
            if (other == null)
            {
                return false;
            }

            return ModelIndex.Equals(other.ModelIndex)
                && Handle.Equals(other.Handle)
                && Matrix.Equals(other.Matrix)
                && UprootLimit.Equals(other.UprootLimit)
                && ObjectMatrix.Equals(other.ObjectMatrix)
                && CreatedBy.Equals(other.CreatedBy)
                && IsPickup.Equals(other.IsPickup)
                && IsPickupInShop.Equals(other.IsPickupInShop)
                && IsPickupOutOfStock.Equals(other.IsPickupOutOfStock)
                && IsGlassCracked.Equals(other.IsGlassCracked)
                && IsGlassBroken.Equals(other.IsGlassBroken)
                && HasBeenDamaged.Equals(other.HasBeenDamaged)
                && UseCarColors.Equals(other.UseCarColors)
                && CollisionDamageMultiplier.Equals(other.CollisionDamageMultiplier)
                && CollisionDamageEffect.Equals(other.CollisionDamageEffect)
                && SpecialCollisionResponseCases.Equals(other.SpecialCollisionResponseCases)
                && EndOfLifeTime.Equals(other.EndOfLifeTime)
                && EntityType.Equals(other.EntityType)
                && EntityStatus.Equals(other.EntityStatus)
                && EntityFlags.Equals(other.EntityFlags);
        }

        public PhysicalObject DeepClone()
        {
            return new PhysicalObject(this);
        }
    }

    public enum ObjectCreatedBy
    {
        Unknown,
        Game,
        Mission,
        Temp,
        Cutscene
    }
}
