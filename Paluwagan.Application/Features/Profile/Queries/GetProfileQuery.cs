using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Profile.Queries
{
    public sealed record GetProfileQuery() : IQuery<ProfileResponse>;
}
