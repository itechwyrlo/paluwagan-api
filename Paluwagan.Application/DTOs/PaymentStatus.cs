using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
    public sealed record PaymentStatus(
        Guid MemberId,
        string MemberName,
        int Round,
        bool IsPaid,
        DateTime? PaidAt
    );
}