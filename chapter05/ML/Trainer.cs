using System;

using chapter05.ML.Base;
using chapter05.ML.Objects;

using Microsoft.ML;

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

            var trainingDataView = MlContext.Data.LoadFromTextFile<FileData>(trainingFileName, ',', hasHeader: false);

            var dataProcessPipeline = MlContext.Transforms
                .Concatenate(FEATURES, nameof(FileData.Size), nameof(FileData.Size), nameof(FileData.Header))
                .Append(MlContext.Clustering.Trainers.KMeans(FEATURES, numberOfClusters: 3));

            var trainedModel = dataProcessPipeline.Fit(trainingDataView);
            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            var testDataView = MlContext.Data.LoadFromTextFile<FileData>(testFileName, ',', hasHeader: false);

            var modelMetrics = MlContext.Clustering.Evaluate(data: testDataView,
                labelColumnName: FEATURES,
                scoreColumnName: nameof(FileTypePrediction.Distances));

            Console.WriteLine($"Average Distance: {modelMetrics.AverageDistance}");
            Console.WriteLine($"Davies Bould Index: {modelMetrics.DaviesBouldinIndex}");
            Console.WriteLine($"Normalized Mutual Information: {modelMetrics.NormalizedMutualInformation}");
        }
    }
}