using GTASaveData;
using GTASaveData.GTA3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

using IIIBlock = GTASaveData.GTA3.Block;
//using IVBlock = GTASaveData.GTA4.Block;
//using VCBlock = GTASaveData.VC.Block;
//using SABlock = GTASaveData.SA.Block;

namespace TestApp
{
    public class ViewModel : ObservableObject
    {
        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;

        private SaveFile m_currentSaveFile;
        private GTASaveData.DataFormat m_currentFileFormat;
        private GameType m_selectedGame;
        private int m_selectedBlockIndex;
        private bool m_showEntireFileChecked;
        private string m_text;
        private string m_statusText;

        public SaveFile CurrentSaveFile
        {
            get { return m_currentSaveFile; }
            set { m_currentSaveFile = value; OnPropertyChanged(); }
        }

        public GTASaveData.DataFormat CurrentFileFormat
        {
            get { return m_currentFileFormat; }
            set { m_currentFileFormat = value; OnPropertyChanged(); }
        }

        public GameType SelectedGame
        {
            get { return m_selectedGame; }
            set { m_selectedGame = value; OnPropertyChanged(); }
        }

        public int SelectedBlockIndex
        {
            get { return m_selectedBlockIndex; }
            set { m_selectedBlockIndex = value; OnPropertyChanged(); }
        }

        public bool ShowEntireFileChecked
        {
            get { return m_showEntireFileChecked; }
            set { m_showEntireFileChecked = value; UpdateTextBox(); OnPropertyChanged(); }
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

        public void OnLoad()
        {
            GTA3Save save = CurrentSaveFile as GTA3Save;
            if (save == null)
            {
                return;
            }

            var scr = save.Scripts;
            var ptp = save.PedTypeInfo;

            int oldV = scr.GetGlobal(34);
            scr.SetGlobal(34, 6969);
            int newV = scr.GetGlobal(34);
        }

        public ViewModel()
        {
            m_selectedBlockIndex = -1;
        }

        public void LoadSaveData(string path)
        {
            switch (SelectedGame)
            {
                case GameType.III: DoLoad<GTA3Save>(path); break;
                //case GameType.VC: DoLoad<ViceCitySave>(path); break;
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

                OnLoad();
                OnPropertyChanged(nameof(BlockNameForCurrentGame));
            }
        }

        private bool DoLoad<T>(string path) where T : SaveFile, new()
        {
            
            try
            {
                if (!SaveFile.GetFileFormat<T>(path, out GTASaveData.DataFormat fmt))
                {
                    RequestMessageBoxError(string.Format("Invalid save file! (Game: {0})", SelectedGame));
                    return false;
                }

                CleanupOldSaveData();
                CurrentSaveFile = SaveFile.Load<T>(path, fmt);
                CurrentFileFormat = fmt;

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
            //else if (CurrentSaveFile is ViceCitySave)
            //{
            //    (CurrentSaveFile as ViceCitySave).Dispose();
            //}
            //else if (CurrentSaveFile is SanAndreasSave)
            //{
            //    (CurrentSaveFile as SanAndreasSave).Dispose();
            //}
        }

        public void CloseSaveData()
        {
            CleanupOldSaveData();
            CurrentSaveFile = null;
            CurrentFileFormat = GTASaveData.DataFormat.Default;
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

        public static Dictionary<GameType, string[]> BlockNames => new Dictionary<GameType, string[]>()
        {
            { GameType.III, Enum.GetNames(typeof(IIIBlock)) },
            //{ GameType.VC, Enum.GetNames(typeof(VCBlock)) },
            //{ GameType.SA, Enum.GetNames(typeof(SABlock)) },
            //{ GameType.IV, Enum.GetNames(typeof(IVBlock)) },
        };

        #region UI Controls
        public void UpdateTextBox()
        {
            if (CurrentSaveFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            if (!ShowEntireFileChecked)
            {
                IReadOnlyList<SaveDataObject> blocks = CurrentSaveFile.Blocks;
                Text = blocks[SelectedBlockIndex].ToJsonString();
            }
            else
            {
                Text = CurrentSaveFile.ToJsonString();
            }
        }

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
        #endregion
    }
}