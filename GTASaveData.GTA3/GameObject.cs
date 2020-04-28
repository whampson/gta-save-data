using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class GameObject : SaveDataObject, IEquatable<GameObject>
    {
        // TODO: private members

        public short ModelIndex;
        public int Handle;
        //public Matrix Matrix;
        public float UprootLimit;
        //public Matrix ObjectMatrix;
        public byte CreatedBy;   // TODO: enum
        public bool IsPickup;
        public bool IsPickupInShop;
        public bool IsPickupOutOfStock;
        public bool IsGlassCracked;
        public bool IsGlassBroken;
        public bool HasBeenDamaged;
        public bool UseCarColors;
        public float CollisionDamageMultiplier;
        public byte CollisionDamageEffect;
        public byte SpecialCollisionResponseCases;
        public uint EndOfLifeTime;
        public long EntityFlags;        // TODO: enum

        public GameObject()
        {
            //Matrix = new Matrix();
            //ObjectMatrix = new Matrix();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            bool ios = fmt.iOS;
            bool ps2 = fmt.PS2;
            bool ps2Japan = GTA3Save.IsJapanesePS2(fmt);

            ModelIndex = buf.ReadInt16();
            Handle = buf.ReadInt32();
            //Matrix = buf.Read<Matrix>();
            if ((ps2 && !ps2Japan) || !ps2)
            {
                buf.ReadInt32();
            }
            UprootLimit = buf.ReadFloat();
            //ObjectMatrix = buf.Read<Matrix>();
            if ((ps2 && !ps2Japan) || !ps2)
            {
                buf.ReadInt32();
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
            EntityFlags = buf.ReadInt32();
            EntityFlags |= (ios)
                        ? ((long) buf.ReadInt16()) << 32
                        : ((long) buf.ReadInt32()) << 32;

            Debug.Assert(buf.Offset == SizeOf<GameObject>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            // TODO

            Debug.Assert(buf.Offset == SizeOf<GameObject>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            throw new NotImplementedException();
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

            return false;   // TODO
        }
    }
}
