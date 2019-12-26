using System;

namespace GTASaveData.GTA3
{
    [Flags]
    public enum StoredCarImmunities
    {
        BulletProof = 1 << 0,
        FireProof = 1 << 1,
        ExplosionProof = 1 << 2,
        CollisionProof = 1 << 3,
        MeleeProof = 1 << 4
    }
}
