﻿using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Gang : SaveDataObject, IEquatable<Gang>
    {
        private int m_vehicleModel;
        private sbyte m_pedModelOverride;
        private WeaponType m_weapon1;
        private WeaponType m_weapon2;

        public int VehicleModel
        {
            get { return m_vehicleModel; }
            set { m_vehicleModel = value; OnPropertyChanged(); }
        }

        public sbyte PedModelOverride
        {
            get { return m_pedModelOverride; }
            set { m_pedModelOverride = value; OnPropertyChanged(); }
        }

        public WeaponType Weapon1
        {
            get { return m_weapon1; }
            set { m_weapon1 = value; OnPropertyChanged(); }
        }

        public WeaponType Weapon2
        {
            get { return m_weapon2; }
            set { m_weapon2 = value; OnPropertyChanged(); }
        }

        public Gang()
        {
            VehicleModel = -1;
            PedModelOverride = -1;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            VehicleModel = buf.ReadInt32();
            PedModelOverride = buf.ReadSByte();
            buf.Align4Bytes();
            Weapon1 = (WeaponType) buf.ReadInt32();
            Weapon2 = (WeaponType) buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Gang>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(VehicleModel);
            buf.Write(PedModelOverride);
            buf.Align4Bytes();
            buf.Write((int) Weapon1);
            buf.Write((int) Weapon2);

            Debug.Assert(buf.Offset == SizeOf<Gang>());
        }

        protected override int GetSize(DataFormat fmt)
        {
            return 16;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Gang);
        }

        public bool Equals(Gang other)
        {
            if (other == null)
            {
                return false;
            }

            return VehicleModel.Equals(other.VehicleModel)
                && PedModelOverride.Equals(other.PedModelOverride)
                && Weapon1.Equals(other.Weapon1)
                && Weapon2.Equals(other.Weapon2);
        }
    }
}