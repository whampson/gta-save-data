using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
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
            Matrix = other.Matrix;
        }

        public Vector3D GetPosition()
        {
            return Matrix.Position;
        }

        public void SetPosition(Vector3D pos)
        {
            Matrix m = Matrix;
            m.Position = pos;

            Matrix = m;
        }

        public void SetHeading(float angle)
        {
            Matrix = Matrix.RotateZ(Matrix, angle);
        }

        public void SetOrientation(float xAngle, float yAngle, float zAngle)
        {
            Matrix = Matrix.Rotate(Matrix, xAngle, yAngle, zAngle);
        }

        public void ResetOrientation()
        {
            Matrix m = Matrix.Identity;
            m.Position = Matrix.Position;

            Matrix = m;
        }

        protected void LoadEntityFlags(StreamBuffer buf, FileFormat fmt)
        {
            long eFlags = buf.ReadUInt32();
            eFlags |= (fmt.IsiOS)
                ? ((long) buf.ReadUInt16()) << 32
                : ((long) buf.ReadUInt32()) << 32;

            EntityFlags = (EntityFlags) (eFlags & ~0xFF);
            EntityType = (EntityType) (eFlags & 0x07);
            EntityStatus = (EntityStatus) ((eFlags & 0xF8) >> 3);
        }

        protected void SaveEntityFlags(StreamBuffer buf, FileFormat fmt)
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
        PlayerDisabled
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
        RenderDamaged = (1L << 23),
        
        BulletProof = (1L << 24),
        FireProof = (1L << 25),
        CollisionProof = (1L << 26),
        MeeeProof = (1L << 27),
        OnlyDamagedByPlayer = (1L << 28),
        StreamingDontDelete = (1L << 29),
        ZoneCulled = (1L << 30),
        ZoneCulled2 = (1L << 31),

        RemoveFromWorld = (1L << 32),
        HasHitWall = (1L << 33),
        ImBeingRendered = (1L << 34),
        TouchingWater = (1L << 35),
        IsSubway = (1L << 36),
        DrawLast = (1L << 37),
        NoBrightHeadLights = (1L << 38),
        DoNotRender = (1L << 39),

        DistanceFade = (1L << 40),
        UnknownFlag = (1L << 41)
    }

    public enum PoolType
    {
        None,
        Treadable,
        Building,
        Object,
        Dummy
    }
}
