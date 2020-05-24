using System;
using System.Collections.Generic;
using System.Text;

namespace GTASaveData.GTA3
{
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
}
