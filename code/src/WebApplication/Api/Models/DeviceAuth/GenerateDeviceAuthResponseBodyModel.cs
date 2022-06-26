using Newtonsoft.Json;

namespace WebApplication.Api.Models.DeviceAuth
{
    public class GenerateDeviceAuthResponseBodyModel
    {
        [JsonProperty(PropertyName = "device_code")]
        public string DeviceCode { get; set; }

        [JsonProperty(PropertyName = "qr_code_bitmap_data")]
        public string QRCodeBitmapData { get; set; }

        [JsonProperty(PropertyName = "user_code")]
        public string UserCode { get; set; }

        [JsonProperty(PropertyName = "verification_uri")]
        public string VerificationUri { get; set; }

        [JsonProperty(PropertyName = "verification_uri_complete")]
        public string VerificationUriComplete { get; set; }
    }
}
