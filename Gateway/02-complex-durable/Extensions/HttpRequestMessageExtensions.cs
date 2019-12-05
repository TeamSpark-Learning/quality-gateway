using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _02_complex.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetHeaderValue(this HttpRequestMessage httpRequestMessage, string key)
        {
            return httpRequestMessage.Headers.GetValues(key).Single();
        }

        public static async Task<dynamic> GetBodyValueAsync(this HttpRequestMessage httpRequestMessage)
        {
            return await httpRequestMessage.Content.ReadAsStringAsync();
        }
    }
}