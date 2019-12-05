using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using _02_complex.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace _02_complex.Functions
{
    public static class Starter
    {
        [FunctionName("Starter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "long-running-gateway")] HttpRequest req,
            [Queue("long-running-gateway", Connection = "AzureWebJobsStorage")] ICollector<CloudQueueMessage> messages,
            ILogger log)
        {
            var payload = await QualityGatewayRequest.ParseAsync(req);

            var message = new CloudQueueMessage(JsonConvert.SerializeObject(payload));
            log.LogInformation(message.AsString);

            messages.Add(message);

            return new OkObjectResult(string.Empty);
        }
    }
}
