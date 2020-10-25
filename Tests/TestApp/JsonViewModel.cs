using GTASaveData;
using GTASaveData.Interfaces;
using System.Collections.Generic;

namespace TestApp
{
    public class JsonViewModel : TabPageViewModelBase
    {
        private int m_selectedBlockIndex;
        private string m_text;

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

        public JsonViewModel(MainViewModel mainViewModel)
            : base("JSON Viewer", PageVisibility.Always, mainViewModel)
        {
            MainViewModel.TabRefresh += MainViewModel_TabRefresh;
        }

        private void MainViewModel_TabRefresh(object sender, TabRefreshEventArgs e)
        {
            Text = "";
            SelectedBlockIndex = -1;
        }

        public void UpdateTextBox()
        {
            if (MainViewModel.CurrentSaveFile == null || SelectedBlockIndex < 0)
            {
                Text = "";
                return;
            }

            IReadOnlyList<ISaveDataObject> blocks = (MainViewModel.CurrentSaveFile as ISaveFile).Blocks;
            Text = (blocks[SelectedBlockIndex] as SaveDataObject).ToJsonString();
        }
    }
}
