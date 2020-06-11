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
        public MainWindow()
        {
            InitializeComponent();

            ViewModel.FileDialogRequested += FileDialogRequested;
            ViewModel.MessageBoxRequested += MessageBoxRequested;
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel) DataContext; }
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
                ViewModel.CurrentFileFormat = fmt;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
