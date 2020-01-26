using System;

using chapter10.lib.Common;
using chapter10.lib.ML.Base;
using chapter10.lib.ML.Objects;

using Microsoft.ML;

namespace chapter10.lib.ML
{
    public class WebContentTrainer : BaseML
    {
        public void Train(string trainingFileName, string testingFileName)
        {
            if (!System.IO.File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!System.IO.File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            var dataView = MlContext.Data.LoadFromTextFile<WebPageInputItem>(trainingFileName, hasHeader: false);

            var dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText("FeaturizeText", nameof(WebPageInputItem.HTMLContent))
                .Append(MlContext.Transforms.Concatenate(FEATURES, "FeaturizeText"));

            var trainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(WebPageInputItem.Label),
                featureColumnName: FEATURES);

            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(dataView);

            MlContext.Model.Save(trainedModel, dataView.Schema, Constants.MODEL_PATH);

            var testingDataView = MlContext.Data.LoadFromTextFile<WebPagePredictionItem>(testingFileName, hasHeader: false);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            var modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testDataView,
                labelColumnName: nameof(WebPagePredictionItem.Prediction),
                scoreColumnName: nameof(WebPagePredictionItem.Score));

            Console.WriteLine($"Entropy: {modelMetrics.Entropy}");
            Console.WriteLine($"Log Loss: {modelMetrics.LogLoss}");
            Console.WriteLine($"Log Loss Reduction: {modelMetrics.LogLossReduction}");
        }
    }
}