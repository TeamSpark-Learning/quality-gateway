using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace _02_complex.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetHeaderValue(this HttpRequest httpRequest, string key)
        {
            return httpRequest.Headers[key];
        }

        public static async Task<string> GetBodyValueAsync(this HttpRequest httpRequest)
        {
            using (var reader = new StreamReader(httpRequest.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}