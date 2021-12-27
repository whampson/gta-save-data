using System;
using System.Diagnostics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Automobile : Vehicle,
        IEquatable<Automobile>, IDeepClonable<Automobile>
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

        public Automobile(short model)
            : this(model, 0)
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

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            base.ReadData(buf, prm);

            Damage = buf.ReadObject<DamageManager>();
            buf.Skip(SizeOf<Automobile>(prm) - buf.Offset);    // The rest is useless

            Debug.Assert(buf.Offset == SizeOf<Automobile>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            base.WriteData(buf, prm);

            buf.Write(Damage);
            buf.Skip(SizeOf<Automobile>(prm) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Automobile>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            var t = prm.FileType;
            if (t.IsPS2 && t.FlagJapan) return 0x630;
            if (t.IsPC || t.IsXbox) return 0x5A8;
            if (t.IsMobile) return 0x5AC;
            if (t.IsPS2) return 0x650;
            throw SizeNotDefined(t);
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

        public Automobile DeepClone()
        {
            return new Automobile(this);
        }
    }
}
