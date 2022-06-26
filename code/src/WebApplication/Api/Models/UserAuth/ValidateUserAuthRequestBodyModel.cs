using Newtonsoft.Json;

namespace WebApplication.Api.Models.UserAuth
{
    public class ValidateUserAuthRequestBodyModel
    {
        [JsonProperty(PropertyName = "user_code")]
        public string UserCode { get; set; }
    }
}
