using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class ZoneInfo : SaveDataObject,
        IEquatable<ZoneInfo>, IDeepClonable<ZoneInfo>
    {
        public const int CarThresholdCapacity = 6;
        public const int GangDensityCapacity = 9;

        private short m_carDensity;
        private ObservableArray<short> m_carThreshold;
        private short m_copCarDensity;
        private ObservableArray<short> m_gangCarDensity;
        private short m_pedDensity;
        private short m_copPedDensity;
        private ObservableArray<short> m_gangPedDensity;
        private short m_pedGroup;

        public short CarDensity
        {
            get { return m_carDensity; }
            set { m_carDensity = value; OnPropertyChanged(); }
        }

        public ObservableArray<short> CarThreshold
        {
            get { return m_carThreshold; }
            set { m_carThreshold = value; OnPropertyChanged(); }
        }

        public short CopCarDensity
        {
            get { return m_copCarDensity; }
            set { m_copCarDensity = value; OnPropertyChanged(); }
        }

        public ObservableArray<short> GangCarDensity
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

        public ObservableArray<short> GangPedDensity
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
            CarThreshold = ArrayHelper.CreateArray<short>(CarThresholdCapacity);
            GangCarDensity = ArrayHelper.CreateArray<short>(GangDensityCapacity);
            GangPedDensity = ArrayHelper.CreateArray<short>(GangDensityCapacity);
        }

        public ZoneInfo(ZoneInfo other)
        {
            CarDensity = other.CarDensity;
            CarThreshold = ArrayHelper.DeepClone(other.CarThreshold);
            CopCarDensity = other.CopCarDensity;
            GangCarDensity = ArrayHelper.DeepClone(other.GangCarDensity);
            PedDensity = other.PedDensity;
            CopPedDensity = other.CopPedDensity;
            GangPedDensity = ArrayHelper.DeepClone(other.GangPedDensity);
            PedGroup = other.PedGroup;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            CarDensity = buf.ReadInt16();
            CarThreshold = buf.ReadArray<short>(CarThresholdCapacity);
            CopCarDensity = buf.ReadInt16();
            GangCarDensity = buf.ReadArray<short>(GangDensityCapacity);
            PedDensity = buf.ReadInt16();
            CopPedDensity = buf.ReadInt16();
            GangPedDensity = buf.ReadArray<short>(GangDensityCapacity);
            PedGroup = buf.ReadInt16();

            Debug.Assert(buf.Offset == SizeOfType<ZoneInfo>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(CarDensity);
            buf.Write(CarThreshold, CarThresholdCapacity);
            buf.Write(CopCarDensity);
            buf.Write(GangCarDensity, GangDensityCapacity);
            buf.Write(PedDensity);
            buf.Write(CopPedDensity);
            buf.Write(GangPedDensity, GangDensityCapacity);
            buf.Write(PedGroup);

            Debug.Assert(buf.Offset == SizeOfType<ZoneInfo>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 58;      // not aligned
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

        public ZoneInfo DeepClone()
        {
            return new ZoneInfo(this);
        }
    }
}