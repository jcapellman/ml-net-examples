using System;

using chapter09.lib.Common;
using chapter09.lib.Helpers;
using chapter09.lib.ML.Base;
using chapter09.lib.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter09.lib.ML
{
    public class FileClassificationTrainer : BaseML
    {
        private IDataView GetDataView(string fileName)
        {
            return MlContext.Data.LoadFromTextFile(path: fileName,
                columns: new[]
                {
                    new TextLoader.Column(nameof(FileData.FileSize), DataKind.Single, 0),
                    new TextLoader.Column(nameof(FileData.Is64Bit), DataKind.Single, 1),
                    new TextLoader.Column(nameof(FileData.NumberImportFunctions), DataKind.Single, 2),
                    new TextLoader.Column(nameof(FileData.NumberExportFunctions), DataKind.Single, 3),
                    new TextLoader.Column(nameof(FileData.IsSigned), DataKind.Single, 4),
                    new TextLoader.Column(nameof(FileData.NumberImports), DataKind.Single, 5),
                    new TextLoader.Column(nameof(FileData.Label), DataKind.Boolean, 6)
                },
                hasHeader: false,
                separatorChar: ',');
        }

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

            var trainingDataView = GetDataView(trainingFileName);

            var dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText(nameof(FileData.Strings))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.FileSize)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.Is64Bit)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.IsSigned)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberImportFunctions)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberExportFunctions)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberImports)))
                .Append(MlContext.Transforms.Concatenate(FEATURES, 
                    nameof(FileData.Strings), nameof(FileData.FileSize), nameof(FileData.Is64Bit),
                    nameof(FileData.IsSigned), nameof(FileData.NumberImportFunctions), nameof(FileData.NumberExportFunctions),
                    nameof(FileData.NumberImports)));

            var trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(FileData.Label),
                featureColumnName: "FeaturesNormalizedByMeanVar",
                numberOfLeaves: 2,
                numberOfTrees: 1000,
                minimumExampleCountPerLeaf: 1,
                learningRate: 0.2);

            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, Constants.MODEL_PATH);

            var testingDataView = GetDataView(testingFileName);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            var modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testDataView,
                labelColumnName: nameof(FileDataPrediction.Label),
                scoreColumnName: nameof(FileDataPrediction.Score));

            Console.WriteLine($"Entropy: {modelMetrics.Entropy}");
            Console.WriteLine($"Log Loss: {modelMetrics.LogLoss}");
            Console.WriteLine($"Log Loss Reduction: {modelMetrics.LogLossReduction}");
        }
    }
}