using System;

namespace Paluwagan.Application.DTOs
{
    public sealed record MarkPaymentAsPaidResponse(
        Guid MemberId,
        int Round,
        bool IsPaid,
        DateTime? PaidAt
    );
}
