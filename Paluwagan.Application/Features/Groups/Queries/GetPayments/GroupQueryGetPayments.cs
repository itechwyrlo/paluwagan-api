using System;
using System.Collections.Generic;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Groups.Queries.GetPayments
{
    public sealed record GroupQueryGetPayments(Guid groupId) : IQuery<IReadOnlyList<PaymentStatus>>;
}
