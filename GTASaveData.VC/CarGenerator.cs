using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.VC
{
    public sealed class CarGenerator : Chunk,
        IEquatable<CarGenerator>
    {
        private int m_model;
        private Vector3d m_position;
        private float m_heading;
        private int m_color1;
        private int m_color2;
        private bool m_forceSpawn;
        private int m_alarmChance;
        private int m_lockChance;
        private int m_minDelay;
        private int m_maxDelay;
        private int m_timer;
        private int m_vehiclePoolIndex;
        private bool m_generate;
        private bool m_hasRecentlyBeenStolen;

        public int Model
        {
            get { return m_model; }
            set { m_model = value; OnPropertyChanged(); }
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

        public int LockChance
        {
            get { return m_lockChance; }
            set { m_lockChance = value; OnPropertyChanged(); }
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

        public int Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public int VehiclePoolIndex
        {
            get { return m_vehiclePoolIndex; }
            set { m_vehiclePoolIndex = value; OnPropertyChanged(); }
        }

        public bool Generate
        {
            get { return m_generate; }
            set { m_generate = value; OnPropertyChanged(); }
        }

        public bool HasRecentlyBeenStolen
        {
            get { return m_hasRecentlyBeenStolen; }
            set { m_hasRecentlyBeenStolen = value; OnPropertyChanged(); }
        }

        public CarGenerator()
        {
            m_position = new Vector3d();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_model = r.ReadInt32();
            m_position = r.ReadChunk<Vector3d>();
            m_heading = r.ReadSingle();
            m_color1 = r.ReadInt16();
            m_color2 = r.ReadInt16();
            m_forceSpawn = r.ReadBool();
            m_alarmChance = r.ReadByte();
            m_lockChance = r.ReadByte();
            r.Align();
            m_minDelay = r.ReadUInt16();
            m_maxDelay = r.ReadUInt16();
            m_timer = r.ReadInt32();
            m_vehiclePoolIndex = r.ReadInt32();
            m_generate = r.ReadBool(2);
            m_hasRecentlyBeenStolen = r.ReadBool();
            r.Align();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_model);
            w.Write(m_position);
            w.Write(m_heading);
            w.Write((ushort) m_color1);
            w.Write((ushort) m_color2);
            w.Write(m_forceSpawn);
            w.Write((byte) m_alarmChance);
            w.Write((byte) m_lockChance);
            w.Align();
            w.Write((ushort) m_minDelay);
            w.Write((ushort) m_maxDelay);
            w.Write(m_timer);
            w.Write(m_vehiclePoolIndex);
            w.Write(m_generate, 2);
            w.Write(m_hasRecentlyBeenStolen);
            w.Align();
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
                && m_lockChance.Equals(other.m_lockChance)
                && m_minDelay.Equals(other.m_minDelay)
                && m_maxDelay.Equals(other.m_maxDelay)
                && m_timer.Equals(other.m_timer)
                && m_vehiclePoolIndex.Equals(other.m_vehiclePoolIndex)
                && m_generate.Equals(other.m_generate)
                && m_hasRecentlyBeenStolen.Equals(other.m_hasRecentlyBeenStolen);
        }

    }
}
