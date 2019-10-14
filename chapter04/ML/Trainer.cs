using System;
using System.IO;

using chapter04.Common;
using chapter04.ML.Base;
using chapter04.ML.Objects;

using Microsoft.ML;

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

            IEstimator<ITransformer> dataProcessPipeline = MlContext.Transforms.Concatenate("Features", 
                    typeof(CarInventory).ToPropertyList<CarInventory>(nameof(CarInventory.Label)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(inputColumnName: "Features",
                    outputColumnName: "FeaturesNormalizedByMeanVar"));

            var trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(CarInventory.Label),
                featureColumnName: "FeaturesNormalizedByMeanVar",
                numberOfLeaves: 2,
                numberOfTrees: 1000,
                minimumExampleCountPerLeaf: 1,
                learningRate: 0.2);

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var fccPipeline = trainedModel.Append(MlContext.Transforms
                .CalculateFeatureContribution(trainedModel.LastTransformer)
                .Fit(dataProcessPipeline.Fit(trainingDataView).Transform(trainingDataView)));

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

          //  var modelMetrics = MlContext.BinaryClassification.Evaluate(data: testSetTransform,
         //       labelColumnName: nameof(CarInventory.Label),
         //       scoreColumnName: "Score");

        //    Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
         //                     $"Precision Recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}");
        }
    }
}