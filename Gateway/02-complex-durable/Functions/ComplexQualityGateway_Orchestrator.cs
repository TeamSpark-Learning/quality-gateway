using _02_complex.Models;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _02_complex.Functions
{
    public static partial class ComplexQualityGate
    {
        [FunctionName(nameof(Orchestrator))]
        public static async Task<bool> Orchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var payload = new ActivityPayload();
            payload.Request = JsonConvert.DeserializeObject<QualityGatewayRequest>(context.GetInput<string>());

            log.LogInformation("orchstrator:" + JsonConvert.SerializeObject(payload));

            await context.CallActivityAsync(nameof(Activity_Started), payload);

            payload.Message = "Stay tuned";
            await context.CallActivityAsync(nameof(Activity_Feed), payload);

            bool result = false;
            try
            {
                result = await context.CallActivityAsync<bool>(nameof(Activity_Check), payload);
            }
            catch (Exception e)
            {
                dynamic taskLog = await context.CallActivityAsync<object>(nameof(Activity_CreateLog), payload);
                payload.Request.TaskLogId = taskLog.id;

                payload.Message = e.Message;
                await context.CallActivityAsync(nameof(Activity_AppendLog), payload);
            }

            payload.Message = "Check completed";
            await context.CallActivityAsync(nameof(Activity_Feed), payload);

            payload.Message = $"Is healthy: {result}";
            await context.CallActivityAsync(nameof(Activity_Feed), payload);

            payload.Message = result ? "succeeded" : "failed";
            await context.CallActivityAsync(nameof(Activity_Finished), payload);

            return result;
        }
    }
}