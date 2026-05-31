using System;
using System.Linq;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.DTOs
{
    public sealed record GroupSummaryResponse
    {  public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal ContributionAmount { get; init; }
        public string Schedule { get; init; } = string.Empty;
        public int TotalSlots { get; init; }
        public int CurrentRound { get; init; }
        public string OrganizerId { get; init; } = string.Empty;
        public bool MyPaymentStatus { get; init; }
        public int PaidCount { get; init; }
    }
}
