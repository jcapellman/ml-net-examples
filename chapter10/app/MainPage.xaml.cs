using System;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using chapter10_library.Enums;

using chapter10_app.ViewModels;

namespace chapter10_app
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => (MainPageViewModel) DataContext;

        public MainPage()
        {
            InitializeComponent();

            DataContext = new MainPageViewModel();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e) => Navigate();

        private async void WvMain_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var html = await sender.InvokeScriptAsync("eval", new[] { "document.documentElement.outerHTML;" });
            
            var (classificationResult, browserContent) = ViewModel.Classify(html);

            switch (classificationResult)
            {
                case Classification.BENIGN:
                    return;
                case Classification.MALICIOUS:
                    sender.NavigateToString(browserContent);
                    break;
            }
        }

        private void Navigate()
        {
            wvMain.Navigate(ViewModel.BuildUri());
        }

        private void TxtBxUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && ViewModel.EnableGoButton)
            {
                Navigate();
            }
        }
    }
}