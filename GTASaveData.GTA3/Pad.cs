using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Pad : NonSerializableObject,
        IEquatable<Pad>
    {
        private int m_mode;

        public int Mode
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

            return m_mode.Equals(other.m_mode);
        }
    }
}
