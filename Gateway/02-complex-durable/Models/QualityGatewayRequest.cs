using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using _02_complex.Extensions;
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

        public static async Task<QualityGatewayRequest> ParseAsync(HttpRequestMessage httpRequestMessage)
        {
            var request = new QualityGatewayRequest();

            request.ProjectId = httpRequestMessage.GetHeaderValue(nameof(ProjectId));
            request.PlanId = httpRequestMessage.GetHeaderValue(nameof(PlanId));
            request.JobId = httpRequestMessage.GetHeaderValue(nameof(JobId));
            request.TimelineId = httpRequestMessage.GetHeaderValue(nameof(TimelineId));
            request.TaskInstanceId = httpRequestMessage.GetHeaderValue(nameof(TaskInstanceId));
            request.HubName = httpRequestMessage.GetHeaderValue(nameof(HubName));
            request.PlanUrl = httpRequestMessage.GetHeaderValue(nameof(PlanUrl));
            request.AuthToken = httpRequestMessage.GetHeaderValue(nameof(AuthToken));

            var requestBody = await httpRequestMessage.Content.ReadAsStringAsync();
            request.CustomBody = JsonConvert.DeserializeObject(requestBody);

            return request;
        }
    }
}