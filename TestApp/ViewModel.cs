using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.VC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

namespace TestApp
{
    public enum Game
    {
        [Description("GTA 3")]
        GTA3,

        [Description("Vice City")]
        VC,

        [Description("San Andreas")]
        SA,

        [Description("Liberty City Stories")]
        LCS,

        [Description("Vice City Stories")]
        VCS,

        [Description("GTA 4")]
        GTA4
    }

    public class ViewModel : ObservableObject
    {
        public void OnLoadSaveData()
        {
            // TODO: stuff
        }

        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;

        private SaveData m_currentSaveDataFile;
        private FileFormat m_currentFileFormat;
        private Game m_selectedGame;
        private int m_selectedBlockIndex;
        private string m_text;
        private string m_statusText;

        public SaveData CurrentSaveDataFile
        {
            get { return m_currentSaveDataFile; }
            set { m_currentSaveDataFile = value; OnPropertyChanged(); }
        }

        public FileFormat CurrentFileFormat
        {
            get { return m_currentFileFormat; }
            set { m_currentFileFormat = value; OnPropertyChanged(); }
        }

        public Game SelectedGame
        {
            get { return m_selectedGame; }
            set { m_selectedGame = value; OnPropertyChanged(); }
        }

        public int SelectedBlockIndex
        {
            get { return m_selectedBlockIndex; }
            set { m_selectedBlockIndex = value; OnPropertyChanged(); }
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
                    () => CurrentSaveDataFile != null
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
                    () => CurrentSaveDataFile != null
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

        public ViewModel()
        {
            m_selectedBlockIndex = -1;
        }

        public void LoadSaveData(string path)
        {
            switch (m_selectedGame)
            {
                case Game.GTA3:
                    CurrentFileFormat = SaveData.GetFileFormat<GTA3Save>(path);
                    CurrentSaveDataFile = SaveData.LoadFromFile<GTA3Save>(path, CurrentFileFormat);
                    break;
                case Game.VC:
                    CurrentFileFormat = SaveData.GetFileFormat<VCSave>(path);
                    CurrentSaveDataFile = SaveData.LoadFromFile<VCSave>(path, CurrentFileFormat);
                    break;
                default:
                    RequestMessageBoxError("Selected game not yet supported!");
                    return;
            }

            if (CurrentSaveDataFile == null)
            {
                RequestMessageBoxError("Failed to open file!");
                return;
            }
            
            SelectedBlockIndex = 0;
            UpdateTextBox();
            StatusText = "Loaded " + path + ".";

            OnLoadSaveData();
            OnPropertyChanged(nameof(BlockNameForCurrentGame));
        }

        public void CloseSaveData()
        {
            CurrentSaveDataFile = null;
            CurrentFileFormat = null;
            SelectedBlockIndex = -1;

            UpdateTextBox();
            StatusText = "File closed.";
        }

        public void StoreSaveData(string path)
        {
            if (CurrentSaveDataFile == null)
            {
                RequestMessageBoxError("Please open a file first.");
                return;
            }

            CurrentSaveDataFile.Write(path, CurrentFileFormat);
            StatusText = "File saved.";
        }

        public void UpdateTextBox()
        {
            if (CurrentSaveDataFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            IReadOnlyList<Chunk> blocks = CurrentSaveDataFile.Blocks;
            Debug.Assert(CurrentSaveDataFile.Blocks.Count == BlockNames[SelectedGame].Length);

            Text = blocks[SelectedBlockIndex].ToString();
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
        #region Misc
        public static Dictionary<Game, string[]> BlockNames => new Dictionary<Game, string[]>()
        {
            { Game.GTA3, GTA3BlockNames },
            { Game.VC, VCBlockNames }
        };

        public static string[] GTA3BlockNames => new[]
        {
            "0: SimpleVars",
            "1: Scripts",
            "2: PedPool",
            "3: Garages",
            "4: Vehicles",
            "5: Objects",
            "6: PathFind",
            "7: Cranes",
            "8: Pickups",
            "9: PhoneInfo",
            "10: Restarts",
            "11: RadarBlips",
            "12: Zones",
            "13: GangData",
            "14: CarGenerators",
            "15: Particles",
            "16: AudioScriptObjects",
            "17: PlayerInfo",
            "18: Stats",
            "19: Streaming",
            "20: PedTypeInfo"
        };

        public static string[] VCBlockNames => new[]
        {
            "0: SimpleVars",
            "1: Scripts",
            "2: PedPool",
            "3: Garages",
            "4: GameLogic",
            "5: Vehicles",
            "6: Objects",
            "7: PathFind",
            "8: Cranes",
            "9: Pickups",
            "10: PhoneInfo",
            "11: Restarts",
            "12: RadarBlips",
            "13: Zones",
            "14: GangData",
            "15: CarGenerators",
            "16: Particles",
            "17: AudioScriptObjects",
            "18: ScriptPaths",
            "19: PlayerInfo",
            "20: Stats",
            "21: SetPieces",
            "22: Streaming",
            "23: PedTypeInfo"
        };
    }
    #endregion
}
