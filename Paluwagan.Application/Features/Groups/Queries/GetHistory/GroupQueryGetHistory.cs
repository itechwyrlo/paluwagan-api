using System;
using System.Collections.Generic;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Groups.Queries.GetHistory
{
    public sealed record GroupQueryGetHistory(Guid groupId) : IQuery<IReadOnlyList<PayoutHistory>>;
}
