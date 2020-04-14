using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.VC
{
    [Size(0x2C)]
    public class CarGenerator : SaveDataObject, ICarGenerator, IEquatable<CarGenerator>
    {
        private int m_modelIndex;
        private Vector m_position;
        private float m_angle;
        private short m_color1;
        private short m_color2;
        private bool m_forceSpawn;
        private byte m_alarmChance;
        private byte m_lockedChance;
        private ushort m_minDelay;
        private ushort m_maxDelay;
        private uint m_timer;
        private int m_vehicleHandle;
        private short m_usesRemaining;
        private bool m_isBlocking;

        public int Model
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
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

        public short Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public short Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public bool ForceSpawn
        {
            get { return m_forceSpawn; }
            set { m_forceSpawn = value; OnPropertyChanged(); }
        }

        public byte AlarmChance
        {
            get { return m_alarmChance; }
            set { m_alarmChance = value; OnPropertyChanged(); }
        }

        public byte LockedChance
        {
            get { return m_lockedChance; }
            set { m_lockedChance = value; OnPropertyChanged(); }
        }

        public ushort MinDelay
        {
            get { return m_minDelay; }
            set { m_minDelay = value; OnPropertyChanged(); }
        }

        public ushort MaxDelay
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

        public short UsesRemaining
        {
            get { return m_usesRemaining; }
            set { m_usesRemaining = value; OnPropertyChanged(); }
        }

        public bool IsBlocking
        {
            get { return m_isBlocking; }
            set { m_isBlocking = value; OnPropertyChanged(); }
        }

        bool ICarGenerator.Enabled
        {
            get { return UsesRemaining > 0; }
            set { UsesRemaining = (short) ((value) ? 101 : 0); OnPropertyChanged(); }
        }

        int ICarGenerator.Color1
        {
            get { return Color1; }
            set { Color1 = (short) value; OnPropertyChanged(); }
        }

        int ICarGenerator.Color2
        {
            get { return Color2; }
            set { Color2 = (short) value; OnPropertyChanged(); }
        }

        public CarGenerator()
        {
            Position = new Vector();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.Read<Vector>();
            Angle = buf.ReadFloat();
            Color1 = buf.ReadInt16();
            Color2 = buf.ReadInt16();
            ForceSpawn = buf.ReadBool();
            AlarmChance = buf.ReadByte();
            LockedChance = buf.ReadByte();
            buf.ReadByte();
            MinDelay = buf.ReadUInt16();
            MaxDelay = buf.ReadUInt16();
            Timer = buf.ReadUInt32();
            VehicleHandle = buf.ReadInt32();
            UsesRemaining = buf.ReadInt16();
            IsBlocking = buf.ReadBool();
            buf.ReadByte();

            Debug.Assert(buf.Offset == SizeOf<CarGenerator>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write(Angle);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write(ForceSpawn);
            buf.Write(AlarmChance);
            buf.Write(LockedChance);
            buf.Write((byte) 0);
            buf.Write(MinDelay);
            buf.Write(MaxDelay);
            buf.Write(Timer);
            buf.Write(VehicleHandle);
            buf.Write(UsesRemaining);
            buf.Write(IsBlocking);
            buf.Write((byte) 0);

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

            return Model.Equals(other.Model)
                && Position.Equals(other.Position)
                && Angle.Equals(other.Angle)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && ForceSpawn.Equals(other.ForceSpawn)
                && AlarmChance.Equals(other.AlarmChance)
                && LockedChance.Equals(other.LockedChance)
                && MinDelay.Equals(other.MinDelay)
                && MaxDelay.Equals(other.MaxDelay)
                && Timer.Equals(other.Timer)
                && VehicleHandle.Equals(other.VehicleHandle)
                && UsesRemaining.Equals(other.UsesRemaining)
                && IsBlocking.Equals(other.IsBlocking);
        }
    }
}
