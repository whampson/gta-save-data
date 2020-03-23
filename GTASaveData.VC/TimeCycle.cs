using GTASaveData.Types;
using System;

namespace GTASaveData.VC
{
    public class TimeCycle : GTAObject, IEquatable<TimeCycle>
    {
        private int m_extraColour;
        private bool m_extraColourOn;
        private float m_extraColourInter;

        public int ExtraColour
        {
            get { return m_extraColour; }
            set { m_extraColour = value; OnPropertyChanged(); }
        }

        public bool ExtraColourOn
        {
            get { return m_extraColourOn; }
            set { m_extraColourOn = value; OnPropertyChanged(); }
        }

        public float ExtraColourInter
        {
            get { return m_extraColourInter; }
            set { m_extraColourInter = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeCycle);
        }

        public bool Equals(TimeCycle other)
        {
            if (other == null)
            {
                return false;
            }

            return ExtraColour.Equals(other.ExtraColour)
                && ExtraColourOn.Equals(other.ExtraColourOn)
                && ExtraColourInter.Equals(other.ExtraColourInter);
        }
    }
}
