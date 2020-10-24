using System.Collections.Generic;

namespace GTASaveData.Interfaces
{
    public interface IGarageData
    {
        bool FreeBombs { get; set; }
        bool FreeResprays { get; set; }
        IEnumerable<IStoredCar> CarsInSafeHouse { get; }
        IEnumerable<IGarage> Garages { get; }
    }
}
