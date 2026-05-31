using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain.Enums;

namespace Paluwagan.Application.Features.Groups.Commands
{
    public record CreateGroupCommand(
        string Name,
        decimal ContributionAmount,
        PaymentSchedule Schedule,
        int NumberOfSlots,
        DateTime StartDate
    ) : ICommand<CreateGroupResponse>;
}
