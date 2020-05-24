using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Boat : Vehicle, IEquatable<Boat>
    {
        // This class sucks, nothing is editable :-(
        // But I will keep it for completeness.

        public Boat()
        : this(0, -1)
        { }

        public Boat(short model, int handle)
            : base(VehicleType.Boat, model, handle)
        { }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.ReadObjectData(buf, fmt);
            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Boat>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.WriteObjectData(buf, fmt);
            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Boat>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (fmt.Mobile)
            {
                return 0x488;
            }
            if (GTA3Save.IsJapanesePS2(fmt))
            {
                return 0x478;
            }
            if (fmt.PS2)
            {
                return 0x498;
            }
            if (fmt.PC || fmt.Xbox)
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

            return true;    // lol
        }
    }
}
