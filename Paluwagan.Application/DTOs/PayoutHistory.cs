using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
    public sealed record PayoutHistory(
        Guid MemberId,
        string MemberName,
        int Round,
        decimal Amount,
        DateTime PaidAt
    );
}