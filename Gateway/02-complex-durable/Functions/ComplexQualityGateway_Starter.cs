using System.Net.Http;
using System.Threading.Tasks;
using _02_complex.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _02_complex.Functions
{
    public static partial class ComplexQualityGate
    {
        [FunctionName(nameof(Starter))]
        public static async Task<HttpResponseMessage> Starter(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "complex-quality-gate")]HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var payload = await QualityGatewayRequest.ParseAsync(req);
            
            log.LogInformation("starter: " + JsonConvert.SerializeObject(payload));

            string instanceId = await starter.StartNewAsync<string>(nameof(Orchestrator), JsonConvert.SerializeObject(payload));

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}