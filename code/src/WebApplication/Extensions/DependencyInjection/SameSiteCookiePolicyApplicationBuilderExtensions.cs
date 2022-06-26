namespace WebApplication.Extensions.DependencyInjection
{
    public static class SameSiteCookiePolicyApplicationBuilderExtensions
    {
        public static IServiceCollection AddSameSiteCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = context => CheckSameSite(context.Context, context.CookieOptions);
                options.OnDeleteCookie = context => CheckSameSite(context.Context, context.CookieOptions);
            });

            return services;
        }

        public static void CheckSameSite(HttpContext context, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = context.Request.Headers["User-Agent"].ToString();

                if (!SupportsNoneSameSite(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        private static bool SupportsNoneSameSite(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return true;
            }

            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return false;
            }

            if (userAgent.Contains("Safari") && userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && userAgent.Contains("Version/"))
            {
                return false;
            }

            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return false;
            }

            return true;
        }
    }
}
