using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using UWP_Browser_Classification.Enums;

namespace UWP_Browser_Classification.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _enableGoButton;

        public bool EnableGoButton
        {
            get => _enableGoButton;

            set
            {
                _enableGoButton = value;
                OnPropertyChanged();
            }
        }

        private string _webServiceURL;

        public string WebServiceURL
        {
            get => _webServiceURL;

            set
            {
                _webServiceURL = value;

                OnPropertyChanged();

                EnableGoButton = !string.IsNullOrEmpty(value);
            }
        }

        public Uri BuildUri()
        {
            var webServiceUrl = WebServiceURL;

            if (!webServiceUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) &&
                !webServiceUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                webServiceUrl = $"http://{webServiceUrl}";
            }

            return new Uri(webServiceUrl);
        }

        public async Task<Classification> ClassifyAsync(string html)
        {
            // TODO: Run Model

            return Classification.BENIGN;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}