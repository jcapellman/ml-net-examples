using System;
using System.IO;

using chapter03.ML.Base;
using chapter03.ML.Objects;

using Microsoft.ML;

namespace chapter03.ML
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

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            var dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(EmploymentHistory.DurationInMonths))
                .Append(MlContext.Transforms.Categorical.OneHotEncoding("PositionNameEncoded", "PositionName"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("IsMarried"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("BSDegree"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("MSDegree"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("YearsExperience")
                .Append(MlContext.Transforms.NormalizeMeanVariance("AgeAtHire"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("HasKids"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("WithinMonthOfVesting"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("DeskDecorations"))
                .Append(MlContext.Transforms.NormalizeMeanVariance("LongCommute"))
                .Append(MlContext.Transforms.Concatenate("Features", "PositionNameEncoded", 
                    "IsMarried", "BSDegree", "MSDegree", "YearsExperience", "AgeAtHire", "HasKids", 
                    "WithinMonthOfVesting", "DeskDecorations", "LongCommute")));

            var trainer = MlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            var modelMetrics = MlContext.Regression.Evaluate(
                data: testSetTransform, 
                labelColumnName: nameof(EmploymentHistory.DurationInMonths));

            Console.WriteLine($"Loss Function: {modelMetrics.LossFunction:P2}{Environment.NewLine}" +
                              $"Mean Absolute Error: {modelMetrics.MeanAbsoluteError:P2}{Environment.NewLine}" +
                              $"Mean Squared Error: {modelMetrics.MeanSquaredError:P2}{Environment.NewLine}" +
                              $"RSquared: {modelMetrics.RSquared:P2}{Environment.NewLine}" +
                              $"Root Mean Squared Error: {modelMetrics.RootMeanSquaredError:P2}");
        }
    }
}