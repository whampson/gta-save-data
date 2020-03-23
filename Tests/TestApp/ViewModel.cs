using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.Types;
using GTASaveData.VC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

namespace TestApp
{
    public class ViewModel : ObservableObject
    {
        public void OnLoad()
        { }

        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;

        private SaveFile m_currentSaveFile;
        private SaveFileFormat m_currentFileFormat;
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

        public SaveFileFormat CurrentFileFormat
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
            set { m_showEntireFileChecked = value; UpdateTextBox();  OnPropertyChanged(); }
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

        public ViewModel()
        {
            m_selectedBlockIndex = -1;
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
                //case GameType.TLAD: DoLoad<LostAndDamnedSave>(path); break;
                //case GameType.TBOGT: DoLoad<BalladOfGayTonySave>(path); break;
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
            try {
                if (!SaveFile.GetFileFormat<T>(path, out SaveFileFormat fmt))
                {
                    RequestMessageBoxError(string.Format("Invalid save file! (Game: {0})", SelectedGame));
                    return false;
                }

                CurrentFileFormat = fmt;
                CurrentSaveFile = SaveFile.Load<T>(path, fmt);
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

        public void CloseSaveData()
        {
            CurrentSaveFile = null;
            CurrentFileFormat = SaveFileFormat.Default;
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

            if (!ShowEntireFileChecked)
            {
                IReadOnlyList<SaveDataObject> blocks = CurrentSaveFile.Blocks;
                Debug.Assert(CurrentSaveFile.Blocks.Count == BlockNames[SelectedGame].Length);
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

        #region Misc
        public static Dictionary<GameType, string[]> BlockNames => new Dictionary<GameType, string[]>()
        {
            { GameType.III, GTA3BlockNames },
            { GameType.VC, VCBlockNames },
            { GameType.SA, SABlockNames },
        };

        public static string[] GTA3BlockNames => new[]
        {
            "0: Scripts",
            "1: PedPool",
            "2: Garages",
            "3: Vehicles",
            "4: Objects",
            "5: PathFind",
            "6: Cranes",
            "7: Pickups",
            "8: PhoneInfo",
            "9: RestartPoints",
            "10: RadarBlips",
            "11: Zones",
            "12: GangData",
            "13: CarGenerators",
            "14: Particles",
            "15: AudioScriptObjects",
            "16: PlayerInfo",
            "17: Stats",
            "18: Streaming",
            "19: PedTypeInfo"
        };

        public static string[] VCBlockNames => new[]
        {
            "0: Scripts",
            "1: PedPool",
            "2: Garages",
            "3: GameLogic",
            "4: Vehicles",
            "5: Objects",
            "6: PathFind",
            "7: Cranes",
            "8: Pickups",
            "9: PhoneInfo",
            "10: RestartPoints",
            "11: RadarBlips",
            "12: Zones",
            "13: GangData",
            "14: CarGenerators",
            "15: Particles",
            "16: AudioScriptObjects",
            "17: ScriptPaths",
            "18: PlayerInfo",
            "29: Stats",
            "20: SetPieces",
            "21: Streaming",
            "22: PedTypeInfo"
        };

        public static string[] SABlockNames => new[]
        {
            "0: SimpleVars",
            "1: Scripts",
            "2: Players&Objects",
            "3: Garages",
            "4: GameLogic",
            "5: PathFind",
            "6: Pickups",
            "7: PhoneInfo",
            "8: RestartPoints",
            "9: RadarBlips",
            "10: Zones",
            "11: GangData",
            "12: CarGenerators",
            "13: PedGenerators",
            "14: AudioScriptObjects",
            "15: PlayerInfo",
            "16: Stats",
            "17: SetPieces",
            "18: Models",
            "19: PedRelationships",
            "20: Tags",
            "21: IPL",
            "22: Shopping",
            "23: GangWars",
            "24: UniqueStuntJumps",
            "25: ENEX",
            "26: RadioData",
            "27: 3DMarkers"
        };
    }
    #endregion
}
