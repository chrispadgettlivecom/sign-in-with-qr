using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Authentication;

namespace WebApplication.Controllers
{
    public class UserAuthController : Controller
    {
        public IActionResult LogIn(string userCode)
        {
            var challengeItems = new Dictionary<string, string>
            {
                {"UserCode", userCode}
            };

            var challengeProperties = new AuthenticationProperties(challengeItems)
            {
                RedirectUri = Url.Action("LoggedIn")
            };

            return Challenge(challengeProperties, AzureADB2CDefaults.SignInWithDeviceUserPolicyId);
        }

        public IActionResult LoggedIn()
        {
            return NoContent();
        }
    }
}
