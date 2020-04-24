using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(200)]
    public class Streaming : SaveDataObject, IEquatable<Streaming>
    {
        public static class Limits
        {
            public const int NumberOfModelFlags = 200;
        }

        private Array<byte> m_modelFlags;
        public Array<byte> ModelFlags
        {
            get { return m_modelFlags; }
            set { m_modelFlags = value; OnPropertyChanged(); }
        }

        public Streaming()
        {
            ModelFlags = new Array<byte>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            ModelFlags = buf.Read<byte>(Limits.NumberOfModelFlags);

            Debug.Assert(buf.Offset == SizeOf<Streaming>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(ModelFlags.ToArray(), Limits.NumberOfModelFlags);

            Debug.Assert(buf.Offset == SizeOf<Streaming>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Streaming);
        }

        public bool Equals(Streaming other)
        {
            if (other == null)
            {
                return false;
            }

            return ModelFlags.SequenceEqual(other.ModelFlags);
        }
    }
}
