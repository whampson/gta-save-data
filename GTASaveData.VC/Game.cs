using GTASaveData.Types;
using System;

namespace GTASaveData.VC
{
    public class Game : GTAObject, IEquatable<Game>
    {
        private LevelType m_currLevel;
        private AreaType m_currArea;
        private bool m_allTaxisHaveNitro;   // TODO: move to Vehicle

        public LevelType CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public AreaType CurrArea
        {
            get { return m_currArea; }
            set { m_currArea = value; OnPropertyChanged(); }
        }

        public bool AllTaxisHaveNitro
        {
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
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

            return CurrLevel.Equals(other.CurrLevel)
                && CurrArea.Equals(other.CurrArea)
                && AllTaxisHaveNitro.Equals(other.AllTaxisHaveNitro);
        }
    }
}
