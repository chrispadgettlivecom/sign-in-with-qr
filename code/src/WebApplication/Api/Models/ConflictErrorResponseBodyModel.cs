using Newtonsoft.Json;
using System.Net;

namespace WebApplication.Api.Models
{
    public class ConflictErrorResponseBodyModel
    {
        [JsonProperty("version")]
        public string Version => "1.0.0";

        [JsonProperty("status")]
        public int Status => (int)HttpStatusCode.Conflict;

        [JsonProperty("userMessage")]
        public string UserMessage { get; set; }
    }
}
