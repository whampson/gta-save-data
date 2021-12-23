using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.IV;
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
using IVBlock = GTASaveData.IV.DataBlock;


namespace TestApp
{
    public class MainViewModel : ObservableObject
    {
        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;
        public EventHandler<FileTypeListEventArgs> PopulateFileTypeList;
        public event EventHandler<TabRefreshEventArgs> TabRefresh;

        private SaveFile m_currentSaveFile;
        private FileType m_currentFileFormat;
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

        public SaveFile CurrentSaveFile
        {
            get { return m_currentSaveFile; }
            set { m_currentSaveFile = value; OnPropertyChanged(); }
        }

        public FileType CurrentFileFormat
        {
            get { return m_currentFileFormat; }
            set { m_currentFileFormat = value; OnPropertyChanged(); }
        }

        public Game SelectedGame
        {
            get { return m_selectedGame; }
            set{ m_selectedGame = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BlockNamesForCurrentGame));
                OnPropertyChanged(nameof(FileFormatsForCurrentGame));
            }
        }

        public string StatusText
        {
            get { return m_statusText; }
            set { m_statusText = value; OnPropertyChanged(); }
        }

        public static Dictionary<Game, IEnumerable<FileType>> FileFormats => new Dictionary<Game, IEnumerable<FileType>>()
        {
            { Game.None, new List<FileType>() },
            { Game.GTA3, GTA3SaveFile.FileFormats.GetAll() },
            { Game.VC, SaveFileVC.FileFormats.GetAll() },
            { Game.SA, SaveFileSA.FileFormats.GetAll() },
            { Game.LCS, SaveFileLCS.FileFormats.GetAll() },
            { Game.VCS, SaveFileVCS.FileFormats.GetAll() },
            { Game.IV, SaveFileIV.FileFormats.GetAll() },
        };

        public IEnumerable<FileType> FileFormatsForCurrentGame
        {
            get { return FileFormats[SelectedGame]; }
        }

        public static Dictionary<Game, string[]> BlockNames => new Dictionary<Game, string[]>()
        {
            { Game.None, new string[0] },
            { Game.GTA3, Enum.GetNames(typeof(IIIBlock)) },
            { Game.VC, Enum.GetNames(typeof(VCBlock)) },
            { Game.SA, Enum.GetNames(typeof(SABlock)) },
            { Game.LCS, Enum.GetNames(typeof(LCSBlock)) },
            { Game.VCS, Enum.GetNames(typeof(VCSBlock)) },
            { Game.IV, Enum.GetNames(typeof(IVBlock)) },
        };

        public string[] BlockNamesForCurrentGame
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
            // lol
            if (OpenIfValid<GTA3SaveFile>(path, Game.GTA3)) { }
            else if (OpenIfValid<SaveFileVC>(path, Game.VC)) { }
            else if (OpenIfValid<SaveFileSA>(path, Game.SA)) { }
            else if (OpenIfValid<SaveFileLCS>(path, Game.LCS)) { }
            else if (OpenIfValid<SaveFileVCS>(path, Game.VCS)) { }
            else if (OpenIfValid<SaveFileIV>(path, Game.IV)) { }

            if (CurrentSaveFile != null)
            {
                OnTabRefresh(TabRefreshTrigger.FileLoaded);
                StatusText = "Loaded " + path + ".";

                OnLoadingFile();
            }
        }

        private bool OpenIfValid<T>(string path, Game game) where T : SaveFile, new()
        {
            if (SaveFile.TryLoad<T>(path, out var _))
            {
                DoLoad<T>(path);
                SelectedGame = game;
                return true;
            }

            return false;
        }

        private bool DoLoad<T>(string path) where T : SaveFile, new()
        {
            try
            {
                bool detected = SaveFile.TryGetFileType<T>(path, out FileType fmt);
                if (!detected)
                {
                    RequestMessageBoxError(string.Format("Unable to detect file type!"));
                    return false;
                }
                CurrentFileFormat = fmt;

                CleanupOldSaveData();
                CurrentSaveFile = SaveFile.Load<T>(path, CurrentFileFormat);

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
            if (CurrentSaveFile is GTA3SaveFile)
            {
                (CurrentSaveFile as GTA3SaveFile).Dispose();
            }
            else if (CurrentSaveFile is SaveFileVC)
            {
                (CurrentSaveFile as SaveFileVC).Dispose();
            }
            else if (CurrentSaveFile is SaveFileSA)
            {
                (CurrentSaveFile as SaveFileSA).Dispose();
            }
        }

        public void CloseSaveData()
        {
            CleanupOldSaveData();
            CurrentSaveFile = null;
            CurrentFileFormat = FileType.Default;

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

            CurrentSaveFile.FileType = CurrentFileFormat;
            CurrentSaveFile.Save(path);
            StatusText = "File saved.";
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
        [Description("")]
        None,

        [Description("GTA III")]
        GTA3,

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