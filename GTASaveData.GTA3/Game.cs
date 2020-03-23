using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Game : GTAObject, IEquatable<Game>
    {
        private LevelType m_currLevel;

        public LevelType CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        public bool Equals(Game other)
        {
            if (other == null)
            {
                return false;
            }

            return CurrLevel.Equals(other.CurrLevel);
        }
    }
}
