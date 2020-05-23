using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class GameObject : SaveDataObject, IEquatable<GameObject>
    {
        private short m_modelIndex;
        private int m_handle;
        private Matrix m_matrix;
        private float m_uprootLimit;
        private Matrix m_objectMatrix;
        private byte m_createdBy;   // TODO: enum
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
        private long m_entityFlags;        // TODO: enum

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

        public Matrix Matrix
        {
            get { return m_matrix; }
            set { m_matrix = value; OnPropertyChanged(); }
        }

        public float UprootLimit
        {
            get { return m_uprootLimit; }
            set { m_uprootLimit = value; OnPropertyChanged(); }
        }

        public Matrix ObjectMatrix
        {
            get { return m_objectMatrix; }
            set { m_objectMatrix = value; OnPropertyChanged(); }
        }

        public byte CreatedBy
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

        public long EntityFlags
        {
            get { return m_entityFlags; }
            set { m_entityFlags = value; OnPropertyChanged(); }
        }


        public GameObject()
        { }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            bool ios = fmt.iOS;
            bool ps2 = fmt.PS2;
            bool ps2japan = GTA3Save.IsJapanesePS2(fmt);

            ModelIndex = buf.ReadInt16();
            Handle = buf.ReadInt32();
            Matrix = buf.Read<CompressedMatrix>().Decompress();
            if (ps2japan || !ps2)
            {
                buf.Skip(4);
            }
            UprootLimit = buf.ReadFloat();
            ObjectMatrix = buf.Read<CompressedMatrix>().Decompress();
            if (ps2japan || !ps2)
            {
                buf.Skip(4);
            }
            CreatedBy = buf.ReadByte();
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
            EntityFlags = buf.ReadUInt32();
            if (ios)
            {
                EntityFlags |= ((long) buf.ReadUInt16()) << 32;
            }
            else
            {
                EntityFlags |= ((long) buf.ReadUInt32()) << 32;
            }

            Debug.Assert(buf.Offset == SizeOf<GameObject>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            bool ios = fmt.iOS;
            bool ps2 = fmt.PS2;
            bool ps2japan = GTA3Save.IsJapanesePS2(fmt);

            buf.Write(ModelIndex);
            buf.Write(Handle);
            buf.Write(Matrix.Compress());
            if (ps2japan || !ps2)
            {
                buf.Skip(4);
            }
            buf.Write(UprootLimit);
            buf.Write(ObjectMatrix.Compress());
            if (ps2japan || !ps2)
            {
                buf.Skip(4);
            }
            buf.Write(CreatedBy);
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
            buf.Write((uint) (EntityFlags & 0xFFFFFFFF));
            if (ios)
            {
                buf.Write((ushort) (EntityFlags >> 32));
            }
            else
            {
                buf.Write((uint) (EntityFlags >> 32));
            }

            Debug.Assert(buf.Offset == SizeOf<GameObject>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (fmt.PS2)        // non-Japanese
            {
                return 0x4C;
            }
            if (fmt.iOS)
            {
                return 0x52;
            }

            return 0x54;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameObject);
        }

        public bool Equals(GameObject other)
        {
            if (other == null)
            {
                return false;
            }

            return ModelIndex.Equals(ModelIndex)
                && Handle.Equals(Handle)
                && Matrix.Equals(Matrix)
                && UprootLimit.Equals(UprootLimit)
                && ObjectMatrix.Equals(ObjectMatrix)
                && CreatedBy.Equals(CreatedBy)
                && IsPickup.Equals(IsPickup)
                && IsPickupInShop.Equals(IsPickupInShop)
                && IsPickupOutOfStock.Equals(IsPickupOutOfStock)
                && IsGlassCracked.Equals(IsGlassCracked)
                && IsGlassBroken.Equals(IsGlassBroken)
                && HasBeenDamaged.Equals(HasBeenDamaged)
                && UseCarColors.Equals(UseCarColors)
                && CollisionDamageMultiplier.Equals(CollisionDamageMultiplier)
                && CollisionDamageEffect.Equals(CollisionDamageEffect)
                && SpecialCollisionResponseCases.Equals(SpecialCollisionResponseCases)
                && EndOfLifeTime.Equals(EndOfLifeTime)
                && EntityFlags.Equals(EntityFlags);
        }
    }

    public enum ObjectType
    {
        None,
        Treadable,
        Building,
        Object,
        Dummy
    }
}
