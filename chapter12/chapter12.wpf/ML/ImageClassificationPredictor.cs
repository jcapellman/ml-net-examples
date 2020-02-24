using chapter12.wpf.ML.Base;
using chapter12.wpf.ML.Objects;

using Microsoft.ML;

namespace chapter12.wpf.ML
{
    public class ImageClassificationPredictor : BaseML
    {
        private ITransformer _model;

        private struct InceptionSettings
        {
            public const int ImageHeight = 224;
            public const int ImageWidth = 224;
            public const float Mean = 117;
            public const float Scale = 1;
            public const bool ChannelsLast = true;
        }

        public ImageDataPredictionItem Predict(string filePath) => 
            Predict(new ImageDataInputItem 
                {
                    ImagePath = filePath 
                }
            );

        public bool Initialize()
        {
            IEstimator<ITransformer> pipeline = MlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: _imagesFolder, inputColumnName: nameof(ImageDataInputItem.ImagePath))
                .Append(MlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                .Append(MlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                .Append(MlContext.Model.LoadTensorFlowModel(_inceptionTensorFlowModel)
                .ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                .Append(MlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
                .Append(MlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                .Append(MlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
                .AppendCacheCheckpoint(MlContext);

            IDataView trainingData = MlContext.Data.LoadFromTextFile<ImageDataInputItem>(path: _trainTagsTsv, hasHeader: false);

            _model = pipeline.Fit(trainingData);

            return true;
        }

        public ImageDataPredictionItem Predict(ImageDataInputItem image)
        {
            var predictor = MlContext.Model.CreatePredictionEngine<ImageDataInputItem, ImageDataPredictionItem>(_model);

            return predictor.Predict(image);
        }
    }
}