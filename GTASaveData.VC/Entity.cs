using System;
using System.Numerics;

namespace GTASaveData.VC
{
    public abstract class Entity : SaveDataObject
    {
        private Matrix m_matrix;
        private EntityType m_entityType;
        private EntityStatus m_entityStatus;
        private EntityFlags m_entityFlags;

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

        public Matrix Matrix
        {
            get { return m_matrix; }
            set { m_matrix = value; OnPropertyChanged(); }
        }
        
        public Entity()
        {
            EntityType = EntityType.None;
            EntityStatus = EntityStatus.Abandoned;
            EntityFlags = EntityFlags.IsVisible;
            Matrix = Matrix.Identity;
        }

        public Entity(Entity other)
        {
            EntityType = other.EntityType;
            EntityStatus = other.EntityStatus;
            EntityFlags = other.EntityFlags;
            Matrix = new Matrix(other.Matrix);
        }

        public Vector3 Position
        {
            get { return Matrix.Position; }
            set
            {
                Matrix m = Matrix;
                m.Position = value;
                Matrix = m;
                OnPropertyChanged();
            }
        }

        public void SetHeading(float angle)
        {
            Matrix.RotateZ(angle);
        }

        public void SetOrientation(float xAngle, float yAngle, float zAngle)
        {
            Matrix.Rotate(xAngle, yAngle, zAngle);
        }

        public void ResetOrientation()
        {
            Matrix m = Matrix.Identity;
            m.Position = Matrix.Position;

            Matrix = m;
        }

        protected void LoadEntityFlags(DataBuffer buf, FileFormat fmt)
        {
            long eFlags = buf.ReadUInt32();
            eFlags |= (fmt.IsiOS)
                ? ((long) buf.ReadUInt16()) << 32
                : ((long) buf.ReadUInt32()) << 32;

            EntityFlags = (EntityFlags) (eFlags & ~0xFF);
            EntityType = (EntityType) (eFlags & 0x07);
            EntityStatus = (EntityStatus) ((eFlags & 0xF8) >> 3);
        }

        protected void SaveEntityFlags(DataBuffer buf, FileFormat fmt)
        {
            long eFlags = (long) EntityFlags;
            eFlags |= ((long) EntityType) & 0x07;
            eFlags |= (((long) EntityStatus) & 0x1F) << 3;
            buf.Write((uint) (eFlags & 0xFFFFFFFF));
            if (fmt.IsiOS)
                buf.Write((ushort) (eFlags >> 32));
            else
                buf.Write((uint) (eFlags >> 32));
        }
    }

    public enum EntityType : byte
    {
        None,
        Building,
        Vehicle,
        Ped,
        Object,
        Dummy
    }

    public enum EntityStatus : byte
    {
        Player,
        PlayerPlayBackFromBuffer,
        Simple,
        Physics,
        Abandoned,
        Wrecked,
        TrainMoving,
        TrainNotMoving,
        Heli,
        Plane,
        PlayerRemote,
        PlayerDisabled,
        Ghost
    }

    [Flags]
    public enum EntityFlags : long
    {
        None = 0,

        UsesCollision = (1L << 8),
        CollisionProcessed = (1L << 9),
        IsStatic = (1L << 10),
        HasContacted = (1L << 11),
        PedPhysics = (1L << 12),
        IsStuck = (1L << 13),
        IsInSafePosition = (1L << 14),
        UseCollisionRecords = (1L << 15),
        
        WasPostponed = (1L << 16),
        ExplosionProof = (1L << 17),
        IsVisible = (1L << 18),
        HasCollided = (1L << 19),
        RenderScorched = (1L << 20),
        HasBlip = (1L << 21),
        IsBigBuilding = (1L << 22),
        IsStreamBigBuilding = (1L << 23),
        
        RenderDamaged = (1L << 24),
        BulletProof = (1L << 25),
        FireProof = (1L << 26),
        CollisionProof = (1L << 27),
        MeleeProof = (1L << 28),
        OnlyDamagedByPlayer = (1L << 29),
        StreamingDontDelete = (1L << 30),
        RemoveFromWorld = (1L << 31),

        HasHitWall = (1L << 32),
        ImBeingRendered = (1L << 33),
        TouchingWater = (1L << 34),
        IsSubway = (1L << 35),
        DrawLast = (1L << 36),
        NoBrightHeadLights = (1L << 37),
        DoNotRender = (1L << 38),
        DistanceFade = (1L << 39),

        UnknownFlag1 = (1L << 40),
        UnknownFlag2 = (1L << 41),
        OffScreen = (1L << 42),
        StaticWaitingForCollision = (1L << 43),
        DontStream = (1L << 44),
        UnderWater = (1L << 45),
        HasPreRenderEffects = (1L << 46),
    }
}
