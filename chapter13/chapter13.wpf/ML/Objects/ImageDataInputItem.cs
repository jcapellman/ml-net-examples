using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.ML.Data;

namespace chapter13.wpf.ML.Objects
{
    public class ImageDataInputItem
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;

        public static IEnumerable<ImageDataInputItem> ReadFromFile(string imageFolder)
        {
            return Directory
                .GetFiles(imageFolder)
                .Where(filePath => Path.GetExtension(filePath) != ".md")
                .Select(filePath => new ImageDataInputItem { ImagePath = filePath, Label = Path.GetFileName(filePath) });
        }
    }
}