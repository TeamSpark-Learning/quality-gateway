using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace _02_complex.Helpers
{
    public class HttpHelper
    {
        public static async Task<string> PostDataAsync(string url, string requestBody, string authToken)
        {
            using (var httpClient = new HttpClient())
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(requestBody);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "", authToken))));

                var response = await httpClient.PostAsync(new Uri(url), byteContent);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> PatchDataAsync(string url, string requestBody, string authToken)
        {
            using (var httpClient = new HttpClient())
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(requestBody);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "", authToken))));

                var response = await httpClient.PatchAsync(new Uri(url), byteContent);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> GetDataAsync(string url, string authToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "", authToken))));

                var response = await httpClient.GetAsync(new Uri(url));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}