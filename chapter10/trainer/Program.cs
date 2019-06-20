using System;
using System.Collections.Generic;
using System.IO;

using chapter10_library.ML.Objects;

using Microsoft.ML;

namespace UWP_Browser_Classification_Trainer
{
    class Program
    {
        protected static readonly MLContext MlContext = new MLContext();

        private static bool SaveModel(ITransformer trainedModel, DataViewSchema schema, string OutputModelPath)
        {
            try
            {
                using (var fileStream =
                    new FileStream(OutputModelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    MlContext.Model.Save(trainedModel, schema, fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveModel: Exception thrown when saving {OutputModelPath}: {ex}");

                return false;
            }
        }

        private static (string ModelOutputPath, string InputDataPath) ParseArgs(IReadOnlyList<string> args)
        {
            const string outputPath = "../../chapter10_app/classification.mdl";
            const string inputDataPath = "data.csv";

            switch (args.Count)
            {
                case 0:
                    return (outputPath, inputDataPath);
                case 1:
                    return (outputPath, args[0]);
                default:
                    return (args[0], args[1]);
            }
        }

        static void Main(string[] args)
        {
            var (modelOutputPath, inputDataPath) = ParseArgs(args);

            var dataView = MlContext.Data.LoadFromTextFile<InputItem>(inputDataPath, hasHeader: false);

            var splitDataView = MlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            var estimator = MlContext.Transforms.Text.FeaturizeText(nameof(InputItem.HTMLContent))
                .Append(MlContext.BinaryClassification.Trainers.FastTree());

            var model = estimator.Fit(splitDataView.TrainSet);

            var predictions = model.Transform(splitDataView.TestSet);

            var metrics = MlContext.BinaryClassification.Evaluate(predictions);

            Console.WriteLine($"Metrics: Entropy={metrics.Entropy} | Accuracy={metrics.Accuracy} | AUC={metrics.AreaUnderRocCurve}");

            var modelCreationResult = SaveModel(model, splitDataView.TrainSet.Schema, modelOutputPath);

            if (modelCreationResult)
            {
                Console.WriteLine($"Successfully saved model to {modelOutputPath}");

                return;
            }

            Console.WriteLine("Failed to write model");
        }
    }
}