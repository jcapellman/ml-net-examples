using System;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using UWP_Browser_Classification.Enums;
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

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            wvMain.Navigate(ViewModel.BuildUri());
        }

        private async void WvMain_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var html = await sender.InvokeScriptAsync("eval", new[] { "document.documentElement.outerHTML;" });
            
            var classification = await ViewModel.ClassifyAsync(html);

            switch (classification)
            {
                case Classification.BENIGN:
                    return;
                case Classification.MALICIOUS:
                    sender.NavigateToString($"<html><body>{ViewModel.WebServiceURL} was found to be a malicious site</body></html>");
                    break;
            }
        }

        private void TxtBxUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                BtnGo_Click(null, null);
            }
        }
    }
}