using System;

using chapter05.ML.Base;
using chapter05.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter05.ML
{
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName, string testFileName)
        {
            if (!System.IO.File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!System.IO.File.Exists(testFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testFileName}");

                return;
            }

            var trainingDataView = MlContext.Data.LoadFromTextFile(path: trainingFileName,
                columns: new[]
                {
                    new TextLoader.Column(nameof(FileData.Label), DataKind.Single, 0),
                    new TextLoader.Column(nameof(FileData.Size), DataKind.Single, 1),
                    new TextLoader.Column(nameof(FileData.Header), DataKind.Single, 2),
                    new TextLoader.Column(nameof(FileData.IsText), DataKind.Single, 3)
                },
                hasHeader: true,
                separatorChar: ',');

            var dataProcessPipeline = MlContext.Transforms.Concatenate(
                FEATURES,
                nameof(FileData.Size), 
                nameof(FileData.Header),
                nameof(FileData.IsText));

            var trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: FEATURES, numberOfClusters: 3);
            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            var testingDataView = MlContext.Data.LoadFromTextFile(path: testFileName,
                columns: new[]
                {
                    new TextLoader.Column(nameof(FileData.Label), DataKind.Single, 0),
                    new TextLoader.Column(nameof(FileData.Size), DataKind.Single, 1),
                    new TextLoader.Column(nameof(FileData.Header), DataKind.Single, 2),
                    new TextLoader.Column(nameof(FileData.IsText), DataKind.Single, 3)
                },
                hasHeader: true,
                separatorChar: ',');

            IDataView testDataView = trainedModel.Transform(testingDataView);

            var modelMetrics = MlContext.Clustering.Evaluate(
                data: testDataView,
                scoreColumnName: "Score",
                featureColumnName: FEATURES);

            Console.WriteLine($"Average Distance: {modelMetrics.AverageDistance}");
            Console.WriteLine($"Davies Bould Index: {modelMetrics.DaviesBouldinIndex}");
            Console.WriteLine($"Normalized Mutual Information: {modelMetrics.NormalizedMutualInformation}");
        }
    }
}