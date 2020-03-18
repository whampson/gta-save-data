using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x48)]
    public class CarGenerator : SerializableObject,
        ICarGenerator,
        IEquatable<CarGenerator>
    {
        private VehicleType m_model;
        private Vector m_position;
        private float m_angle;
        private int m_color1;
        private int m_color2;
        private bool m_forceSpawn;
        private int m_alarmChance;
        private int m_lockedChance;
        private int m_minDelay;
        private int m_maxDelay;
        private uint m_timer;
        private int m_vehicleHandle;
        private int m_usesRemaining;
        private bool m_isBlocking;
        private Vector m_vecInf;
        private Vector m_vecSup;
        private float m_size;

        int ICarGenerator.Model
        {
            get { return (int) m_model; }
            set { m_model = (VehicleType) value; OnPropertyChanged(); }
        }

        bool ICarGenerator.Enabled
        {
            get { return m_usesRemaining > 0; }
            set { m_usesRemaining = (value) ? 101 : 0; OnPropertyChanged(); }
        }

        public VehicleType Model
        {
            get { return m_model; }
            set { m_model =  value; OnPropertyChanged(); }
        }

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public float Angle
        {
            get { return m_angle; }
            set { m_angle = value; OnPropertyChanged(); }
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

        public int VehicleHandle
        {
            get { return m_vehicleHandle; }
            set { m_vehicleHandle = value; OnPropertyChanged(); }
        }

        public int UsesRemaining
        {
            get { return m_usesRemaining; }
            set { m_usesRemaining = value; OnPropertyChanged(); }
        }

        public bool IsBlocking
        {
            get { return m_isBlocking; }
            set { m_isBlocking = value; OnPropertyChanged(); }
        }

        public Vector VecInf
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector VecSup
        {
            get { return m_vecSup; }
            set { m_vecSup = value; OnPropertyChanged(); }
        }

        public float Size
        {
            get { return m_size; }
            set { m_size= value; OnPropertyChanged(); }
        }

        public CarGenerator()
        {
            m_position = new Vector();
            m_vecInf = new Vector();
            m_vecSup = new Vector();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_model = (VehicleType) r.ReadInt32();
            m_position = r.ReadObject<Vector>();
            m_angle = r.ReadSingle();
            m_color1 = r.ReadInt16();
            m_color2 = r.ReadInt16();
            m_forceSpawn = r.ReadBool();
            m_alarmChance = r.ReadByte();
            m_lockedChance = r.ReadByte();
            r.ReadByte();
            m_minDelay = r.ReadUInt16();
            m_maxDelay = r.ReadUInt16();
            m_timer = r.ReadUInt32();
            m_vehicleHandle = r.ReadInt32();
            m_usesRemaining = r.ReadInt16();
            m_isBlocking = r.ReadBool();
            r.ReadByte();
            m_vecInf = r.ReadObject<Vector>();
            m_vecSup = r.ReadObject<Vector>();
            m_size = r.ReadSingle();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<CarGenerator>(), "CarGenerator: Invalid read size!");
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_model);
            w.Write(m_position);
            w.Write(m_angle);
            w.Write((short) m_color1);
            w.Write((short) m_color2);
            w.Write(m_forceSpawn);
            w.Write((byte) m_alarmChance);
            w.Write((byte) m_lockedChance);
            w.Align();
            w.Write((ushort) m_minDelay);
            w.Write((ushort) m_maxDelay);
            w.Write(m_timer);
            w.Write(m_vehicleHandle);
            w.Write((short) m_usesRemaining);
            w.Write(m_isBlocking);
            w.Align();
            w.Write(m_vecInf);
            w.Write(m_vecSup);
            w.Write(m_size);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<CarGenerator>(), "CarGenerator: Invalid write size!");
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
                && m_angle.Equals(other.m_angle)
                && m_color1.Equals(other.m_color1)
                && m_color2.Equals(other.m_color2)
                && m_forceSpawn.Equals(other.m_forceSpawn)
                && m_alarmChance.Equals(other.m_alarmChance)
                && m_lockedChance.Equals(other.m_lockedChance)
                && m_minDelay.Equals(other.m_minDelay)
                && m_maxDelay.Equals(other.m_maxDelay)
                && m_timer.Equals(other.m_timer)
                && m_vehicleHandle.Equals(other.m_vehicleHandle)
                && m_usesRemaining.Equals(other.m_usesRemaining)
                && m_isBlocking.Equals(other.m_isBlocking)
                && m_vecInf.Equals(other.m_vecInf)
                && m_vecSup.Equals(other.m_vecSup)
                && m_size.Equals(other.m_size);
        }
    }
}
