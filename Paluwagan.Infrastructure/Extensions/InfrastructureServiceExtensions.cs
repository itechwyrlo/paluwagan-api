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

        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        services.AddHttpClient("Supabase");

        services.AddSignalR();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IStorageService, SupabaseStorageService>();
        services.AddScoped<IChatNotifier, SignalRChatNotifier>();

        return services;
    }
}
