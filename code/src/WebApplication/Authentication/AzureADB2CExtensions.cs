using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace WebApplication.Authentication
{
    public static class AzureADB2CExtensions
    {
        public static AuthenticationBuilder AddAzureADB2C(this AuthenticationBuilder authenticationBuilder, string policyId, Action<AzureADB2COptions> configureOptions)
        {
            var azureADB2COptions = new AzureADB2COptions();
            configureOptions?.Invoke(azureADB2COptions);

            return authenticationBuilder.AddOpenIdConnect(
                policyId,
                openIdConnectOptions =>
                {
                    openIdConnectOptions.Authority = $"{azureADB2COptions.Instance}/{azureADB2COptions.Domain}/{policyId}/v2.0";
                    openIdConnectOptions.CallbackPath = $"/signin/{policyId}";
                    openIdConnectOptions.ClientId = azureADB2COptions.ClientId;
                    openIdConnectOptions.ClientSecret = azureADB2COptions.ClientSecret;
                    openIdConnectOptions.CorrelationCookie.Name = ".Auth.Correlation.";
                    openIdConnectOptions.NonceCookie.Name = ".Auth.OpenIdConnect.Nonce.";
                    openIdConnectOptions.RemoteSignOutPath = $"/signout/{policyId}";
                    openIdConnectOptions.ResponseType = OpenIdConnectResponseType.IdToken;
                    openIdConnectOptions.Scope.Clear();
                    openIdConnectOptions.Scope.Add("openid");
                    openIdConnectOptions.SignedOutCallbackPath = $"/signout-callback/{policyId}";

                    openIdConnectOptions.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            if (context.Properties.Items.ContainsKey("UserCode"))
                            {
                                context.ProtocolMessage.Parameters.Add("user_code", context.Properties.Items["UserCode"]);
                                context.Properties.Items.Remove("UserCode");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
