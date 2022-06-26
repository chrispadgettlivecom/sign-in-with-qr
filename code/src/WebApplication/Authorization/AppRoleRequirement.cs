using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Authorization
{
    public class AppRoleRequirement : IAuthorizationRequirement
    {
        public AppRoleRequirement(string[] allowedRoles)
        {
            AllowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public string[] AllowedRoles { get; }
    }
}
