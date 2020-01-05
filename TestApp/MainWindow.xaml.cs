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

            ViewModel.FileDialogRequested += ViewModel_FileDialogRequested;
        }

        public ViewModel ViewModel
        {
            get { return (ViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void BlockComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateText();
        }

        private void ViewModel_FileDialogRequested(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }
    }
}
