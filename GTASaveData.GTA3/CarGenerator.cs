using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x48)]
    public class CarGenerator : SaveDataObject, ICarGenerator, IEquatable<CarGenerator>
    {
        private int m_modelIndex;
        private Vector m_position;
        private float m_angle;
        private short m_color1;
        private short m_color2;
        private bool m_forceSpawn;
        private byte m_alarm;
        private byte m_doorLock;
        private ushort m_minDelay;
        private ushort m_maxDelay;
        private uint m_timer;
        private int m_handle;
        private short m_usesRemaining;
        private bool m_isBlocking;
        private Vector m_vecInf;
        private Vector m_vecSup;
        private float m_size;

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

        public byte Alarm
        {
            get { return m_alarm; }
            set { m_alarm = value; OnPropertyChanged(); }
        }

        public byte DoorLock
        {
            get { return m_doorLock; }
            set { m_doorLock = value; OnPropertyChanged(); }
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

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
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

        public Vector CollisionBoundingMin
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector CollisionBoundingMax
        {
            get { return m_vecSup; }
            set { m_vecSup = value; OnPropertyChanged(); }
        }

        public float CollisionSize
        {
            get { return m_size; }
            set { m_size= value; OnPropertyChanged(); }
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
            CollisionBoundingMin = new Vector();
            CollisionBoundingMax = new Vector();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.Read<Vector>();
            Angle = buf.ReadFloat();
            Color1 = buf.ReadInt16();
            Color2 = buf.ReadInt16();
            ForceSpawn = buf.ReadBool();
            Alarm = buf.ReadByte();
            DoorLock = buf.ReadByte();
            buf.ReadByte();
            MinDelay = buf.ReadUInt16();
            MaxDelay = buf.ReadUInt16();
            Timer = buf.ReadUInt32();
            Handle = buf.ReadInt32();
            UsesRemaining = buf.ReadInt16();
            IsBlocking = buf.ReadBool();
            buf.ReadByte();
            CollisionBoundingMin = buf.Read<Vector>();
            CollisionBoundingMax = buf.Read<Vector>();
            CollisionSize = buf.ReadFloat();

            Debug.Assert(buf.Offset == SizeOf<CarGenerator>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write(Angle);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write(ForceSpawn);
            buf.Write(Alarm);
            buf.Write(DoorLock);
            buf.Write((byte) 0);
            buf.Write(MinDelay);
            buf.Write(MaxDelay);
            buf.Write(Timer);
            buf.Write(Handle);
            buf.Write(UsesRemaining);
            buf.Write(IsBlocking);
            buf.Write((byte) 0);
            buf.Write(CollisionBoundingMin);
            buf.Write(CollisionBoundingMax);
            buf.Write(CollisionSize);

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
                && Alarm.Equals(other.Alarm)
                && DoorLock.Equals(other.DoorLock)
                && MinDelay.Equals(other.MinDelay)
                && MaxDelay.Equals(other.MaxDelay)
                && Timer.Equals(other.Timer)
                && Handle.Equals(other.Handle)
                && UsesRemaining.Equals(other.UsesRemaining)
                && IsBlocking.Equals(other.IsBlocking)
                && CollisionBoundingMin.Equals(other.CollisionBoundingMin)
                && CollisionBoundingMax.Equals(other.CollisionBoundingMax)
                && CollisionSize.Equals(other.CollisionSize);
        }
    }
}
