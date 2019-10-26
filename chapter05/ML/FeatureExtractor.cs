using System;
using System.IO;

using chapter05.Common;
using chapter05.ML.Base;
using chapter05.ML.Objects;

namespace chapter05.ML
{
    public class FeatureExtractor : BaseML
    {
        public void Extract(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);

            using (var streamWriter =
                new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"../../../Data/{Constants.SAMPLE_DATA}")))
            {
                foreach (var file in files)
                {
                    var extractedData = new FileData(File.ReadAllBytes(file));

                    streamWriter.WriteLine($"{file[0]},{extractedData.Header},{extractedData.Size}");
                }
            }

            Console.WriteLine($"Extracted {files.Length} to {Constants.SAMPLE_DATA}");
        }
    }
}