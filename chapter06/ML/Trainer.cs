using System;
using System.IO;

using chapter04.Common;
using chapter04.ML.Base;
using chapter04.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace chapter04.ML
{
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            var trainingDataView = MlContext.Data.LoadFromTextFile<CarInventory>(trainingFileName, ',');

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.4);

            var dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(CarInventory.Label))
                .Append(MlContext.Transforms.Concatenate("Features",
                    typeof(CarInventory).ToPropertyList<CarInventory>(nameof(CarInventory.Label))));

            var options = new RandomizedPcaTrainer.Options
            {
                FeatureColumnName = "NormalizedFeatures",
                ExampleWeightColumnName = null,
                Rank = 28,
                Oversampling = 20,
                EnsureZeroMean = true,
                Seed = 1
            };

            var trainer = MlContext.AnomalyDetection.Trainers.RandomizedPca(options);

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            var modelMetrics = MlContext.AnomalyDetection.Evaluate(testSetTransform, 
                labelColumnName: nameof(CarInventoryPrediction.Label), 
                scoreColumnName: nameof(CarInventoryPrediction.Score),
                predictedLabelColumnName: nameof(CarInventoryPrediction.PredictedLabel));

            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Detection at FP Count: {modelMetrics.DetectionRateAtFalsePositiveCount}");
        }
    }
}