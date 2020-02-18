using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.VC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            IGrandTheftAutoSave s = CurrentSaveDataFile as IGrandTheftAutoSave;

            Debug.WriteLine("Last mission passed: {0}", (object) s.SimpleVars.LastMissionPassedName);
            Debug.WriteLine("Global timer: {0}", s.SimpleVars.TimeInMilliseconds);
            Debug.WriteLine("Minute length: {0}", s.SimpleVars.MillisecondsPerGameMinute);
            Debug.WriteLine("Camera position: <{0:0.####}, {1:0.####}, {2:0.####}>",
                s.SimpleVars.CameraPosition.X, s.SimpleVars.CameraPosition.Y, s.SimpleVars.CameraPosition.Z);
            Debug.WriteLine("Num active car gens: {0}", s.CarGenerators.NumberOfParkedCarsToGenerate);
        }

        #region Events, Variables, and Properties
        public EventHandler<FileDialogEventArgs> FileDialogRequested;
        public EventHandler<MessageBoxEventArgs> MessageBoxRequested;

        private GrandTheftAutoSave m_currentSaveDataFile;
        private FileFormat m_currentFileFormat;
        private Game m_selectedGame;
        private int m_selectedBlockIndex;
        private string m_text;
        private string m_statusText;

        public GrandTheftAutoSave CurrentSaveDataFile
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
                    CurrentFileFormat = GrandTheftAutoSave.GetFileFormat<GTA3Save>(path);
                    CurrentSaveDataFile = GrandTheftAutoSave.Load<GTA3Save>(path, CurrentFileFormat);
                    break;
                case Game.ViceCity:
                    CurrentFileFormat = GrandTheftAutoSave.GetFileFormat<ViceCitySave>(path);
                    CurrentSaveDataFile = GrandTheftAutoSave.Load<ViceCitySave>(path, CurrentFileFormat);
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

            CurrentSaveDataFile.Save(path);
            StatusText = "File saved.";
        }

        public void UpdateTextBox()
        {
            if (CurrentSaveDataFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            IReadOnlyList<SerializableObject> blocks = CurrentSaveDataFile.Blocks;
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
            { Game.ViceCity, VCBlockNames }
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
