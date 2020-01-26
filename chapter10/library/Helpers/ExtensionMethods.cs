using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace chapter10.lib.Helpers
{
    public static class ExtensionMethods
    {
        public static string[] ToPropertyList<T>(this Type objType, string labelName) => 
            objType.GetProperties().Where(a => a.Name != labelName).Select(a => a.Name).ToArray();

        public static async Task<string> ToWebContentString(this string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}