using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Boat : Vehicle, IEquatable<Boat>
    {
        // This class sucks, nothing is editable :-(
        // But I will keep it for completeness.

        public Boat()
        : this(0, 0)
        { }

        public Boat(short model, int handle)
            : base(VehicleType.Boat, model, handle)
        { }

        public Boat(Boat other)
            : base(other)
        { }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            base.ReadData(buf, fmt);

            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Boat>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            base.WriteData(buf, fmt);

            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Boat>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsMobile)
            {
                return 0x488;
            }
            if (fmt.IsPS2 && fmt.IsJapanese)
            {
                return 0x478;
            }
            if (fmt.IsPS2)
            {
                return 0x498;
            }
            if (fmt.IsPC || fmt.IsXbox)
            {
                return 0x484;
            }

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
    }
}
