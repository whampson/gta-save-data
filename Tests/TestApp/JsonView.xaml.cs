using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for JsonView.xaml
    /// </summary>
    public partial class JsonView : TabPageViewBase
    {
        public JsonView()
        {
            InitializeComponent();
        }

        public JsonViewModel ViewModel
        {
            get { return (JsonViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void BlockComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateTextBox();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel.SelectedBlockIndex == 0)
            {
                ViewModel.UpdateTextBox();
            }
        }
    }
}
