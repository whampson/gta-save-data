using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.Types.Interfaces;
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
        public void OnLoadSaveData()
        {
            //IGTASave s = CurrentSaveDataFile as IGTASave;

            //Debug.WriteLine("Last mission passed: {0}", (object) s.SimpleVars.SaveName);
            //Debug.WriteLine("Global timer: {0}", s.SimpleVars.GlobalTimer);
            //Debug.WriteLine("Minute length: {0}", s.SimpleVars.MillisecondsPerGameMinute);
            //Debug.WriteLine("Camera position: <{0:0.####}, {1:0.####}, {2:0.####}>",
            //    s.SimpleVars.CameraPosition.X, s.SimpleVars.CameraPosition.Y, s.SimpleVars.CameraPosition.Z);
        }

        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;

        private SaveFile m_currentSaveFile;
        private SaveFileFormat m_currentFileFormat;
        private GameType m_selectedGame;
        private int m_selectedBlockIndex;
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
            SaveFileFormat fmt;
            try
            {
                switch (m_selectedGame)
                {
                    case GameType.III:

                        bool valid = SaveFile.GetFileFormat<GTA3Save>(path, out fmt);
                        if (!valid)
                        {
                            RequestMessageBoxError("Invalid save file!");
                            return;
                        }
                        CurrentFileFormat = fmt;
                        CurrentSaveFile = SaveFile.Load<GTA3Save>(path, fmt);
                        break;
                    //case Game.VC:
                    //    CurrentFileFormat = SaveFile.GetFileFormat<ViceCitySave>(path);
                    //    CurrentSaveDataFile = SaveFile.Load<ViceCitySave>(path, CurrentFileFormat);
                    //    break;
                    //case Game.SA:
                    //    CurrentFileFormat = SaveFileFormat.Default;
                    //    CurrentSaveDataFile = SaveFile.Load<SanAndreasSave>(path);
                    //    break;
                    default:
                        RequestMessageBoxError("Selected game not yet supported!");
                        return;
                }
            }
            catch (IOException e)
            {
                RequestMessageBoxError("Failed to open file: " + e.Message);
                return;
            }
            catch (SerializationException e)
            {
                RequestMessageBoxError("Failed to open file: " + e.Message);
                return;
            }

            if (CurrentSaveFile == null)
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

            CurrentSaveFile.Write(path);
            StatusText = "File saved.";
        }

        public void UpdateTextBox()
        {
            if (CurrentSaveFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            //IReadOnlyList<GTAObject> blocks = CurrentSaveDataFile.Blocks;
            //Debug.Assert(CurrentSaveDataFile.Blocks.Count == BlockNames[SelectedGame].Length);

            //Text = blocks[SelectedBlockIndex].ToString();


            Text = CurrentSaveFile.ToJsonString();
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
            "10: RestartPoints",
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
            "11: RestartPoints",
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
