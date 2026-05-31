using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Infrastructure.Configurations
{
    public static class Roles
    {
        public const string Organizer = "Organizer";
        public const string Member = "Member";
    }

    public static class Policies
    {
        public const string OrganizerOnly = "OrganizerOnly";
        public const string MemberOnly = "MemberOnly";
        public const string OrganizerOrMember = "OrganizerOrMember";
    }

    public sealed class JwtSettings
    {
        public const string Section = "JwtSettings";

        public string SecretKey { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; } = 60;
    }
}