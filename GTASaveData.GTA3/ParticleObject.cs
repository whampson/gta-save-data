using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Drawing;

namespace GTASaveData.GTA3
{
    public class ParticleObject : Placeable, IEquatable<ParticleObject>
    {
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
        private Vector m_target;
        private float m_spread;
        private float m_size;
        private uint m_color;
        private bool m_destroyWhenFar;
        private sbyte m_creationChance;
        private int m_unknown;

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

        public Vector Target
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

        public ParticleObject()
        {
            Target = new Vector();
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

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.ReadObjectData(buf, fmt);
            NextParticleObjectPointer = buf.ReadUInt32();
            PrevParticleObjectPointer = buf.ReadUInt32();
            ParticlePointer = buf.ReadUInt32();
            Timer = buf.ReadUInt32();
            Type = (ParticleObjectType) buf.ReadInt32();
            ParticleType = (ParticleType) buf.ReadInt32();
            NumEffectCycles = buf.ReadByte();
            SkipFrames = buf.ReadByte();
            buf.Align4Bytes();
            FrameCounter = buf.ReadUInt16();
            State = (ParticleObjectState) buf.ReadInt16();
            Target = buf.Read<Vector>();
            Spread = buf.ReadFloat();
            Size = buf.ReadFloat();
            Color = buf.ReadUInt32();
            DestroyWhenFar = buf.ReadBool();
            CreationChance = buf.ReadSByte();
            buf.Align4Bytes();
            if (fmt.PS2)
            {
                m_unknown = buf.ReadInt32();
            }

            Debug.Assert(buf.Offset == SizeOf<ParticleObject>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            base.WriteObjectData(buf, fmt);
            buf.Write(NextParticleObjectPointer);
            buf.Write(PrevParticleObjectPointer);
            buf.Write(ParticlePointer);
            buf.Write(Timer);
            buf.Write((int) Type);
            buf.Write((int) ParticleType);
            buf.Write(NumEffectCycles);
            buf.Write(SkipFrames);
            buf.Align4Bytes();
            buf.Write(FrameCounter);
            buf.Write((short) State);
            buf.Write(Target);
            buf.Write(Spread);
            buf.Write(Size);
            buf.Write(Color);
            buf.Write(DestroyWhenFar);
            buf.Write(CreationChance);
            buf.Align4Bytes();
            if (fmt.PS2)
            {
                buf.Write(m_unknown);
            }

            Debug.Assert(buf.Offset == SizeOf<ParticleObject>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (GTA3Save.IsJapanesePS2(fmt)) return 0x80;
            else if (fmt.PS2) return 0xA0;
            
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

            return base.Equals(other)
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
                && CreationChance.Equals(other.CreationChance);
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

    public enum ParticleType
    {
        // TODO: fix style
        PARTICLE_SPARK = 0,
        PARTICLE_SPARK_SMALL,
        PARTICLE_WHEEL_DIRT,
        PARTICLE_WHEEL_WATER,
        PARTICLE_BLOOD,
        PARTICLE_BLOOD_SMALL,
        PARTICLE_BLOOD_SPURT,
        PARTICLE_DEBRIS,
        PARTICLE_DEBRIS2,
        PARTICLE_WATER,
        PARTICLE_FLAME,
        PARTICLE_FIREBALL,
        PARTICLE_GUNFLASH,
        PARTICLE_GUNFLASH_NOANIM,
        PARTICLE_GUNSMOKE,
        PARTICLE_GUNSMOKE2,
        PARTICLE_SMOKE,
        PARTICLE_SMOKE_SLOWMOTION,
        PARTICLE_GARAGEPAINT_SPRAY,
        PARTICLE_SHARD,
        PARTICLE_SPLASH,
        PARTICLE_CARFLAME,
        PARTICLE_STEAM,
        PARTICLE_STEAM2,
        PARTICLE_STEAM_NY,
        PARTICLE_STEAM_NY_SLOWMOTION,
        PARTICLE_ENGINE_STEAM,
        PARTICLE_RAINDROP,
        PARTICLE_RAINDROP_SMALL,
        PARTICLE_RAIN_SPLASH,
        PARTICLE_RAIN_SPLASH_BIGGROW,
        PARTICLE_RAIN_SPLASHUP,
        PARTICLE_WATERSPRAY,
        PARTICLE_EXPLOSION_MEDIUM,
        PARTICLE_EXPLOSION_LARGE,
        PARTICLE_EXPLOSION_MFAST,
        PARTICLE_EXPLOSION_LFAST,
        PARTICLE_CAR_SPLASH,
        PARTICLE_BOAT_SPLASH,
        PARTICLE_BOAT_THRUSTJET,
        PARTICLE_BOAT_WAKE,
        PARTICLE_WATER_HYDRANT,
        PARTICLE_WATER_CANNON,
        PARTICLE_EXTINGUISH_STEAM,
        PARTICLE_PED_SPLASH,
        PARTICLE_PEDFOOT_DUST,
        PARTICLE_HELI_DUST,
        PARTICLE_HELI_ATTACK,
        PARTICLE_ENGINE_SMOKE,
        PARTICLE_ENGINE_SMOKE2,
        PARTICLE_CARFLAME_SMOKE,
        PARTICLE_FIREBALL_SMOKE,
        PARTICLE_PAINT_SMOKE,
        PARTICLE_TREE_LEAVES,
        PARTICLE_CARCOLLISION_DUST,
        PARTICLE_CAR_DEBRIS,
        PARTICLE_HELI_DEBRIS,
        PARTICLE_EXHAUST_FUMES,
        PARTICLE_RUBBER_SMOKE,
        PARTICLE_BURNINGRUBBER_SMOKE,
        PARTICLE_BULLETHIT_SMOKE,
        PARTICLE_GUNSHELL_FIRST,
        PARTICLE_GUNSHELL,
        PARTICLE_GUNSHELL_BUMP1,
        PARTICLE_GUNSHELL_BUMP2,
        PARTICLE_TEST,
        PARTICLE_BIRD_FRONT,
        PARTICLE_RAINDROP_2D
    }
}