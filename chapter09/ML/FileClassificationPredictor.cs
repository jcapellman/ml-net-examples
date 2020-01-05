using System.IO;

using chapter09.Data;
using chapter09.Helpers;
using chapter09.ML.Objects;

using Microsoft.ML;

namespace chapter09.ML
{
    public class FileClassificationPredictor
    {
        protected readonly MLContext MlContext;

        protected FileClassificationPredictor()
        {
            MlContext = new MLContext(2020);
        }

        public FileClassificationResponseItem Predict(FileClassificationResponseItem file)
        {
            ITransformer mlModel;

            using (var stream = new FileStream(Common.Constants.MODEL_PATH, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileData, FileDataPrediction>(mlModel);

            var prediction = predictionEngine.Predict(file.ToFileData());

            file.Confidence = prediction.Probability;
            file.IsMalicious = prediction.Label;

            return file;
        }
    }
}
