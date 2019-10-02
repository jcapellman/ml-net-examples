using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using chapter03_logistic_regression.ML.Base;
using chapter03_logistic_regression.ML.Objects;

using Microsoft.ML;

namespace chapter03_logistic_regression.ML
{
    public class Predictor : BaseML
    {
        private static Regex _stringRex;

        public Predictor()
        {
            _stringRex = new Regex(@"[ -~\t]{8,}", RegexOptions.Compiled);
        }

        protected string GetStrings(byte[] data)
        {
            var stringLines = new StringBuilder();

            if (data == null || data.Length == 0)
            {
                return stringLines.ToString();
            }
            
            using (var ms = new MemoryStream(data, false))
            {
                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(1252), false, 2048, false))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        line = line.Replace("^", "").Replace(")", "").Replace("-", "");

                        stringLines.Append(string.Join(string.Empty,
                            _stringRex.Matches(line).Where(a => !string.IsNullOrEmpty(a.Value) && !string.IsNullOrWhiteSpace(a.Value)).ToList()));
                    }
                }
            }

            return string.Join(string.Empty, stringLines);
        }

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

            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileInput, FilePrediction>(mlModel);

            var prediction = predictionEngine.Predict(new FileInput
            {
                Strings = GetStrings(File.ReadAllBytes(inputDataFile))
            });

            Console.WriteLine(
                                $"Based on the file ({inputDataFile})" +
                                $"the file is classified as {(prediction.IsMalicious ? "malicious" : "benign")}" + 
                                $"at a confidence level of {prediction.Probability:P0}");
        }
    }
}