using System.Windows.Controls;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for EmpireEditor.xaml
    /// </summary>
    public partial class EmpireEditor : TabPageViewBase
    {
        public EmpireEditorViewModel ViewModel
        {
            get { return (EmpireEditorViewModel) DataContext; }
            set { DataContext = value; }
        }

        public EmpireEditor()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedEmpire != null)
            {
                ViewModel.StoreEmpire(ViewModel.SelectedEmpire);
            }
        }
    }
}
