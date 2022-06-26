using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireAppRole(this AuthorizationPolicyBuilder policyBuilder, params string[] allowedRoles)
        {
            if (policyBuilder == null)
            {
                throw new ArgumentNullException(nameof(policyBuilder));
            }

            var requirement = new AppRoleRequirement(allowedRoles);
            policyBuilder.Requirements.Add(requirement);
            return policyBuilder;
        }
    }
}
