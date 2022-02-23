using System.Text;
using LinkShortening.Services;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings;
using LinkShortening.Settings.Contracts;
using LoggerService;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LinkShortening.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureMongoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LinkShorteningDatabaseSettings>(
                configuration.GetSection(nameof(LinkShorteningDatabaseSettings)));

            services.AddSingleton<ILinkShorteningDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<LinkShorteningDatabaseSettings>>().Value);

            services.Configure<LinkShorteningDatabaseSettings>(
                configuration.GetSection("LinkShortening"));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUrlServices, UrlServices>();
            services.AddScoped<IUserServices, UserServices>();


            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddSingleton<UserServices>();
            services.AddSingleton<UrlServices>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtKey").ToString())),
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                });
        }



    }
}
