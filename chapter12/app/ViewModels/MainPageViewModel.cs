using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using chapter12.lib.ML;

namespace chapter12_app.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly ImageClassificationPredictor _prediction = new ImageClassificationPredictor();

        private string _imageClassification;

        public string ImageClassification
        {
            get => _imageClassification;

            set
            {
                _imageClassification = value;
                OnPropertyChanged();
            }
        }

        public bool Initialize() => _prediction.Initialize();

        public void Classify(string imagePath)
        {
            var result = _prediction.Predict(imagePath);

            ImageClassification = $"Image ({imagePath}) is a {result.PredictedLabelValue} with a confidence of {result.Score.Max()}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}