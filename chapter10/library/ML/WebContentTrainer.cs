using System;
using System.IO;

using chapter10.lib.ML.Base;
using chapter10.lib.ML.Objects;

using Microsoft.ML;

namespace chapter10.lib.ML
{
    public class WebContentTrainer : BaseML
    {
        public void Train(string trainingFileName, string testingFileName, string modelFileName)
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

            if (!System.IO.File.Exists(modelFileName))
            {
                Console.WriteLine($"Failed to find model file ({modelFileName}");

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

            MlContext.Model.Save(trainedModel, dataView.Schema, Path.Combine(AppContext.BaseDirectory, modelFileName));

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