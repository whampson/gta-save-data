using System;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Flags representing the status of collected cars for the Portand Harbor Import/Export garage.
    /// </summary>
    [Flags]
    public enum CollectCars1
    {
        Securicar   = 0b_0000_0000_0000_0001,
        Moonbeam    = 0b_0000_0000_0000_0010,
        Coach       = 0b_0000_0000_0000_0100,
        Flatbed     = 0b_0000_0000_0000_1000,
        Linerunner  = 0b_0000_0000_0001_0000,
        Trashmaster = 0b_0000_0000_0010_0000,
        Patriot     = 0b_0000_0000_0100_0000,
        MrWhoopee   = 0b_0000_0000_1000_0000,
        Blista      = 0b_0000_0001_0000_0000,
        Mule        = 0b_0000_0010_0000_0000,
        Yankee      = 0b_0000_0100_0000_0000,
        Bobcat      = 0b_0000_1000_0000_0000,
        Dodo        = 0b_0001_0000_0000_0000,
        Bus         = 0b_0010_0000_0000_0000,
        Rumpo       = 0b_0100_0000_0000_0000,
        Pony        = 0b_1000_0000_0000_0000,
    }

    /// <summary>
    /// Flags representing the status of collected cars for the Pike Creek Import/Export garage.
    /// </summary>
    [Flags]
    public enum CollectCars2
    {
        Sentinel    = 0b_0000_0000_0000_0001,
        Cheetah     = 0b_0000_0000_0000_0010,
        Banshee     = 0b_0000_0000_0000_0100,
        Idaho       = 0b_0000_0000_0000_1000,
        Infernus    = 0b_0000_0000_0001_0000,
        Taxi        = 0b_0000_0000_0010_0000,
        Kuruma      = 0b_0000_0000_0100_0000,
        Stretch     = 0b_0000_0000_1000_0000,
        Perennial   = 0b_0000_0001_0000_0000,
        Stinger     = 0b_0000_0010_0000_0000,
        Manana      = 0b_0000_0100_0000_0000,
        Landstalker = 0b_0000_1000_0000_0000,
        Stallion    = 0b_0001_0000_0000_0000,
        BFInjection = 0b_0010_0000_0000_0000,
        Cabbie      = 0b_0100_0000_0000_0000,
        Esperanto   = 0b_1000_0000_0000_0000,
    }

    /// <summary>
    /// Flags representing the status of collected cars for an unused Import/Export garage.
    /// </summary>
    [Flags]
    public enum CollectCars3
    {
        Landstalker = 0b_0000_0000_0000_0001
    }

    /// <summary>
    /// Flags representing the status of collected cars for the Emergency Vehicle Crane in Portland Harbor.
    /// </summary>
    [Flags]
    public enum CollectCarsMilitaryCrane
    {
        Firetruck   = 0b_0000_0000_0000_0001,
        Ambulance   = 0b_0000_0000_0000_0010,
        Enforcer    = 0b_0000_0000_0000_0100,
        FbiCar      = 0b_0000_0000_0000_1000,
        Rhino       = 0b_0000_0000_0001_0000,
        BarracksOL  = 0b_0000_0000_0010_0000,
        Police      = 0b_0000_0000_0100_0000,
    }
}
