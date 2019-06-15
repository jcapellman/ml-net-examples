using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using UWP_Browser_Classification.ViewModels;

namespace UWP_Browser_Classification
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => (MainPageViewModel) DataContext;

        public MainPage()
        {
            InitializeComponent();

            DataContext = new MainPageViewModel();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            var webServiceUrl = ViewModel.WebServiceURL;

            if (!webServiceUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) &&
                !webServiceUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                webServiceUrl = $"http://{webServiceUrl}";
            }

            wvMain.Navigate(new Uri(webServiceUrl));
        }
    }
}