using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(58)]
    public class ZoneInfo : SaveDataObject, IEquatable<ZoneInfo>
    {
        public static class Limits
        {
            public const int MaxNumCarThreshold = 6;
            public const int MaxNumGangDensity = 9;
        }

        private short m_carDensity;
        private Array<short> m_carThreshold;
        private short m_copCarDensity;
        private Array<short> m_gangCarDensity;
        private short m_pedDensity;
        private short m_copPedDensity;
        private Array<short> m_gangPedDensity;
        private short m_pedGroup;

        public short CarDensity
        {
            get { return m_carDensity; }
            set { m_carDensity = value; OnPropertyChanged(); }
        }

        public Array<short> CarThreshold
        {
            get { return m_carThreshold; }
            set { m_carThreshold = value; OnPropertyChanged(); }
        }

        public short CopCarDensity
        {
            get { return m_copCarDensity; }
            set { m_copCarDensity = value; OnPropertyChanged(); }
        }

        public Array<short> GangCarDensity
        {
            get { return m_gangCarDensity; }
            set { m_gangCarDensity = value; OnPropertyChanged(); }
        }

        public short PedDensity
        {
            get { return m_pedDensity; }
            set { m_pedDensity = value; OnPropertyChanged(); }
        }

        public short CopPedDensity
        {
            get { return m_copPedDensity; }
            set { m_copPedDensity = value; OnPropertyChanged(); }
        }

        public Array<short> GangPedDensity
        {
            get { return m_gangPedDensity; }
            set { m_gangPedDensity = value; OnPropertyChanged(); }
        }

        public short PedGroup
        {
            get { return m_pedGroup; }
            set { m_pedGroup = value; OnPropertyChanged(); }
        }


        public ZoneInfo()
        {
            CarThreshold = new Array<short>();
            GangCarDensity = new Array<short>();
            GangPedDensity = new Array<short>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            CarDensity = buf.ReadInt16();
            CarThreshold = buf.ReadArray<short>(Limits.MaxNumCarThreshold);
            CopCarDensity = buf.ReadInt16();
            GangCarDensity = buf.ReadArray<short>(Limits.MaxNumGangDensity);
            PedDensity = buf.ReadInt16();
            CopPedDensity = buf.ReadInt16();
            GangPedDensity = buf.ReadArray<short>(Limits.MaxNumGangDensity);
            PedGroup = buf.ReadInt16();

            Debug.Assert(buf.Offset == SizeOf<ZoneInfo>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(CarDensity);
            buf.Write(CarThreshold.ToArray(), Limits.MaxNumCarThreshold);
            buf.Write(CopCarDensity);
            buf.Write(GangCarDensity.ToArray(), Limits.MaxNumGangDensity);
            buf.Write(PedDensity);
            buf.Write(CopPedDensity);
            buf.Write(GangPedDensity.ToArray(), Limits.MaxNumGangDensity);
            buf.Write(PedGroup);

            Debug.Assert(buf.Offset == SizeOf<ZoneInfo>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ZoneInfo);
        }

        public bool Equals(ZoneInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return CarDensity.Equals(other.CarDensity)
                && CarThreshold.SequenceEqual(other.CarThreshold)
                && CopCarDensity.Equals(other.CopCarDensity)
                && GangCarDensity.SequenceEqual(other.GangCarDensity)
                && PedDensity.Equals(other.PedDensity)
                && CopPedDensity.Equals(other.CopPedDensity)
                && GangPedDensity.SequenceEqual(other.GangPedDensity)
                && PedGroup.Equals(other.PedGroup);
        }
    }
}