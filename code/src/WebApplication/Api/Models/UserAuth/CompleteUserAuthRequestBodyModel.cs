using Newtonsoft.Json;

namespace WebApplication.Api.Models.UserAuth
{
    public class CompleteUserAuthRequestBodyModel
    {
        [JsonProperty(PropertyName = "user_code")]
        public string UserCode { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
