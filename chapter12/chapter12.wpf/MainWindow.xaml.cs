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

            var initialization = ViewModel.Initialize();

            if (initialization)
            {
                return;
            }

            MessageBox.Show("Failed to initialize model - verify the model has been copied properly");
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectFile();
        }
    }
}