using System;
using System.IO;

using chapter05.Enums;
using chapter05.ML.Base;
using chapter05.ML.Objects;

using Microsoft.ML;

namespace chapter05.ML
{
    public class Predictor : BaseML
    {
        public void Predict(string inputDataFile)
        {
            if (!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");

                return;
            }

            if (!File.Exists(inputDataFile))
            {
                Console.WriteLine($"Failed to find input data at {inputDataFile}");

                return;
            }

            ITransformer mlModel;
            
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return;
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileData, FileTypePrediction>(mlModel);

            var prediction = predictionEngine.Predict(new FileData(File.ReadAllBytes(inputDataFile)));

            Console.WriteLine(
                $"Based on input file:{Environment.NewLine}" +
                $"{inputDataFile}{Environment.NewLine}{Environment.NewLine}" +
                $"The file is predicted to be a {(FileTypes) (prediction.PredictedClusterId - 1)}{Environment.NewLine}");

            Console.WriteLine("Distances from all clusters:");

            for (var x = 0; x < prediction.Distances.Length; x++) { 
                Console.WriteLine($"{(FileTypes)x}: {prediction.Distances[x]}");
            }
        }
    }
}