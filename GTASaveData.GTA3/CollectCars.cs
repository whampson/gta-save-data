using System;

namespace GTASaveData.GTA3
{
    [Flags]
    public enum CollectCars1
    {
        Securicar   = (1 << 0),
        Moonbeam    = (1 << 1),
        Coach       = (1 << 2),
        Flatbed     = (1 << 3),
        Linerunner  = (1 << 4),
        Trashmaster = (1 << 5),
        Patriot     = (1 << 6),
        MrWhoopee   = (1 << 7),
        Blista      = (1 << 8),
        Mule        = (1 << 9),
        Yankee      = (1 << 10),
        Bobcat      = (1 << 11),
        Dodo        = (1 << 12),
        Bus         = (1 << 13),
        Rumpo       = (1 << 14),
        Pony        = (1 << 15),
    }

    [Flags]
    public enum CollectCars2
    {
        Sentinel    = (1 << 0),
        Cheetah     = (1 << 1),
        Banshee     = (1 << 2),
        Idaho       = (1 << 3),
        Infernus    = (1 << 4),
        Taxi        = (1 << 5),
        Kuruma      = (1 << 6),
        Stretch     = (1 << 7),
        Perennial   = (1 << 8),
        Stinger     = (1 << 9),
        Manana      = (1 << 10),
        Landstalker = (1 << 11),
        Stallion    = (1 << 12),
        BFInjection = (1 << 13),
        Cabbie      = (1 << 14),
        Esperanto   = (1 << 15),
    }

    [Flags]
    public enum CollectCars3
    {
        Landstalker = (1 << 0)
    }

    [Flags]
    public enum CollectCarsMilitaryCrane
    {
        Firetruck   = (1 << 0),
        Ambulance   = (1 << 1),
        Enforcer    = (1 << 2),
        FbiCar      = (1 << 3),
        Rhino       = (1 << 4),
        BarracksOL  = (1 << 5),
        Police      = (1 << 6),
    }
}
