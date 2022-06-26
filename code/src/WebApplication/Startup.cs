using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using WebApplication.Authentication;
using WebApplication.Authorization;
using WebApplication.Extensions.DependencyInjection;
using WebApplication.Generators;
using WebApplication.Options;
using WebApplication.Providers;
using WebApplication.Repositories;
using WebApplication.Repositories.Stores;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostEnvironment { get; }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            if (HostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseForwardedHeaders();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseHsts(options => options.MaxAge(365));
            applicationBuilder.UseXContentTypeOptions();
            applicationBuilder.UseReferrerPolicy(options => options.NoReferrer());
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseXfo(options => options.Deny());

            applicationBuilder.UseRedirectValidation(options =>
            {
                options.AllowSameHostRedirectsToHttps();

                if (Configuration["RedirectValidation:AllowedDestinations"] != null)
                {
                    options.AllowedDestinations(Configuration["RedirectValidation:AllowedDestinations"].Split(';'));
                }
            });

            applicationBuilder.UseRouting();
            applicationBuilder.UseCookiePolicy();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints(routeBuilder =>
            {
                routeBuilder.MapControllers();
                routeBuilder.MapDefaultControllerRoute();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSameSiteCookiePolicy();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".Auth.Cookies";
                })
                .AddAzureADB2C(AzureADB2CDefaults.SignInWithDeviceUserPolicyId, ConfigureAzureADB2COptions)
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("AzureAD", options);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("app_access", policyBuilder => policyBuilder.RequireAppRole("app_access"));
            });

            services.AddSingleton<IAuthorizationHandler, AppRoleHandler>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseCamelCasing(true);
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddControllersWithViews();

            AddOptions<DeviceUserAuthOptions>(services, "DeviceUserAuth");

            services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();

            services.AddSingleton<IHashProvider, CryptoHashProvider>();

            services.AddTransient<IDeviceAuthRepository, AzureTableDeviceAuthRepository>(serviceProvider =>
            {
                var cloudTableClient = serviceProvider.GetRequiredService<CloudTableClient>();
                var cloudTable = cloudTableClient.GetTableReference("DeviceAuths");
                cloudTable.CreateIfNotExists();
                var azureTable = new AzureTable(cloudTable);
                var hashProvider = serviceProvider.GetRequiredService<IHashProvider>();
                return new AzureTableDeviceAuthRepository(azureTable, hashProvider);
            });

            services.AddTransient<IUserAuthRepository, AzureTableUserAuthRepository>(serviceProvider =>
            {
                var cloudTableClient = serviceProvider.GetRequiredService<CloudTableClient>();
                var cloudTable = cloudTableClient.GetTableReference("UserAuths");
                cloudTable.CreateIfNotExists();
                var azureTable = new AzureTable(cloudTable);
                var hashProvider = serviceProvider.GetRequiredService<IHashProvider>();
                return new AzureTableUserAuthRepository(azureTable, hashProvider);
            });

            services.AddSingleton(serviceProvider =>
            {
                var storageAccount = serviceProvider.GetRequiredService<CloudStorageAccount>();
                return storageAccount.CreateCloudTableClient();
            });

            services.AddSingleton(serviceProvider =>
            {
                var options = new AzureStorageOptions();
                Configuration.Bind("AzureStorage", options);
                return CloudStorageAccount.Parse(options.ConnectionString);
            });
        }

        private void AddOptions<TOptions>(IServiceCollection services, string configurationKey, Action<TOptions> configureOptions = null)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .Configure(options =>
                {
                    Configuration.Bind(configurationKey, options);
                    configureOptions?.Invoke(options);
                })
                .ValidateDataAnnotations();
        }

        private void ConfigureAzureADB2COptions(AzureADB2COptions options)
        {
            Configuration.Bind("AzureADB2C", options);
        }
    }
}
