using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
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
        private Vector3d m_vecInf;
        private Vector3d m_vecSup;
        private float m_unknown;

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

        private CarGenerator(SaveDataSerializer r, FileFormat fmt)
        {
            m_model = r.ReadInt32();
            m_position = r.ReadObject<Vector3d>();
            m_heading = r.ReadSingle();
            m_color1 = r.ReadInt16();
            m_color2 = r.ReadInt16();
            m_forceSpawn = r.ReadBool();
            m_alarmChance = r.ReadByte();
            m_lockChance = r.ReadByte();
            r.Align();
            m_minDelay = r.ReadInt16();
            m_maxDelay = r.ReadInt16();
            m_timer = r.ReadInt32();
            m_vehiclePoolIndex = r.ReadInt32();
            m_generate = r.ReadBool(2);
            m_hasRecentlyBeenStolen = r.ReadBool();
            r.Align();
            m_vecInf = r.ReadObject<Vector3d>();
            m_vecSup = r.ReadObject<Vector3d>();
            m_unknown = r.ReadSingle();
        }

        protected override void WriteObjectData(SaveDataSerializer w, FileFormat fmt)
        {
            w.Write(m_model);
            w.WriteObject(m_position);
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
            w.WriteObject(m_vecInf);
            w.WriteObject(m_vecSup);
            w.Write(m_unknown);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

            return m_model.Equals(m_model)
                && m_position.Equals(m_position)
                && m_heading.Equals(m_heading)
                && m_color1.Equals(m_color1)
                && m_color2.Equals(m_color2)
                && m_forceSpawn.Equals(m_forceSpawn)
                && m_alarmChance.Equals(m_alarmChance)
                && m_lockChance.Equals(m_lockChance)
                && m_minDelay.Equals(m_minDelay)
                && m_maxDelay.Equals(m_maxDelay)
                && m_timer.Equals(m_timer)
                && m_vehiclePoolIndex.Equals(m_vehiclePoolIndex)
                && m_generate.Equals(m_generate)
                && m_hasRecentlyBeenStolen.Equals(m_hasRecentlyBeenStolen)
                && m_vecInf.Equals(m_vecInf)
                && m_vecSup.Equals(m_vecSup)
                && m_unknown.Equals(m_unknown);
        }

    }
}
