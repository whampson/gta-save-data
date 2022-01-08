using System;

namespace GTASaveData.GTA3
{
    public enum AndOrState
    {
        None = 0,
        And1 = 1,
        And2,
        And3,
        And4,
        And5,
        And6,
        And7,
        And8,
        Or1 = 21,
        Or2,
        Or3,
        Or4,
        Or5,
        Or6,
        Or7,
        Or8
    }

    public enum BombType
    {
        None,
        Timer,
        OnIgnition,
        Remote,
        TimerArmed,
        OnIgnitionArmed
    }

    public enum CameraMode
    {
        None,
        Bumper = 0,
        Near,
        Middle,
        Far,
        TopDown,
        Cinematic
    }

    public enum CharCreatedBy
    {
        Random = 1,
        Mission
    }

    public enum GarageState
    {
        FullyClosed,
        Opened,
        Closing,
        Opening,
        OpenedContainsCar,
        ClosedContainsCar,
        AfterDropOff
    }

    public enum GarageType
    {
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
        None,
        Industrial,
        Commercial,
        Suburban
    }

    public enum QuickSaveState
    {
        None,
        Normal,
        OnMission
    }

    public enum RadioStation
    {
        HeadRadio,
        DoubleClef,
        JahRadio,
        RiseFM,
        Lips106,
        GameFM,
        MsxFM,
        Flashback,
        Chatterbox,
        UserTrack,
        PoliceRadio,
        None
    }

    [Flags]
    public enum StoredCarFlags
    {
        BulletProof = 0b00001,
        FireProof = 0b00010,
        ExplosionProof = 0b00100,
        CollisionProof = 0b01000,
        MeleeProof = 0b10000,

        None = 0,
        All = BulletProof | FireProof | ExplosionProof | CollisionProof | MeleeProof
    }

    /// <summary>
    /// Portland import/export garage list.
    /// </summary>
    [Flags]
    public enum CollectCars1Types
    {
        Securicar = (1 << 0),
        Moonbeam = (1 << 1),
        Coach = (1 << 2),
        Flatbed = (1 << 3),
        Linerunner = (1 << 4),
        Trashmaster = (1 << 5),
        Patriot = (1 << 6),
        MrWhoopee = (1 << 7),
        Blista = (1 << 8),
        Mule = (1 << 9),
        Yankee = (1 << 10),
        Bobcat = (1 << 11),
        Dodo = (1 << 12),
        Bus = (1 << 13),
        Rumpo = (1 << 14),
        Pony = (1 << 15),
    }

    /// <summary>
    /// Shoreside Vale import/export garage list.
    /// </summary>
    [Flags]
    public enum CollectCars2Types
    {
        Sentinel = (1 << 0),
        Cheetah = (1 << 1),
        Banshee = (1 << 2),
        Idaho = (1 << 3),
        Infernus = (1 << 4),
        Taxi = (1 << 5),
        Kuruma = (1 << 6),
        Stretch = (1 << 7),
        Perennial = (1 << 8),
        Stinger = (1 << 9),
        Manana = (1 << 10),
        Landstalker = (1 << 11),
        Stallion = (1 << 12),
        BFInjection = (1 << 13),
        Cabbie = (1 << 14),
        Esperanto = (1 << 15),
    }

    /// <summary>
    /// Unused and useless import/export garage list.
    /// </summary>
    /// <remarks>
    /// Only accepts Landstalkers. Only the first bit can be set;
    /// all the other bits cannot be set in the game due to how the collection logic works.
    /// </remarks>
    [Flags]
    public enum CollectCar3Types
    {
        Landstalker = (1 << 0),
    }

    public enum WeatherType
    {
        None = -1,
        Sunny,
        Cloudy,
        Rainy,
        Foggy
    }
}
