using System.Threading.Tasks;
using _02_complex.Helpers;
using _02_complex.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace _02_complex.Functions
{
    public static class Callback
    {
        [FunctionName("Callback")]
        public static async Task RunAsync(
            [QueueTrigger("long-running-gateway", Connection = "AzureWebJobsStorage")]
            CloudQueueMessage message,
            ILogger log)
        {
            var payload = JsonConvert.DeserializeObject<QualityGatewayRequest>(message.AsString);

            // Task Event example: 
            // url: {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/events?api-version=2.0-preview.1 
            // body : ex: { "name": "TaskCompleted", "taskId": "taskInstanceId", "jobId": "jobId", "result": "succeeded" }

            const string TaskEventsUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/events?api-version=2.0-preview.1";
            var taskCompletedUrl = string.Format(TaskEventsUrl,
                payload.PlanUrl,
                payload.ProjectId,
                payload.HubName,
                payload.PlanId);

            log.LogInformation(taskCompletedUrl);

            var requestBody = JsonConvert.SerializeObject(new
            {
                jobId = payload.JobId,
                taskId = payload.TaskInstanceId,
                name = "TaskCompleted",
                result = true ? "succeeded" : "failed"
            });

            await HttpHelper.PostDataAsync(taskCompletedUrl, requestBody, payload.AuthToken);
        }
    }
}
