using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Weather : GTAObject,
        IEquatable<Weather>
    {
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_interpolationValue;
        private int m_weatherTypeInList;

        public WeatherType OldWeatherType
        {
            get { return m_oldWeatherType; }
            set { m_oldWeatherType = value; OnPropertyChanged(); }
        }

        public WeatherType NewWeatherType
        {
            get { return m_newWeatherType; }
            set { m_newWeatherType = value; OnPropertyChanged(); }
        }

        public WeatherType ForcedWeatherType
        {
            get { return m_forcedWeatherType; }
            set { m_forcedWeatherType = value; OnPropertyChanged(); }
        }

        public float InterpolationValue
        {
            get { return m_interpolationValue; }
            set { m_interpolationValue = value; OnPropertyChanged(); }
        }

        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Weather);
        }

        public bool Equals(Weather other)
        {
            if (other == null)
            {
                return false;
            }

            return m_oldWeatherType.Equals(other.m_oldWeatherType)
                && m_newWeatherType.Equals(other.m_newWeatherType)
                && m_forcedWeatherType.Equals(other.m_forcedWeatherType)
                && m_interpolationValue.Equals(other.m_interpolationValue)
                && m_weatherTypeInList.Equals(other.m_weatherTypeInList);
        }
    }
}
