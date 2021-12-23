using System;
using GTASaveData.Interfaces;

namespace GTASaveData.Types
{
    /// <summary>
    /// Unused data structure in some GTA save files.
    /// </summary>
    public class Collective : SaveDataObject,
        IEquatable<Collective>, IDeepClonable<Collective>
    {
        private int m_index;
        private int m_pedIndex;

        public int Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public int PedIndex
        {
            get { return m_pedIndex; }
            set { m_pedIndex = value; OnPropertyChanged(); }
        }

        public Collective()
        {
            Index = -1;
            PedIndex = 0;
        }

        public Collective(Collective other)
        {
            Index = other.Index;
            PedIndex = other.PedIndex;
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            Index = buf.ReadInt32();
            PedIndex = buf.ReadInt32();
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            buf.Write(Index);
            buf.Write(PedIndex);
        }

        protected override int GetSize(FileType fmt)
        {
            return 8;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Collective);
        }

        public bool Equals(Collective other)
        {
            if (other == null)
            {
                return false;
            }

            return Index.Equals(other.Index)
                && PedIndex.Equals(other.PedIndex);
        }

        public Collective DeepClone()
        {
            return new Collective(this);
        }
    }
}
