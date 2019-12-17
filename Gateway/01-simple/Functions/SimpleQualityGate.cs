using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace _01_simple.Functions
{
    public static class SimpleQualityGate
    {
        [FunctionName("SimpleQualityGate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "simple-quality-gate")] HttpRequest req,
            ILogger log)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var resultStatus = Environment.GetEnvironmentVariable("STATUS")
                ?? "successful";
            
            return new OkObjectResult(new
            {
                status = resultStatus
            });
        }
    }
}
