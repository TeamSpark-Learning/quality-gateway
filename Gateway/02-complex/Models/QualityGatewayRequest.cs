using System.Threading.Tasks;
using _02_complex.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _02_complex.Models
{
    public class QualityGatewayRequest
    {
        #region Original request properties

        public string ProjectId { get; set;}
        public string PlanId { get; set; }
        public string JobId { get; set; }
        public string TimelineId { get; set; }
        public string TaskInstanceId { get; set; }
        public string HubName { get; set; }
        public string PlanUrl { get; set; }
        public string AuthToken { get; set; }

        public dynamic CustomBody { get; set; }
        // public Dictionary<string, string> CustomHeaders { get; private set; }

        #endregion


        #region Runtime properties

        public string TaskLogId { get; set; }

        #endregion

        public static async Task<QualityGatewayRequest> ParseAsync(HttpRequest httpRequest)
        {
            var request = new QualityGatewayRequest();

            request.ProjectId = httpRequest.GetHeaderValue(nameof(ProjectId));
            request.PlanId = httpRequest.GetHeaderValue(nameof(PlanId));
            request.JobId = httpRequest.GetHeaderValue(nameof(JobId));
            request.TimelineId = httpRequest.GetHeaderValue(nameof(TimelineId));
            request.TaskInstanceId = httpRequest.GetHeaderValue(nameof(TaskInstanceId));
            request.HubName = httpRequest.GetHeaderValue(nameof(HubName));
            request.PlanUrl = httpRequest.GetHeaderValue(nameof(PlanUrl));
            request.AuthToken = httpRequest.GetHeaderValue(nameof(AuthToken));

            var requestBody = await httpRequest.GetBodyValueAsync();
            request.CustomBody = JsonConvert.DeserializeObject(requestBody);

            return request;
        }
    }
}