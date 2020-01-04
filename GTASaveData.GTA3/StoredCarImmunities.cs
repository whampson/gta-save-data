using System;

namespace GTASaveData.GTA3
{
    [Flags]
    public enum StoredCarImmunities
    {
        BulletProof     = 0b_0000_0001,
        FireProof       = 0b_0000_0010,
        ExplosionProof  = 0b_0000_0100,
        CollisionProof  = 0b_0000_1000,
        MeleeProof      = 0b_0001_0000
    }
}
