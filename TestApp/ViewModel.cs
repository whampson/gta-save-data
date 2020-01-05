using GTASaveData;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

namespace TestApp
{
    public enum BlockType
    {
        SimpleVars,
        Scripts,
        PedPool,
        Garages,
        Vehicles,
        Objects,
        PathFind,
        Cranes,
        Pickups,
        PhoneInfo,
        Restarts,
        RadarBlips,
        Zones,
        GangData,
        CarGenerators,
        Particles,
        AudioScriptObjects,
        PlayerInfo,
        Stats,
        Streaming,
        PedTypeInfo
    }

    public class ViewModel : ObservableObject
    {
        public EventHandler<FileDialogEventArgs> FileDialogRequested;

        private FileFormat m_fileFormat;
        private GTA3SaveData m_currentFile;
        private BlockType m_selectedBlock;
        private string m_text;

        public FileFormat[] FileFormats
        {
            get { return GTA3SaveData.FileFormats.GetAll(); }
        }

        public FileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public GTA3SaveData CurrentFile
        {
            get { return m_currentFile; }
            set { m_currentFile = value; OnPropertyChanged(); }
        }

        public BlockType SelectedBlock
        {
            get { return m_selectedBlock; }
            set { m_selectedBlock = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get { return m_text; }
            set { m_text = value; OnPropertyChanged(); }
        }

        public ICommand LoadFileCommand
        {
            get
            {
                return new RelayCommand(
                    () => FileDialogRequested?.Invoke(this, new FileDialogEventArgs(FileDialogType.OpenFileDialog, FileDialogRequested_Callback)),
                    () => FileFormat != null
                );
            }
        }

        private void FileDialogRequested_Callback(bool? result, FileDialogEventArgs e)
        {
            if (result != true)
            {
                return;
            }

            LoadFile(e.FileName);
        }

        public void LoadFile(string path)
        {
            CurrentFile = GTA3SaveData.Load(path, FileFormat);
            SelectedBlock = BlockType.SimpleVars;
            UpdateText();
        }

        public void UpdateText()
        {
            if (CurrentFile == null)
            {
                Text = "";
                return;
            }

            IList<SaveDataObject> blocks = CurrentFile.GetAllBlocks();
            SaveDataObject block = blocks[(int) SelectedBlock];
            Text = block.ToString();
        }
    }
}
