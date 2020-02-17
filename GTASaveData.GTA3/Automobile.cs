using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class Automobile : Vehicle,
        IEquatable<Automobile>
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
                    size = 1556;
                }
                else if (fmt.SupportsPC || fmt.SupportsXbox)
                {
                    size = 1384;
                }
                else if (fmt.SupportsMobile)
                {
                    size = 1388;
                }

                return size;
            }
        }

        public Automobile()
            : base()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_unknownArray0 = r.ReadBytes(Limits.GetUnknownArray0Size(fmt));
            m_position = r.ReadObject<Vector3d>();
            m_unknownArray1 = r.ReadBytes(Limits.GetUnknownArray1Size(fmt));
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_unknownArray0.ToArray(), Limits.GetUnknownArray0Size(fmt));
            w.Write(m_position);
            w.Write(m_unknownArray1.ToArray(), Limits.GetUnknownArray1Size(fmt));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Automobile);
        }

        public bool Equals(Automobile other)
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
