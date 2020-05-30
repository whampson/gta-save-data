using GTASaveData;
using GTASaveData.Extensions;
using GTASaveData.GTA3;
using GTASaveData.Types.Interfaces;
using GTASaveData.VC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

using IIIBlock = GTASaveData.GTA3.DataBlock;
//using IVBlock = GTASaveData.GTA4.Block;
using VCBlock = GTASaveData.VC.DataBlock;
//using SABlock = GTASaveData.SA.Block;

namespace TestApp
{
    public class ViewModel : ObservableObject
    {
        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;
        public EventHandler<FileTypeListEventArgs> PopulateFileTypeList;

        private GTASaveData.SaveData m_currentSaveFile;
        private FileFormat m_currentFileFormat;
        private GameType m_selectedGame;
        private int m_selectedBlockIndex;
        private bool m_autoDetectFileType;
        private bool m_showEntireFile;
        private string m_text;
        private string m_statusText;

        public GTASaveData.SaveData CurrentSaveFile
        {
            get { return m_currentSaveFile; }
            set { m_currentSaveFile = value; OnPropertyChanged(); }
        }

        public FileFormat CurrentFileFormat
        {
            get { return m_currentFileFormat; }
            set { m_currentFileFormat = value; OnPropertyChanged(); }
        }

        public GameType SelectedGame
        {
            get { return m_selectedGame; }
            set { m_selectedGame = value; OnPopulateFileTypeList(); OnPropertyChanged(); }
        }

        public int SelectedBlockIndex
        {
            get { return m_selectedBlockIndex; }
            set { m_selectedBlockIndex = value; OnPropertyChanged(); }
        }

        public bool AutoDetectFileType
        {
            get { return m_autoDetectFileType; }
            set { m_autoDetectFileType = value; OnPropertyChanged(); }
        }

        public bool ShowEntireFile
        {
            get { return m_showEntireFile; }
            set { m_showEntireFile = value; UpdateTextBox(); OnPropertyChanged(); }
        }

        public string Text
        {
            get { return m_text; }
            set { m_text = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get { return m_statusText; }
            set { m_statusText = value; OnPropertyChanged(); }
        }

        public string[] BlockNameForCurrentGame
        {
            get { return BlockNames[SelectedGame]; }
        }

        public ViewModel()
        {
            SelectedBlockIndex = -1;
        }

        public void Initialize()
        {
            OnPopulateFileTypeList();
            AutoDetectFileType = true;
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
            // Do random stuff here :p

            if (CurrentSaveFile is GTA3Save)
            {
                // Fix save with multiple player peds
                GTA3Save save = CurrentSaveFile as GTA3Save;
                PlayerPedPool peds = save.PlayerPedPool;
                if (save.PlayerPedPool.NumPlayerPeds > 1)
                {
                    PlayerPed theOneTruePlayer = peds[0];
                    peds.PlayerPeds.Clear();
                    peds.PlayerPeds.Add(theOneTruePlayer);
                }
            }
        }

        public void LoadSaveData(string path)
        {
            switch (SelectedGame)
            {
                case GameType.III: DoLoad<GTA3Save>(path); break;
                case GameType.VC: DoLoad<ViceCitySave>(path); break;
                //case GameType.SA: DoLoad<SanAndreasSave>(path); break;
                //case GameType.LCS: DoLoad<LibertyCityStoriesSave>(path); break;
                //case GameType.VCS: DoLoad<ViceCityStoriesSave>(path); break;
                //case GameType.IV: DoLoad<GTA4Save>(path); break;
                default: RequestMessageBoxError("Selected game not yet supported!"); return;
            }

            if (CurrentSaveFile != null)
            {
                SelectedBlockIndex = 0;
                UpdateTextBox();
                StatusText = "Loaded " + path + ".";

                OnLoadingFile();
                OnPropertyChanged(nameof(BlockNameForCurrentGame));
            }
        }

        private bool DoLoad<T>(string path) where T : GTASaveData.SaveData, new()
        {
            try
            {
                if (AutoDetectFileType)
                {
                    bool detected = GTASaveData.SaveData.GetFileFormat<T>(path, out FileFormat fmt);
                    if (!detected)
                    {
                        RequestMessageBoxError(string.Format("Unable to detect file type!"));
                        return false;
                    }
                    CurrentFileFormat = fmt;
                }
                
                CleanupOldSaveData();
                CurrentSaveFile = GTASaveData.SaveData.Load<T>(path, CurrentFileFormat);
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
            else if (CurrentSaveFile is ViceCitySave)
            {
                (CurrentSaveFile as ViceCitySave).Dispose();
            }
            //else if (CurrentSaveFile is SanAndreasSave)
            //{
            //    (CurrentSaveFile as SanAndreasSave).Dispose();
            //}
        }

        private IEnumerable<FileFormat> GetFileTypes()
        {
            switch (SelectedGame)
            {
                case GameType.III: return GTA3Save.FileFormats.GetAll();
                case GameType.VC: return ViceCitySave.FileFormats.GetAll();
                //case GameType.SA: return SanAndreasSave.FileFormats.GetAll();
            }

            throw new NotSupportedException("Selected game not supported!");
        }

        public void CloseSaveData()
        {
            CleanupOldSaveData();
            CurrentSaveFile = null;
            if (AutoDetectFileType)
            {
                CurrentFileFormat = FileFormat.Default;
            }
            SelectedBlockIndex = -1;

            UpdateTextBox();
            StatusText = "File closed.";
        }

        public void StoreSaveData(string path)
        {
            if (CurrentSaveFile == null)
            {
                RequestMessageBoxError("Please open a file first.");
                return;
            }

            CurrentSaveFile.Save(path);
            StatusText = "File saved.";
        }

        public void UpdateTextBox()
        {
            if (CurrentSaveFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            if (!ShowEntireFile)
            {
                IReadOnlyList<SaveDataObject> blocks = (CurrentSaveFile as ISaveData).Blocks;
                Text = blocks[SelectedBlockIndex].ToJsonString();
            }
            else
            {
                Text = CurrentSaveFile.ToJsonString();
            }
        }

        public void SetFileTypeByName(string fileTypeName)
        {
            if (fileTypeName == null)
            {
                AutoDetectFileType = true;
            }
            else
            {
                AutoDetectFileType = false;
                CurrentFileFormat = GetFileTypes().FirstOrDefault(x => x.Name == fileTypeName);
            }
        }

        public static Dictionary<GameType, string[]> BlockNames => new Dictionary<GameType, string[]>()
        {
            { GameType.III, Enum.GetNames(typeof(IIIBlock)) },
            { GameType.VC, Enum.GetNames(typeof(VCBlock)) },
            //{ GameType.SA, Enum.GetNames(typeof(SABlock)) },
            //{ GameType.IV, Enum.GetNames(typeof(IVBlock)) },
        };

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

        private void OnPopulateFileTypeList()
        {
            PopulateFileTypeList?.Invoke(this, new FileTypeListEventArgs(GetFileTypes()));
        }
        #endregion
    }
}