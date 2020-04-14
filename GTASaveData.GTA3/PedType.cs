using System;

namespace GTASaveData.GTA3
{
    public enum PedType
    {
        Player1,
        Player2,
        Player3,
        Player4,
        CivMale,
        CivFemale,
        Cop,
        Gang1,
        Gang2,
        Gang3,
        Gang4,
        Gang5,
        Gang6,
        Gang7,
        Gang8,
        Gang9,
        Emergency,
        Fireman,
        Criminal,
        Prostitute = 20,
        Special,
    }

    [Flags]
    public enum PedTypeFlags
    {
        Player1 = (1 << 0),
        Player2 = (1 << 1),
        Player3 = (1 << 2),
        Player4 = (1 << 3),
        CivMale = (1 << 4),
        CivFemale = (1 << 5),
        Cop = (1 << 6),
        Gang1 = (1 << 7),
        Gang2 = (1 << 8),
        Gang3 = (1 << 9),
        Gang4 = (1 << 10),
        Gang5 = (1 << 11),
        Gang6 = (1 << 12),
        Gang7 = (1 << 13),
        Gang8 = (1 << 14),
        Gang9 = (1 << 15),
        Emergency = (1 << 16),
        Prostitute = (1 << 17),
        Criminal = (1 << 18),
        Special = (1 << 19),
        Gun = (1 << 20),
        CopCar = (1 << 21),
        FastCar = (1 << 22),
        Explosion = (1 << 23),
        Fireman = (1 << 24),
        DeadPeds = (1 << 25),
    }
}
