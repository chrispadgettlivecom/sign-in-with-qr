using Newtonsoft.Json;

namespace WebApplication.Api.Models.DeviceAuth
{
    public class VerifyDeviceAuthRequestBodyModel
    {
        [JsonProperty(PropertyName = "device_code")]
        public string DeviceCode { get; set; }
    }
}
