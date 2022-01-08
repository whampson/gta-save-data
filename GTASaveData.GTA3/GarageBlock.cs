using GTASaveData.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// The data block in GTA3 save files that stores garage information,
    /// including vehicles stored by the player.
    /// </summary>
    public class GarageBlock : SaveDataObject,
        IEquatable<GarageBlock>, IDeepClonable<GarageBlock>
    {
        public const int NumGarageStoredCars = 6;
        public const int NumSafeHouses = 3;
        public const int NumStoredCars = NumSafeHouses * NumGarageStoredCars;

        private const int JunkSize = 244;

        private int m_numGarages;
        private bool m_bombsAreFree;
        private bool m_respraysAreFree;
        private int m_carsCollected;
        private int m_bankVansCollected;
        private int m_policeCarsCollected;
        private CollectCars1Types m_carTypesCollected1;
        private CollectCars2Types m_carTypesCollected2;
        private CollectCar3Types m_carTypesCollected3;
        private uint m_lastTimeHelpMessage;
        private ObservableArray<StoredCar> m_carsInSafeHouse;
        private ObservableArray<Garage> m_garages;

        /// <summary>
        /// The number of slots used in the <see cref="Garages"/> array.
        /// </summary>
        public int NumGarages
        { 
            get { return m_numGarages; }
            set { m_numGarages = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// When set, bombs fitted to the player's vehicle at 8-Ball's Bomb Shop incur zero cost.
        /// </summary>
        public bool FreeBombs
        { 
            get { return m_bombsAreFree; }
            set { m_bombsAreFree = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// When set, Pay'n'Spray visits incur zero cost.
        /// </summary>
        public bool FreeResprays
        { 
            get { return m_respraysAreFree; }
            set { m_respraysAreFree = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Unused variable meant to keep track of cars collected.
        /// Leftover from early implementation of Import/Export garages?
        /// </summary>
        /// <remarks>
        /// This field is totally useless; it is not used at all by the game.
        /// </remarks>
        public int CarsCollected
        { 
            get { return m_carsCollected; }
            set { m_carsCollected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The number of Securicars collected.
        /// </summary>
        /// <remarks>
        /// This value is only incremented when the <see cref="Garage.Type"/> is
        /// <see cref="GarageType.CollectSpecificCars"/> and the <see cref="Garage.TargetModelIndex"/>
        /// is 118 (<c>MI_SECURICA</c>).
        /// </remarks>
        public int BankVansCollected
        { 
            get { return m_bankVansCollected; }
            set { m_bankVansCollected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The number of Police cars collected.
        /// </summary>
        /// <remarks>
        /// This value is only incremented when the <see cref="Garage.Type"/> is
        /// <see cref="GarageType.CollectSpecificCars"/> and the <see cref="Garage.TargetModelIndex"/>
        /// is 116 (<c>MI_POLICE</c>).
        /// <para>
        /// There is no garage in the vanilla game set up to collect Police cars, but
        /// the functionality is still there, so it's possible to create one.
        /// </para>
        /// </remarks>
        public int PoliceCarsCollected
        { 
            get { return m_policeCarsCollected; }
            set { m_policeCarsCollected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Collected car mask for <see cref="GarageType.CollectCars1"/>.
        /// </summary>
        /// <remarks>
        /// This is the list of cars collected for the Portland Import/Export
        /// garage in the vanilla game.
        /// </remarks>
        public CollectCars1Types CarTypesCollected1
        { 
            get { return m_carTypesCollected1; }
            set { m_carTypesCollected1 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Collected car mask for <see cref="GarageType.CollectCars2"/>.
        /// </summary>
        /// <remarks>
        /// This is the list of cars collected for the Shoreside Vale Import/Export
        /// garage in the vanilla game.
        /// </remarks>
        public CollectCars2Types CarTypesCollected2
        { 
            get { return m_carTypesCollected2; }
            set { m_carTypesCollected2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Collected car mask for <see cref="GarageType.CollectCars3"/>.
        /// </summary>
        /// <remarks>
        /// This garage type is not used in the vanilla game, but it still functions... sort of.
        /// Interestingly, all 16 vehicles needed are Landstalkers, but only one of them can be
        /// "collected" due to how the game's collection logic works.
        /// </remarks>
        public CollectCar3Types CarTypesCollected3
        { 
            get { return m_carTypesCollected3; }
            set { m_carTypesCollected3 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The last time the <c>GA_21</c> message was shown.
        /// </summary>
        /// <remarks>
        /// <c>GA_21</c>: <i>You cannot store any more cars in this garage.</i>
        /// </remarks>
        public uint LastTimeHelpMessage
        { 
            get { return m_lastTimeHelpMessage; }
            set { m_lastTimeHelpMessage = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of cars stored in safehouse garages by the player.
        /// </summary>
        /// <remarks>
        /// The cars in this list are interpolated based on the safehouse
        /// at which they're stored; <c>CarsInSafeHouse[0]</c> is car 0 for
        /// the Portland safehouse, <c>CarsInSafeHouse[1]</c> is car 0 for
        /// the Staunton Island safehouse, <c>CarsInSafeHouse[2]</c> is car
        /// 0 for the Shoreside Vale safehouse, and so on. Use
        /// <see cref="CarsInSafeHouse1"/>, <see cref="CarsInSafeHouse2"/>,
        /// and <see cref="CarsInSafeHouse3"/> to get a list of stored cars
        /// for each safehouse.
        /// </remarks>
        public ObservableArray<StoredCar> CarsInSafeHouse
        { 
            get { return m_carsInSafeHouse; }
            set { m_carsInSafeHouse = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of garages in use by the game script.
        /// </summary>
        /// <remarks>
        /// The number of slots in use is controlled by <see cref="NumGarages"/>. The
        /// total size of the array is controlled by  <see cref="GTA3SaveParams.NumGarages"/>.
        /// </remarks>
        public ObservableArray<Garage> Garages
        { 
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of cars stored in <see cref="GarageType.Hideout1"/>
        /// (Portland safehouse).
        /// </summary>
        public IEnumerable<StoredCar> CarsInSafeHouse1
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % NumSafeHouses) == 0)
                    {
                        yield return CarsInSafeHouse[i];
                    }
                }
            }
        }

        /// <summary>
        /// The list of cars stored in <see cref="GarageType.Hideout2"/>
        /// (Staunton Island safehouse).
        /// </summary>
        public IEnumerable<StoredCar> CarsInSafeHouse2
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % NumSafeHouses) == 1)
                    {
                        yield return CarsInSafeHouse[i];
                    }
                }
            }
        }

        /// <summary>
        /// The list of cars stored in <see cref="GarageType.Hideout3"/>
        /// (Shoreside Vale safehouse).
        /// </summary>
        public IEnumerable<StoredCar> CarsInSafeHouse3
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % NumSafeHouses) == 2)
                    {
                        yield return CarsInSafeHouse[i];
                    }
                }
            }
        }

        public GarageBlock()
        {
            CarsInSafeHouse = ArrayHelper.CreateArray<StoredCar>(NumStoredCars);
            Garages = new ObservableArray<Garage>();
        }

        public GarageBlock(GarageBlock other)
        {
            NumGarages = other.NumGarages;
            FreeBombs = other.FreeBombs;
            FreeResprays = other.FreeResprays;
            CarsCollected = other.CarsCollected;
            BankVansCollected = other.BankVansCollected;
            PoliceCarsCollected = other.PoliceCarsCollected;
            CarTypesCollected1 = other.CarTypesCollected1;
            CarTypesCollected2 = other.CarTypesCollected2;
            CarTypesCollected3 = other.CarTypesCollected3;
            LastTimeHelpMessage = other.LastTimeHelpMessage;
            CarsInSafeHouse = ArrayHelper.DeepClone(other.CarsInSafeHouse);
            Garages = ArrayHelper.DeepClone(other.Garages);
        }

        /// <summary>
        /// Returns the next free slot in the <see cref="Garages"/> array, or <c>null</c>if no slots are free.
        /// </summary>
        public Garage FindNextFreeSlot()
        {
            return Garages.FirstOrDefault(g => !g.IsUsed());
        }

        /// <summary>
        /// Returns the first garage of the specified type, or <c>null</c> if none exist.
        /// </summary>
        public Garage FindGarageType(GarageType type)
        {
            return Garages.FirstOrDefault(g => g.Type == type);
        }

        /// <summary>
        /// Returns the garage that intersects the specified point, or <c>null</c> if none exist.
        /// </summary>
        public Garage FindGarageAt(Vector3 point)
        {
            foreach (var g in Garages)
            {
                if (g.IsPointWithinGarage(point))
                {
                    return g;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the garage of a specified type that intersects the specified point, or <c>null</c> if none exist.
        /// </summary>
        public Garage FindGarageTypeAt(GarageType type, Vector3 point)
        {
            foreach (var g in Garages.Where(x => x.Type == type))
            {
                if (g.IsPointWithinGarage(point))
                {
                    return g;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of garages that intersect the specified sphere, or <c>null</c> if none exist.
        /// </summary>
        public IEnumerable<Garage> FindGaragesNear(Vector3 point, float radius)
        {
            var nearest = new List<Garage>();
            foreach (var g in Garages)
            {
                if (g.IsGarageWithinSphere(point, radius))
                {
                    nearest.Add(g);
                }
            }

            return nearest;
        }

        /// <summary>
        /// Returns a list of garages of the specified type that intersect the specified sphere, or <c>null</c> if none exist.
        /// </summary>
        public IEnumerable<Garage> FindGarageTypesNear(GarageType type, Vector3 point, float radius)
        {
            var nearest = new List<Garage>();
            foreach (var g in Garages.Where(x => x.Type == type))
            {
                if (g.IsGarageWithinSphere(point, radius))
                {
                    nearest.Add(g);
                }
            }

            return nearest;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            NumGarages = buf.ReadInt32();
            FreeBombs = buf.ReadBool(4);
            FreeResprays = buf.ReadBool(4);
            CarsCollected = buf.ReadInt32();
            BankVansCollected = buf.ReadInt32();
            PoliceCarsCollected = buf.ReadInt32();
            CarTypesCollected1 = (CollectCars1Types) buf.ReadInt32();
            CarTypesCollected2 = (CollectCars2Types) buf.ReadInt32();
            CarTypesCollected3 = (CollectCar3Types) buf.ReadInt32();
            LastTimeHelpMessage = buf.ReadUInt32();
            CarsInSafeHouse = buf.ReadArray<StoredCar>(NumStoredCars);
            Garages = buf.ReadArray<Garage>(p.NumGarages);

            // Game writes some garbage here due to incorrect size calculation
            buf.Skip(JunkSize);

            Debug.Assert(buf.Offset == SizeOf<GarageBlock>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            buf.Write(NumGarages);
            buf.Write(FreeBombs, 4);
            buf.Write(FreeResprays, 4);
            buf.Write(CarsCollected);
            buf.Write(BankVansCollected);
            buf.Write(PoliceCarsCollected);
            buf.Write((int) CarTypesCollected1);
            buf.Write((int) CarTypesCollected2);
            buf.Write((int) CarTypesCollected3);
            buf.Write(LastTimeHelpMessage);
            buf.Write(CarsInSafeHouse, NumStoredCars);
            buf.Write(Garages, p.NumGarages);

            // Game writes some garbage here due to incorrect size calculation
            buf.Skip(JunkSize);

            Debug.Assert(buf.Offset == SizeOf<GarageBlock>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            return sizeof(int)
                + 2 * sizeof(int)
                + 3 * sizeof(int)
                + 3 * sizeof(int)
                + sizeof(int)
                + SizeOf<StoredCar>(prm) * NumStoredCars
                + SizeOf<Garage>(prm) * p.NumGarages
                + JunkSize;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GarageBlock);
        }

        public bool Equals(GarageBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return NumGarages.Equals(other.NumGarages)
                && FreeBombs.Equals(other.FreeBombs)
                && FreeResprays.Equals(other.FreeResprays)
                && CarsCollected.Equals(other.CarsCollected)
                && BankVansCollected.Equals(other.BankVansCollected)
                && PoliceCarsCollected.Equals(other.PoliceCarsCollected)
                && CarTypesCollected1.Equals(other.CarTypesCollected1)
                && CarTypesCollected2.Equals(other.CarTypesCollected2)
                && CarTypesCollected3.Equals(other.CarTypesCollected3)
                && LastTimeHelpMessage.Equals(other.LastTimeHelpMessage)
                && CarsInSafeHouse.SequenceEqual(other.CarsInSafeHouse)
                && Garages.SequenceEqual(other.Garages);
        }

        public GarageBlock DeepClone()
        {
            return new GarageBlock(this);
        }
    }
}
