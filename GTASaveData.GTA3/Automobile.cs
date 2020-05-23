﻿using System;
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
        {
            Damage = new DamageManager();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.ReadObjectData(buf, fmt);
            Damage = buf.Read<DamageManager>();
            //buf.Skip(0x30A);    // The rest is useless
            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Automobile>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.WriteObjectData(buf, fmt);
            buf.Write(Damage);
            //buf.Skip(0x30A);
            buf.Skip(GetSize(fmt) - buf.Offset);

            Debug.Assert(buf.Offset == SizeOf<Automobile>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (fmt.Mobile)
            {
                return 0x5AC;
            }
            if (GTA3Save.IsJapanesePS2(fmt))
            {
                return 0x59C;
            }
            if (fmt.PS2)
            {
                return 0x5BC;
            }
            if (fmt.PC || fmt.Xbox)
            {
                return 0x5A8;   // PC/Xbox
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

            return Damage.Equals(other.Damage);
        }
    }
}