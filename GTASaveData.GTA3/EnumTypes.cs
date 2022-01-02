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
        Unknown,
        Random,
        Mission
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

    public enum WeatherType
    {
        None = -1,
        Sunny,
        Cloudy,
        Rainy,
        Foggy
    }
}
