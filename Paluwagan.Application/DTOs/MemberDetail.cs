using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
    public sealed record MemberDetail(
        Guid UserId,
        string FullName,
        int SlotNumber,
        int PayoutOrder,
        string? QrCodeUrl
    );
}