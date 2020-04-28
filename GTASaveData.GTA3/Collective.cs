using System;

namespace GTASaveData.GTA3
{
    public class Collective : SaveDataObject, IEquatable<Collective>
    {
        private int m_index;
        private int m_field04h;

        public int Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public int Field04h
        {
            get { return m_field04h; }
            set { m_field04h = value; OnPropertyChanged(); }
        }

        public Collective()
        {
            Index = -1;
            Field04h = 0;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Index = buf.ReadInt32();
            Field04h = buf.ReadInt32();
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Index);
            buf.Write(Field04h);
        }

        protected override int GetSize(DataFormat fmt)
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
                && Field04h.Equals(other.Field04h);
        }
    }
}
