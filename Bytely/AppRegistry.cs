using Bytely.Models.Settings;
using Microsoft.AspNetCore.HttpLogging;

namespace Bytely.Web
{
    public static class AppRegistry
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("Cookie");
            });

            builder.Services.Configure<UrlProviderSettings>(builder.Configuration.GetSection(nameof(UrlProviderSettings)));
            builder.Services.Configure<UserCookieSettings>(builder.Configuration.GetSection(nameof(UserCookieSettings)));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
