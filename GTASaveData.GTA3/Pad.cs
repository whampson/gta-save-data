using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Pad : GTAObject, IEquatable<Pad>
    {
        private short m_mode;

        public short Mode
        {
            get { return m_mode; }
            set { m_mode = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pad);
        }

        public bool Equals(Pad other)
        {
            if (other == null)
            {
                return false;
            }

            return Mode.Equals(other.Mode);
        }
    }
}
