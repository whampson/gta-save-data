using GTASaveData.Serialization;
using System;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    public sealed class VehiclePool : Chunk,
        IEquatable<VehiclePool>
    {
        private FullyObservableCollection<VehiclePoolItem> m_pool;

        public FullyObservableCollection<VehiclePoolItem> Pool
        {
            get { return m_pool; }
            set { m_pool = value; OnPropertyChanged(); }
        }

        public VehiclePool()
        {
            m_pool = new FullyObservableCollection<VehiclePoolItem>();
        }

        private VehiclePool(SaveDataSerializer serializer, FileFormat format)
        {
            int numCars = serializer.ReadInt32();
            int numBoats = serializer.ReadInt32();

            m_pool = new FullyObservableCollection<VehiclePoolItem>(serializer.ReadArray<VehiclePoolItem>(numCars + numBoats, format));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
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

            serializer.Write(numCars);
            serializer.Write(numBoats);
            serializer.WriteArray(m_pool.ToArray(), format: format);
        }

        public override int GetHashCode() 
        {
            return base.GetHashCode();
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