using System.Reflection;

using Microsoft.ML;

using UWP_Browser_Classification.Enums;
using UWP_Browser_Classification.ML.Objects;

namespace UWP_Browser_Classification.ML
{
    public class Prediction
    {
        private readonly MLContext MlContext = new MLContext();
        private ITransformer _model;

        private const double THRESHOLD = 0.5;

        public Prediction()
        {
            var assembly = typeof(Prediction).GetTypeInfo().Assembly;

            var resource = assembly.GetManifestResourceStream("classification.mdl");

            _model = MlContext.Model.Load(resource, out var schema);
        }

        public Classification Predict(string html)
        {
            var predictor = MlContext.Model.CreatePredictionEngine<InputItem, OutputItem>(_model);

            var result = predictor.Predict(new InputItem { Content = html });

            return result.Confidence > THRESHOLD ? Classification.MALICIOUS : Classification.BENIGN;
        }
    }
}