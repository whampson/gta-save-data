using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public enum VehicleType
    {
        Car,
        Boat
    }

    public enum CarLock
    {
        None,
        Unlocked,
        Locked,
        LockoutPlayerOnly,
        LockedPlayerInside,
        LockedInitially,
        ForceShutDoors,
        SkipShutDoors
    }

    public abstract class Vehicle : SaveDataObject, IEquatable<Vehicle>
    {
        private VehicleType m_type;
        private short m_modelIndex;
        private int m_handle;
        private Matrix m_matrix;
        private EntityType m_entityType;
        private EntityStatus m_entityStatus;
        private EntityFlags m_entityFlags;
        private AutoPilot m_autoPilot;
        private byte m_color1;
        private byte m_color2;
        private short m_alarmState;
        private byte m_maxNumPassengers;
        private float m_field1D0h;
        private float m_field1D4h;
        private float m_field1D8h;
        private float m_field1DCh;
        private float m_steerAngle;
        private float m_gasPedal;
        private float m_brakePedal;
        private byte m_vehicleCreatedBy;
        private bool m_isLawEnforcer;
        private bool m_isLocked;
        private bool m_isEngineOn;
        private bool m_isHandbrakeOn;
        private bool m_lightsOn;
        private bool m_hasFreebies;
        private float m_health;
        private byte m_currentGear;
        private float m_changeGearTime;
        private uint m_timeOfDeath;
        private short m_bombTimer;
        private CarLock m_doorLock;

        public VehicleType Type
        {
            get { return m_type; }
            private set { m_type = value; OnPropertyChanged(); }
        }

        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public Matrix Matrix
        {
            get { return m_matrix; }
            set { m_matrix = value; OnPropertyChanged(); }
        }

        public EntityType EntityType
        {
            get { return m_entityType; }
            set { m_entityType = value; OnPropertyChanged(); }
        }

        public EntityStatus EntityStatus
        {
            get { return m_entityStatus; }
            set { m_entityStatus = value; OnPropertyChanged(); }
        }

        public EntityFlags EntityFlags
        {
            get { return m_entityFlags; }
            set { m_entityFlags = value; OnPropertyChanged(); }
        }

        public AutoPilot AutoPilot
        {
            get { return m_autoPilot; }
            set { m_autoPilot = value; OnPropertyChanged(); }
        }

        public byte Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public byte Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public short AlarmState
        {
            get { return m_alarmState; }
            set { m_alarmState = value; OnPropertyChanged(); }
        }

        public byte MaxNumPassengers
        {
            get { return m_maxNumPassengers; }
            set { m_maxNumPassengers = value; OnPropertyChanged(); }
        }

        public float Field1D0h
        {
            get { return m_field1D0h; }
            set { m_field1D0h = value; OnPropertyChanged(); }
        }

        public float Field1D4h
        {
            get { return m_field1D4h; }
            set { m_field1D4h = value; OnPropertyChanged(); }
        }

        public float Field1D8h
        {
            get { return m_field1D8h; }
            set { m_field1D8h = value; OnPropertyChanged(); }
        }

        public float Field1DCh
        {
            get { return m_field1DCh; }
            set { m_field1DCh = value; OnPropertyChanged(); }
        }

        public float SteerAngle
        {
            get { return m_steerAngle; }
            set { m_steerAngle = value; OnPropertyChanged(); }
        }

        public float GasPedal
        {
            get { return m_gasPedal; }
            set { m_gasPedal = value; OnPropertyChanged(); }
        }

        public float BrakePedal
        {
            get { return m_brakePedal; }
            set { m_brakePedal = value; OnPropertyChanged(); }
        }

        public byte VehicleCreatedBy
        {
            get { return m_vehicleCreatedBy; }
            set { m_vehicleCreatedBy = value; OnPropertyChanged(); }
        }

        public bool IsLawEnforcer
        {
            get { return m_isLawEnforcer; }
            set { m_isLawEnforcer = value; OnPropertyChanged(); }
        }

        public bool IsLocked
        {
            get { return m_isLocked; }
            set { m_isLocked = value; OnPropertyChanged(); }
        }

        public bool IsEngineOn
        {
            get { return m_isEngineOn; }
            set { m_isEngineOn = value; OnPropertyChanged(); }
        }

        public bool IsHandbrakeOn
        {
            get { return m_isHandbrakeOn; }
            set { m_isHandbrakeOn = value; OnPropertyChanged(); }
        }

        public bool LightsOn
        {
            get { return m_lightsOn; }
            set { m_lightsOn = value; OnPropertyChanged(); }
        }

        public bool HasFreebies
        {
            get { return m_hasFreebies; }
            set { m_hasFreebies = value; OnPropertyChanged(); }
        }

        public float Health
        {
            get { return m_health; }
            set { m_health = value; OnPropertyChanged(); }
        }

        public byte CurrentGear
        {
            get { return m_currentGear; }
            set { m_currentGear = value; OnPropertyChanged(); }
        }

        public float ChangeGearTime
        {
            get { return m_changeGearTime; }
            set { m_changeGearTime = value; OnPropertyChanged(); }
        }

        public uint TimeOfDeath
        {
            get { return m_timeOfDeath; }
            set { m_timeOfDeath = value; OnPropertyChanged(); }
        }

        public short BombTimer
        {
            get { return m_bombTimer; }
            set { m_bombTimer = value; OnPropertyChanged(); }
        }

        public CarLock DoorLock
        {
            get { return m_doorLock; }
            set { m_doorLock = value; OnPropertyChanged(); }
        }


        public Vehicle(VehicleType type, short model, int handle)
        {
            Type = type;
            ModelIndex = model;
            Handle = handle;
            AutoPilot = new AutoPilot();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Skip(4);
            Matrix m = new Matrix();
            m.Right = buf.Read<Vector3D>();
            buf.Skip(4);
            m.Forward = buf.Read<Vector3D>();
            buf.Skip(4);
            m.Up = buf.Read<Vector3D>();
            buf.Skip(4);
            m.Position = buf.Read<Vector3D>();
            Matrix = m;
            buf.Skip(16);
            long eFlags = buf.ReadInt64();
            EntityFlags = (EntityFlags) (eFlags & ~0xFF);
            EntityType = (EntityType) (eFlags & 0x07);
            EntityStatus = (EntityStatus) ((eFlags & 0xF8) >> 3);
            buf.Skip(212);
            AutoPilot = buf.Read<AutoPilot>();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            buf.Skip(2);
            AlarmState = buf.ReadInt16();
            buf.Skip(43);
            MaxNumPassengers = buf.ReadByte();
            buf.Skip(2);
            Field1D0h = buf.ReadFloat();
            Field1D4h = buf.ReadFloat();
            Field1D8h = buf.ReadFloat();
            Field1DCh = buf.ReadFloat();
            buf.Skip(8);
            SteerAngle = buf.ReadFloat();
            GasPedal = buf.ReadFloat();
            BrakePedal = buf.ReadFloat();
            VehicleCreatedBy = buf.ReadByte();
            byte flags = buf.ReadByte();
            IsLawEnforcer = (flags & 0x01) != 0;
            IsLocked = (flags & 0x08) != 0;
            IsEngineOn = (flags & 0x10) != 0;
            IsHandbrakeOn = (flags & 0x20) != 0;
            LightsOn = (flags & 0x40) != 0;
            HasFreebies = (flags & 0x80) != 0;
            buf.Skip(10);
            Health = buf.ReadFloat();
            CurrentGear = buf.ReadByte();
            buf.Skip(3);
            ChangeGearTime = buf.ReadFloat();
            buf.Skip(4);
            TimeOfDeath = buf.ReadUInt32();
            buf.Skip(2);
            BombTimer = buf.ReadInt16();
            buf.Skip(12);
            DoorLock = (CarLock) buf.ReadByte();
            buf.Skip(99);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Skip(4);
            buf.Write(Matrix.Right);
            buf.Skip(4);
            buf.Write(Matrix.Forward);
            buf.Skip(4);
            buf.Write(Matrix.Up);
            buf.Skip(4);
            buf.Write(Matrix.Position);
            buf.Skip(16);
            long eFlags = (long) EntityFlags;
            eFlags |= ((long) EntityType) & 0x07;
            eFlags |= (((long) EntityStatus) & 0x1F) << 3;
            buf.Write(eFlags);
            buf.Skip(212);
            buf.Write(AutoPilot);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Skip(2);
            buf.Write(AlarmState);
            buf.Skip(43);
            buf.Write(MaxNumPassengers);
            buf.Skip(2);
            buf.Write(Field1D0h);
            buf.Write(Field1D4h);
            buf.Write(Field1D8h);
            buf.Write(Field1DCh);
            buf.Skip(8);
            buf.Write(SteerAngle);
            buf.Write(GasPedal);
            buf.Write(BrakePedal);
            buf.Write(VehicleCreatedBy);
            byte flags = 0;
            if (IsLawEnforcer) flags |= 0x01;
            if (IsLocked) flags |= 0x08;
            if (IsEngineOn) flags |= 0x10;
            if (IsHandbrakeOn) flags |= 0x20;
            if (LightsOn) flags |= 0x40;
            if (HasFreebies) flags |= 0x80;
            buf.Write(flags);
            buf.Skip(10);
            buf.Write(Health);
            buf.Write(CurrentGear);
            buf.Skip(3);
            buf.Write(ChangeGearTime);
            buf.Skip(4);
            buf.Write(TimeOfDeath);
            buf.Skip(2);
            buf.Write(BombTimer);
            buf.Skip(12);
            buf.Write((byte) DoorLock);
            buf.Skip(99);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vehicle);
        }

        public bool Equals(Vehicle other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && ModelIndex.Equals(other.ModelIndex)
                && Handle.Equals(other.Handle)
                && Matrix.Equals(other.Matrix)
                && EntityType.Equals(other.EntityType)
                && EntityStatus.Equals(other.EntityStatus)
                && EntityFlags.Equals(other.EntityFlags)
                && AutoPilot.Equals(other.AutoPilot)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && AlarmState.Equals(other.AlarmState)
                && MaxNumPassengers.Equals(other.MaxNumPassengers)
                && Field1D0h.Equals(other.Field1D0h)
                && Field1D4h.Equals(other.Field1D4h)
                && Field1D8h.Equals(other.Field1D8h)
                && Field1DCh.Equals(other.Field1DCh)
                && SteerAngle.Equals(other.SteerAngle)
                && GasPedal.Equals(other.GasPedal)
                && BrakePedal.Equals(other.BrakePedal)
                && VehicleCreatedBy.Equals(other.VehicleCreatedBy)
                && IsLawEnforcer.Equals(other.IsLawEnforcer)
                && IsLocked.Equals(other.IsLocked)
                && IsEngineOn.Equals(other.IsEngineOn)
                && IsHandbrakeOn.Equals(other.IsHandbrakeOn)
                && LightsOn.Equals(other.LightsOn)
                && HasFreebies.Equals(other.HasFreebies)
                && Health.Equals(other.Health)
                && CurrentGear.Equals(other.CurrentGear)
                && ChangeGearTime.Equals(other.ChangeGearTime)
                && TimeOfDeath.Equals(other.TimeOfDeath)
                && BombTimer.Equals(other.BombTimer)
                && DoorLock.Equals(other.DoorLock);
        }
    }
}
