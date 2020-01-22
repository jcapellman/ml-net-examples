using System;
using System.IO;

using chapter10.lib.ML.Objects;

using Microsoft.ML;

namespace chapter10.lib.ML
{
    public class Trainer
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

        public static void TrainModel(string modelOutputPath, string inputDataPath)
        {
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