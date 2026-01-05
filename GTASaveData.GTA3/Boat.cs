using System;
using System.Diagnostics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Boat : Vehicle,
        IEquatable<Boat>, IDeepClonable<Boat>
    {
        // This class sucks, nothing is editable :-(
        // But I will keep it for completeness.

        public Boat()
        : this(0, 0)
        { }

        public Boat(short model)
            : this(model, 0)
        { }

        public Boat(short model, int handle)
            : base(VehicleType.Boat, model, handle)
        { }

        public Boat(Boat other)
            : base(other)
        { }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            base.ReadData(buf, prm);
            buf.Skip(SizeOf<Boat>(prm) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Boat>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            base.WriteData(buf, prm);
            buf.Skip(SizeOf<Boat>(prm) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Boat>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            if (p.IsPC || p.IsXbox) return 0x484;
            if (p.IsMobile) return 0x488;
            if (p.IsPS2JP) return 0x50C;
            if (p.IsPS2) return 0x52C;
            throw SizeNotDefined(p);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Boat);
        }

        public bool Equals(Boat other)
        {
            if (other == null)
            {
                return false;
            }

            return base.Equals(other);
        }

        public Boat DeepClone()
        {
            return new Boat(this);
        }
    }
}
