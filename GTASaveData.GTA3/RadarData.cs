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

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "RDR");
            RadarBlips = buf.Read<RadarBlip>(Limits.MaxNumRadarBlips);

            Debug.Assert(buf.Offset == size + GTA3Save.BlockHeaderSize);
            Debug.Assert(size == SizeOfType<RadarData>() - GTA3Save.BlockHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "RDR", SizeOfType<RadarData>() - GTA3Save.BlockHeaderSize);
            buf.Write(RadarBlips.ToArray(), Limits.MaxNumRadarBlips);

            Debug.Assert(buf.Offset == SizeOfType<RadarData>());
        }

        protected override int GetSize(FileFormat fmt)
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
