using System;
using System.IO;

using chapter06.ML.Base;
using chapter06.ML.Objects;

using Microsoft.ML;

namespace chapter06.ML
{
    public class Trainer : BaseML
    {
        private const int PvalueHistoryLength = 30;
        private const int SeasonalityWindowSize = 30;
        private const int TrainingWindowSize = 90;
        private const int Confidence = 98;

        private IDataView GetDataView(string fileName, bool training = true) => 
            MlContext.Data.LoadFromTextFile<NetworkTrafficHistory>(fileName, separatorChar: ',', hasHeader: false);

        public void Train(string trainingFileName, string testingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            var trainingDataView = GetDataView(trainingFileName);

            var trainingPipeLine = MlContext.Transforms.DetectSpikeBySsa(
                nameof(NetworkTrafficPrediction.Prediction),
                nameof(NetworkTrafficHistory.BytesTransferred),
                confidence: Confidence,
                pvalueHistoryLength: PvalueHistoryLength,
                trainingWindowSize: TrainingWindowSize,
                seasonalityWindowSize: SeasonalityWindowSize);

            ITransformer trainedModel = trainingPipeLine.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            var testingDataView = GetDataView(testingFileName, true);

            var testSetTransform = trainedModel.Transform(testingDataView);

            var modelMetrics = MlContext.AnomalyDetection.Evaluate(testSetTransform);

            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Detection at FP Count: {modelMetrics.DetectionRateAtFalsePositiveCount}");
        }
    }
}