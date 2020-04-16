using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x98)]
    public class GangData : SaveDataObject, IEquatable<GangData>
    {
        public static class Limits
        {
            public const int NumberOfGangs = 9;
        }

        private Array<GangInfo> m_gangs;
        public Array<GangInfo> Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value;OnPropertyChanged(); }
        }

        public GangData()
        {
            Gangs = new Array<GangInfo>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "GNG");

            Gangs = buf.ReadArray<GangInfo>(Limits.NumberOfGangs);

            Debug.Assert(buf.Offset == SizeOf<GangData>());
            Debug.Assert(size == SizeOf<GangData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "GNG", SizeOf<GangData>() - GTA3Save.SaveHeaderSize);
            buf.Write(Gangs.ToArray(), Limits.NumberOfGangs);

            Debug.Assert(buf.Offset == SizeOf<GangData>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GangData);
        }

        public bool Equals(GangData other)
        {
            if (other == null)
            {
                return false;
            }

            return Gangs.SequenceEqual(other.Gangs);
        }
    }
}
