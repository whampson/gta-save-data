using System;

namespace GTASaveData.GTA3
{
    [Flags]
    public enum StoredCarFlags
    {
        BulletProof     = 0b00001,
        FireProof       = 0b00010,
        ExplosionProof  = 0b00100,
        CollisionProof  = 0b01000,
        MeleeProof      = 0b10000
    }
}
