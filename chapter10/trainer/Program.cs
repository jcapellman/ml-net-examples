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
            using (var fileStream = new FileStream(OutputModelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(trainedModel, schema, fileStream);
            }

            return true;
        }

        private static (string ModelOutputPath, string InputDataPath) ParseArgs(string[] args)
        {
            var outputPath = "../../chapter10_app/classification.mdl";
            var inputDataPath = "data.csv";

            switch (args.Length)
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

            var featuresColumnName = "Features";

            var estimator = MlContext.Transforms.Text.FeaturizeText(nameof(InputItem.Content))
                .Append(MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: featuresColumnName));

            var model = estimator.Fit(splitDataView.TrainSet);

            var predictions = model.Transform(splitDataView.TestSet);

            var metrics = MlContext.BinaryClassification.Evaluate(predictions, "Label");

            SaveModel(model, splitDataView.TrainSet.Schema, modelOutputPath);
        }
    }
}