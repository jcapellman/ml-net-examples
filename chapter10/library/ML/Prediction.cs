using System;
using System.Reflection;

using chapter10_library.Common;
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

        public bool Initialize()
        {
            try
            {
                var assembly = typeof(Prediction).GetTypeInfo().Assembly;

                var resource = assembly.GetManifestResourceStream(Constants.MODEL_NAME);

                _model = MlContext.Model.Load(resource, out var schema);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Classification Predict(string html)
        {
            var predictor = MlContext.Model.CreatePredictionEngine<InputItem, OutputItem>(_model);

            var result = predictor.Predict(new InputItem { HTMLContent = html });

            return result.Probability > THRESHOLD ? Classification.MALICIOUS : Classification.BENIGN;
        }
    }
}