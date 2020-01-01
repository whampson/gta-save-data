using GTASaveData.GTA3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using WpfEssentials;

namespace TestApp
{
    public class ViewModel : ObservableObject
    {
        private WeatherType m_weather;
        private BombType m_bomb;
        private GarageState m_state;
        private RadioStation m_radio;
        private Weapon m_weapon;

        public WeatherType Weather
        {
            get { return m_weather; }
            set { m_weather = value; OnPropertyChanged(); }
        }

        public BombType Bomb
        {
            get { return m_bomb; }
            set { m_bomb = value; OnPropertyChanged(); }
        }

        public GarageState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public RadioStation Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        public Weapon Weapon
        {
            get { return m_weapon; }
            set { m_weapon = value; OnPropertyChanged(); }
        }
    }
}
