using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Automobile : Vehicle, IEquatable<Automobile>
    {
        private DamageManager m_damage;

        public DamageManager Damage
        {
            get { return m_damage; }
            set { m_damage = value; OnPropertyChanged(); }
        }

        public Automobile()
            : this(0, 0)
        { }

        public Automobile(short model, int handle)
            : base(VehicleType.Car, model, handle)
        {
            Damage = new DamageManager();
        }

        public Automobile(Automobile other)
            : base(other)
        {
            Damage = new DamageManager(other.Damage);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            base.ReadData(buf, fmt);

            Damage = buf.Read<DamageManager>();
            buf.Skip(GetSize(fmt) - buf.Offset);    // The rest is useless

            Debug.Assert(buf.Offset == SizeOfType<Automobile>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            base.WriteData(buf, fmt);

            buf.Write(Damage);
            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Automobile>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsMobile)
            {
                return 0x5AC;
            }
            if (GTA3Save.IsJapanesePS2(fmt))
            {
                return 0x59C;
            }
            if (fmt.IsPS2)
            {
                return 0x5BC;
            }
            if (fmt.IsPC || fmt.IsXbox)
            {
                return 0x5A8;
            }

            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Automobile);
        }

        public bool Equals(Automobile other)
        {
            if (other == null)
            {
                return false;
            }

            return base.Equals(other)
                && Damage.Equals(other.Damage);
        }
    }
}
