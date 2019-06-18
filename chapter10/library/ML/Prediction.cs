using System.Reflection;

using chapter10_library.Enums;
using chapter10_library.ML.Objects;

using Microsoft.ML;

namespace chapter10_library.ML
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