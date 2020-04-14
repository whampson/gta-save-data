using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x10)]
    public class GangInfo : SaveDataObject, IEquatable<GangInfo>
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


        public GangInfo()
        {
            VehicleModel = -1;
            PedModelOverride = -1;
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            VehicleModel = buf.ReadInt32();
            PedModelOverride = buf.ReadSByte();
            buf.Align4Bytes();
            Weapon1 = (WeaponType) buf.ReadInt32();
            Weapon2 = (WeaponType) buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<GangInfo>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(VehicleModel);
            buf.Write(PedModelOverride);
            buf.Align4Bytes();
            buf.Write((int) Weapon1);
            buf.Write((int) Weapon2);

            Debug.Assert(buf.Offset == SizeOf<GangInfo>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GangInfo);
        }

        public bool Equals(GangInfo other)
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
