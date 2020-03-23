using GTASaveData.Types;
using System;

namespace GTASaveData.VC
{
    public class Pad : GTAObject, IEquatable<Pad>
    {
        private short m_mode;
        private bool m_invertLook4Pad;

        public short Mode
        {
            get { return m_mode; }
            set { m_mode = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        {
            get { return m_invertLook4Pad; }
            set { m_invertLook4Pad = value; OnPropertyChanged(); }
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

            return Mode.Equals(other.Mode)
                && InvertLook.Equals(other.InvertLook);
        }
    }
}
