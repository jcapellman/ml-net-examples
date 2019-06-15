using System.ComponentModel;using System.Runtime.CompilerServices;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
