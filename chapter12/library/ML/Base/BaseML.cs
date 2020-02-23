using System;
using System.IO;

using Microsoft.ML;

namespace chapter12.lib.ML.Base
{
    public class BaseML
    {
        protected static string _assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
        protected static string _imagesFolder = Path.Combine(_assetsPath, "images");
        protected string _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
        protected string _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
        protected string _predictSingleImage = Path.Combine(_imagesFolder, "toaster3.jpg");
        protected string _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");

        protected MLContext MlContext;

        public BaseML()
        {
            MlContext = new MLContext(2020);
        }
    }
}