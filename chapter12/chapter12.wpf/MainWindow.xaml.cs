using chapter12.wpf.ViewModels;

using System.Windows;

namespace chapter12.wpf
{
    public partial class MainWindow : Window
    {
        private MainPageViewModel ViewModel => (MainPageViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainPageViewModel();

            (bool Success, string Exception) = ViewModel.Initialize();

            if (Success)
            {
                return;
            }

            MessageBox.Show($"Failed to initialize model - {Exception}");

            Application.Current.Shutdown();
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectFile();
        }
    }
}