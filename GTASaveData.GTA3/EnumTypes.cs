using System;
using System.ComponentModel;

namespace GTASaveData.GTA3
{
    public enum BombType
    {
        [Description("(none)")]
        None,

        [Description("Timer")]
        Timer,

        [Description("Ignition")]
        OnIgnition,

        [Description("Remote")]
        Remote,

        [Description("Timer (armed)")]
        TimerArmed,

        [Description("Ignition (armed)")]
        OnIgnitionArmed
    }

    public enum GarageState
    {
        Closed,
        Opened,
        Closing,
        Opening,
        OpenedContainsCar,
        ClosedAfterDropOff
    }

    public enum GarageType
    {
        [Description("(none)")]
        None,
        Mission,
        BombShop1,
        BombShop2,
        BombShop3,
        Respray,
        CollectorsItems,
        CollectSpecificCars,
        CollectCars1,
        CollectCars2,
        CollectCars3,
        ForCarToComeOutOf,
        SixtySeconds,
        Crusher,
        MissionKeepCar,
        ForScriptToOpen,
        Hideout1,
        Hideout2,
        Hideout3,
        ForScriptToOpenAndClose,
        KeepsOpeningForSpecificCar,
        MissionKeepCarRemainClosed
    }

    public enum Language
    {
        English,
        French,
        German,
        Italian,
        Spanish,
        Japanese,
    }

    public enum Level
    {
        [Description("(none)")]
        None,

        [Description("Portland")]
        Industrial,

        [Description("Staunton Island")]
        Commercial,

        [Description("Shoreside Vale")]
        Suburban
    }

    public enum QuickSaveState
    {
        [Description("(none)")]
        None,

        Normal,
        OnMission
    }

    public enum RadioStation
    {
        [Description("Head Radio")]
        HeadRadio,

        [Description("Double Cleff FM")]
        DoubleClef,

        [Description("Jah Radio")]
        JahRadio,

        [Description("Rise FM")]
        RiseFM,

        [Description("Lips 106")]
        Lips106,

        [Description("Game FM")]
        GameFM,

        [Description("MSX FM")]
        MsxFM,

        [Description("Flashback 95.6")]
        Flashback,

        [Description("Chatterbox 109")]
        Chatterbox,

        [Description("MP3 Player")]
        UserTrack,

        [Description("Police Radio")]
        PoliceRadio,

        [Description("(none)")]
        None
    }

    [Flags]
    public enum StoredCarFlags
    {
        BulletProof = 0b00001,
        FireProof = 0b00010,
        ExplosionProof = 0b00100,
        CollisionProof = 0b01000,
        MeleeProof = 0b10000
    }

    public enum WeatherType
    {
        [Description("(none)")]
        None = -1,

        Sunny,
        Cloudy,
        Rainy,
        Foggy
    }
}
