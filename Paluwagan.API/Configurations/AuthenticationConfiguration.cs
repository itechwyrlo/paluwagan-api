using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Paluwagan.Domain.Entities;
using Paluwagan.Infrastructure.Configurations;
using Paluwagan.Persistence.Data;

namespace Paluwagan.API.Configurations
{

    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration
                .GetSection(JwtSettings.Section)
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings is missing from configuration.");

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;

                    options.User.RequireUniqueEmail = true;

                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddEntityFrameworkStores<PaluwaganDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(
                                """{"error":"Unauthorized. Token is missing or invalid."}"""
                            );
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(
                                """{"error":"Forbidden. You do not have permission to access this resource."}"""
                            );
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.OrganizerOnly, policy =>
                    policy.RequireRole(Roles.Organizer));

                options.AddPolicy(Policies.MemberOnly, policy =>
                    policy.RequireRole(Roles.Member));

                options.AddPolicy(Policies.OrganizerOrMember, policy =>
                    policy.RequireRole(Roles.Organizer, Roles.Member));
            });

            return services;
        }

       
    }
}