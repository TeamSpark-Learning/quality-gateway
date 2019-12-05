using System;
using System.Threading.Tasks;
using _02_complex.Helpers;
using _02_complex.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _02_complex.Functions
{
    public static partial class ComplexQualityGate
    {
        [FunctionName(nameof(Activity_Check))]
        public static async Task<bool> Activity_Check([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            await Task.Delay(TimeSpan.FromMinutes(3));
            return true;
        }

        [FunctionName(nameof(Activity_Started))]
        public static async Task Activity_Started([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var payload = context.GetInput<ActivityPayload>();

            // Task Event example: 
            // url: {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/events?api-version=2.0-preview.1 
            // body : { "name": "TaskStarted", "taskId": "taskInstanceId", "jobId": "jobId" }

            const string TaskEventsUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/events?api-version=2.0-preview.1";
            var taskStartedEventUrl = string.Format(TaskEventsUrl,
                payload.Request.PlanUrl,
                payload.Request.ProjectId,
                payload.Request.HubName,
                payload.Request.PlanId);

            log.LogInformation(taskStartedEventUrl);

            var requestBody = JsonConvert.SerializeObject(new
            {
                jobId = payload.Request.JobId,
                taskId = payload.Request.TaskInstanceId,
                name = "TaskStarted"
            });

            await HttpHelper.PostDataAsync(taskStartedEventUrl, requestBody, payload.Request.AuthToken);
        }

        [FunctionName(nameof(Activity_Finished))]
        public static async Task Activity_Finished([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var payload = context.GetInput<ActivityPayload>();

            // Task Event example: 
            // url: {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/events?api-version=2.0-preview.1 
            // body : ex: { "name": "TaskCompleted", "taskId": "taskInstanceId", "jobId": "jobId", "result": "succeeded" }

            const string TaskEventsUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/events?api-version=2.0-preview.1";
            var taskCompletedUrl = string.Format(TaskEventsUrl,
                payload.Request.PlanUrl,
                payload.Request.ProjectId,
                payload.Request.HubName,
                payload.Request.PlanId);

            log.LogInformation(taskCompletedUrl);

            var requestBody = JsonConvert.SerializeObject(new
            {
                jobId = payload.Request.JobId,
                taskId = payload.Request.TaskInstanceId,
                name = "TaskCompleted",
                result = payload.Message
            });

            await HttpHelper.PostDataAsync(taskCompletedUrl, requestBody, payload.Request.AuthToken);
        }

        [FunctionName(nameof(Activity_Feed))]
        public static async Task Activity_Feed([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var payload = context.GetInput<ActivityPayload>();

            // Task feed example:
            // url : {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/timelines/{timelineId}/records/{jobId}/feed?api-version=4.1
            // body : {"value":["2019-01-04T12:32:42.2042287Z Task started."],"count":1}

            const string SendTaskFeedUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/timelines/{4}/records/{5}/feed?api-version=4.1";
            var taskFeedUrl = string.Format(SendTaskFeedUrl,
                payload.Request.PlanUrl,
                payload.Request.ProjectId,
                payload.Request.HubName,
                payload.Request.PlanId,
                payload.Request.TimelineId,
                payload.Request.JobId);

            log.LogInformation(taskFeedUrl);

            var requestBody = JsonConvert.SerializeObject(new
            {
                value = new string[1]
                {
                    $"{DateTime.UtcNow:0} {payload.Message}"
                },
                count = 1
            });

            await HttpHelper.PostDataAsync(taskFeedUrl, requestBody, payload.Request.AuthToken);
        }

        [FunctionName(nameof(Activity_CreateLog))]
        public static async Task<object> Activity_CreateLog([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var payload = context.GetInput<ActivityPayload>();

            // Create task log
            // url: {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/logs?api-version=4.1"
            // body: {"path":"logs\\{taskInstanceId}"}, example: {"path":"logs\\3b9c4dc6-1e5d-4379-b16c-6231d7620059"}

            const string CreateTaskLogUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/logs?api-version=4.1";
            var taskLogCreateUrl = string.Format(CreateTaskLogUrl,
                payload.Request.PlanUrl,
                payload.Request.ProjectId,
                payload.Request.HubName,
                payload.Request.PlanId);

            log.LogInformation(taskLogCreateUrl);

            var requestBody = JsonConvert.SerializeObject(new
            {
                path = payload.Request.TaskInstanceId
            });

            var response = await HttpHelper.PostDataAsync(taskLogCreateUrl, requestBody, payload.Request.AuthToken);
            return JsonConvert.DeserializeObject(response);
        }

        [FunctionName(nameof(Activity_AppendLog))]
        public static async Task Activity_AppendLog([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var payload = context.GetInput<ActivityPayload>();

            // Append to task log
            // url: {planUri}/{projectId}/_apis/distributedtask/hubs/{hubName}/plans/{planId}/logs/{taskLogId}?api-version=4.1
            // body: log messages stream data

            const string AppendLogContentUrl = "{0}/{1}/_apis/distributedtask/hubs/{2}/plans/{3}/logs/{4}?api-version=4.1";
            var appendTaskLogUrl = string.Format(AppendLogContentUrl,
                payload.Request.PlanUrl,
                payload.Request.ProjectId,
                payload.Request.HubName,
                payload.Request.PlanId,
                payload.Request.TaskLogId);

            log.LogInformation(appendTaskLogUrl);

            var requestBody = payload.Message;

            await HttpHelper.PostDataAsync(appendTaskLogUrl, requestBody, payload.Request.AuthToken);
        }
    }
}