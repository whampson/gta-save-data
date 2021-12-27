using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using GTASaveData.Interfaces;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class ParticleObject : SaveDataObject,
        IEquatable<ParticleObject>, IDeepClonable<ParticleObject>
    {
        private Vector3 m_position;
        private uint m_nextParticleObjectPointer;
        private uint m_prevParticleObjectPointer;
        private uint m_particlePointer;
        private uint m_timer;
        private ParticleObjectType m_type;
        private ParticleType m_particleType;
        private byte m_numEffectCycles;
        private byte m_skipFrames;
        private ushort m_frameCounter;
        private ParticleObjectState m_state;
        private Vector3 m_target;
        private float m_spread;
        private float m_size;
        private uint m_color;
        private bool m_destroyWhenFar;
        private sbyte m_creationChance;
        private int m_unknown;

        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint NextParticleObjectPointer
        {
            get { return m_nextParticleObjectPointer; }
            set { m_nextParticleObjectPointer = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint PrevParticleObjectPointer
        {
            get { return m_prevParticleObjectPointer; }
            set { m_prevParticleObjectPointer = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public uint ParticlePointer
        {
            get { return m_particlePointer; }
            set { m_particlePointer = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public ParticleObjectType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public ParticleType ParticleType
        {
            get { return m_particleType; }
            set { m_particleType = value; OnPropertyChanged(); }
        }

        public byte NumEffectCycles
        {
            get { return m_numEffectCycles; }
            set { m_numEffectCycles = value; OnPropertyChanged(); }
        }

        public byte SkipFrames
        {
            get { return m_skipFrames; }
            set { m_skipFrames = value; OnPropertyChanged(); }
        }

        public ushort FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        [Obsolete("Value overridden by the game.")]
        public ParticleObjectState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public Vector3 Target
        {
            get { return m_target; }
            set { m_target = value; OnPropertyChanged(); }
        }

        public float Spread
        {
            get { return m_spread; }
            set { m_spread = value; OnPropertyChanged(); }
        }

        public float Size
        {
            get { return m_size; }
            set { m_size = value; OnPropertyChanged(); }
        }

        public uint Color
        {
            get { return m_color; }
            set { m_color = value; OnPropertyChanged(); }
        }

        public bool DestroyWhenFar
        {
            get { return m_destroyWhenFar; }
            set { m_destroyWhenFar = value; OnPropertyChanged(); }
        }

        public sbyte CreationChance
        {
            get { return m_creationChance; }
            set { m_creationChance = value; OnPropertyChanged(); }
        }

        public int Unknown
        {
            get { return m_unknown; }
            set { m_unknown = value; OnPropertyChanged(); }
        }

        public ParticleObject()
        {
            Position = new Vector3();
            Target = new Vector3();
        }

        public ParticleObject(ParticleObject other)
        {
            Position = other.Position;
            NextParticleObjectPointer = other.NextParticleObjectPointer;
            PrevParticleObjectPointer = other.PrevParticleObjectPointer;
            ParticlePointer = other.ParticlePointer;
            Timer = other.Timer;
            Type = other.Type;
            ParticleType = other.ParticleType;
            NumEffectCycles = other.NumEffectCycles;
            SkipFrames = other.SkipFrames;
            FrameCounter = other.FrameCounter;
            State = other.State;
            Target = other.Target;
            Spread = other.Spread;
            Size = other.Size;
            Color = other.Color;
            DestroyWhenFar = other.DestroyWhenFar;
            CreationChance = other.CreationChance;
            Unknown = other.Unknown;
        }

        public Color GetColor()
        {
            uint rgb = Color & (0xFFFFFF00 >> 8);
            uint a = Color & 0xFF;

            return System.Drawing.Color.FromArgb((int) ((a << 24) | rgb));
        }

        public void SetColor(Color c)
        {
            uint argb = (uint) c.ToArgb();

            uint a = argb >> 24;
            uint rgb = argb & (0xFFFFFF00 >> 8);

            Color = (rgb << 8) | a;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            var t = prm.FileType;

            if (!t.IsPS2) buf.Skip(4);
            buf.Skip(48);
            Position = buf.ReadStruct<Vector3>();
            buf.Skip(4);
            if (!(t.IsPS2 && t.FlagJapan)) buf.Skip(8);
            if (t.IsPS2 && !t.FlagJapan) buf.Skip(24);
            NextParticleObjectPointer = buf.ReadUInt32();
            PrevParticleObjectPointer = buf.ReadUInt32();
            ParticlePointer = buf.ReadUInt32();
            Timer = buf.ReadUInt32();
            Type = (ParticleObjectType) buf.ReadInt32();
            ParticleType = (ParticleType) buf.ReadInt32();
            NumEffectCycles = buf.ReadByte();
            SkipFrames = buf.ReadByte();
            buf.Align4();
            FrameCounter = buf.ReadUInt16();
            State = (ParticleObjectState) buf.ReadInt16();
            Target = buf.ReadStruct<Vector3>();
            Spread = buf.ReadFloat();
            Size = buf.ReadFloat();
            Color = buf.ReadUInt32();
            DestroyWhenFar = buf.ReadBool();
            CreationChance = buf.ReadSByte();
            buf.Align4();
            if (t.IsPS2) Unknown = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<ParticleObject>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            var t = prm.FileType;

            if (!t.IsPS2) buf.Skip(4);
            buf.Skip(48);
            buf.Write(Position);
            buf.Skip(4);
            if (!(t.IsPS2 && t.FlagJapan)) buf.Skip(8);
            if (t.IsPS2 && !t.FlagJapan) buf.Skip(24);
            buf.Write(NextParticleObjectPointer);
            buf.Write(PrevParticleObjectPointer);
            buf.Write(ParticlePointer);
            buf.Write(Timer);
            buf.Write((int) Type);
            buf.Write((int) ParticleType);
            buf.Write(NumEffectCycles);
            buf.Write(SkipFrames);
            buf.Align4();
            buf.Write(FrameCounter);
            buf.Write((short) State);
            buf.Write(Target);
            buf.Write(Spread);
            buf.Write(Size);
            buf.Write(Color);
            buf.Write(DestroyWhenFar);
            buf.Write(CreationChance);
            buf.Align4();
            if (t.IsPS2) buf.Write(Unknown);

            Debug.Assert(buf.Offset == SizeOf<ParticleObject>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            var t = prm.FileType;
            if (t.IsPS2 && t.FlagJapan) return 0x80;
            if (t.IsPS2) return 0xA0;

            return 0x88;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParticleObject);
        }

        public bool Equals(ParticleObject other)
        {
            if (other == null)
            {
                return false;
            }

            return Position.Equals(other.Position)
                && NextParticleObjectPointer.Equals(other.NextParticleObjectPointer)
                && PrevParticleObjectPointer.Equals(other.PrevParticleObjectPointer)
                && ParticlePointer.Equals(other.ParticlePointer)
                && Timer.Equals(other.Timer)
                && Type.Equals(other.Type)
                && ParticleType.Equals(other.ParticleType)
                && NumEffectCycles.Equals(other.NumEffectCycles)
                && SkipFrames.Equals(other.SkipFrames)
                && FrameCounter.Equals(other.FrameCounter)
                && State.Equals(other.State)
                && Target.Equals(other.Target)
                && Spread.Equals(other.Spread)
                && Size.Equals(other.Size)
                && Color.Equals(other.Color)
                && DestroyWhenFar.Equals(other.DestroyWhenFar)
                && CreationChance.Equals(other.CreationChance)
                && Unknown.Equals(other.Unknown);
        }

        public ParticleObject DeepClone()
        {
            return new ParticleObject(this);
        }
    }

    public enum ParticleObjectState
    {
        Initialized,
        UpdateClose,
        UpdateFar,
        Free,
    }

    public enum ParticleObjectType
    {
        PavementSteam,
        PavementSteamSlowMotion,
        WallSteam,
        WallSteamSlowMotion,
        DarkSmoke,
        FireHydrant,
        CarWaterSplash,
        PedWaterSplash,
        SplashesAround,
        SmallFire,
        BigFire,
        DryIce,
        DryIceSlowMotion,
        FireTrail,
        SmokeTrail,
        FireballAndSmoke,
        RocketTrail,
        ExplosionOnce,
        CatalinasGunFlash,
        CatalinasShotgunFlash
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
