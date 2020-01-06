using System;

using chapter09.lib.Common;
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
                    new TextLoader.Column(nameof(FileData.IsLarge), DataKind.Single, 0),
                    new TextLoader.Column(nameof(FileData.IsPE), DataKind.Single, 1),
                    new TextLoader.Column(nameof(FileData.HasImports), DataKind.Single, 2),
                    new TextLoader.Column(nameof(FileData.Label), DataKind.Single, 3)
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

            var dataProcessPipeline = MlContext.Transforms.Concatenate(
                FEATURES,
                nameof(FileData.IsLarge),
                nameof(FileData.IsPE),
                nameof(FileData.HasImports));

            var trainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(featureColumnName: FEATURES);
            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, Constants.MODEL_PATH);

            var testingDataView = GetDataView(testingFileName);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            var modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testDataView,
                labelColumnName: "Label",
                scoreColumnName: "Score");

            Console.WriteLine($"Entropy: {modelMetrics.Entropy}");
            Console.WriteLine($"Log Loss: {modelMetrics.LogLoss}");
            Console.WriteLine($"Log Loss Reduction: {modelMetrics.LogLossReduction}");
        }
    }
}