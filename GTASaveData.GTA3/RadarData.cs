﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class RadarData : SaveDataObject,
        IEquatable<RadarData>, IDeepClonable<RadarData>,
        IEnumerable<RadarBlip>
    {
        public const int MaxNumRadarBlips = 32;

        private Array<RadarBlip> m_radarBlips;

        public Array<RadarBlip> RadarBlips
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

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadBlockHeader(buf, "RDR");
            RadarBlips = buf.Read<RadarBlip>(MaxNumRadarBlips);

            Debug.Assert(buf.Offset == size + GTA3VCSave.BlockHeaderSize);
            Debug.Assert(size == SizeOfType<RadarData>() - GTA3VCSave.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            GTA3VCSave.WriteBlockHeader(buf, "RDR", SizeOfType<RadarData>() - GTA3VCSave.BlockHeaderSize);
            buf.Write(RadarBlips, MaxNumRadarBlips);

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
