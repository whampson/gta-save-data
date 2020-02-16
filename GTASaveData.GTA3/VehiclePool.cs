using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class VehiclePool : SerializableObject,
        IEquatable<VehiclePool>
    {
        private Array<VehiclePoolItem> m_pool;

        public Array<VehiclePoolItem> Pool
        {
            get { return m_pool; }
            set { m_pool = value; OnPropertyChanged(); }
        }

        public VehiclePool()
        {
            m_pool = new Array<VehiclePoolItem>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            int numCars = r.ReadInt32();
            int numBoats = r.ReadInt32();

            m_pool = r.ReadArray<VehiclePoolItem>(numCars + numBoats, fmt);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            int numCars = 0;
            int numBoats = 0;
            foreach (var item in m_pool)
            {
                if (item.IsBoat)
                {
                    numBoats++;
                }
                else
                {
                    numCars++;
                }
            }

            w.Write(numCars);
            w.Write(numBoats);
            w.Write(m_pool.ToArray(), format: fmt);
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

            return m_pool.SequenceEqual(other.m_pool);
        }
    }
}