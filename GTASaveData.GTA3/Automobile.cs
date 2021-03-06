﻿using System;
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

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            base.ReadData(buf, fmt);

            Damage = buf.ReadObject<DamageManager>();
            buf.Skip(SizeOfType<Automobile>(fmt) - buf.Offset);    // The rest is useless

            Debug.Assert(buf.Offset == SizeOfType<Automobile>(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            base.WriteData(buf, fmt);

            buf.Write(Damage);
            buf.Skip(SizeOfType<Automobile>(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOfType<Automobile>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2 && fmt.IsJapanese) return 0x630;
            if (fmt.IsPC || fmt.IsXbox) return 0x5A8;
            if (fmt.IsMobile) return 0x5AC;
            if (fmt.IsPS2) return 0x650;
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

        public Automobile DeepClone()
        {
            return new Automobile(this);
        }
    }
}
