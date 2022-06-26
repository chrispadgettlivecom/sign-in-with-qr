using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Authorization
{
    public class AppRoleHandler : AuthorizationHandler<AppRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppRoleRequirement requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (requirement == null)
            {
                throw new ArgumentNullException(nameof(requirement));
            }

            if (context.User?.Claims == null || !context.User.Claims.Any())
            {
                return Task.CompletedTask;
            }

            var roleClaims = context.User.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (roleClaims == null || !roleClaims.Any())
            {
                return Task.CompletedTask;
            }

            var roles = roleClaims.Select(roleClaim => roleClaim.Value);

            if (!roles.Contains("app_access") || !roles.Intersect(requirement.AllowedRoles).Any())
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
