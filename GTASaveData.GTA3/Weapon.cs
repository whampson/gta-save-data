using System.ComponentModel;

namespace GTASaveData.GTA3
{
    public enum Weapon
    {
        [Description("(none)")]
        None,

        [Description("Bat")]
        Bat,

        [Description("Pistol")]
        Pistol,

        [Description("Uzi")]
        Uzi,

        [Description("Shotgun")]
        Shotgun,

        [Description("AK47")]
        AK47,

        [Description("M16")]
        M16,

        [Description("Sniper Rifle")]
        SniperRifle,

        [Description("Rocket Launcher")]
        RocketLauncher,

        [Description("Flamethrower")]
        Flamethrower,

        [Description("Molotov Cocktail")]
        Molotov,

        [Description("Grenade")]
        Grenade,

        [Description("Detonator")]
        Detonator
    }
}
