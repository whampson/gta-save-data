using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Streaming : SaveDataObject,
        IEquatable<Streaming>, IDeepClonable<Streaming>
    {
        public const int NumModels = 200;

        private ObservableArray<StreamingFlags> m_modelFlags;
        public ObservableArray<StreamingFlags> ModelFlags
        {
            get { return m_modelFlags; }
            set { m_modelFlags = value; OnPropertyChanged(); }
        }

        public Streaming()
        {
            ModelFlags = ArrayHelper.CreateArray<StreamingFlags>(NumModels);
        }

        public Streaming(Streaming other)
        {
            ModelFlags = ArrayHelper.DeepClone(other.ModelFlags);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            ModelFlags = buf.ReadArray<StreamingFlags>(NumModels);

            Debug.Assert(buf.Offset == SizeOf<Streaming>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(ModelFlags, NumModels);

            Debug.Assert(buf.Offset == SizeOf<Streaming>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 200;
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

        public Streaming DeepClone()
        {
            return new Streaming(this);
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
