using System;
using System.Threading.Tasks;

using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using chapter12_app.ViewModels;

namespace chapter12_app
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => (MainPageViewModel) DataContext;

        public MainPage()
        {
            InitializeComponent();

            DataContext = new MainPageViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var initialization = ViewModel.Initialize();

            if (initialization)
            {
                return;
            }

            await ShowMessage("Failed to initialize model - verify the model has been created");

            Application.Current.Exit();

            base.OnNavigatedTo(e);
        }

        public async Task<IUICommand> ShowMessage(string message)
        {
            var dialog = new MessageDialog(message);

            return await dialog.ShowAsync();
        }
    }
}