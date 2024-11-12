
using ContactBook.Core.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;


namespace ContactBook
{
    /// <summary>
    /// Interaction logic for PearsonPage.xaml
    /// </summary>
    public partial class PearsonPage : Page
    {
        public PearsonPage()
        {
            InitializeComponent();
            DataContext = new PearsonPageViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is PearsonPageViewModel viewModel)
            {
                viewModel.LoadDataFromDatabase();
            }
        }
    }
}
