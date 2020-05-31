using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class CarGenerator : SaveDataObject, ICarGenerator, IEquatable<CarGenerator>
    {
        private int m_modelIndex;
        private Vector3D m_position;
        private float m_heading;
        private short m_color1;
        private short m_color2;
        private bool m_forceSpawn;
        private byte m_alarmChance;
        private byte m_lockedChance;
        private ushort m_minDelay;
        private ushort m_maxDelay;
        private uint m_timer;
        private int m_handle;
        private bool m_enabled;
        private bool m_isBlocking;
        private Vector3D m_vecInf;
        private Vector3D m_vecSup;
        private float m_size;

        public int Model
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public float Heading
        {
            get { return m_heading; }
            set { m_heading = value; OnPropertyChanged(); }
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

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; OnPropertyChanged(); }
        }

        public bool IsBlocking
        {
            get { return m_isBlocking; }
            set { m_isBlocking = value; OnPropertyChanged(); }
        }

        public Vector3D CollisionBoundingMin
        {
            get { return m_vecInf; }
            set { m_vecInf = value; OnPropertyChanged(); }
        }

        public Vector3D CollisionBoundingMax
        {
            get { return m_vecSup; }
            set { m_vecSup = value; OnPropertyChanged(); }
        }

        public float CollisionSize
        {
            get { return m_size; }
            set { m_size= value; OnPropertyChanged(); }
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

        int ICarGenerator.AlarmChance
        {
            get { return AlarmChance; }
            set { AlarmChance = (byte) value; OnPropertyChanged(); }
        }

        int ICarGenerator.LockedChance
        {
            get { return LockedChance; }
            set { LockedChance = (byte) value; OnPropertyChanged(); }
        }

        public CarGenerator()
        { }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Model = buf.ReadInt32();
            Position = buf.Read<Vector3D>();
            Heading = buf.ReadFloat();
            Color1 = buf.ReadInt16();
            Color2 = buf.ReadInt16();
            ForceSpawn = buf.ReadBool();
            AlarmChance = buf.ReadByte();
            LockedChance = buf.ReadByte();
            buf.ReadByte();
            MinDelay = buf.ReadUInt16();
            MaxDelay = buf.ReadUInt16();
            Timer = buf.ReadUInt32();
            Handle = buf.ReadInt32();
            Enabled = buf.ReadBool(2);
            IsBlocking = buf.ReadBool();
            buf.ReadByte();
            CollisionBoundingMin = buf.Read<Vector3D>();
            CollisionBoundingMax = buf.Read<Vector3D>();
            CollisionSize = buf.ReadFloat();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Model);
            buf.Write(Position);
            buf.Write(Heading);
            buf.Write(Color1);
            buf.Write(Color2);
            buf.Write(ForceSpawn);
            buf.Write(AlarmChance);
            buf.Write(LockedChance);
            buf.Write((byte) 0);
            buf.Write(MinDelay);
            buf.Write(MaxDelay);
            buf.Write(Timer);
            buf.Write(Handle);
            buf.Write((short) (Enabled ? -1 : 0));
            buf.Write(IsBlocking);
            buf.Write((byte) 0);
            buf.Write(CollisionBoundingMin);
            buf.Write(CollisionBoundingMax);
            buf.Write(CollisionSize);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x48;
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
                && Heading.Equals(other.Heading)
                && Color1.Equals(other.Color1)
                && Color2.Equals(other.Color2)
                && ForceSpawn.Equals(other.ForceSpawn)
                && AlarmChance.Equals(other.AlarmChance)
                && LockedChance.Equals(other.LockedChance)
                && MinDelay.Equals(other.MinDelay)
                && MaxDelay.Equals(other.MaxDelay)
                && Timer.Equals(other.Timer)
                && Handle.Equals(other.Handle)
                && Enabled.Equals(other.Enabled)
                && IsBlocking.Equals(other.IsBlocking)
                && CollisionBoundingMin.Equals(other.CollisionBoundingMin)
                && CollisionBoundingMax.Equals(other.CollisionBoundingMax)
                && CollisionSize.Equals(other.CollisionSize);
        }
    }
}
