using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x48)]
    public class CarGenerator : GTAObject,
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

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_model = (VehicleType) buf.ReadInt32();
            m_position = buf.ReadObject<Vector>();
            m_angle = buf.ReadSingle();
            m_color1 = buf.ReadInt16();
            m_color2 = buf.ReadInt16();
            m_forceSpawn = buf.ReadBool();
            m_alarmChance = buf.ReadByte();
            m_lockedChance = buf.ReadByte();
            buf.ReadByte();
            m_minDelay = buf.ReadUInt16();
            m_maxDelay = buf.ReadUInt16();
            m_timer = buf.ReadUInt32();
            m_vehicleHandle = buf.ReadInt32();
            m_usesRemaining = buf.ReadInt16();
            m_isBlocking = buf.ReadBool();
            buf.ReadByte();
            m_vecInf = buf.ReadObject<Vector>();
            m_vecSup = buf.ReadObject<Vector>();
            m_size = buf.ReadSingle();

            Debug.Assert(buf.Offset == SizeOf<CarGenerator>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((int) m_model);
            buf.Write(m_position);
            buf.Write(m_angle);
            buf.Write((short) m_color1);
            buf.Write((short) m_color2);
            buf.Write(m_forceSpawn);
            buf.Write((byte) m_alarmChance);
            buf.Write((byte) m_lockedChance);
            buf.Write((byte) 0);
            buf.Write((ushort) m_minDelay);
            buf.Write((ushort) m_maxDelay);
            buf.Write(m_timer);
            buf.Write(m_vehicleHandle);
            buf.Write((short) m_usesRemaining);
            buf.Write(m_isBlocking);
            buf.Write((byte) 0);
            buf.Write(m_vecInf);
            buf.Write(m_vecSup);
            buf.Write(m_size);

            Debug.Assert(buf.Offset == SizeOf<CarGenerator>());
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
