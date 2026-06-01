using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Profile.Commands
{
    public sealed record UpdatePaymentDetailsCommand(
        string? GCashNumber,
        string? MayaNumber
    ) : ICommand<ProfileResponse>;
}
