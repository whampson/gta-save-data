using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public abstract class Vehicle : Entity,
        IEquatable<Vehicle>
    {
        private VehicleType m_type;
        private short m_modelIndex;
        private int m_handle;
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
        private VehicleCreatedBy m_createdBy;
        private bool m_isLawEnforcer;
        private bool m_isLockedByScript;
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

        public VehicleCreatedBy CreatedBy
        {
            get { return m_createdBy; }
            set { m_createdBy = value; OnPropertyChanged(); }
        }

        public bool IsLawEnforcer
        {
            get { return m_isLawEnforcer; }
            set { m_isLawEnforcer = value; OnPropertyChanged(); }
        }

        public bool IsLockedByScript
        {
            get { return m_isLockedByScript; }
            set { m_isLockedByScript = value; OnPropertyChanged(); }
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

        public static Vehicle Create(int model, VehicleType type = VehicleType.Car)
        {
            switch (type)
            {
                case VehicleType.Car:
                    return new Automobile((short) model);
                case VehicleType.Boat:
                    return new Boat((short) model);
                default:
                    throw new InvalidOperationException("Invalid VehicleType: " + (int) type);
            }
        }

        public static Vehicle CreateDefault(VehicleType type = VehicleType.Car)
        {
            return Create(0, type);
        }

        public Vehicle(VehicleType type, short model, int handle)
            : base()
        {
            Type = type;
            ModelIndex = model;
            Handle = handle;
            EntityType = EntityType.Vehicle;
            EntityFlags |= EntityFlags.UsesCollision | EntityFlags.UseCollisionRecords | EntityFlags.IsInSafePosition;
            Health = 1000;
            IsEngineOn = true;
            HasFreebies = true;
            AutoPilot = new AutoPilot();
        }

        public Vehicle(Vehicle other)
            : base(other)
        {
            Type = other.Type;
            ModelIndex = other.ModelIndex;
            Handle = other.Handle;
            AutoPilot = new AutoPilot(other.AutoPilot);
            Color1 = other.Color1;
            Color2 = other.Color2;
            AlarmState = other.AlarmState;
            MaxNumPassengers = other.MaxNumPassengers;
            Field1D0h = other.Field1D0h;
            Field1D4h = other.Field1D4h;
            Field1D8h = other.Field1D8h;
            Field1DCh = other.Field1DCh;
            SteerAngle = other.SteerAngle;
            GasPedal = other.GasPedal;
            BrakePedal = other.BrakePedal;
            CreatedBy = other.CreatedBy;
            IsLawEnforcer = other.IsLawEnforcer;
            IsLockedByScript = other.IsLockedByScript;
            IsEngineOn = other.IsEngineOn;
            IsHandbrakeOn = other.IsHandbrakeOn;
            LightsOn = other.LightsOn;
            HasFreebies = other.HasFreebies;
            Health = other.Health;
            CurrentGear = other.CurrentGear;
            ChangeGearTime = other.ChangeGearTime;
            TimeOfDeath = other.TimeOfDeath;
            BombTimer = other.BombTimer;
            DoorLock = other.DoorLock;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            if (!fmt.IsPS2) buf.Skip(4);
            Matrix = buf.ReadStruct<Matrix>();
            if (fmt.IsPC || fmt.IsXbox) buf.Skip(12);
            if (fmt.IsiOS) buf.Skip(15);
            if (fmt.IsAndroid) buf.Skip(16);
            if (fmt.IsPS2 && !fmt.FlagJapan) buf.Skip(32);
            LoadEntityFlags(buf, fmt);
            if (fmt.IsiOS) buf.Skip(1);
            if (fmt.IsPS2) buf.Skip(236);
            else buf.Skip(212);
            AutoPilot = buf.ReadObject<AutoPilot>();
            Color1 = buf.ReadByte();
            Color2 = buf.ReadByte();
            buf.Skip(2);
            AlarmState = buf.ReadInt16();
            buf.Skip(42);
            MaxNumPassengers = buf.ReadByte();
            buf.Skip(3);
            Field1D0h = buf.ReadFloat();
            Field1D4h = buf.ReadFloat();
            Field1D8h = buf.ReadFloat();
            Field1DCh = buf.ReadFloat();
            buf.Skip(8);
            SteerAngle = buf.ReadFloat();
            GasPedal = buf.ReadFloat();
            BrakePedal = buf.ReadFloat();
            CreatedBy = (VehicleCreatedBy) buf.ReadByte();
            byte flags = buf.ReadByte();
            IsLawEnforcer = (flags & 0x01) != 0;
            IsLockedByScript = (flags & 0x08) != 0;
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
            DoorLock = (CarLock) buf.ReadInt32();
            if (fmt.IsPS2) buf.Skip(156);
            else buf.Skip(96);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            if (!fmt.IsPS2) buf.Skip(4);
            buf.Write(Matrix);
            if (fmt.IsPC || fmt.IsXbox) buf.Skip(12);
            if (fmt.IsiOS) buf.Skip(15);
            if (fmt.IsAndroid) buf.Skip(16);
            if (fmt.IsPS2 && !fmt.FlagJapan) buf.Skip(32);
            SaveEntityFlags(buf, fmt);
            if (fmt.IsiOS) buf.Skip(1);
            if (fmt.IsPS2) buf.Skip(236);
            else buf.Skip(212);
            buf.Write(AutoPilot);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Skip(2);
            buf.Write(AlarmState);
            buf.Skip(42);
            buf.Write(MaxNumPassengers);
            buf.Skip(3);
            buf.Write(Field1D0h);
            buf.Write(Field1D4h);
            buf.Write(Field1D8h);
            buf.Write(Field1DCh);
            buf.Skip(8);
            buf.Write(SteerAngle);
            buf.Write(GasPedal);
            buf.Write(BrakePedal);
            buf.Write((byte) CreatedBy);
            byte flags = 0;
            if (IsLawEnforcer) flags |= 0x01;
            if (IsLockedByScript) flags |= 0x08;
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
            buf.Write((int) DoorLock);
            if (fmt.IsPS2) buf.Skip(156);
            else buf.Skip(96);
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
                && CreatedBy.Equals(other.CreatedBy)
                && IsLawEnforcer.Equals(other.IsLawEnforcer)
                && IsLockedByScript.Equals(other.IsLockedByScript)
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

    public enum VehicleType
    {
        Car,
        Boat
    }

    public enum VehicleCreatedBy
    {
        Unknown,
        Random,
        Mission,
        Parked,
        Permanent
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
}
