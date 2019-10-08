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

            var trainingDataView = MlContext.Data.LoadFromTextFile<EmploymentHistory>(trainingFileName, ',');

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.4);

            var dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(EmploymentHistory.DurationInMonths))
                .Append(MlContext.Transforms.Concatenate("Features",
                    typeof(EmploymentHistory).ToPropertyList<EmploymentHistory>(nameof(EmploymentHistory.DurationInMonths)))));

            var trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            var modelMetrics = MlContext.Regression.Evaluate(testSetTransform);

            Console.WriteLine($"Loss Function: {modelMetrics.LossFunction:0.##}{Environment.NewLine}" +
                              $"Mean Absolute Error: {modelMetrics.MeanAbsoluteError:#.##}{Environment.NewLine}" +
                              $"Mean Squared Error: {modelMetrics.MeanSquaredError:#.##}{Environment.NewLine}" +
                              $"RSquared: {modelMetrics.RSquared:0.##}{Environment.NewLine}" +
                              $"Root Mean Squared Error: {modelMetrics.RootMeanSquaredError:#.##}");
        }
    }
}