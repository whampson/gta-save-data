using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class CarGenerator : SerializableObject,
        ICarGenerator,
        IEquatable<CarGenerator>
    {
        private VehicleModel m_model;
        private Vector3d m_position;
        private float m_heading;
        private int m_color1;
        private int m_color2;
        private bool m_forceSpawn;
        private int m_alarmChance;
        private int m_lockedChance;
        private int m_minDelay;
        private int m_maxDelay;
        private uint m_timer;
        private int m_vehiclePoolIndex;
        private bool m_enabled;
        private bool m_hasRecentlyBeenStolen;
        private Vector3d m_vecInf;
        private Vector3d m_vecSup;
        private float m_unknown;

        int ICarGenerator.Model
        {
            get { return (int) m_model; }
            set { m_model = (VehicleModel) value; OnPropertyChanged(); }
        }

        public VehicleModel Model
        {
            get { return m_model; }
            set { m_model =  value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public float Heading
        {
            get { return m_heading; }
            set { m_heading = value; OnPropertyChanged(); }
        }

        public int Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public int Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public bool ForceSpawn
        {
            get { return m_forceSpawn; }
            set { m_forceSpawn = value; OnPropertyChanged(); }
        }

        public int AlarmChance
        {
            get { return m_alarmChance; }
            set { m_alarmChance = value; OnPropertyChanged(); }
        }

        public int LockedChance
        {
            get { return m_lockedChance; }
            set { m_lockedChance = value; OnPropertyChanged(); }
        }

        public int MinDelay
        {
            get { return m_minDelay; }
            set { m_minDelay = value; OnPropertyChanged(); }
        }

        public int MaxDelay
        {
            get { return m_maxDelay; }
            set { m_maxDelay = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public int VehiclePoolIndex
        {
            get { return m_vehiclePoolIndex; }
            set { m_vehiclePoolIndex = value; OnPropertyChanged(); }
        }

        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; OnPropertyChanged(); }
        }

        public bool HasRecentlyBeenStolen
        {
            get { return m_hasRecentlyBeenStolen; }
            set { m_hasRecentlyBeenStolen = value; OnPropertyChanged(); }
        }

        public Vector3d VecInf
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector3d VecSup
        {
            get { return m_vecSup; }
            set { m_vecSup = value; OnPropertyChanged(); }
        }

        public float Unknown
        {
            get { return m_unknown; }
            set { m_unknown= value; OnPropertyChanged(); }
        }

        public CarGenerator()
        {
            m_position = new Vector3d();
            m_vecInf = new Vector3d();
            m_vecSup = new Vector3d();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_model = (VehicleModel) r.ReadInt32();
            m_position = r.ReadObject<Vector3d>();
            m_heading = r.ReadSingle();
            m_color1 = r.ReadInt16();
            m_color2 = r.ReadInt16();
            m_forceSpawn = r.ReadBool();
            m_alarmChance = r.ReadByte();
            m_lockedChance = r.ReadByte();
            r.Align();
            m_minDelay = r.ReadUInt16();
            m_maxDelay = r.ReadUInt16();
            m_timer = r.ReadUInt32();
            m_vehiclePoolIndex = r.ReadInt32();
            m_enabled = r.ReadBool(2);
            m_hasRecentlyBeenStolen = r.ReadBool();
            r.Align();
            m_vecInf = r.ReadObject<Vector3d>();
            m_vecSup = r.ReadObject<Vector3d>();
            m_unknown = r.ReadSingle();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_model);
            w.Write(m_position);
            w.Write(m_heading);
            w.Write((short) m_color1);
            w.Write((short) m_color2);
            w.Write(m_forceSpawn);
            w.Write((byte) m_alarmChance);
            w.Write((byte) m_lockedChance);
            w.Align();
            w.Write((ushort) m_minDelay);
            w.Write((ushort) m_maxDelay);
            w.Write(m_timer);
            w.Write(m_vehiclePoolIndex);
            w.Write(m_enabled, 2);
            w.Write(m_hasRecentlyBeenStolen);
            w.Align();
            w.Write(m_vecInf);
            w.Write(m_vecSup);
            w.Write(m_unknown);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CarGenerator);
        }

        public bool Equals(CarGenerator other)
        {
            if (other == null)
            {
                return false;
            }

            return m_model.Equals(other.m_model)
                && m_position.Equals(other.m_position)
                && m_heading.Equals(other.m_heading)
                && m_color1.Equals(other.m_color1)
                && m_color2.Equals(other.m_color2)
                && m_forceSpawn.Equals(other.m_forceSpawn)
                && m_alarmChance.Equals(other.m_alarmChance)
                && m_lockedChance.Equals(other.m_lockedChance)
                && m_minDelay.Equals(other.m_minDelay)
                && m_maxDelay.Equals(other.m_maxDelay)
                && m_timer.Equals(other.m_timer)
                && m_vehiclePoolIndex.Equals(other.m_vehiclePoolIndex)
                && m_enabled.Equals(other.m_enabled)
                && m_hasRecentlyBeenStolen.Equals(other.m_hasRecentlyBeenStolen)
                && m_vecInf.Equals(other.m_vecInf)
                && m_vecSup.Equals(other.m_vecSup)
                && m_unknown.Equals(other.m_unknown);
        }
    }
}
