using System;
using System.Linq;
using System.Net.Http;

namespace chapter10.lib.Helpers
{
    public static class ExtensionMethods
    {
        public static string[] ToPropertyList<T>(this Type objType, string labelName) => 
            objType.GetProperties().Where(a => a.Name != labelName).Select(a => a.Name).ToArray();

        public static string ToWebContentString(this string url)
        {
            using (var handler = new HttpClientHandler())
            {
                if (handler.SupportsRedirectConfiguration)
                {
                    handler.AllowAutoRedirect = true;
                    handler.MaxAutomaticRedirections = 5;
                }

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip |
                                                     System.Net.DecompressionMethods.Deflate;
                }

                using (var httpClient = new HttpClient(handler))
                {
                    return httpClient.GetStringAsync(url).Result;
                }
            }
        }
    }
}