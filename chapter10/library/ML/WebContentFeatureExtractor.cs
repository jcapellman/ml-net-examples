using System;
using System.Collections.Generic;
using System.IO;

using chapter10.lib.Helpers;

namespace chapter10.lib.ML
{
    public class WebContentFeatureExtractor
    {
        private static async void GetContentFile(string inputFile, string outputFile)
        {
            var lines = File.ReadAllLines(inputFile);

            var urlContent = new List<string>();

            foreach (var line in lines)
            {
                var url = line.Split(',')[0];
                var label = Convert.ToBoolean(line.Split(',')[1]);

                var content = await url.ToWebContentString();

                urlContent.Add($"{label},{content}");
            }

            File.WriteAllText(outputFile, string.Join(",", urlContent));
        }

        public void Extract(string trainingURLList, string testURLList, string trainingOutputFileName, string testingOutputFileName)
        {
            GetContentFile(trainingURLList, trainingOutputFileName);

            GetContentFile(testURLList, testingOutputFileName);
        }
    }
}