using GTASaveData;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfEssentials.Win32;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FileTypeGroupName = "FileTypeGroup";
        private readonly RadioMenuItem AutoDetectFileTypeItem = new RadioMenuItem()
        {
            Header = "Auto Detect",
            GroupName = FileTypeGroupName
        };

        public MainWindow()
        {
            InitializeComponent();

            ViewModel.FileDialogRequested += FileDialogRequested;
            ViewModel.MessageBoxRequested += MessageBoxRequested;
            //ViewModel.PopulateFileTypeList += PopulateFileTypeList;
            AutoDetectFileTypeItem.Checked += FileTypeMenuItem_Checked;
        }

        public ViewModel ViewModel
        {
            get { return (ViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void FileDialogRequested(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }

        private void MessageBoxRequested(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }

        //private void PopulateFileTypeList(object sender, FileTypeListEventArgs e)
        //{
        //    foreach (var item in m_fileTypeList.Items)
        //    {
        //        if (item is RadioMenuItem radioItem)
        //        {
        //            if (radioItem != AutoDetectFileTypeItem)
        //            {
        //                radioItem.Checked -= FileTypeMenuItem_Checked;
        //            }
        //        }
        //    }
        //    m_fileTypeList.Items.Clear();

        //    foreach (var type in e.FileTypes)
        //    {
        //        RadioMenuItem item = new RadioMenuItem
        //        {
        //            Header = type.Name,
        //            GroupName = FileTypeGroupName,
        //        };
        //        item.Checked += FileTypeMenuItem_Checked;
        //        m_fileTypeList.Items.Add(item);
        //    }
        //}

        private void BlockComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateTextBox();
        }

        private void GameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi)
            {
                if (mi.DataContext is Game game)
                {
                    ViewModel.SelectedGame = game;
                }
            }
        }

        private void FileTypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item && item.Header is FileFormat fmt)
            {
                //if (item.Parent is ItemsControl ic)
                //{
                //    MenuItem menuItem = ic.Items.OfType<MenuItem>().FirstOrDefault(i => i.IsChecked);
                //    if (menuItem != null)
                //    {
                //        menuItem.IsChecked = false;
                //    }
                //}
                //item.IsChecked = true;
                ViewModel.CurrentFileFormat = fmt;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateTextBox();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        private void FileTypeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioMenuItem menuItem)
            {
                ViewModel.SetFileTypeByName(menuItem.Header as string);
            }
        }
    }
}
