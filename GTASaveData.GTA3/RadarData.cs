using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class RadarData : SaveDataObject, IEquatable<RadarData>
    {
        public static class Limits
        {
            public const int MaxNumRadarBlips = 32;
        }

        private Array<RadarBlip> m_radarBlips;
        public Array<RadarBlip> RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public RadarData()
        {
            RadarBlips = new Array<RadarBlip>();
        }

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "RDR");
            RadarBlips = buf.Read<RadarBlip>(Limits.MaxNumRadarBlips);

            Debug.Assert(buf.Offset == size + GTA3Save.SaveHeaderSize);
            Debug.Assert(size == SizeOf<RadarData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "RDR", SizeOf<RadarData>() - GTA3Save.SaveHeaderSize);
            buf.Write(RadarBlips.ToArray(), Limits.MaxNumRadarBlips);

            Debug.Assert(buf.Offset == SizeOf<RadarData>());
        }

        protected override int GetSize(SaveDataFormat fmt)
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
    }
}
