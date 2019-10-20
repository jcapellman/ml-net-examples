using System;
using System.IO;

using chapter05.ML.Base;
using chapter05.ML.Objects;

using Microsoft.ML;

namespace chapter05.ML
{
    public class Predictor : BaseML
    {
        public void Predict(string inputDataFile)
        {
            if (!System.IO.File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");

                return;
            }

            if (!System.IO.File.Exists(inputDataFile))
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

            var json = System.IO.File.ReadAllText(inputDataFile);

            // TODO Read in Bytes
            var prediction = predictionEngine.Predict(new FileData());

            Console.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" + 
                                $"The car price is a {(prediction.PredictedLabel ? "good" : "bad")} deal, with a {prediction.Probability:P0} confidence");
        }
    }
}