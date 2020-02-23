using Microsoft.ML.Data;

namespace chapter12.lib.ML.Objects
{
    public class ImageDataInputItem
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;
    }
}