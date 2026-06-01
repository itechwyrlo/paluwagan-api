using System;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Groups.Commands
{
    public sealed record MarkPaymentAsPaidCommand(
        Guid GroupId,
        Guid MemberId,
        int Round
    ) : ICommand<MarkPaymentAsPaidResponse>;
}
