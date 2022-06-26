using Newtonsoft.Json;

namespace WebApplication.Api.Models.DeviceAuth
{
    public class VerifyDeviceAuthResponseBodyModel
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
