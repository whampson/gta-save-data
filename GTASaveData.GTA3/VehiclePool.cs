using System;
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

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int numCars = buf.ReadInt32();
            int numBoats = buf.ReadInt32();

            Cars = buf.Read<Automobile>(numCars);
            Boats = buf.Read<Boat>(numBoats);

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Cars.Count);
            buf.Write(Boats.Count);
            buf.Write(Cars.ToArray());
            buf.Write(Boats.ToArray());

            Debug.Assert(buf.Offset == SizeOf(this, fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            return SizeOf<Automobile>(fmt) * Cars.Count
                + SizeOf<Boat>(fmt) * Boats.Count
                + 2 * sizeof(int);
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