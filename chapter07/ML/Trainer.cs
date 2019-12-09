using System;
using System.IO;

using chapter07.ML.Base;
using chapter07.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace chapter07.ML
{
    public class Trainer : BaseML
    {
        private const string UserIDEncoding = "UserIDEncoding";

        private const string MovieIDEncoding = "MovieIDEncoding";

        private (IDataView DataView, IEstimator<ITransformer> Transformer) GetDataView(string fileName, bool training = true)
        {
            var trainingDataView = MlContext.Data.LoadFromTextFile<MusicRating>(fileName, ',');

            if (!training)
            {
                return (trainingDataView, null);
            }

            IEstimator<ITransformer> dataProcessPipeline = 
                MlContext.Transforms.Conversion.MapValueToKey(outputColumnName: UserIDEncoding, inputColumnName: nameof(MusicRating.UserID))
                .Append(MlContext.Transforms.Conversion.MapValueToKey(outputColumnName: MovieIDEncoding, inputColumnName: nameof(MusicRating.MovieID)));

            return (trainingDataView, dataProcessPipeline);
        }

        public void Train(string trainingFileName, string testingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            var trainingDataView = GetDataView(trainingFileName);

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = UserIDEncoding,
                MatrixRowIndexColumnName = MovieIDEncoding,
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 10,
                Quiet = false
            };

            var trainerEstimator = trainingDataView.Transformer.Append(MlContext.Recommendation().Trainers.MatrixFactorization(options));

            ITransformer trainedModel = trainerEstimator.Fit(trainingDataView.DataView);

            MlContext.Model.Save(trainedModel, trainingDataView.DataView.Schema, ModelPath);

            Console.WriteLine($"Model saved to {ModelPath}");

            var testingDataView = GetDataView(testingFileName, true);

            var testSetTransform = trainedModel.Transform(testingDataView.DataView);

            var modelMetrics = MlContext.Recommendation().Evaluate(testSetTransform);

            Console.WriteLine($"Loss Function: {modelMetrics.LossFunction:P2}{Environment.NewLine}" +
                              $"Mean Absolute Error: {modelMetrics.MeanAbsoluteError:P2}{Environment.NewLine}" +
                              $"Mean Squared Error: {modelMetrics.MeanSquaredError:P2}{Environment.NewLine}" +
                              $"R Squared: {modelMetrics.RSquared:P2}{Environment.NewLine}" +
                              $"Root Mean Squared Error: {modelMetrics.RootMeanSquaredError:P2}");
        }
    }
}