using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Configurations;
using Paluwagan.Infrastructure.ExternalServices.Context;
using Paluwagan.Infrastructure.ExternalServices.Cookie;
using Paluwagan.Infrastructure.Services;

namespace Paluwagan.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));
        services.Configure<SupabaseOptions>(configuration.GetSection("Supabase"));
        services.Configure<FirebaseOptions>(configuration.GetSection(FirebaseOptions.Section));

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        services.AddHttpClient("Supabase");

        services.AddSignalR();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IStorageService, SupabaseStorageService>();
        services.AddScoped<IChatNotifier, SignalRChatNotifier>();
        services.AddScoped<INotificationService, FirebaseNotificationService>();

        var credentialJson = Environment.GetEnvironmentVariable("FIREBASE_ADMINSDK_JSON");
        var serviceAccountPath = configuration
            .GetSection(FirebaseOptions.Section)
            .Get<FirebaseOptions>()
            ?.ServiceAccountPath;

        if (FirebaseApp.DefaultInstance is null)
        {
            GoogleCredential? credential = null;

            if (!string.IsNullOrWhiteSpace(credentialJson))
                credential = GoogleCredential.FromJson(credentialJson);
            else if (!string.IsNullOrWhiteSpace(serviceAccountPath) && File.Exists(serviceAccountPath))
                credential = GoogleCredential.FromFile(serviceAccountPath);

            if (credential is not null)
                FirebaseApp.Create(new AppOptions { Credential = credential });
            else
                Console.Error.WriteLine("[STARTUP WARNING] Firebase credentials not found. Push notifications disabled.");
        }

        return services;
    }
}
