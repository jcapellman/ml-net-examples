using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace chapter10.lib.ML
{
    public class FeatureExtract
    {
        private static async Task<string> GetWebContent(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private static async void GetContentFile(string inputFile, string outputFile)
        {
            var urls = File.ReadAllLines(inputFile);

            var urlContent = new List<string>();

            foreach (var url in urls)
            {
                var content = await GetWebContent(url);

                urlContent.Add(content);
            }

            File.WriteAllText(outputFile, string.Join(",", urlContent));
        }

        public static void Extract(string cleanFile, string maliciousFile)
        {
            GetContentFile(cleanFile, "clean.csv");

            GetContentFile(maliciousFile, "malicious.csv");
        }
    }
}