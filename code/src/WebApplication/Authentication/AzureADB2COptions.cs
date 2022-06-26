using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace WebApplication.Authentication
{
    public class AzureADB2COptions : OpenIdConnectOptions
    {
        public string Instance { get; set; }

        public string Domain { get; set; }
    }
}
