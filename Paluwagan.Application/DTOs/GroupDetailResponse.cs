using Paluwagan.Domain.Enums;

namespace Paluwagan.Application.DTOs
{
    public sealed record GroupDetailResponse(
        string Id,
        string Name,
        decimal ContributionAmount,
        string Schedule,
        int NumberOfSlots,
        int CurrentRound,
        string OrganizerId,
        DateTime StartDate,
        GroupStatus Status,
        int PaidCount,
        decimal CollectedAmount
    );
}
