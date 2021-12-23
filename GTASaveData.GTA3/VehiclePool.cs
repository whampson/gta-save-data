using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class VehiclePool : SaveDataObject,
        IEquatable<VehiclePool>, IDeepClonable<VehiclePool>
    {
        private ObservableArray<Automobile> m_cars;
        private ObservableArray<Boat> m_boats;

        public ObservableArray<Automobile> Cars
        {
            get { return m_cars; }
            set { m_cars = value; OnPropertyChanged(); }
        }

        public ObservableArray<Boat> Boats
        {
            get { return m_boats; }
            set { m_boats = value; OnPropertyChanged(); }
        }

        public VehiclePool()
        {
            Cars = new ObservableArray<Automobile>();
            Boats = new ObservableArray<Boat>();
        }

        public VehiclePool(VehiclePool other)
        {
            Cars = ArrayHelper.DeepClone(other.Cars);
            Boats = ArrayHelper.DeepClone(other.Boats);
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            int numCars = buf.ReadInt32();
            int numBoats = buf.ReadInt32();

            Cars.Clear();
            Boats.Clear();

            for (int i = 0; i < numCars + numBoats; i++)
            {
                VehicleType type = (VehicleType) buf.ReadInt32();
                short model = buf.ReadInt16();
                int handle = buf.ReadInt32();
                if (type == VehicleType.Car)
                {
                    Automobile a = new Automobile(model, handle);
                    Serializer.Read(a, buf, fmt);
                    Cars.Add(a);
                }
                else if (type == VehicleType.Boat)
                {
                    Boat b = new Boat(model, handle);
                    Serializer.Read(b, buf, fmt);
                    Boats.Add(b);
                }
            }

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            List<Vehicle> vehicles = Cars.Select(x => x as Vehicle).Concat(Boats.Select(x => x as Vehicle)).ToList();

            buf.Write(Cars.Count);
            buf.Write(Boats.Count);

            foreach (Vehicle v in vehicles)
            {
                buf.Write((int) v.Type);
                buf.Write(v.ModelIndex);
                buf.Write(v.Handle);
                buf.Write(v, fmt);
            }

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override int GetSize(FileType fmt)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int sizeOfCars = (SizeOf<Automobile>(fmt) + headerSize) * Cars.Count;
            int sizeOfBoats = (SizeOf<Boat>(fmt) + headerSize) * Boats.Count;

            return 2 * sizeof(int) + sizeOfCars + sizeOfBoats;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VehiclePool);
        }

        public bool Equals(VehiclePool other)
        {
            if (other == null)
            {
                return false;
            }

            return Cars.SequenceEqual(other.Cars)
                && Boats.SequenceEqual(other.Boats);
        }

        public VehiclePool DeepClone()
        {
            return new VehiclePool(this);
        }
    }
}