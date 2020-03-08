using System;
using System.IO;

using chapter13.wpf.ML.Base;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter13.wpf.ML
{
    public class ImageClassificationPredictor : BaseML
    {
        private static readonly string ML_NET_MODEL = Path.Combine(Environment.CurrentDirectory, "chapter13.mdl");

        private ITransformer _model;

        public struct ImageNetSettings
        {
            public const int imageHeight = 416;
            public const int imageWidth = 416;
        }

        public struct TinyYoloModelSettings
        {
            public const string ModelInput = "image";

            public const string ModelOutput = "grid";
        }

        public (bool Success, string Exception) Initialize()
        {
            try
            {
                if (File.Exists(ML_NET_MODEL))
                {
                    _model = MlContext.Model.Load(ML_NET_MODEL, out DataViewSchema modelSchema);

                    return (true, string.Empty);
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        public (string[] Labels, byte[] DrawnImageBytes) Predict(string fileName)
        {
            var scoredData = _model.Transform(MlContext.Data.LoadFromBinary(fileName));

            var probabilities = scoredData.GetColumn<float[]>(TinyYoloModelSettings.ModelOutput);

            return probabilities;
        }
    }
}