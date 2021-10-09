using GTASaveData;
using GTASaveData.VCS;
using System;
using WpfEssentials;

namespace TestApp
{
    public class EmpireEditorViewModel : TabPageViewModelBase
    {
        const int NumEmpireSites = 30;

        // PS2
        const int EmpireGangArray = 263;
        const int EmpireGangDensityArray = 293;
        const int EmpireTypeArray = 323;
        const int EmpireTierArray = 353;
        const int EmpireHealthArray = 383;
        const int EmpireStateArray = 413;
        const int Empire443Array = 443;
        const int Empire473Array = 473;
        const int Empire503Array = 503;

        private bool m_componentsEditable;
        private EmpireSite m_selectedEmpire;
        private ObservableArray<EmpireSite> m_empires;
        private SaveFileVCS m_save;

        public bool ComponentsEnabled
        {
            get { return m_componentsEditable; }
            set { m_componentsEditable = value; OnPropertyChanged(); }
        }

        public EmpireSite SelectedEmpire
        {
            get { return m_selectedEmpire; }
            set { m_selectedEmpire = value; OnPropertyChanged(); }
        }

        public ObservableArray<EmpireSite> Empires
        {
            get { return m_empires; }
            set { m_empires = value; OnPropertyChanged(); }
        }

        public EmpireEditorViewModel(MainViewModel mainViewModel)
            : base("Empire Editor", PageVisibility.WhenFileLoaded, mainViewModel)
        {
            Empires = new ObservableArray<EmpireSite>();
            MainViewModel.TabRefresh += MainViewModel_TabRefresh;
        }

        public EmpireSite LoadEmpire(int index)
        {
            if (index < 0 || index >= NumEmpireSites)
            {
                throw new IndexOutOfRangeException($"There are only {NumEmpireSites} empire sites!");
            }

            return new EmpireSite(index)
            {
                Gang = (VcsGang) m_save.Scripts.GetGlobal(EmpireGangArray + index),
                GangDensity = m_save.Scripts.GetGlobal(EmpireGangDensityArray + index),
                Type = (EmpireType) m_save.Scripts.GetGlobal(EmpireTypeArray + index),
                Tier = (EmpireTier) m_save.Scripts.GetGlobal(EmpireTierArray + index),
                Health = m_save.Scripts.GetGlobal(EmpireHealthArray + index),
                State = m_save.Scripts.GetGlobal(EmpireStateArray + index),
                Unknown443 = m_save.Scripts.GetGlobal(Empire443Array + index),
                Unknown473 = m_save.Scripts.GetGlobal(Empire473Array + index),
                Unknown503 = m_save.Scripts.GetGlobal(Empire503Array + index),
            };

        }

        public void StoreEmpire(EmpireSite empire)
        {
            m_save.Scripts.SetGlobal(EmpireGangArray + empire.Index, (int) empire.Gang);
            m_save.Scripts.SetGlobal(EmpireGangDensityArray + empire.Index, empire.GangDensity);
            m_save.Scripts.SetGlobal(EmpireTypeArray + empire.Index, (int) empire.Type);
            m_save.Scripts.SetGlobal(EmpireTierArray + empire.Index, (int) empire.Tier);
            m_save.Scripts.SetGlobal(EmpireHealthArray + empire.Index, empire.Health);
            m_save.Scripts.SetGlobal(EmpireStateArray + empire.Index, empire.State);
            m_save.Scripts.SetGlobal(Empire443Array + empire.Index, empire.Unknown443);
            m_save.Scripts.SetGlobal(Empire473Array + empire.Index, empire.Unknown473);
            m_save.Scripts.SetGlobal(Empire503Array + empire.Index, empire.Unknown503);
        }

        private void MainViewModel_TabRefresh(object sender, TabRefreshEventArgs e)
        {
            ComponentsEnabled = (MainViewModel.SelectedGame == Game.VCS) && (MainViewModel.CurrentSaveFile != null);
            Empires.Clear();
            m_save = null;

            if (ComponentsEnabled)
            {
                m_save = (MainViewModel.CurrentSaveFile) as SaveFileVCS;
                for (int i = 0; i < NumEmpireSites; i++)
                {
                    Empires.Add(LoadEmpire(i));
                }
            }
        }
    }

    public enum VcsGang
    {
        None = -1,
        Umberto,
        Cholo,
        Sharks,
        Army,
        Security,
        Bikers,
        Vance,
        Golf,
        Marty
    }

    public enum EmpireType
    {
        None,
        Protection,
        LoanShark,
        Prostitution,
        Drugs,
        Smuggling,
        Robbery
    }

    public enum EmpireTier
    {
        None,
        SmallTime,
        MediumVenture,
        HighRoller
    }

    public enum EmpireState /*of mind*/
    {
        Normal,
        ForSale,

        CannotDevelop = 4,
    }

    public class EmpireSite : ObservableObject
    {
        private VcsGang m_gang;         // $263
        private int m_gangDensity;      // $293
        private EmpireType m_type;      // $323
        private EmpireTier m_tier;      // $353
        private int m_health;           // $383
        private int m_state;            // $413
        private int m_unknown443;
        private int m_unknown473;
        private int m_unknown503;

        public int Index { get; }
        public string StateString { get; private set; }
        public string HealthString { get; private set; }

        public VcsGang Gang
        {
            get { return m_gang; }
            set { m_gang = value; OnPropertyChanged(); }
        }

        public int GangDensity
        {
            get { return m_gangDensity; }
            set { m_gangDensity = value; OnPropertyChanged(); }
        }

        public EmpireType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        } 

        public EmpireTier Tier
        {
            get { return m_tier; }
            set { m_tier = value; OnPropertyChanged(); UpdateHealthString(); }
        }

        public int Health
        {
            get { return m_health; }
            set { m_health = value; OnPropertyChanged(); UpdateHealthString(); }
        }

        public int State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); UpdateStateString(); }
        }

        public int Unknown443
        {
            get { return m_unknown443; }
            set { m_unknown443 = value; OnPropertyChanged(); }
        }

        public int Unknown473
        {
            get { return m_unknown473; }
            set { m_unknown473 = value; OnPropertyChanged(); }
        }

        public int Unknown503
        {
            get { return m_unknown503; }
            set { m_unknown503 = value; OnPropertyChanged(); }
        }

        public EmpireSite(int index)
        {
            Index = index;
        }

        public override string ToString()
        {
            return $"({Gang},{Type},{Tier},{State})";
        }

        private void UpdateHealthString()
        {
            HealthString = Tier switch
            {
                EmpireTier.SmallTime => string.Format("{0:P0}", Math.Min(Health, 2) / 2.0),
                EmpireTier.MediumVenture => string.Format("{0:P0}", Math.Min(Health, 3) / 3.0),
                EmpireTier.HighRoller => string.Format("{0:P0}", Math.Min(Health, 4) / 4.0),
                _ => "",
            };
            OnPropertyChanged(nameof(HealthString));
        }

        private void UpdateStateString()
        {
            StateString = State switch
            {
                0 => "Idle",
                1 => "For Sale",
                3 => "???",
                4 => "In Repair?",
                5 => "Unavailable",
                _ => "",
            };
            OnPropertyChanged(nameof(StateString));
        }
    }
}
