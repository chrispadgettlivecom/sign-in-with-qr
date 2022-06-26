using Microsoft.Azure.Cosmos.Table;

namespace WebApplication.Entities
{
    public class DeviceUserAuthEntity : TableEntity
    {
        public string DeviceCode { get; set; }

        public string UserCode { get; set; }

        public string UserId { get; set; }
    }
}
