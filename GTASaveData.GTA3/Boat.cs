using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class Boat : Vehicle,
        IEquatable<Boat>
    {
        public static class Limits
        {
            public static int GetUnknownArray0Size(FileFormat fmt)
            {
                return (fmt.SupportsPS2) ? 48 : 52;
            }

            public static int GetUnknownArray1Size(FileFormat fmt)
            {
                int size = 0;
                if (fmt.SupportsPS2)
                {
                    size = 1140;
                }
                else if (fmt.SupportsPC || fmt.SupportsXbox)
                {
                    size = 1092;
                }
                else if (fmt.SupportsMobile)
                {
                    size = 1096;
                }

                return size;
            }
        }

        public Boat()
            : base()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_unknownArray0 = r.ReadBytes(Limits.GetUnknownArray0Size(fmt));
            m_position = r.ReadObject<Vector>();
            m_unknownArray1 = r.ReadBytes(Limits.GetUnknownArray1Size(fmt));
        }

        protected override void WriteObjectData(Serializer serializer, FileFormat format)
        {
            serializer.Write(m_unknownArray0.ToArray(), Limits.GetUnknownArray0Size(format));
            serializer.Write(m_position);
            serializer.Write(m_unknownArray1.ToArray(), Limits.GetUnknownArray1Size(format));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Boat);
        }

        public bool Equals(Boat other)
        {
            if (other == null)
            {
                return false;
            }

            return m_unknownArray0.SequenceEqual(other.m_unknownArray0)
                && m_position.Equals(other.m_position)
                && m_unknownArray1.SequenceEqual(other.m_unknownArray1);
        }
    }
}
