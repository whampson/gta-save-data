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

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            base.ReadData(buf, fmt);
            buf.Skip(SizeOfType<Boat>(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Boat>(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            base.WriteData(buf, fmt);
            buf.Skip(SizeOfType<Boat>(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Boat>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2 && fmt.IsJapanese) return 0x50C;
            if (fmt.IsPC || fmt.IsXbox) return 0x484;
            if (fmt.IsMobile) return 0x488;
            if (fmt.IsPS2) return 0x52C;
            throw SizeNotDefined(fmt);
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
