using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class RadarData : SaveDataObject,
        IEquatable<RadarData>, IDeepClonable<RadarData>,
        IEnumerable<RadarBlip>
    {
        public const int MaxNumRadarBlips = 32;

        private ObservableArray<RadarBlip> m_radarBlips;

        public ObservableArray<RadarBlip> RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public RadarBlip this[int i]
        {
            get { return RadarBlips[i]; }
            set { RadarBlips[i] = value; OnPropertyChanged(); }
        }

        public RadarData()
        {
            RadarBlips = ArrayHelper.CreateArray<RadarBlip>(MaxNumRadarBlips);
        }

        public RadarData(RadarData other)
        {
            RadarBlips = ArrayHelper.DeepClone(other.RadarBlips);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            int size = GTA3Save.ReadBlockHeader(buf, out string tag);
            Debug.Assert(tag == "RDR");
            RadarBlips = buf.ReadArray<RadarBlip>(MaxNumRadarBlips);

            Debug.Assert(buf.Offset == size + GTA3Save.BlockHeaderSize);
            Debug.Assert(size == SizeOf<RadarData>() - GTA3Save.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            GTA3Save.WriteBlockHeader(buf, "RDR", SizeOf<RadarData>() - GTA3Save.BlockHeaderSize);
            buf.Write(RadarBlips, MaxNumRadarBlips);

            Debug.Assert(buf.Offset == SizeOf<RadarData>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0x608;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RadarData);
        }

        public bool Equals(RadarData other)
        {
            if (other == null)
            {
                return false;
            }

            return RadarBlips.SequenceEqual(other.RadarBlips);
        }

        public RadarData DeepClone()
        {
            return new RadarData(this);
        }

        public IEnumerator<RadarBlip> GetEnumerator()
        {
            return RadarBlips.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
