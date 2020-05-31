using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class VehiclePool : SaveDataObject, IEquatable<VehiclePool>
    {
        private Array<Automobile> m_cars;
        private Array<Boat> m_boats;

        public Array<Automobile> Cars
        {
            get { return m_cars; }
            set { m_cars = value; OnPropertyChanged(); }
        }

        public Array<Boat> Boats
        {
            get { return m_boats; }
            set { m_boats = value; OnPropertyChanged(); }
        }

        public VehiclePool()
        {
            Cars = new Array<Automobile>();
            Boats = new Array<Boat>();
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
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

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
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

            Debug.Assert(buf.Offset == SizeOfObject(this, fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            int headerSize = 2 * sizeof(int) + sizeof(short);
            int sizeOfCars = (SizeOfType<Automobile>(fmt) + headerSize) * Cars.Count;
            int sizeOfBoats = (SizeOfType<Boat>(fmt) + headerSize) * Boats.Count;

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
    }
}