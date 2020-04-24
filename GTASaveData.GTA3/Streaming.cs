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
            public const int NumberOfModels = 200;
        }

        private Array<StreamingFlags> m_modelFlags;
        public Array<StreamingFlags> ModelFlags
        {
            get { return m_modelFlags; }
            set { m_modelFlags = value; OnPropertyChanged(); }
        }

        public Streaming()
        {
            ModelFlags = new Array<StreamingFlags>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            ModelFlags = buf.Read<StreamingFlags>(Limits.NumberOfModels);

            Debug.Assert(buf.Offset == SizeOf<Streaming>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(ModelFlags.ToArray(), Limits.NumberOfModels);

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

    [Flags]
    public enum StreamingFlags : byte
    {
        None,
        DontRemove = 1,
        ScriptOwned = 2,
        Dependency = 4,
        Priority = 8,
        NoFade = 16,

        CantRemove = DontRemove | ScriptOwned,
        KeepInMemory = DontRemove | ScriptOwned | Dependency
    }
}
