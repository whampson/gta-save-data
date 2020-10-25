using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.GTA4;
using GTASaveData.LCS;
using GTASaveData.SA;
using GTASaveData.VC;
using GTASaveData.VCS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

using IIIBlock = GTASaveData.GTA3.DataBlock;
using VCBlock = GTASaveData.VC.DataBlock;
using SABlock = GTASaveData.SA.DataBlock;
using LCSBlock = GTASaveData.LCS.DataBlock;
using VCSBlock = GTASaveData.VCS.DataBlock;
using IVBlock = GTASaveData.GTA4.DataBlock;


namespace TestApp
{
    public class MainViewModel : ObservableObject
    {
        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;
        public EventHandler<FileTypeListEventArgs> PopulateFileTypeList;
        public event EventHandler<TabRefreshEventArgs> TabRefresh;

        private SaveData m_currentSaveFile;
        private FileFormat m_currentFileFormat;
        private Game m_selectedGame;
        private string m_statusText;
        private ObservableCollection<TabPageViewModelBase> m_tabs;
        private int m_selectedTabIndex;

        public ObservableCollection<TabPageViewModelBase> Tabs
        {
            get { return m_tabs; }
            set { m_tabs = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get { return m_selectedTabIndex; }
            set { m_selectedTabIndex = value; OnPropertyChanged(); }
        }

        public SaveData CurrentSaveFile
        {
            get { return m_currentSaveFile; }
            set { m_currentSaveFile = value; OnPropertyChanged(); }
        }

        public FileFormat CurrentFileFormat
        {
            get { return m_currentFileFormat; }
            set { m_currentFileFormat = value; OnPropertyChanged(); }
        }

        public Game SelectedGame
        {
            get { return m_selectedGame; }
            set
            {
                m_selectedGame = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BlockNameForCurrentGame));
            }
        }

        public string StatusText
        {
            get { return m_statusText; }
            set { m_statusText = value; OnPropertyChanged(); }
        }

        public IEnumerable<FileFormat> FileFormats
        {
            get
            {
                return SelectedGame switch
                {
                    Game.III => GTA3Save.FileFormats.GetAll(),
                    Game.VC => VCSave.FileFormats.GetAll(),
                    Game.SA => SASave.FileFormats.GetAll(),
                    Game.LCS => LCSSave.FileFormats.GetAll(),
                    Game.VCS => VCSSave.FileFormats.GetAll(),
                    Game.IV => GTA4Save.FileFormats.GetAll(),
                    _ => new FileFormat[0],
                };
            }
        }

        public static Dictionary<Game, string[]> BlockNames => new Dictionary<Game, string[]>()
        {
            { Game.III, Enum.GetNames(typeof(IIIBlock)) },
            { Game.VC, Enum.GetNames(typeof(VCBlock)) },
            { Game.SA, Enum.GetNames(typeof(SABlock)) },
            { Game.LCS, Enum.GetNames(typeof(LCSBlock)) },
            { Game.VCS, Enum.GetNames(typeof(VCSBlock)) },
            { Game.IV, Enum.GetNames(typeof(IVBlock)) },
        };

        public string[] BlockNameForCurrentGame
        {
            get { return BlockNames[SelectedGame]; }
        }

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabPageViewModelBase>();
            SelectedTabIndex = -1;
        }

        public void Initialize()
        {
            PopulateTabs();
        }
        #endregion

        #region Commands
        public ICommand FileOpenCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => RequestFileDialog(FileDialogType.OpenFileDialog)
                );
            }
        }

        public ICommand FileCloseCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => CloseSaveData(),
                    () => CurrentSaveFile != null
                );
            }
        }

        public ICommand FileSaveAsCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => RequestFileDialog(FileDialogType.SaveFileDialog),
                    () => CurrentSaveFile != null
                );
            }
        }

        public ICommand FileExitCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => Application.Current.Shutdown()
                );
            }
        }
        #endregion

        public void OnLoadingFile()
        {
            // Do fun stuff here :p
        }

        public void PopulateTabs()
        {
            m_tabs.Add(new JsonViewModel(this));
            m_tabs.Add(new EmpireEditorViewModel(this));

            OnTabRefresh(TabRefreshTrigger.WindowLoaded);
        }

        public void LoadSaveData(string path)
        {
            switch (SelectedGame)
            {
                case Game.III: DoLoad<GTA3Save>(path); break;
                case Game.VC: DoLoad<VCSave>(path); break;
                case Game.SA: DoLoad<SASave>(path); break;
                case Game.LCS: DoLoad<LCSSave>(path); break;
                case Game.VCS: DoLoad<VCSSave>(path); break;
                case Game.IV: DoLoad<GTA4Save>(path); break;
                default: RequestMessageBoxError("Selected game not yet supported!"); return;
            }

            if (CurrentSaveFile != null)
            {
                OnTabRefresh(TabRefreshTrigger.FileLoaded);
                StatusText = "Loaded " + path + ".";

                OnLoadingFile();
            }
        }

        private bool DoLoad<T>(string path) where T : GTASaveData.SaveData, new()
        {
            try
            {
                bool detected = SaveData.GetFileFormat<T>(path, out FileFormat fmt);
                if (!detected)
                {
                    RequestMessageBoxError(string.Format("Unable to detect file type!"));
                    return false;
                }
                CurrentFileFormat = fmt;
                
                CleanupOldSaveData();
                CurrentSaveFile = SaveData.Load<T>(path, CurrentFileFormat);

                return true;
            }
            catch (IOException e)
            {
                RequestMessageBoxError("Failed to open file: " + e.Message);
                return false;
            }
            catch (SerializationException e)
            {
                RequestMessageBoxError("Failed to open file: " + e.Message);
                return false;
            }
        }

        private void CleanupOldSaveData()
        {
            if (CurrentSaveFile is GTA3Save)
            {
                (CurrentSaveFile as GTA3Save).Dispose();
            }
            else if (CurrentSaveFile is VCSave)
            {
                (CurrentSaveFile as VCSave).Dispose();
            }
            else if (CurrentSaveFile is SASave)
            {
                (CurrentSaveFile as SASave).Dispose();
            }
        }

        public void CloseSaveData()
        {
            CleanupOldSaveData();
            CurrentSaveFile = null;
            CurrentFileFormat = FileFormat.Default;

            OnTabRefresh(TabRefreshTrigger.FileClosed);
            StatusText = "File closed.";
        }

        public void StoreSaveData(string path)
        {
            if (CurrentSaveFile == null)
            {
                RequestMessageBoxError("Please open a file first.");
                return;
            }

            CurrentSaveFile.FileFormat = CurrentFileFormat;
            CurrentSaveFile.Save(path);
            StatusText = "File saved.";
        }

        public void SetFileTypeByName(string fileTypeName)
        {
            CurrentFileFormat = FileFormats.FirstOrDefault(x => x.Name == fileTypeName);
        }

        #region View Operations
        private void RequestFileDialog(FileDialogType type)
        {
            FileDialogRequested?.Invoke(this, new FileDialogEventArgs(type, FileDialogRequested_Callback));
        }

        private void FileDialogRequested_Callback(bool? result, FileDialogEventArgs e)
        {
            if (result != true)
            {
                return;
            }

            switch (e.DialogType)
            {
                case FileDialogType.OpenFileDialog:
                    LoadSaveData(e.FileName);
                    break;
                case FileDialogType.SaveFileDialog:
                    StoreSaveData(e.FileName);
                    break;
            }
        }

        private void RequestMessageBoxError(string text)
        {
            MessageBoxRequested?.Invoke(this, new MessageBoxEventArgs(
                text, "Error", icon: MessageBoxImage.Error));
        }

        private void OnTabRefresh(TabRefreshTrigger trigger, int desiredTabIndex = 0)
        {
            TabRefresh?.Invoke(this, new TabRefreshEventArgs(trigger));
            SelectedTabIndex = desiredTabIndex;
        }
        #endregion
    }

    public enum TabRefreshTrigger
    {
        /// <summary>
        /// Refresh occurred after the window finished loading.
        /// </summary>
        WindowLoaded,

        /// <summary>
        /// Refresh occurred after a file was loaded.
        /// </summary>
        FileLoaded,

        /// <summary>
        /// Refresh occurred after a file was closed.
        /// </summary>
        FileClosed
    }

    /// <summary>
    /// Parameters for handling a tab refresh event.
    /// </summary>
    public class TabRefreshEventArgs : EventArgs
    {
        public TabRefreshEventArgs(TabRefreshTrigger trigger)
        {
            Trigger = trigger;
        }

        public TabRefreshTrigger Trigger
        {
            get;
        }
    }

    public enum Game
    {
        [Description("GTA III")]
        III,

        [Description("Vice City")]
        VC,

        [Description("San Andreas")]
        SA,

        [Description("Liberty City Stories")]
        LCS,

        [Description("Vice City Stories")]
        VCS,

        [Description("GTA IV")]
        IV
    }
}